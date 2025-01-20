using Microsoft.Extensions.Logging;
using GCalderonExamenP3.Repositories;

namespace GCalderonExamenP3
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            string dbPath = FileAccessHelper.GetLocalFilePath("GabrielCalderon.db3");
            builder.Services.AddSingleton<PaisRepository>(s => ActivatorUtilities.CreateInstance<PaisRepository>(s, dbPath));
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
    public class FileAccessHelper
    {
        public static string GetLocalFilePath(string filename)
        {
            return System.IO.Path.Combine(FileSystem.AppDataDirectory, filename);
        }
    }
}
