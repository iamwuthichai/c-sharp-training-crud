using Quartz;
using TrainingAPI001.Entities;

namespace TrainingAPI001.Jobs
{
    public class InsertMovieJob : IJob, IInsertMovieJob
    {
        private readonly ApplicationDbContext _db;

        public InsertMovieJob(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var newMovie = new Movie
            {
                Title = "Auto-Generated Movie " + DateTime.Now.ToString("HH:mm:ss"),
                Intheaters = false,
                ReleaseDate = DateTime.Now,
                Poster = null
            };

            _db.Movies.Add(newMovie);
            await _db.SaveChangesAsync();

            Console.WriteLine($"[Quartz] Movie inserted at {DateTime.Now}");
        }
    }
}
