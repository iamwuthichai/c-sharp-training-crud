using TrainingAPI001.Entities;

namespace TrainingAPI001.Repositories
{
    public interface IGenresRepository
    {
        Task<int> Create(Genre genre);
        Task<Genre> GetById(int id);
        Task<List<Genre>> GetAll();
    }
}
