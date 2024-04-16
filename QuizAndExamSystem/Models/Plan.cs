using System.ComponentModel.DataAnnotations;

namespace ExamSystem.Models
{
    public class Plan    
    {
        [Key]
        public int Id { get; set; }
        public string? Image { get; set; } = string.Empty;
        public string? Name { get; set; } = string.Empty;
        public float Price { get; set; }= 0.00f;
        public string? Description { get; set; } = string.Empty;

        
    }
}
