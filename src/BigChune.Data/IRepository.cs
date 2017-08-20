
namespace BigChune.Data
{
    public interface IRepository<in T>
    {
        void Add(T item);
        void Remove(T item);
        void Update(T item);
    }
}
