using TrainingAPI001.Entities;

namespace TrainingAPI001.Repositories
{
    public class GenreRepository : IGenresRepository
    {
        private readonly ApplicationDbContext context;
        public GenreRepository(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<int> Create(Genre genre)
        {
            context.Add(genre);
            await context.SaveChangesAsync();
            return genre.Id;
        }
    }
}
