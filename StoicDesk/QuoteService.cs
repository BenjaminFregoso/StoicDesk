using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace StoicDesk
{
    public class QuoteService
    {
        private static readonly HttpClient _http = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(15)
        };

        // Cambia esta URL por la de tu API en Hostinger
        private const string ApiUrl = "https://bnck.com.mx/api/";

        public async Task<Quote> GetRandomQuoteAsync()
        {
            try
            {
                var json = await _http.GetStringAsync(ApiUrl);
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<Quote>(json, options);
            }
            catch (Exception ex)
            {
                return new Quote
                {
                    Frase = $"Error al conectar con la API: {ex.Message}",
                    Autor = "Sistema"
                };
            }
        }
    }

    public class Quote
    {
        public int Id { get; set; }
        public string Frase { get; set; } = "";
        public string Autor { get; set; } = "";
    }
}