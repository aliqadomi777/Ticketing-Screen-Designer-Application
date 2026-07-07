
using Ticketing_Screen_Designer.DTO.Banks;
namespace Ticketing_Screen_Designer.Interfaces.Services
{
    public interface IBankService
    {
        int CreateBank(CreateBankRequestDto request);
        BankResponseDto GetBankDetails(int id);
    }
}
