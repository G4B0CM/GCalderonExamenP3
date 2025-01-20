using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;

namespace GCalderonExamenP3.Models
{
    public class PaisAPI
    {
        public Pais paisNormal {  get; set; }

        public async void CargarPaisAPI(string Nombre)
        {
            
            var httpClient = new HttpClient();
            var response = await httpClient.GetStringAsync("https://restcountries.com/v3.1/name/" + Nombre + "?fields=name,region,maps");


            var pais = JsonSerializer.Deserialize<List<Models.Pais>>(response);

            paisNormal = pais.FirstOrDefault();

        }
    }
    
}
