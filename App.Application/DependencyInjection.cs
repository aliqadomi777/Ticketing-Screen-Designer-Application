using App.Application.Interfaces;
using App.Application.Services;
using Microsoft.Extensions.DependencyInjection;
namespace App.Application
{
    public static class DependencyInjection
    {

        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddTransient<IBankService, BankService>();
            services.AddTransient<IScreenService, ScreenService>();
            services.AddTransient<ITicketService, TicketService>();
            services.AddTransient<IMessageService, MessageService>();
            services.AddTransient<IButtonTypeService, ButtonTypeService>();
            services.AddTransient<IServiceTypeService, ServiceTypeService>();
            services.AddTransient<ButtonService>();
            services.AddTransient<IButtonService>(sp => sp.GetRequiredService<ButtonService>());
            services.AddTransient<IAddButtonService>(sp => sp.GetRequiredService<ButtonService>());
            return services;
        }

    }
}
