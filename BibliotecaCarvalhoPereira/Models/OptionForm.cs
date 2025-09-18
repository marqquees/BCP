namespace BibliotecaCarvalhoPereira.Models
{
    public class OptionForm
    {
        public static readonly IReadOnlyList<string> Language = ["Português", "Inglês", "Espanhol", "Francês", "Alemão", "Italiano"];
        public static readonly IReadOnlyList<string> Format = ["Físico", "Fotocópia", "PDF", "DOCX", "Ebook", "EPUB"];
       
        public static readonly IReadOnlyList<string> Gender = 
        [
            "Ficção",
            "Biografia",
            "Didático",
            "Ensaio",
            "Poesia",
            "Teatro",
            "Autoajuda",
            "Religião",
            "Romance",
            "Tese",
            "Outros"
        ];
    }
}
