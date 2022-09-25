
namespace Services.Storages
{
    public interface IStorage<T> 
    {
        public void Add(T item);
        public void Delete(T item);
        public void Update(T item);

    }
}
