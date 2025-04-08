using Quartz;

namespace TrainingAPI001.Jobs
{
    public interface IInsertMovieJob
    {
        Task Execute(IJobExecutionContext context);
    }
}