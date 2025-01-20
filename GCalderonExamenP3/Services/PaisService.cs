using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GCalderonExamenP3.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GCalderonExamenP3.Services
{
    public class PaisService
    {
        private readonly HttpClient _httpClient;

        public PaisService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Pais>> ObtenerPaisesAsync(string Nombre)
        {
            var url = "https://restcountries.com/v3.1/name/" + Nombre + "?fields=name,region,maps";
            var response = await _httpClient.GetStringAsync(url);
            return JsonSerializer.Deserialize<List<Pais>>(response, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }
    }
}
