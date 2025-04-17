using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IMDB.Domain.Models.DBModel;
using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.ResponseModel;
using IMDB.Repository;
using IMDB.Repository.Interfaces;
using IMDBAPI.Services.CustomException;
using IMDBAPI.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;

namespace IMDBAPI.Services
{
    public class ActorService:IActorService
    {
        public readonly IActorRepository _actorRepository;
        private readonly IMapper _mapper;
        public ActorService(IActorRepository actorRepository,IMapper mapper)
        {
            _actorRepository = actorRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(ActorRequest actorRequest)
        {
            var actor = _mapper.Map<Actor>(actorRequest);
            var createdActor = await _actorRepository.CreateAsync(actor);

            return createdActor;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var actor = await _actorRepository.GetByIdAsync(id);
            if (actor == null)
            {
                throw new NotFoundException($"Actor with ID {id} not found.");
            }

            return await _actorRepository.DeleteAsync(id);
        }

        public async Task<List<ActorResponse>> GetAllAsync()
        {
            var actors = await _actorRepository.GetAllAsync();

            if(actors== null || !actors.Any())
            {
                throw new NotFoundException("No actors found.");
            }

            return _mapper.Map<List<ActorResponse>>(actors);
        }

        public async Task<ActorResponse> GetByIdAsync(int id)
        {
            var actor = await _actorRepository.GetByIdAsync(id);
            if (actor == null)
            {
                throw new NotFoundException($"Actor with ID {id} not found.");
            }

            return _mapper.Map<ActorResponse>(actor);
        }

        public async Task<ActorResponse> UpdateAsync(int id, JsonPatchDocument<ActorRequest> document)
        {
            var existingActor = await _actorRepository.GetByIdAsync(id);
            if (existingActor == null)
            {
                throw new NotFoundException($"Actor with ID {id} not found.");
            }
            var actorRequest = new ActorRequest
            {
                Name = existingActor.Name,
                Bio = existingActor.Bio,
                DOB=existingActor.DOB,
                Gender=existingActor.Gender,
            };

            document.ApplyTo(actorRequest, error => {
                throw new ArgumentException($"Invalid request.{error.ErrorMessage}");
            });
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(actorRequest);
            var validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(actorRequest, validationContext, validationResult, true);
            if (!isValid)
            {
                throw new ValidationException(string.Join(',', validationResult.Select(vr => vr.ErrorMessage)));
            }
            var updatedActor = _mapper.Map<Actor>(actorRequest);
            updatedActor.Id = id;
            await _actorRepository.UpdateAsync(updatedActor);

            var updatedActorEntity = await _actorRepository.GetByIdAsync(id);
            return _mapper.Map<ActorResponse>(updatedActorEntity);
        }
        public async Task<List<int>> GetActorsFromMoviesAsync(int movieId)
        {
            var actors = await _actorRepository.GetActorsFromMovies(movieId);
            return actors;
        }
    }
}
