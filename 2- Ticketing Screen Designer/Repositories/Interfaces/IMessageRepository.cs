namespace Ticketing_Screen_Designer.Repositories.Interfaces
{
    public interface IMessageRepository<T> where T : class
    {
        bool Update(int id, T entity);


    }
}
