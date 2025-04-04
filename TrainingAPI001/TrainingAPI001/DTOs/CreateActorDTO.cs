namespace TrainingAPI001.DTOs
{
    public class CreateActorDTO
    {
        public string Name { get; set; } = null!;
        public DateTime DateOfbirth { get; set; }
        public IFormFile? Picture { get; set; }
    }
}
