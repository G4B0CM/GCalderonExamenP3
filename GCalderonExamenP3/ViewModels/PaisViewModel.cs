using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GCalderonExamenP3.Models;
using GCalderonExamenP3.Repositories;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Windows.Input;
using GCalderonExamenP3.Services;

namespace GCalderonExamenP3.ViewModels
{
    public class PaisViewModel : ObservableObject, IQueryAttributable
    {
        private string _statusMessage;
        private readonly PaisService _userService;
        public ObservableCollection<Pais> Paises { get; set; }
        private PaisAPI paisAPI;
        private Models.Pais _pais;
        private readonly PaisRepository _paisRepository;

        public ICommand SaveCommand { get; }
        public ICommand ObtenerTodosLosPaises { get; }
        public ICommand EliminarPaisCommand { get; }
        public ICommand LeerPaisCommand { get; }
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
            LeerPaisCommand = new AsyncRelayCommand(CargarPaisesAsync);
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
                if (string.IsNullOrEmpty(_pais.Region))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }
                if (string.IsNullOrEmpty(_pais.LinkGoogle))
                {
                    throw new Exception("El nombre no puede estar vacío.");
                }

                _paisRepository.agregarPais(_pais.Nombre,_pais.Region,_pais.LinkGoogle);

                StatusMessage = $"Pais {_pais.Nombre} guardado exitosamente.";
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

                _paisRepository.EliminarPais(paisAEliminar.Nombre);
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
                var paises = _paisRepository.ObtenerTodosLosPaises();
                Paises.Clear();
                foreach (var pais in paises)
                {
                    Paises.Add(pais);
                }

                StatusMessage = $"Se cargaron {Paises.Count} personas.";
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al obtener personas: {ex.Message}";
            }
        }
        public async Task CargarPaisesAsync()
        {

            var paises = await _userService.ObtenerPaisesAsync("Ecuador");
            paises.Clear();

            foreach (var pais in paises)
            {
                
                Paises.Add(pais);
            }
        }

        public void ApplyQueryAttributes(IDictionary<string, object> query)
        {
            if (query.ContainsKey("pais") && query["pais"] is Models.Pais pais)
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
    }
}
