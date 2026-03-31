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
            "Generalidades",
            "Filosofia e Psicologia",
            "Religião e Teologia",
            "Ciências Sociais",
            "Língua e Linguística",
            "Ciências Naturais e Matemática",
            "Tecnologia e Ciências Aplicadas",
            "Artes, Desporto e Lazer",
            "Literatura",
            "História e Geografia Humana"
        ];
        
        /// <summary>
        /// Lista de opções para o campo "Proprietário" do formulário de cadastro de livros.
        /// </summary>
        public static readonly IReadOnlyList<string> Owner =
        [
            "Luís Ricardo Pereira",
            "Maria Paula Pereira",
            "Ana Luísa Marques",
            "Daniel Marques"
        ];
    }
}
