using Serilog;
using System;
using System.Data.SqlClient;
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

                return _addRepository.Add(bankModel);
            }
            catch (DuplicateRecordException)
            {
                throw;
            }

            catch (SqlException ex)
            {
                Log.Error(ex,
                          "SQL error {SqlErrorNumber} while creating bank '{BankName}'.",
                          ex.Number,
                          request.BankName);

                throw new DataAccessException(
                    "A database error occurred while creating the bank.",
                    ex);
            }
            catch (Exception ex)
            {
                Log.Error(ex,
                    "Unexpected error while creating bank '{BankName}'.",
                    request.BankName);

                throw new DataAccessException(
                    "An unexpected error occurred while creating the bank.",
                    ex);
            }
        }
        public BankResponseDto GetBankDetails(int bankId)
        {
            if (bankId <= 0)
            {
                throw new ArgumentException(
                    "Bank ID must be a positive integer.",
                    nameof(bankId));
            }

            try
            {
                var bank = _fetchRepository.GetById(bankId);

                if (bank == null)
                    return null;

                return new BankResponseDto
                {
                    BankId = bank.BankId,
                    BankName = bank.BankName
                };
            }
            catch (SqlException ex)
            {
                Log.Error(ex,
                          "SQL error {SqlErrorNumber} while retrieving bank with ID: '{bankId}'.",
                          ex.Number,
                          bankId);

                throw new DataAccessException(
                    "A database error occurred while retrieving bank info",
                    ex);
            }


            catch (Exception ex)
            {
                Log.Error(ex,
                    "Failed business operation 'GetBankDetails' for BankId {BankId}.",
                    bankId);

                throw new DataAccessException(
                    $"Could not retrieve bank {bankId}.",
                    ex);
            }
        }
    }
}

