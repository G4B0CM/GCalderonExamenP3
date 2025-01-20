using GCalderonExamenP3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GCalderonExamenP3.Repositories
{
    internal class PaisAPIRepository
    {
        private readonly HttpClient _httpClient;
        public PaisAPIRepository()
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
