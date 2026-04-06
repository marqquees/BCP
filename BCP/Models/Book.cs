using System.ComponentModel.DataAnnotations;

namespace BCP.Models
{
    /// <summary>
    /// Modelo de dados para representar um livro, contendo informações como
    /// ISBN, título, autor, editora, ano de publicação, entre outros.
    /// </summary>
    public class Book
    {
        [Key]
        public int Id { get; init; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MaxLength(150, ErrorMessage = "O título deve conter no máximo 150 caracteres.")]
        public string? Title { get; set; }

        [StringLength(13, ErrorMessage = "O ISBN deve conter 13 caracteres.")]
        public string? ISBN { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? Format { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MaxLength(100, ErrorMessage = "O autor deve conter no máximo 100 caracteres.")]
        public string? Author { get; set; }

        [MaxLength(100, ErrorMessage = "O assunto deve conter no máximo 100 caracteres.")]
        public string? Subject { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [MaxLength(100, ErrorMessage = "A editora deve conter no máximo 100 caracteres.")]
        public string? Publisher { get; set; }

        [Range(1, 9999, ErrorMessage = "A edição deve ser um número entre 1 e 9999.")]
        public int? Edition { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        [Range(1000, 2100, ErrorMessage = "O ano de publicação deve ser um ano válido e deve conter 4 dígitos.")]
        public int? PublicationYear { get; set; }

        [MaxLength(500, ErrorMessage = "A descrição deve conter no máximo 500 caracteres.")]
        public string? Description { get; set; }

        [MaxLength(200, ErrorMessage = "A nota deve conter no máximo 200 caracteres.")]
        public string? Note { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? Owner { get; set; }
    }
}
