namespace TrainingAPI001.DTOs
{
    public class MovieDTO
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public int InTheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Poster { get; set; }
    }
}
