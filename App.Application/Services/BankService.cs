using App.Application.DTO.Banks;
using App.Application.Interfaces;
using App.Domain.Interfaces;
using App.Domain.Models;
using App.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Data.SqlClient;
namespace App.Application.Services
{
    public class BankService : IBankService
    {
        private readonly IFetchableRepository<BankModel> _fetchRepository;
        private readonly IAddableRepository<BankModel> _addRepository;
        private readonly ILogger<BankModel> _logger;

        public BankService(IFetchableRepository<BankModel> fetchRepository,
            IAddableRepository<BankModel> addRepository, ILogger<BankModel> logger
)
        {
            _fetchRepository = fetchRepository;
            _addRepository = addRepository;
            _logger = logger;
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
                _logger.LogError(ex,
                          ex.Message,
                          ex.Number,
                          request.BankName);

                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                    ex.Message,
                    request.BankName);
                throw;

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
                _logger.LogError(ex,
                          ex.Message,
                          ex.Number,
                          bankId);

                throw;

            }


            catch (Exception ex)
            {
                _logger.LogError(ex,
                    ex.Message,
                    bankId);

                throw;

            }
        }
    }
}

