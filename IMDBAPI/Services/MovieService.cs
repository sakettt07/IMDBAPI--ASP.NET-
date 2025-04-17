using System;
using System.Collections.Generic;
using System.Linq;
using IMDB.Repository.Interfaces;
using IMDB.Domain.Models.ResponseModel;
using AutoMapper;
using IMDBAPI.Services.CustomException;
using System.Threading.Tasks;
using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.DBModel;
using Microsoft.AspNetCore.JsonPatch;
using System.ComponentModel.DataAnnotations;
using IMDBAPI.Services.Interfaces;
using IMDBAPI.Services;
using Microsoft.AspNetCore.Http;

namespace IMDB.Service
{
    public class MovieService:IMovieService
    {
        public readonly IMovieRepository _movieRepository;
        public static IMapper _mapper;
        public readonly IActorService _actorService;
        public readonly IGenreService _genreService;
        public readonly IProducerService _producerService;
        public readonly SupabaseStorageService _supabaseStorageService;

        public MovieService(IMovieRepository movieRepository, IMapper mapper, IActorService actorService, IGenreService genreService,SupabaseStorageService supabaseStorageService , IProducerService producerService)
        {
            _movieRepository = movieRepository;
            _mapper = mapper;
            _actorService = actorService;
            _genreService = genreService;
            _producerService = producerService;
            _supabaseStorageService = supabaseStorageService;
        }

        public async Task<int> CreateAsync(MovieRequest movieRequest)
        {

            if (movieRequest.Actors == null || !movieRequest.Actors.Any() || movieRequest.Actors.Contains(-1))
                throw new ArgumentException("Invalid actor ID. Please select from the given list.");


            var allActors = _mapper.Map<List<Actor>>(await _actorService.GetAllAsync());
            var validActors = allActors.Where(a => movieRequest.Actors.Contains(a.Id)).ToList();
            var invalidActorIds = movieRequest.Actors.Except(validActors.Select(a => a.Id)).ToList();

            if (invalidActorIds.Any())
                throw new ArgumentException($"Invalid Actor IDs: {string.Join(", ", invalidActorIds)}");

            if (movieRequest.Genres == null || !movieRequest.Genres.Any())
                throw new ArgumentException("Invalid genre ID. Please select from the given list.");

            var allGenres = _mapper.Map<List<Genre>>(await _genreService.GetAllAsync());
            var validGenres = allGenres.Where(g => movieRequest.Genres.Contains(g.Id)).ToList();
            var invalidGenreIds = movieRequest.Genres.Except(validGenres.Select(g => g.Id)).ToList();

            if (invalidGenreIds.Any())
                throw new ArgumentException($"Invalid Genre IDs: {string.Join(", ", invalidGenreIds)}");

            var selectedProducer = _mapper.Map<Producer>(await _producerService.GetByIdAsync(movieRequest.ProducerId));

            if (selectedProducer == null)
                throw new ArgumentException($"Producer with ID {movieRequest.ProducerId} does not exist");
            var newMovie = new Movie
            {
                Name = movieRequest.Name,
                YearOfRelease = movieRequest.YearOfRelease,
                Plot = movieRequest.Plot,
                CoverImage = movieRequest.CoverImage,
                ProducerId = selectedProducer.Id,
            };

            return await _movieRepository.CreateAsync(newMovie,validActors,validGenres);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(id);
            if (existingMovie == null)
                throw new NotFoundException($"Movie with ID {id} not found.");

            return await _movieRepository.DeleteAsync(id);
        }

        public async Task<List<MovieResponse>> GetAllAsync(int? year = null)
        {
            var movieData = await _movieRepository.GetAllAsync();
            if (movieData == null || !movieData.Any())
                throw new NotFoundException("No movies found.");

            if (year.HasValue)
            {
                movieData = movieData.Where(m => m.YearOfRelease == year).ToList();
                if (!movieData.Any())
                    throw new NotFoundException($"No movies found for year {year}");
            }
            return _mapper.Map<List<MovieResponse>>(movieData);
        }
        public async Task<MovieResponse> GetByIdAsync(int id)
        {
            var movie = await _movieRepository.GetByIdAsync(id);
            if (movie == null)
                throw new NotFoundException($"Movie with ID {id} not found.");
            return new MovieResponse
            {
                Id = movie.Id,
                Name = movie.Name,
                YearOfRelease = movie.YearOfRelease,
                Plot = movie.Plot,
                CoverImage = movie.CoverImage,
                ProducerId = movie.ProducerId,
                ActorsIds= await _actorService.GetActorsFromMoviesAsync(id),
                GenresIds = await _genreService.GetGenresFromMoviesAsync(id)
            };
        }

        public async Task<MovieResponse> UpdateAsync(int id, JsonPatchDocument<MovieRequest> document)
        {
            var existingMovie = await _movieRepository.GetByIdAsync(id);
            if (existingMovie == null)
                throw new NotFoundException($"Movie with ID {id} not found.");
            var movieRequest = new MovieRequest
            {
                Name = existingMovie.Name,
                YearOfRelease = existingMovie.YearOfRelease,
                Plot = existingMovie.Plot,
                CoverImage = existingMovie.CoverImage,
                ProducerId = existingMovie.ProducerId,
                Actors=await _actorService.GetActorsFromMoviesAsync(id),
                Genres = await _genreService.GetGenresFromMoviesAsync(id)
            };
            document.ApplyTo(movieRequest, error =>
            {
                throw new ArgumentException("Invalid request body.");
            });
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(movieRequest);
            var validationResult = new List<ValidationResult>();
            bool isValid = Validator.TryValidateObject(movieRequest, validationContext, validationResult, true);
            if (!isValid)
            {
                throw new ValidationException(string.Join(',', validationResult.Select(vr => vr.ErrorMessage)));
            }
            var allGenres = _mapper.Map<List<Genre>>(await _genreService.GetAllAsync());
            var validGenres = allGenres.Where(g => movieRequest.Genres.Contains(g.Id)).ToList();
            var invalidGenreIds = movieRequest.Genres.Except(validGenres.Select(g => g.Id)).ToList();
            if (invalidGenreIds.Any())
                throw new ArgumentException($"Invalid Genre IDs: {string.Join(", ", invalidGenreIds)}");
            var allActors = _mapper.Map<List<Actor>>(await _actorService.GetAllAsync());
            var validActors = allActors.Where(a => movieRequest.Actors.Contains(a.Id)).ToList();
            var invalidActorIds = movieRequest.Actors.Except(validActors.Select(a => a.Id)).ToList();

            if (invalidActorIds.Any())
                throw new ArgumentException($"Invalid Actor IDs: {string.Join(", ", invalidActorIds)}");

            var selectedProducer = _mapper.Map<Producer>(await _producerService.GetByIdAsync(movieRequest.ProducerId));

            if (selectedProducer == null)
                throw new ArgumentException($"Producer with ID {movieRequest.ProducerId} does not exist");

            existingMovie.Name = movieRequest.Name;
            existingMovie.YearOfRelease = movieRequest.YearOfRelease;

            existingMovie.Plot = movieRequest.Plot;
            existingMovie.CoverImage = movieRequest.CoverImage;
            existingMovie.ProducerId = selectedProducer.Id;
            return _mapper.Map<MovieResponse>(await _movieRepository.UpdateAsync(existingMovie,validActors,validGenres));
        }
        public async Task<string> UploadCoverImageAsync(IFormFile file)
        {
            var stream = file.OpenReadStream();
            if (stream == null)
                throw new ArgumentException("File stream is null");
            return await _supabaseStorageService.UploadImageAsync(stream,file.FileName);
        }
    }
}
