namespace BibliotecaCarvalhoPereira.Models
{
    public class OpcoesFormulario
    {
        public static readonly IReadOnlyList<string> Idioma = ["Português", "Inglês", "Espanhol", "Francês", "Alemão", "Italiano"];
        public static readonly IReadOnlyList<string> Formato = ["Físico", "Fotocópia", "PDF", "DOCX", "Ebook", "EPUB"];
       
        public static readonly IReadOnlyList<string> Genero = 
        [
            "Ficção",
            "Biografia",
            "Didático",
            "Ensaio",
            "Poesia",
            "Teatro",
            "Autoajuda",
            "Religião",
            "Tese",
            "Outros"
        ];
    }
}
