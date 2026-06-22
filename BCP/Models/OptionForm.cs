namespace BCP.Models;

/// <summary>
/// Classe estática que contém as opções para os campos do formulário de cadastro de livros.
/// </summary>
public static class OptionForm
{
    /// <summary>
    ///     Lista de opções para o campo "Formato" do formulário de cadastro de livros.
    /// </summary>
    public static readonly IReadOnlyList<string> Format =
    [
        "Ebook",
        "Fotocópia",
        "Físico",
        ".pdf",
        ".docx",
        ".epub"
    ];

    /// <summary>
    /// Lista de opções para o campo "Género" do formulário de cadastro de livros.
    /// </summary>
    public static readonly IReadOnlyList<string> Gender =
    [
        "Artes, Desporto e Lazer",
        "Ciências Sociais",
        "Ciências Naturais e Matemática",
        "Filosofia e Psicologia",
        "Generalidades",
        "História e Geografia Humana",
        "Língua e Linguística",
        "Literatura",
        "Religião e Teologia",
        "Tecnologia e Ciências Aplicadas"
    ];

    /// <summary>
    /// Lista de opções para o campo "Proprietário" do formulário de cadastro de livros.
    /// </summary>
    public static readonly IReadOnlyList<string> Owner =
    [
        "Ana Luísa Marques",
        "Daniel Marques",
        "Luís Ricardo Pereira",
        "Maria Paula Pereira"
    ];
}