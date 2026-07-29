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

            IConfiguration configuration = null;
            try
            {
                configuration = new ConfigurationBuilder()
                                    .AddJsonFile(configPath, optional: false)
                                    .Build();
            }
            catch (Exception)
            {
                MessageBox.Show($"Invalid DBconfig Format",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            string authMode = configuration["AuthenticationMode"];
            if (string.IsNullOrWhiteSpace(authMode))
            {
                MessageBox.Show("AuthenticationMode is missing from the configuration file.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            var connectionSection = configuration.GetSection("ConnectionStrings");
            if (!connectionSection.Exists())
            {
                MessageBox.Show("ConnectionStrings section does not exist",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            string server = connectionSection["Server"];
            string database = connectionSection["Database"];
            string trustCert = connectionSection["TrustServerCertificate"];


            var missingFields = new List<string>();

            if (string.IsNullOrWhiteSpace(trustCert))
            {
                missingFields.Add("TrustServerCertificate");
            }
            else
            {
                if (trustCert != "False" && trustCert != "True")
                {
                    MessageBox.Show($"TrustServerCertificate must be filled with True or False only", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Environment.Exit(1);
                }
            }
            if (string.IsNullOrWhiteSpace(server))
            {
                missingFields.Add("Server");
            }
            if (string.IsNullOrWhiteSpace(database))
            {
                missingFields.Add("Database");
            }

            string connectionString = "";

            if (authMode == "Windows Authentication")
            {
                var winSection = connectionSection.GetSection("WindowsAuthentication");
                string trustedConnection = winSection["Trusted_Connection"];

                if (missingFields.Count == 0)
                {
                    connectionString = $"Server={server};Database={database};Trusted_Connection=True;TrustServerCertificate={trustCert};";
                }
            }
            else if (authMode == "Server Authentication")
            {
                var sqlSection = connectionSection.GetSection("SqlServerAuthentication");
                string userId = sqlSection["UserId"];
                string password = sqlSection["Password"];

                if (string.IsNullOrWhiteSpace(userId))
                {
                    missingFields.Add("SqlServerAuthentication:UserId");
                }
                if (string.IsNullOrWhiteSpace(password))
                {
                    missingFields.Add("SqlServerAuthentication:Password");
                }

                if (missingFields.Count == 0)
                {
                    connectionString = $"Server={server};Database={database};User ID={userId};Password={password};TrustServerCertificate={trustCert};";
                }
            }
            else
            {
                MessageBox.Show($"Unsupported AuthenticationMode: '{authMode}'. Use 'Windows Authentication' or 'Server Authentication'.",
                                "Invalid Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }

            if (missingFields.Count > 0)
            {
                string errorMessage = string.Join(", ", missingFields);
                MessageBox.Show($"The following parameters are required and cannot be empty: {errorMessage}",
                                "Missing Configuration", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Environment.Exit(1);
            }



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
