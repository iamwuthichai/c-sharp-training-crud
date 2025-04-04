using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using TrainingAPI001.DTOs;
using TrainingAPI001.Entities;
using TrainingAPI001.Repositories;
using TrainingAPI001.Services;

namespace TrainingAPI001.Endpoints
{
    public static class ActorsEndpoints
    {
        private readonly static string container = "actors";
        public static RouteGroupBuilder MapActors(this RouteGroupBuilder group) 
        {
            //group.MapGet("/", GetActors)
            //    .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("actors-get"));
            group.MapPost("/", Create).DisableAntiforgery();
            return group;
        }

        static async Task<Created<ActorDTO>> Create([FromForm] CreateActorDTO createActorDTO,
            IActorsRepository repository, IOutputCacheStore outputCacheStore,
            IMapper mapper, IFileStorage fileStorage)
        {
            var actor = mapper.Map<Actor>(createActorDTO);

            if (createActorDTO.Picture is not null)
            {
                var url = await fileStorage.Store(container, createActorDTO.Picture);
                actor.Picture = url;
            }

            var id = await repository.Create(actor);
            await outputCacheStore.EvictByTagAsync("actors", default);
            var actorDTO = mapper.Map<ActorDTO>(actor);
            return TypedResults.Created($"/actors/{id}", actorDTO);
        }
    }
}
