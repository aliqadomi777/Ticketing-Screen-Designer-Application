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
            var bankModel = new BankModel
            {
                BankName = request.BankName
            };
            int generatedId = _addRepository.Add(bankModel);
            return generatedId;
        }
        public BankResponseDto GetBankDetails(int bankId)
        {
            //if (bankId <= 0) throw new ArgumentException("Invalid Bank ID format.", nameof(bankId));

            var bank = _fetchRepository.GetById(bankId);
            if (bank == null) return null;

            return new BankResponseDto
            {
                BankId = bank.BankId,
                BankName = bank.BankName
            };
        }
    }
}

