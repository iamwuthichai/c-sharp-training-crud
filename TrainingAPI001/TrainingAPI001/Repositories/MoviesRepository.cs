using System.Formats.Asn1;
using Microsoft.EntityFrameworkCore;
using TrainingAPI001.DTOs;
using TrainingAPI001.Entities;

namespace TrainingAPI001.Repositories
{
    public class MoviesRepository(ApplicationDbContext context,
        IHttpContextAccessor httpContextAccessor) : IMoviesRepository
    {
        public async Task<List<Movie>> GetAll(PaginationDTO pagination)
        {
            var queryable = context.Movies.AsQueryable();
            await httpContextAccessor.HttpContext!
                .InsertPaginationParameterInResponseHeader(queryable);
            return await queryable.OrderBy(m => m.Title).Pagination(pagination)
                .ToListAsync();
        }

        public async Task<Movie?> GetById(int id)
        {
            return await context.Movies
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
    }
}
