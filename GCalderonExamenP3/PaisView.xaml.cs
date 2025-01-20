using GCalderonExamenP3.ViewModels;
namespace GCalderonExamenP3;

public partial class PaisView : ContentPage
{
	public PaisView()
	{
		InitializeComponent();
		BindingContext = new PaisViewModel();
	}

    private void GCalderonBotonLimpiar_Clicked(object sender, EventArgs e)
    {
		GCalderonNombre.Text = "";
    }
}