using _2__Ticketing_Screen_Designer.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Templates;
using System;
using System.Collections.Generic;
using System.IO;
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

            if (!Directory.Exists(logDirectory))
            {
                Directory.CreateDirectory(logDirectory);
            }

            string logFilePath = Path.Combine(logDirectory, "errors.json");

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.File(
                    formatter: new ExpressionTemplate(
                        template: "{ { Timestamp: ToString(@t, 'yyyy-MM-dd HH:mm:ss zzz'), Message: @m, Exception: @x, Parameters: @p } }\n"
                    ),
                    path: logFilePath,
                    rollingInterval: RollingInterval.Day)
                .CreateLogger();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);


            string configPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Database",
                "dbconfig.json");

            if (!File.Exists(configPath))
            {
                MessageBox.Show($"Configuration file not found:\n{configPath}");
                Environment.Exit(1);
            }
            IConfigurationSection connectionSection = null;
            try
            {
                var configuration = new ConfigurationBuilder()
                                    .AddJsonFile(configPath, optional: false)
                                    .Build();

                connectionSection = configuration.GetSection("ConnectionStrings:DefaultConnection");
            }
            catch (Exception)
            {
                MessageBox.Show($"Invalid DBconfig Format",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Environment.Exit(1);
            }


            if (!connectionSection.Exists())
            {
                MessageBox.Show("Connection string to database does not exist",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            string server = connectionSection["Server"];
            string database = connectionSection["Database"];
            string userId = connectionSection["UserId"];
            string password = connectionSection["Password"];
            var missingFields = new List<string>();

            if (string.IsNullOrWhiteSpace(server)) missingFields.Add("Server");
            if (string.IsNullOrWhiteSpace(database)) missingFields.Add("Database");
            if (string.IsNullOrWhiteSpace(userId)) missingFields.Add("UserId");
            if (string.IsNullOrWhiteSpace(password)) missingFields.Add("Password");

            if (missingFields.Count > 0)
            {
                string errorMessage = string.Join(", ", missingFields);
                MessageBox.Show($"Connection string values cannot be empty for the following parameters: {errorMessage}",
                                "Missing Configuration",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            string connectionString = $"Server={server};Database={database};User Id={userId};Password={password};" +
                         $"TrustServerCertificate=True;";
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


            services.AddTransient<IButtonRepository<ButtonModel>>(provider => new ButtonRepository(connectionString));
            services.AddTransient<IAddableRepository<ButtonModel>>(provider => new ButtonRepository(connectionString));
            services.AddTransient<IDeleteableRepository<ButtonModel>>(provider => new ButtonRepository(connectionString));
            services.AddTransient<IListableRepository<ButtonModel>>(provider => new ButtonRepository(connectionString));
            services.AddTransient<IUpdateableRepository<ButtonModel>>(provider => new ButtonRepository(connectionString));
            services.AddTransient<IButtonService, ButtonService>();

            services.AddTransient<IAddableRepository<TicketModel>>(provider => new TicketRepository(connectionString));
            services.AddTransient<IDeleteableRepository<TicketModel>>(provider => new TicketRepository(connectionString));
            services.AddTransient<ITicketRepository<TicketModel>>(provider => new TicketRepository(connectionString));
            services.AddTransient<ITicketService, TicketService>();


            services.AddTransient<IAddableRepository<MessageModel>>(provider => new MessageRepository(connectionString));
            services.AddTransient<IDeleteableRepository<MessageModel>>(provider => new MessageRepository(connectionString));
            services.AddTransient<IUpdateableRepository<MessageModel>>(provider => new MessageRepository(connectionString));
            services.AddTransient<IMessageService, MessageService>();

            services.AddTransient<IGetAllRepository<ButtonTypes>>(provider => new ButtonTypeRepository(connectionString));
            services.AddTransient<IFetchableRepository<ButtonTypes>>(provider => new ButtonTypeRepository(connectionString));
            services.AddTransient<IButtonTypeService, ButtonTypeService>();

            services.AddTransient<IGetAllRepository<ServiceType>>(provider => new ServiceRepository(connectionString));
            services.AddTransient<IFetchableRepository<ServiceType>>(provider => new ServiceRepository(connectionString));
            services.AddTransient<IServiceTypeService, ServiceTypeService>();

            services.AddTransient<LoginForm>();
            services.AddTransient<MainForm>();
            services.AddTransient<RegisterForm>();
            services.AddTransient<EditScreenForm>();
            services.AddTransient<AddScreenForm>();
            services.AddTransient<AddEditButton>();




            using (var serviceProvider = services.BuildServiceProvider())
            {
                try
                {
                    var loginForm = serviceProvider.GetRequiredService<LoginForm>();
                    Application.Run(loginForm);
                }

                catch (Exception)
                {
                    MessageBox.Show($"An Error occured", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
