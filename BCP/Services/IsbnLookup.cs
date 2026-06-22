using System.Text.Json;
using System.Text.RegularExpressions;
using BCP.Models;

namespace BCP.Services;

/// <summary>
/// Serviço para buscar informações de um livro a partir do ISBN utilizando as APIs do Google Books e Open Library.
/// </summary>
/// <param name="httpClient">
/// Instância de HttpClient para realizar as requisições HTTP às APIs externas.
/// Deve ser configurada com políticas de retry e timeout adequadas para garantir resiliência e perfórmance.
/// </param>
/// <param name="logger">
/// Instância de ILogger para registrar logs de erros e informações durante o processo de consulta às APIs.
/// </param>
public class IsbnLookup(HttpClient httpClient, ILogger<IsbnLookup> logger)
{
    /// <summary>
    /// Preenche os dados de um livro a partir do seu ISBN, consultando as APIs do Google Books e Open Library.
    /// </summary>
    /// <param name="book">
    /// Instância de Book para a qual preencher os dados.
    /// </param>
    /// <returns>
    /// true se os dados do livro foram preenchidos com sucesso, false caso contrário.
    /// </returns>
    public async Task<bool> FillBookDataFromIsbnAsync(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.ISBN)) return false;

        var isbn = book.ISBN.Replace("-", "").Replace(" ", "").Trim();

        // 1. Google Books API
        try
        {
            var urlGoogleBook = $"https://www.googleapis.com/books/v1/volumes?q=isbn:{isbn}";
            var responseGoogle = await httpClient.GetFromJsonAsync<JsonElement>(urlGoogleBook);

            if (responseGoogle.TryGetProperty("items", out var items) &&
                items.ValueKind == JsonValueKind.Array && items.GetArrayLength() > 0)
            {
                var item = items.EnumerateArray().First().GetProperty("volumeInfo");

                if (item.TryGetProperty("title", out var title))
                    book.Title = title.GetString();

                if (item.TryGetProperty("authors", out var authors))
                    book.Author = string.Join(", ", authors.EnumerateArray().Select(a => a.GetString()));

                if (item.TryGetProperty("publisher", out var publisher))
                    book.Publisher = publisher.GetString();

                if (item.TryGetProperty("publishedDate", out var publishedDate))
                {
                    var dateStr = publishedDate.GetString();
                    var match = Regex.Match(dateStr ?? string.Empty, @"\b\d{4}\b");
                    if (match.Success) book.PublishedYear = int.Parse(match.Value);
                }

                if (item.TryGetProperty("categories", out var categories))
                    book.Subject = string.Join(", ",
                        categories.EnumerateArray().Take(3).Select(c => c.GetString()));

                if (item.TryGetProperty("description", out var description))
                {
                    var desc = description.GetString();
                    book.Description = desc?.Length > 500 ? desc[.. 497] + "..." : desc;
                }
            }
        }
        catch (Exception error)
        {
            logger.LogError(error, "Erro ao consultar a Google Books API para o ISBN {Isbn}.", isbn);
        }

        // 2. Open Library API
        try
        {
            var urlOpenLibrary = $"https://openlibrary.org/api/books?bibkeys=ISBN:{isbn}&format=json&jscmd=data";
            var responseOpenLibrary = await httpClient.GetFromJsonAsync<JsonElement>(urlOpenLibrary);

            if (responseOpenLibrary.TryGetProperty($"ISBN:{isbn}", out var items)
                && items.ValueKind == JsonValueKind.Object && items.GetRawText() != "{}")
            {
                if (items.TryGetProperty("title", out var title))
                    book.Title = title.GetString();

                if (items.TryGetProperty("authors", out var authors))
                    book.Author = string.Join(", ",
                        authors.EnumerateArray().Select(a => a.GetProperty("name").GetString()));

                if (items.TryGetProperty("publishers", out var publishers))
                    book.Publisher = publishers.EnumerateArray().First().GetProperty("name").GetString();

                if (items.TryGetProperty("publish_date", out var publishDate))
                {
                    var dateStr = publishDate.GetString();
                    var match = Regex.Match(dateStr ?? string.Empty, @"\b\d{4}\b");
                    if (match.Success) book.PublishedYear = int.Parse(match.Value);
                }

                if (items.TryGetProperty("subjects", out var subjects))
                    book.Subject = string.Join(", ",
                        subjects.EnumerateArray().Take(3).Select(s => s.GetProperty("name").GetString()));

                if (items.TryGetProperty("description", out var description))
                {
                    var desc = description.GetProperty("value").GetString() ?? description.GetString();
                    book.Description = desc?.Length > 500 ? desc[.. 497] + "..." : desc;
                }
            }
        }
        catch (Exception error)
        {
            logger.LogError(error, "Erro ao consultar a Open Library API para o ISBN {Isbn}.", isbn);
            return false;
        }

        return true;
    }
}