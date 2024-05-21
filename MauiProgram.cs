using mauicrud.confgdata;
using Microsoft.Extensions.Logging
    ;



using mauicrud.logicaSchede;
using mauicrud.Views;



namespace mauicrud
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


            var dbcontext = new LavoratoreDb();
            dbcontext.Database.EnsureCreated();
            dbcontext.Dispose();


            builder.Services.AddDbContext<LavoratoreDb>();

            builder.Services.AddTransient<Lavoratorepage>();
            builder.Services.AddTransient<Lavoratorescheda>();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<Mainscheda>();

            Routing.RegisterRoute(nameof(Lavoratorepage), typeof(Lavoratorepage));

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
