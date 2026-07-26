

namespace App.Domain.Interfaces
{
    public interface ITicketRepository<T> where T : class
    {
        bool Update(int id, int id1);


    }
}