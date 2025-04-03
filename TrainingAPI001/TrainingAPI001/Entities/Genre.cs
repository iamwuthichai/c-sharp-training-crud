using System.ComponentModel.DataAnnotations;

namespace TrainingAPI001.Entities
{
    public class Genre
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
    }
}
