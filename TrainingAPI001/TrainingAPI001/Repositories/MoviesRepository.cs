using System.Formats.Asn1;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TrainingAPI001.DTOs;
using TrainingAPI001.Entities;

namespace TrainingAPI001.Repositories
{
    public class MoviesRepository(ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor, IMapper mapper) : IMoviesRepository
    {
        public async Task<List<Movie>> GetAll(PaginationDTO pagination)
        {
            var queryable = context.Movies.AsQueryable();
            await httpContextAccessor.HttpContext!
                .InsertPaginationParameterInResponseHeader(queryable);
            return await queryable.OrderBy(m => m.Title)
                .Include(m => m.Comments)
                .Pagination(pagination)
                .ToListAsync();
        }

        public async Task<Movie?> GetById(int id)
        {
            return await context.Movies
                .Include(m => m.Comments)
                .Include(m => m.GenresMovies)
                    .ThenInclude(gm => gm.Genre)
                .Include(m => m.ActorMovies.OrderBy(am => am.Order))
                    .ThenInclude(am => am.Actor)
                .AsNoTracking().FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<bool> Exists(int id)
        {
            return await context.Movies.AnyAsync(m => m.Id == id);
        }

        public async Task<int> Create(Movie movie)
        {
            context.Add(movie);
            await context.SaveChangesAsync();
            return movie.Id;
        }
 
        public async Task Update(Movie movie)
        {
            context.Update(movie);
            await context.SaveChangesAsync();
        }

        public async Task Delete(int id)
        {
            await context.Movies.Where(m => m.Id == id).ExecuteDeleteAsync();
        }

        public async Task Assign(int id, List<int> genresIds)
        {
            var movie = await context.Movies.Include(m => m.GenresMovies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null) {
                throw new ArgumentException($"There's no movie with id {id}");
            }

            var genresMovies = genresIds.Select(genresId => new GenreMovie { GenreId = genresId });

            movie.GenresMovies = mapper.Map(genresMovies, movie.GenresMovies);

            await context.SaveChangesAsync();
        }

        public async Task Assign(int id, List<ActorMovie> actors)
        {
            for (int i = 1; i <= actors.Count; i++)
            {
                actors[i - 1].Order = 1;
            }

            var movie = await context.Movies.Include(m => m.ActorMovies)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (movie is null) { 
                throw new ArgumentException($"There's not movie with id {id}");
            }

            movie.ActorMovies = mapper.Map(actors, movie.ActorMovies);

            await context.SaveChangesAsync();
        }
    }
}
