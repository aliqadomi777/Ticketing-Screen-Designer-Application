
using App.Application.DTO.Banks;
namespace App.Application.Interfaces
{
    public interface IBankService
    {
        int CreateBank(CreateBankRequestDto request);
        BankResponseDto GetBankDetails(int id);
    }
}
