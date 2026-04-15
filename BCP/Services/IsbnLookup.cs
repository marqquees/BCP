using System.Text.Json;
using System.Text.RegularExpressions;
using BCP.Models;

namespace BCP.Services;

/// <summary>
/// Serviço responsável por consultar as APIs do Google Books e Open Library para preencher os dados de um livro a partir do seu ISBN.
/// </summary>
/// <param name="httpClient">
/// Instância de HttpClient injetada para realizar as requisições HTTP às APIs externas.
/// </param>
/// <param name="logger">
/// Instância de ILogger injetada para registrar logs de erros e informações durante o processo de consulta às APIs.
/// </param>
public class IsbnLookup(HttpClient httpClient, ILogger<IsbnLookup> logger)
{
    /// <summary>
    /// Preenche os dados de um livro a partir do seu ISBN, consultando as APIs do Google Books e Open Library.
    /// </summary>
    /// <param name="book">
    /// O objeto Book que contém o ISBN a ser consultado.
    /// Os campos Title, Author, Publisher, PublishedYear, Subject e Description,
    /// serão preenchidos com as informações retornadas pelas APIs, se disponíveis.
    /// </param>
    /// <returns>
    /// True se a consulta e preenchimento foram realizados com sucesso, mesmo que algumas informações possam estar faltando.
    /// False se ocorreu um erro durante a consulta ou se o ISBN for inválido ou não encontrado em ambas as APIs.
    /// </returns>
    public async Task<bool> FillBookDataFromIsbnAsync(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.ISBN)) return false;
        
        // Limpar o ISBN de caracteres indesejados como os hífens e espaços da formatação.
        string isbn = book.ISBN.Replace("-", "").Replace(" ", "").Trim();
        
        // 1. Primeira tentativa: Google Books API.
        try
        {
            // URL da API do Google Books para consulta por ISBN, incluindo a chave de API.
            string urlGoogleBook = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}";
            
            // Realizar a requisição GET para a API do Google Books para obter a resposta como um JsonElement.
            JsonElement responseGoogle = await httpClient.GetFromJsonAsync<JsonElement>(urlGoogleBook);

            // Verificar se a resposta contém itens e se o array de itens não está vazio.
            if (responseGoogle.TryGetProperty("items", out JsonElement items) && 
                items.ValueKind == JsonValueKind.Array && items.GetArrayLength() > 0)
            {
                // O campo "volumeInfo" contém as informações detalhadas do livro,
                // então precisamos acessar esse campo para extrair os dados.
                JsonElement item = items.EnumerateArray().First().GetProperty("volumeInfo");

                // Prenche o campo Título do livro, se disponível.
                if (item.TryGetProperty("title", out JsonElement title))
                    book.Title = title.GetString();
                
                // Prenche o campo Autor, que pode ser um array de strings, então precisamos concatenar os autores em uma única string.
                // Exemplo: "J.K. Rowling, John Doe".
                if (item.TryGetProperty("authors", out JsonElement authors))
                    book.Author = string.Join(", ", authors.EnumerateArray().Select(a => a.GetString()));
                
                // Prenche o campo Editora, se disponível.
                if (item.TryGetProperty("publisher", out JsonElement publisher))
                    book.Publisher = publisher.GetString();
               
                // Prenche o campo Ano de Publicação, extraindo apenas o ano de uma string que pode conter data completa,
                // utilizando o Regex.
                // Exemplo: "2007-07-21" ou "July 21, 2007" --> extrair apenas "2007".
                if (item.TryGetProperty("publishedDate", out JsonElement publishedDate))
                {
                    string? dateStr = publishedDate.GetString();
                    Match match = Regex.Match(dateStr ?? string.Empty, @"\b\d{4}\b");
                    if (match.Success) book.PublishedYear = int.Parse(match.Value);
                }

                // Prenche o campo Assunto, que pode ser um array de strings, então precisamos concatenar os assuntos em uma única string.
                // Limitar a quantidade de assuntos a 3 para evitar descrições muito longas e para evitar problemas de armazenamento.
                // Exemplo: "Ficção, Aventura, Fantasia".
                if (item.TryGetProperty("categories", out JsonElement categories))
                {
                    book.Subject = string.Join(", ", 
                        categories.EnumerateArray().Take(3).Select(c => c.GetString()));
                }
                    
                // Prenche o campo Descrição, se disponível.
                // Limitar a descrição a 500 caracteres para evitar problemas de armazenamento e exibição.
                if (item.TryGetProperty("description", out JsonElement description))
                {
                    string? desc = description.GetString();
                    book.Description = desc?.Length > 500 ? desc[.. 497] + "..." : desc;
                }
            }
        }
        catch (Exception error)
        {
            // Logar o erro ocorrido durante a consulta à API do Google Books,
            // incluindo o ISBN consultado para facilitar a identificação do problema.
            logger.LogError(error, "Erro ao consultar a Google Books API para o ISBN {Isbn}.", isbn);
        }

        // 2. Segunda tentativa: Open Library API.
        try
        {
            // URL da API do Open Library para consulta por ISBN.
            string urlOpenLibrary = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";
            
            // Realizar a requisição GET para a API do Open Library para obter a resposta como um JsonElement.
            JsonElement responseOpenLibrary = await httpClient.GetFromJsonAsync<JsonElement>(urlOpenLibrary);

            // Verificar se a resposta contém a chave correspondente ao ISBN consultado e se o valor é um objeto JSON válido.
            if (responseOpenLibrary.TryGetProperty($"ISBN:{isbn}", out JsonElement items) 
                && items.ValueKind == JsonValueKind.Object && items.GetRawText() != "{}")
            {
                // Prenche o campo Título do livro, se disponível.
                if (items.TryGetProperty("title", out JsonElement title))
                    book.Title = title.GetString();

                // Prenche o campo Autor, que pode ser um array de objetos com a propriedade "name",
                // então precisamos concatenar os nomes dos autores em uma única string.
                // Exemplo: "J.K. Rowling, John Doe".
                if (items.TryGetProperty("authors", out JsonElement authors))
                {
                    book.Author = string.Join(", ",
                        authors.EnumerateArray().Select(a => a.GetProperty("name").GetString()));
                }
                    
                // Prenche o campo Editora, que pode ser um array de objetos com a propriedade "name",
                // então precisamos pegar o nome da primeira e única editora disponível.
                if (items.TryGetProperty("publishers", out JsonElement publishers))
                    book.Publisher = publishers.EnumerateArray().First().GetProperty("name").GetString();
                    
                // Prenche o campo Ano de Publicação, extraindo apenas o ano de uma string que pode conter data completa,
                // utilizando o Regex.
                // Exemplo: "2007-07-21" ou "July 21, 2007" --> extrair apenas "2007".
                if (items.TryGetProperty("publish_date", out JsonElement publishDate))
                {
                    string? dateStr = publishDate.GetString();
                    Match match = Regex.Match(dateStr ?? string.Empty, @"\b\d{4}\b");
                    if (match.Success) book.PublishedYear = int.Parse(match.Value);
                }
                
                // Prenche o campo Assunto, que pode ser um array de objetos com a propriedade "name",
                // então precisamos concatenar os nomes dos assuntos em uma única string.
                // Limitar a quantidade de assuntos a 3 para evitar descrições muito longas e para evitar problemas de armazenamento.
                // Exemplo: "Ficção, Aventura, Fantasia".
                if (items.TryGetProperty("subjects", out JsonElement subjects))
                {
                    book.Subject = string.Join(", ",
                        subjects.EnumerateArray().Take(3).Select(s => s.GetProperty("name").GetString()));
                }

                // Prenche o campo Descrição, que pode ser um objeto com a propriedade "value" ou uma string simples,
                // então precisamos verificar ambos os casos e extrair a descrição corretamente.
                // Limitar a descrição a 500 caracteres para evitar problemas de armazenamento e exibição.
                if (items.TryGetProperty("description", out JsonElement description))
                {
                    string? desc = description.GetProperty("value").GetString() ?? description.GetString();
                    book.Description = desc?.Length > 500 ? desc[.. 497] + "..." : desc;
                }
            }
        }
        catch (Exception error)
        {
            // Logar o erro ocorrido durante a consulta à API do Open Library,
            // incluindo o ISBN consultado para facilitar a identificação do problema.
            logger.LogError(error, "Erro ao consultar a Open Library API para o ISBN {Isbn}.", isbn);
            return false;
        }
        return true;
    }
}