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
    public class GenreService : IGenreService
    {
        public readonly IGenreRepository _genreRepository;
        private readonly IMapper _mapper;

        public GenreService(IGenreRepository genreRepository,IMapper mapper)
        {
            _genreRepository = genreRepository;
            _mapper = mapper;
        }

        public async Task<int> CreateAsync(GenreRequest genreRequest)
        {
            var genre = _mapper.Map<Genre>(genreRequest);

            var createdGenreId = await _genreRepository.CreateAsync(genre);
            return createdGenreId;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var genre = await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                throw new NotFoundException($"Genre with ID {id} not found.");
            }

            return await _genreRepository.DeleteAsync(id);
        }

        public async Task<List<GenreResponse>> GetAllAsync()
        {
            var genres = await _genreRepository.GetAllAsync();
            if(genres == null || !genres.Any())
            {
                throw new NotFoundException("No genres found.");
            }
            return _mapper.Map<List<GenreResponse>>(genres);
        }

        public async Task<GenreResponse> GetByIdAsync(int id)
        {
            var genre =await _genreRepository.GetByIdAsync(id);
            if (genre == null)
            {
                throw new NotFoundException($"Genre with ID {id} not found.");
            }
            return _mapper.Map<GenreResponse>(genre);
        }

        public async Task<GenreResponse> UpdateAsync(int id,JsonPatchDocument<GenreRequest> document)
        {
            var existingGenre = await _genreRepository.GetByIdAsync(id);
            if (existingGenre == null)
            {
                throw new NotFoundException($"Genre with ID {id} not found.");
            }
            var genreRequest = new GenreRequest
            {
                Name = existingGenre.Name,
            };

            document.ApplyTo(genreRequest, error => {
                throw new ArgumentException($"Invalid request.{error.ErrorMessage}");
            });
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(genreRequest);
            var validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(genreRequest, validationContext, validationResult, true);
            if (!isValid)
            {
                throw new ValidationException(string.Join(',', validationResult.Select(vr => vr.ErrorMessage)));
            }
            var updatedGenre = _mapper.Map<Genre>(genreRequest);
            updatedGenre.Id = id;

            var result = await _genreRepository.UpdateAsync(updatedGenre);

            return _mapper.Map<GenreResponse>(result);
        }

        public async Task<List<int>> GetGenresFromMoviesAsync(int movieId)
        {
            var genres = await _genreRepository.GetGenresFromMoviesAsync(movieId);
            return genres;
        }

    }
}
