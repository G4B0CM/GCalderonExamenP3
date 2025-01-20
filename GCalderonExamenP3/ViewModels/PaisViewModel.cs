using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GCalderonExamenP3.Models;
using GCalderonExamenP3.Repositories;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;

namespace GCalderonExamenP3.ViewModels
{
    public class PaisViewModel : ObservableObject, IQueryAttributable
    {
        private string _statusMessage;
        public ObservableCollection<Pais> Paises { get; set; }

        private Models.Pais _pais;
        private readonly PaisRepository _paisRepository;

        public ICommand SaveCommand { get; }
        public ICommand ObtenerTodosLosPaises { get; }
        public ICommand EliminarPaisCommand { get; }
        public Models.Pais Pais
        {
            get => _pais;
            set
            {
                if (SetProperty(ref _pais, value)) //Profe estoy usando este método porque me instale el CommunityToolkit.MVVM
                {
                    OnPropertyChanged(nameof(_pais.Nombre));
                    OnPropertyChanged(nameof(_pais.Region));
                    OnPropertyChanged(nameof(_pais.LinkGoogle));
                }
            }
        }
        public string Nombre
        {
            get => _pais.Nombre;
            set
            {
                if (_pais.Nombre != value)
                {
                    _pais.Nombre = value;
                    OnPropertyChanged();
                }
            }
        }

        public string Region
        {
            get => _pais.Region;
            set
            {
                if (_pais.Region != value)
                {
                    _pais.Region = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LinkGoogle
        {
            get => _pais.LinkGoogle;
            set
            {
                if (_pais.LinkGoogle != value)
                {
                    _pais.LinkGoogle = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Id => _pais.Id;
        public PaisViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GabrielCalderon.db3");
            _paisRepository = new PaisRepository(dbPath);

            _pais = new Models.Pais();
            Paises = new ObservableCollection<Models.Pais>();
            SaveCommand = new AsyncRelayCommand(Save);
            ObtenerTodosLosPaises = new AsyncRelayCommand(LoadPeople);
            EliminarPaisCommand = new AsyncRelayCommand<Models.Pais>((person) => Eliminar(person));
        }
        public string StatusMessage
        {
            get => _statusMessage;
            set
            {
                _statusMessage = value;
                OnPropertyChanged();
            }
        }

        private async Task Save()
        {
            try
            {
                if (string.IsNullOrEmpty(_pais.Nombre))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                var httpClient = new HttpClient();
                var response = await httpClient.GetStringAsync("https://restcountries.com/v3.1/name/" + _pais.Nombre + "?fields=name,region,maps");

                
                var paises = JsonSerializer.Deserialize<List<Models.Pais>>(response);

                
                foreach (var pais in paises)
                {
                    _paisRepository.agregarPais(pais.Nombre, pais.Region, pais.LinkGoogle);
                }

                Console.WriteLine( "Datos importados desde la API correctamente.");

                StatusMessage = $"Persona {_pais.Nombre} guardada exitosamente.";
                await Shell.Current.GoToAsync($"..?saved={_pais.Nombre}");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar la persona: {ex.Message}";
            }
        }
        private async Task Eliminar(Models.Pais paisAEliminar)
        {
            try
            {
                if (paisAEliminar == null)
                {
                    throw new Exception("Persona no válida.");
                }

                _paisRepository.EliminarPersona(paisAEliminar.Nombre);
                Paises.Remove(paisAEliminar);
                StatusMessage = $"Se eliminó a {paisAEliminar.Nombre}.";

                await Shell.Current.DisplayAlert("Aviso!", $"Gabriel Calderón acaba de eliminar a {paisAEliminar.Nombre}", "Aceptar");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al eliminar a la persona: {ex.Message}";
            }
        }
        private async Task LoadPeople()
        {
            try
            {
                var paises = _paisRepository.GetAllPeople();
                Paises.Clear();
                foreach (var person in paises)
                {
                    Paises.Add(person);
                }

                StatusMessage = $"Se cargaron {Paises.Count} personas.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener personas: {ex.Message}";
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("person") && query["person"] is Models.Pais pais)
            {
                Pais = pais;
            }
            else if (query.ContainsKey("deleted"))
            {
                string nombre = query["deleted"].ToString();
                Models.Pais paisEncontrado = Paises.FirstOrDefault(p => p.Nombre == nombre);

                if (paisEncontrado != null)
                    Paises.Remove(paisEncontrado);
            }
        }
        private async Task LLamarAPI()
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al importar datos: {ex.Message}";
            }
        }
    }
}
