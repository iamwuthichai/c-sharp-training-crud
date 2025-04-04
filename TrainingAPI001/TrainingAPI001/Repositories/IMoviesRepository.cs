using TrainingAPI001.DTOs;
using TrainingAPI001.Entities;

namespace TrainingAPI001.Repositories
{
    public interface IMoviesRepository
    {
        Task<List<Movie>> GetAll(PaginationDTO pagination);
        Task<Movie?> GetById(int id);
        Task<int> Create(Movie movie);
        Task Delete(int id);
        Task<bool> Exists(int id);
        Task Update(Movie movie);
    }
}