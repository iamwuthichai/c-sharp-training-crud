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
        }
    }
}
