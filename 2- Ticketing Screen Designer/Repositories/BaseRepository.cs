namespace Ticketing_Screen_Designer.Interfaces.Repositories
{
    public abstract class BaseRepository
    {
        protected readonly string ConnectionString;
        protected BaseRepository(string connectionString)
        {
            ConnectionString = connectionString;
        }

    }
}