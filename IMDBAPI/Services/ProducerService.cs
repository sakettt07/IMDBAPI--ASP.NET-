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
using Microsoft.AspNetCore.Mvc;

namespace IMDBAPI.Services
{
    public class ProducerService:IProducerService
    {
        public readonly IProducerRepository _producerRepository;
        private readonly IMapper _mapper;
        public ProducerService(IProducerRepository producerRepository, IMapper mapper)
        {
            _producerRepository = producerRepository;
            _mapper = mapper;
        }
        public async Task<int> CreateAsync(ProducerRequest producerRequest)
        {
            var producer = _mapper.Map<Producer>(producerRequest);

            var createdProducer = await _producerRepository.CreateAsync(producer);
            return createdProducer;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var producer = await _producerRepository.GetByIdAsync(id);
            if (producer == null)
            {
                throw new NotFoundException($"producer with ID {id} not found.");
            }

            return await _producerRepository.DeleteAsync(id);
        }
        public async Task<List<ProducerResponse>> GetAllAsync()
        {
            var producers = await _producerRepository.GetAllAsync();
            if (producers == null)
            {
                throw new NotFoundException("No producers found.");
            }
            return _mapper.Map<List<ProducerResponse>>(producers);
        }
        public async Task<ProducerResponse> GetByIdAsync(int id)
        {
            var producer = await _producerRepository.GetByIdAsync(id);
            if (producer == null)
            {
                throw new NotFoundException($"producer with ID {id} not found.");
            }
            return _mapper.Map<ProducerResponse>(producer);
        }
        public async Task<ProducerResponse> UpdateAsync(int id, JsonPatchDocument<ProducerRequest> document)
        {
            var existingProducer = await _producerRepository.GetByIdAsync(id);
            if (existingProducer == null)
            {
                throw new NotFoundException($"Producer with ID {id} not found.");
            }
            var producerRequest = new ProducerRequest
            {
                Name = existingProducer.Name,
                Bio = existingProducer.Bio,
                DOB = existingProducer.DOB,
                Gender = existingProducer.Gender,
            };

            document.ApplyTo(producerRequest, error => {
                throw new ArgumentException($"Invalid request.{error.ErrorMessage}");
            });
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(producerRequest);
            var validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(producerRequest, validationContext, validationResult, true);
            if (!isValid)
            {
                throw new ValidationException(string.Join(',', validationResult.Select(vr => vr.ErrorMessage)));
            }
            var updatedProducer = _mapper.Map<Producer>(producerRequest);
            updatedProducer.Id = id;
            await _producerRepository.UpdateAsync(updatedProducer);

            var updatedProducerEntity = await _producerRepository.GetByIdAsync(id);
            return _mapper.Map<ProducerResponse>(updatedProducerEntity);
        }
    }
}
