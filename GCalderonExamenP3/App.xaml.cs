using GCalderonExamenP3.Repositories;
namespace GCalderonExamenP3
{
    public partial class App : Application
    {
        public static PaisRepository PaisRepo { get; private set; }
        public App(PaisRepository repo)
        {
            InitializeComponent();

            MainPage = new AppShell();

            PaisRepo = repo;
        }
    }
}
