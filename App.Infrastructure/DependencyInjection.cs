using App.Domain.Interfaces;
using App.Domain.Models;
using App.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace App.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, string connectionString)
        {

            services.AddTransient(provider => new ScreenRepository(connectionString));
            services.AddTransient<IFetchableRepository<ScreenModel>>(sp => sp.GetRequiredService<ScreenRepository>());
            services.AddTransient<IAddableRepository<ScreenModel>>(sp => sp.GetRequiredService<ScreenRepository>());
            services.AddTransient<IDeleteableRepository<ScreenModel>>(sp => sp.GetRequiredService<ScreenRepository>());
            services.AddTransient<IListableRepository<ScreenModel>>(sp => sp.GetRequiredService<ScreenRepository>());
            services.AddTransient<IUpdateableRepository<ScreenModel>>(sp => sp.GetRequiredService<ScreenRepository>());

            services.AddTransient(provider => new BankRepository(connectionString));
            services.AddTransient<IFetchableRepository<BankModel>>(sp => sp.GetRequiredService<BankRepository>());
            services.AddTransient<IAddableRepository<BankModel>>(sp => sp.GetRequiredService<BankRepository>());

            services.AddTransient(provider => new ButtonRepository(connectionString));
            services.AddTransient<IButtonRepository<ButtonModel>>(sp => sp.GetRequiredService<ButtonRepository>());
            services.AddTransient<IAddableRepository<ButtonModel>>(sp => sp.GetRequiredService<ButtonRepository>());
            services.AddTransient<IDeleteableRepository<ButtonModel>>(sp => sp.GetRequiredService<ButtonRepository>());
            services.AddTransient<IListableRepository<ButtonModel>>(sp => sp.GetRequiredService<ButtonRepository>());
            services.AddTransient<IUpdateableRepository<ButtonModel>>(sp => sp.GetRequiredService<ButtonRepository>());

            services.AddTransient(provider => new TicketRepository(connectionString));
            services.AddTransient<IAddableRepository<TicketModel>>(sp => sp.GetRequiredService<TicketRepository>());
            services.AddTransient<IDeleteableRepository<TicketModel>>(sp => sp.GetRequiredService<TicketRepository>());
            services.AddTransient<ITicketRepository<TicketModel>>(sp => sp.GetRequiredService<TicketRepository>());

            services.AddTransient(provider => new MessageRepository(connectionString));
            services.AddTransient<IAddableRepository<MessageModel>>(sp => sp.GetRequiredService<MessageRepository>());
            services.AddTransient<IDeleteableRepository<MessageModel>>(sp => sp.GetRequiredService<MessageRepository>());
            services.AddTransient<IUpdateableRepository<MessageModel>>(sp => sp.GetRequiredService<MessageRepository>());

            services.AddTransient(provider => new ButtonTypeRepository(connectionString));
            services.AddTransient<IGetAllRepository<ButtonTypes>>(sp => sp.GetRequiredService<ButtonTypeRepository>());
            services.AddTransient<IFetchableRepository<ButtonTypes>>(sp => sp.GetRequiredService<ButtonTypeRepository>());

            services.AddTransient(provider => new ServiceRepository(connectionString));
            services.AddTransient<IGetAllRepository<ServiceType>>(sp => sp.GetRequiredService<ServiceRepository>());
            services.AddTransient<IFetchableRepository<ServiceType>>(sp => sp.GetRequiredService<ServiceRepository>());

            return services;
        }
    }
}
