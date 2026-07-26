using App.Application.DTO.Tickets;
using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
namespace App.Application.Services
{
    public class TicketService : ITicketService
    {
        private readonly IAddableRepository<TicketModel> _addRepository;
        private readonly IUpdateableRepository<TicketModel> _updateRepository;
        private readonly IDeleteableRepository<TicketModel> _deleteRepository;

        public TicketService(IAddableRepository<TicketModel> addRepository,
            IUpdateableRepository<TicketModel> updateRepository,
            IDeleteableRepository<TicketModel> deleteRepository)
        {
            _addRepository = addRepository;
            _updateRepository = updateRepository;
            _deleteRepository = deleteRepository;
        }
        public int AddTicket(CreateTicketRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            var ticketModel = new TicketModel
            {
                ButtonId = request.ButtonId,
                ServiceId = request.ServiceId
            };
            int generatedId = _addRepository.Add(ticketModel);
            return generatedId;
        }
        public bool UpdateTicket(UpdateTicketRequestDto request)
        {
            ValidationExtensions.ValidateModel(request);
            var ticketModel = new TicketModel
            {
                TicketId = request.TicketId,
                ServiceId = request.ServiceId
            };
            bool isUpdated = _updateRepository.Update(ticketModel);
            return isUpdated;
        }
        public bool DeleteTicket(int ticketId)
        {
            {
                bool isDeleted = _deleteRepository.Delete(ticketId);
                return isDeleted;
            }
        }
    }
}