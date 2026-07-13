

namespace Ticketing_Screen_Designer.Interfaces.Repositories
{
    public interface ITicketRepository<T> where T : class
    {
        bool Update(int id, int id1);


    }
}