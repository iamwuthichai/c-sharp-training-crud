using AutoMapper;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.OutputCaching;
using TrainingAPI001.DTOs;
using TrainingAPI001.Entities;
using TrainingAPI001.Repositories;

namespace TrainingAPI001.Endpoints
{
    public static class GenresEndpoints
    {
        public static RouteGroupBuilder MapGenres(this RouteGroupBuilder group)
        {
            group.MapGet("/", GetGenres)
                .CacheOutput(c => c.Expire(TimeSpan.FromSeconds(60)).Tag("genres-get"));
            group.MapGet("/{id:int}", GetById);
            group.MapPost("/", Create);
            group.MapPut("/{id:int}", Update);
            group.MapDelete("/{id:int}", Delete);
            return group;
        }

        static async Task<Ok<List<GenreDTO>>> GetGenres(IGenresRepository repository, 
            IMapper mapper)
        {
            var genres = await repository.GetAll();
            // Old Code
            //var genreDTO = genres.Select(g => new GenreDTO { Id = g.Id, Name = g.Name }).ToList();
            var genreDTO = mapper.Map<List<GenreDTO>>(genres);
            return TypedResults.Ok(genreDTO);
        }

        static async Task<Results<Ok<GenreDTO>, NotFound>> GetById(int id, 
            IGenresRepository repository, IMapper mapper)
        {
            var genre = await repository.GetById(id);

            if (genre is null)
            {
                return TypedResults.NotFound();
            }

            // Old Code
            //var genreDTO = new GenreDTO
            //{
            //    Id = id,
            //    Name = genre.Name,
            //};
            var genreDTO = mapper.Map<GenreDTO>(genre);

            return TypedResults.Ok(genreDTO);
        }

        static async Task<Created<Genre>> Create(CreateGenresDTO createGenresDTO, 
            IGenresRepository repository,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            // Old Code
            //var genre = new Genre 
            //{
            //    Name = createGenresDTO.Name
            //};
            var genre = mapper.Map<Genre>(createGenresDTO);

            var id = await repository.Create(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default);

            var genreDTO = mapper.Map<GenreDTO>(genre);

            return TypedResults.Created($"/genres/{id}", genre);
        }

        static async Task<Results<NotFound, NoContent>> Update(int id, 
            CreateGenresDTO createGenresDTO,
            IGenresRepository repository,
            IOutputCacheStore outputCacheStore, IMapper mapper)
        {
            var exists = await repository.Exists(id);

            if (!exists)
            {
                return TypedResults.NotFound();
            }

            // Old Code
            //var genre = new Genre
            //{
            //    Id = id,
            //    Name = createGenresDTO.Name
            //};

            var genre = mapper.Map<Genre>(createGenresDTO);
            genre.Id = id;

            await repository.Update(genre);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }

        static async Task<Results<NotFound, NoContent>> Delete(int id, IGenresRepository repository,
            IOutputCacheStore outputCacheStore)
        {
            var exists = await repository.Exists(id);

            if (!exists)
            {
                return TypedResults.NotFound();
            }

            await repository.Delete(id);
            await outputCacheStore.EvictByTagAsync("genres-get", default);
            return TypedResults.NoContent();
        }
    }
}
