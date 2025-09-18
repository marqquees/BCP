using System.ComponentModel.DataAnnotations;

namespace BibliotecaCarvalhoPereira.Models
{
    public class Book
    {
        [Key]
        public int Id { get; set; }
        public string? ISBN { get; set; }
        public string? EAN { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? Title { get; set; }
        public string? Subject { get; set; }
        public string? Subtitle { get; set; }
        public string? Edition { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? Author { get; set; }
        public string? Gender { get; set; }
        public string? Publisher { get; set; }
        public DateOnly PublicationDate { get; set; }
        public string? Language { get; set; }
        public string? Format { get; set; }
        public string? Description { get; set; }
        public string? Note { get; set; }
    }
}
