namespace TrainingAPI001.DTOs
{
    public class CreateMovieDTO
    {
        public string Title { get; set; } = null!;
        public int InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public IFormFile? Poster { get; set; }
    }
}
