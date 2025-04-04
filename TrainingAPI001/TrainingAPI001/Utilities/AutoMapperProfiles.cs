using AutoMapper;
using TrainingAPI001.DTOs;
using TrainingAPI001.Entities;

namespace TrainingAPI001.Utilities
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<Genre, GenreDTO>();
            CreateMap<CreateGenresDTO, Genre>();

            CreateMap<Actor, ActorDTO>();
            CreateMap<CreateActorDTO, Actor>()
                .ForMember(p => p.Picture, options => options.Ignore());

            CreateMap<Movie, MovieDTO>();
            CreateMap<CreateMovieDTO, Movie>()
                .ForMember(p => p.Poster, options => options.Ignore());

            CreateMap<Comment, CommentDTO>();
            CreateMap<CreateCommentDTO, Comment>();
        }
    }
}
