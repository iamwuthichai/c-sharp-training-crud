namespace TrainingAPI001.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public bool Intheaters { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string? Poster { get; set; }
    }
}
