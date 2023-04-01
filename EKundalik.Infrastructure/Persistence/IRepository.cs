namespace EKundalik.Application.Interfaces
{
    public interface IRepository<T> where T : class
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetByIdAsync(int id);

        public Task AddAsync(T obj);
        public Task AddRangeAsync(List<T> obj);

        public Task<bool> UpdateAsync(T entity);
        public Task DeleteAsync(int id);

    }
}
