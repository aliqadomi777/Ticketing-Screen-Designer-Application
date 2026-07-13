using _2__Ticketing_Screen_Designer.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Formatting.Json;
using System;
using System.IO;
using System.Text.Json;
using System.Windows.Forms;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Repositories;
using Ticketing_Screen_Designer.Services;

namespace Ticketing_Screen_Designer
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {

            string logDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            string logFilePath = Path.Combine(logDirectory, "errors.json");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File(new JsonFormatter(), logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //change this later
            string connectionString = "Server=localhost;Database=TicketingScreenDesignerDB;User Id=sa;Password=Sedc0@123;TrustServerCertificate=True;";
            var services = new ServiceCollection();
            services.AddSingleton<IUiStateService, UiStateService>();

            services.AddTransient<IFetchableRepository<BankModel>>(provider => new BankRepository(connectionString));
            services.AddTransient<IAddableRepository<BankModel>>(provider => new BankRepository(connectionString));
            services.AddTransient<IBankService, BankService>();

            services.AddTransient<IFetchableRepository<ScreenModel>>(provider => new ScreenRepository(connectionString));
            services.AddTransient<IAddableRepository<ScreenModel>>(provider => new ScreenRepository(connectionString));
            services.AddTransient<IDeleteableRepository<ScreenModel>>(provider => new ScreenRepository(connectionString));
            services.AddTransient<IListableRepository<ScreenModel>>(provider => new ScreenRepository(connectionString));
            services.AddTransient<IUpdateableRepository<ScreenModel>>(provider => new ScreenRepository(connectionString));
            services.AddTransient<IScreenService, ScreenService>();

            services.AddTransient<LoginForm>();
            services.AddTransient<MainForm>();
            services.AddTransient<RegisterForm>();
            services.AddTransient<EditScreenForm>();
            services.AddTransient<AddScreenForm>();


            using (var serviceProvider = services.BuildServiceProvider())
            {
                var loginForm = serviceProvider.GetRequiredService<LoginForm>();
                Application.Run(loginForm);
            }
        }
    }
}
