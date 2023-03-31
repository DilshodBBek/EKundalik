namespace EKundalik.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetAsync(int id);
        public Task Create(T obj);

        public Task<bool> UpdateAsync(T entity);
        public Task DeleteAsync(int id);

    }
}
