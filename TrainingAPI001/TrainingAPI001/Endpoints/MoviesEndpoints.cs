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
    public static class MoviesEndpoints
    {
        private readonly static string container = "movies";
        public static RouteGroupBuilder MapMovies(this RouteGroupBuilder group)
        {
            group.MapPost("/", Create).DisableAntiforgery();
            return group;
        }

        static async Task<Created<MovieDTO>> Create([FromForm] CreateMovieDTO createMovieDTO,
            IFileStorage fileStorage, IOutputCacheStore outputCacheStore,
            IMapper mapper, IMoviesRepository repository)
        {
            var movie = mapper.Map<Movie>(createMovieDTO);

            if (createMovieDTO.Poster is not null)
            {
                var url = await fileStorage.Store(container, createMovieDTO.Poster);
                movie.Poster = url;
            }

            var id = await repository.Create(movie);
            await outputCacheStore.EvictByTagAsync("movies-get", default);
            var movieDTO = mapper.Map<MovieDTO>(movie);
            return TypedResults.Created($"movies/{id}", movieDTO);
        }
    }
}
