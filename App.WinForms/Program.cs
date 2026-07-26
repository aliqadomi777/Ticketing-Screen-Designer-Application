using App.Application;
using App.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Templates;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
namespace App.WinForms
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {

            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);

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


            string configPath = Path.Combine(
                AppDomain.CurrentDomain.BaseDirectory,
                "Config",
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
            services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.ClearProviders();
                loggingBuilder.AddSerilog(dispose: true);
            });

            services.AddApplicationServices();
            services.AddInfrastructureServices(connectionString);
            services.AddSingleton<IUiStateService, UiStateService>();
            services.AddTransient<LoginForm>();
            services.AddTransient<MainForm>();
            services.AddTransient<RegisterForm>();
            services.AddTransient<EditScreenForm>();
            services.AddTransient<AddEditButton>();




            using (var serviceProvider = services.BuildServiceProvider())
            {
                try
                {

                    var loginForm = serviceProvider.GetRequiredService<LoginForm>();
                    System.Windows.Forms.Application.Run(loginForm);

                }

                catch (Exception)
                {
                    MessageBox.Show($"An Error occured", "Application Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}
