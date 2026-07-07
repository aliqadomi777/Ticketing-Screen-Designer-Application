
namespace Ticketing_Screen_Designer.Interfaces.Repositories
{
    public interface IButtonRepository<T> where T : class
    {
        T GetById(int id, int name);


    }
}