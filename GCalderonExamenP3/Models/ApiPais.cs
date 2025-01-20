using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GCalderonExamenP3.Models
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    namespace GCalderonExamenP3.Models
    {
        public class ApiPais
        {
            public name name { get; set; }
            public string region { get; set; }
            public maps maps { get; set; }
            public List<string> borders { get; set; }

            private static readonly HttpClient _httpClient = new HttpClient
            {
                BaseAddress = new Uri("https://restcountries.com/v3.1/")
            };

            public static async Task<ApiPais> ObtenerPaisPorNombre(string nombre)
            {
                try
                {
                    var response = await _httpClient.GetAsync($"name/{nombre}?fields=name,region,maps");
                    response.EnsureSuccessStatusCode();

                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var paises = JsonSerializer.Deserialize<List<ApiPais>>(jsonResponse);

                    return paises?.Count > 0 ? paises[0] : null;
                }
                catch (Exception)
                {
                    return null; // Devuelve null en caso de error
                }
            }
        }

        public class name
        {
            public string common { get; set; }
            public string official { get; set; }
        }

        public class maps
        {
            public string googleMaps { get; set; }
            public string openStreetMaps { get; set; }
        }
    }
}
