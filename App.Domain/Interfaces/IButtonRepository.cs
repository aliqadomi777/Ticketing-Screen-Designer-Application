
namespace App.Domain.Interfaces
{
    public interface IButtonRepository<T> where T : class
    {
        T GetById(int id, int name);


    }
}