using System.ComponentModel.DataAnnotations;

namespace BibliotecaCarvalhoPereira.Models
{
    public class Livro
    {
        [Key]
        public int Id { get; set; }
        public string? ISBN { get; set; }
        public string? EAN { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? Titulo { get; set; }
        public string? Assunto { get; set; }
        public string? Subtitulo { get; set; }
        public string? Edicao { get; set; }
        
        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public string? Autor { get; set; }
        public string? Genero { get; set; }
        public string? Editora { get; set; }
        public DateOnly DataDePublicacao { get; set; }
        public string? Idioma { get; set; }
        public string? Formato { get; set; }
        public string? Descricao { get; set; }
        public string? Observacao { get; set; }
    }
}
