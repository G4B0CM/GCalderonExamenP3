using GCalderonExamenP3.ViewModels;
namespace GCalderonExamenP3;

public partial class PaisView : ContentPage
{
	public PaisView()
	{
		InitializeComponent();
		BindingContext = new PaisAPIViewModel();
	}

    
}