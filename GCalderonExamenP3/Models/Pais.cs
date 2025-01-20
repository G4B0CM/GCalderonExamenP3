using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;

namespace GCalderonExamenP3.Models
{
    [Table ("Pais")]
    public class Pais
    {
        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        [Unique]
        public string Nombre { get; set; }
        public string Region { get; set; }
        public string LinkGoogle { get; set; }
        public string MiNombre { get; set; }
    }
}
