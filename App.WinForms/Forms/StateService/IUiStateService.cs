public interface IUiStateService
{
    void Set<T>(T state) where T : class;
    T Get<T>() where T : class;
    void Clear<T>() where T : class;
}


