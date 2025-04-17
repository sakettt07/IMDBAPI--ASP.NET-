using AutoMapper;
using IMDB.Domain.Models.DBModel;
using IMDB.Domain.Models.RequestModel;
using IMDB.Domain.Models.ResponseModel;

namespace IMDBAPI.Helper
{
    public class ApplicationMapper:Profile
    {
        public ApplicationMapper()
        {
            CreateMap<ActorRequest, Actor>();
            CreateMap<Actor, ActorResponse>();
            CreateMap<ProducerRequest,Producer>();
            CreateMap<Producer, ProducerResponse>();
            CreateMap<MovieRequest, Movie>();
            CreateMap<Movie, MovieResponse>();
            CreateMap<GenreRequest,Genre>();
            CreateMap<Genre, GenreResponse>();
            CreateMap<ReviewRequest,Review>();
            CreateMap<Review, ReviewResponse>();
        }
    }
}
