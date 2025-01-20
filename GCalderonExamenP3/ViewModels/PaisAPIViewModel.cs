using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GCalderonExamenP3.Models;
using GCalderonExamenP3.Models.GCalderonExamenP3.Models;
using GCalderonExamenP3.Repositories;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using System.Windows.Input;

namespace GCalderonExamenP3.ViewModels
{
    public class PaisAPIViewModel : ObservableObject
    {
        private readonly PaisRepository _paisRepository;

        private ApiPais _paisSeleccionado;
        private string _nombreBusqueda;
        private string _statusMessage;

        public ObservableCollection<ApiPais> Paises { get; set; }
        public ICommand BuscarPaisCommand { get; }
        public ICommand GuardarPaisCommand { get; }

        public PaisAPIViewModel()
        {
            string dbPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "GabrielCalderon.db3");
            _paisRepository = new PaisRepository(dbPath);

            Paises = new ObservableCollection<ApiPais>();
            BuscarPaisCommand = new AsyncRelayCommand(BuscarPaisAsync);
            GuardarPaisCommand = new AsyncRelayCommand(GuardarPaisAsync);
        }

        public string NombreBusqueda
        {
            get => _nombreBusqueda;
            set => SetProperty(ref _nombreBusqueda, value);
        }

        public ApiPais PaisSeleccionado
        {
            get => _paisSeleccionado;
            set => SetProperty(ref _paisSeleccionado, value);
        }

        public string StatusMessage
        {
            get => _statusMessage;
            set => SetProperty(ref _statusMessage, value);
        }

        private async Task BuscarPaisAsync()
        {
            try
            {
                if (string.IsNullOrWhiteSpace(NombreBusqueda))
                {
                    await Shell.Current.DisplayAlert("Error", "Ingrese un nombre de país para buscar.", "OK");
                    return;
                }

                // Llamada al método estático del modelo ApiPais
                var paisObtenido = await ApiPais.ObtenerPaisPorNombre(NombreBusqueda);

                if (paisObtenido != null)
                {
                    PaisSeleccionado = paisObtenido;

                    // Mostrar datos del país encontrado
                    await Shell.Current.DisplayAlert(
                        "País Encontrado",
                        $"Nombre: {PaisSeleccionado.name.common}\n" +
                        $"Región: {PaisSeleccionado.region}\n" +
                        $"Mapa: {PaisSeleccionado.maps.googleMaps}",
                        "OK"
                    );

                    // Actualizar la lista para mostrar el país en el ListView
                    Paises.Clear();
                    Paises.Add(paisObtenido);

                    StatusMessage = "País encontrado correctamente.";
                }
                else
                {
                    await Shell.Current.DisplayAlert("Aviso", "No se encontró un país con ese nombre.", "OK");
                }
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al buscar el país: {ex.Message}";
                await Shell.Current.DisplayAlert("Error", StatusMessage, "OK");
            }
        }

        private async Task GuardarPaisAsync()
        {
            try
            {
                if (PaisSeleccionado == null)
                {
                    await Shell.Current.DisplayAlert("Error", "No hay un país seleccionado para guardar.", "OK");
                    return;
                }

                // Guardar el país seleccionado en la base de datos
                _paisRepository.agregarPais(
                    PaisSeleccionado.name.common,
                    PaisSeleccionado.region,
                    PaisSeleccionado.maps.googleMaps
                );

                StatusMessage = $"País {PaisSeleccionado.name.common} guardado correctamente.";
                await Shell.Current.DisplayAlert("Guardado", StatusMessage, "OK");
            }
            catch (Exception ex)
            {
                StatusMessage = $"Error al guardar el país: {ex.Message}";
                await Shell.Current.DisplayAlert("Error", StatusMessage, "OK");
            }
        }
    }
}