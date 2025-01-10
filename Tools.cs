using System.Net.Http.Json;
using System.Text.Json;
using System.Text.RegularExpressions;
using AIDemo.Models;
using Azure;
using Dapper;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.AI;

namespace AIDemo;

public class Tools
{
    private HttpClient _httpClient;

    private int _userId;

    public Tools(int userId)
    {
        _userId = userId;
        _httpClient = new HttpClient();
    }

    private void LogToConsole(string message)
    {
        var originalForegroundColor = Console.ForegroundColor;
        var originalBackgroundColor = Console.BackgroundColor;
        Console.ForegroundColor = ConsoleColor.Green;
        Console.BackgroundColor = ConsoleColor.DarkGray;
        Console.WriteLine($"[Function Call] {message}");
        Console.ForegroundColor = originalForegroundColor;
        Console.BackgroundColor = originalBackgroundColor;
    }

    public async Task<IEnumerable<BookModel>> GetUserBooks()
    {
        LogToConsole("Getting user books...");
        var baseDirectory = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        var dbPath = Path.Combine(baseDirectory!, "db.sqlite");
        using var connection = new SqliteConnection($"Data Source={dbPath}");
        await connection.OpenAsync();
        var query = "SELECT * FROM Books WHERE UserId = @userId";
        var books = await connection.QueryAsync<BookModel>(query, new { userId = _userId });
        return books;
    }

    public async Task<double> GetBookRating(string bookName)
    {
        LogToConsole($"Searching for book '{bookName}'...");
        try
        {
            var response = await _httpClient.GetAsync($"https://www.goodreads.com/search?utf8=✓&query={Uri.EscapeDataString(bookName)}");
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            var doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);

            var bookNode = doc.DocumentNode.SelectSingleNode("//tr[@itemscope and @itemtype='http://schema.org/Book']");
            if (bookNode != null)
            {
                var ratingNode = bookNode.SelectSingleNode(".//span[@class='minirating']");
                if (ratingNode != null)
                {
                    var ratingText = ratingNode.InnerText;
                    var ratingMatch = Regex.Match(ratingText, @"\d+(\.\d+)?");
                    if (ratingMatch.Success)
                    {
                        return double.Parse(ratingMatch.Value);
                    }
                }
            }

            return 0;
        }
        catch
        {
            return 0;
        }
    }

    public List<AITool> GetTools()
    {
        return
        [
            AIFunctionFactory.Create(
                this.GetBookRating,
                new AIFunctionFactoryCreateOptions
                {
                    Name = "get_book_rating",
                    Description = "Searches for a book rating.",
                    Parameters =
                    [
                        new AIFunctionParameterMetadata("bookName") { Description = "The book name" },
                    ]
                }),

            AIFunctionFactory.Create(
                this.GetUserBooks,
                new AIFunctionFactoryCreateOptions
                {
                    Name = "get_user_books",
                    Description = "Finds all books the current user has read.",
                })
        ];
    }
}
