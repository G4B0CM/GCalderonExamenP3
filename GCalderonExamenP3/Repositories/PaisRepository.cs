using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GCalderonExamenP3.Models;
namespace GCalderonExamenP3.Repositories
{
    
        public class PaisRepository
        {
            string _dbPath;


            public string StatusMessage { get; set; }

            private SQLiteConnection conn;

            private void Init()
            {
                if (conn != null)
                    return;

                conn = new SQLiteConnection(_dbPath);
                conn.CreateTable<Pais>();
            }

            public PaisRepository(string dbPath)
            {
                _dbPath = dbPath;
            }

            public void agregarUsuario(string nombre, string region,string linkGoogle)
            {
                int result = 0;
                try
                {
                    Init();

                    if (string.IsNullOrEmpty(nombre))
                        throw new Exception("Valid name required");

                    if (string.IsNullOrEmpty(region))
                        throw new Exception("Valid description required");

                    result = conn.Insert(new Pais { Nombre = nombre, Region = region,LinkGoogle=linkGoogle, MiNombre = "Gabriel Calderon" });

                    StatusMessage = string.Format("{0} record(s) added (Name: {1})", result, nombre);
                }
                catch (Exception ex)
                {
                    StatusMessage = string.Format("Failed to add {0}. Error: {1}", nombre, ex.Message);
                }

            }

            public List<Pais> GetAllPeople()
            {
                try
                {
                    Init();
                    return conn.Table<Pais>().ToList();
                }
                catch (Exception ex)
                {
                    StatusMessage = string.Format("Failed to retrieve data. {0}", ex.Message);
                }

                return new List<Pais>();
            }
            public void EliminarPersona(string name)
            {
                int result = 0;
                try
                {
                    Init();

                    if (string.IsNullOrEmpty(name))
                        throw new Exception("Valid name required");
                    var person = conn.Table<Models.Pais>().FirstOrDefault(p => p.Nombre == name);
                    result = conn.Delete(person);

                    StatusMessage = string.Format("{0} record(s) deleted (Name: {1})", result, name);
                }
                catch (Exception ex)
                {
                    StatusMessage = string.Format("Failed to delete {0}. Error: {1}", name, ex.Message);
                }
            }

        }
    
}
