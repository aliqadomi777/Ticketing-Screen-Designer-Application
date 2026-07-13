using Serilog;
using System;
using Ticketing_Screen_Designer.DTO.Banks;
using Ticketing_Screen_Designer.Interfaces.Repositories;
using Ticketing_Screen_Designer.Interfaces.Services;
using Ticketing_Screen_Designer.Models;
using Ticketing_Screen_Designer.Utils;
namespace Ticketing_Screen_Designer.Services
{
    public class BankService : IBankService
    {
        private readonly IFetchableRepository<BankModel> _fetchRepository;
        private readonly IAddableRepository<BankModel> _addRepository;
        public BankService(IFetchableRepository<BankModel> fetchRepository, IAddableRepository<BankModel> addRepository)
        {
            _fetchRepository = fetchRepository;
            _addRepository = addRepository;
        }
        public int CreateBank(CreateBankRequestDto request)
        {

            ValidationExtensions.ValidateModel(request);
            try
            {
                var bankModel = new BankModel
                {
                    BankName = request.BankName
                };
                int generatedId = _addRepository.Add(bankModel);
                return generatedId;
            }

            catch (Exception ex) when (!(ex is DuplicateRecordException))
            {
                Log.Error(ex, "Unexpected failure during bank creation process.");
                throw new DataAccessException("An unexpected structural error occurred. Please try again later.", ex);
            }
        }
        public BankResponseDto GetBankDetails(int bankId)
        {
            if (bankId <= 0)
            {
                throw new ArgumentException("Bank ID must be a positive non-zero integer.", nameof(bankId));
            }
            try
            {
                var bank = _fetchRepository.GetById(bankId);

                return bank == null ? null : new BankResponseDto
                {
                    BankId = bank.BankId,
                    BankName = bank.BankName
                };

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Failed executing query for Bank with ID: {bankId}", bankId);
                throw new DataAccessException($"Could not retrieve profile records for Bank ID {bankId}.", ex);
            }

        }
    }
}

