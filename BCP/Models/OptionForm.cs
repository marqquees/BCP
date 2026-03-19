namespace BCP.Models
{
    public static class OptionForm
    {
        /// <summary>
        /// Lista de opções para o campo "Formato" do formulário de cadastro de livros.
        /// </summary>
        public static readonly IReadOnlyList<string> Format = [
            "Fotocópia", 
            "Físico", 
            ".pdf", 
            ".docx", 
            "Ebook", 
            ".epub"
        ];

        /// <summary>
        /// Lista de opções para o campo "Género" do formulário de cadastro de livros.
        /// </summary>
        public static readonly IReadOnlyList<string> Gender =
        [
            "Outro",
            "Artigo Científico",
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
            "Revista"
        ];
        
        /// <summary>
        /// Lista de opções para o campo "Proprietário" do formulário de cadastro de livros.
        /// </summary>
        public static readonly IReadOnlyList<string> Owner =
        [
            "Luís Ricardo Pereira",
            "Paulinha Pereira",
            "Ana Luísa Marques",
            "Daniel Marques"
        ];
    }
}
