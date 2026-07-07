namespace Ticketing_Screen_Designer.Interfaces.Repositories
{
    public interface IMessageRepository<T> where T : class
    {
        bool Update(int id, T entity);


    }
}
