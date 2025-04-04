using TrainingAPI001.Entities;

namespace TrainingAPI001.Repositories
{
    public interface IActorsRepository
    {
        Task<int> Create(Actor actor);
        Task Delete(int id);
        Task<bool> Exist(int id);
        Task<List<Actor>> GetAll();
        Task Update(Actor actor);
    }
}