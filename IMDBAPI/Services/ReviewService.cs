using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using IMDB.Domain.Models.DBModel;
using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.ResponseModel;
using IMDB.Repository.Interfaces;
using IMDBAPI.Services.CustomException;
using IMDBAPI.Services.Interfaces;
using Microsoft.AspNetCore.JsonPatch;
namespace IMDBAPI.Services
{
    public class ReviewService:IReviewService
    {
        public readonly IReviewsRepository _reviewsRepository;
        private readonly IMapper _mapper;
        private readonly IMovieService _movieService;
        public ReviewService(IReviewsRepository reviewsRepository,IMapper mapper, IMovieService movieService)
        {
            _reviewsRepository = reviewsRepository;
            _mapper = mapper;
            _movieService = movieService;
        }
        public async Task<int> CreateAsync(ReviewRequest reviewRequest)
        {

            var movie = await _movieService.GetByIdAsync(reviewRequest.MovieId);
            if (movie == null)
            {
                throw new NotFoundException($"Movie with ID {reviewRequest.MovieId} not found.");
            }
            var review = _mapper.Map<Review>(reviewRequest);
            var createdReview =await  _reviewsRepository.CreateAsync(review);
            return createdReview;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var review = await _reviewsRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new NotFoundException($"review with ID {id} not found.");
            }

            return await _reviewsRepository.DeleteAsync(id);
        }

        public async Task<List<ReviewResponse>> GetAllAsync()
        {
            var reviews = await _reviewsRepository.GetAllAsync();
            if (reviews == null || !reviews.Any())
            {
                throw new NotFoundException("No Review Found");
            }
            return _mapper.Map<List<ReviewResponse>>(reviews);
        }

        public async Task<ReviewResponse> GetByIdAsync(int id)
        {
            var review = await _reviewsRepository.GetByIdAsync(id);
            if (review == null)
            {
                throw new NotFoundException($"review with ID {id} not found.");
            }
            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<ReviewResponse> UpdateAsync(int id, JsonPatchDocument<ReviewRequest> document)
        {
            var existingReview = await _reviewsRepository.GetByIdAsync(id);
            if (existingReview == null)
            {
                throw new NotFoundException($"Review with ID {id} not found.");
            }
            var reviewRequest = new ReviewRequest
            {
                Message = existingReview.Message,
                MovieId = existingReview.MovieId,
            };

            document.ApplyTo(reviewRequest, error =>
            {
                throw new ArgumentException($"Invalid request.{error.ErrorMessage}");
            });
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(reviewRequest);
            var validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(reviewRequest, validationContext, validationResult, true);
            if (!isValid)
            {
                throw new ValidationException(string.Join(',', validationResult.Select(vr => vr.ErrorMessage)));
            }
            var movie = await _movieService.GetByIdAsync(existingReview.MovieId);
            if (movie == null)
            {
                throw new NotFoundException($"Movie with ID {existingReview.MovieId} not found.");
            }
            var updatedReview = _mapper.Map<Review>(reviewRequest);
            updatedReview.Id = id;
            await _reviewsRepository.UpdateAsync(updatedReview);

            var updatedReviewEntity = await _reviewsRepository.GetByIdAsync(id);
            return _mapper.Map<ReviewResponse>(updatedReviewEntity);
        }

    }
}
