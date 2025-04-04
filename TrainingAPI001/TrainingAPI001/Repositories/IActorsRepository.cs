using TrainingAPI001.DTOs;
using TrainingAPI001.Entities;

namespace TrainingAPI001.Repositories
{
    public interface IActorsRepository
    {
        Task<int> Create(Actor actor);
        Task<Actor> GetById(int id);
        Task<List<Actor>> GetByName(string name);
        Task<List<Actor>> GetAll(PaginationDTO pagination);
        Task<bool> Exist(int id);
        Task Delete(int id);
        Task Update(Actor actor);
    }
}