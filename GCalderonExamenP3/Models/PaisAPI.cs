using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.Net.Http;

namespace GCalderonExamenP3.Models
{
    public class PaisAPI
    {
        public Pais paisNormal {  get; set; }
        private readonly HttpClient _httpClient;
        public PaisAPI()
        {
            paisNormal = new Pais();
        }
        
       
    }
    
}
