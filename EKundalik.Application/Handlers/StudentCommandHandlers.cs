using EKundalik.Application.Interfaces;
using EKundalik.Domain.Models;
using EKundalik.Infrastructure.Persistence;

namespace EKundalik.Application.Handlers
{
    public class StudentCommandHandlers : IRepository<Student>
    {
        private readonly EKundalikDbContext _db = new();

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            return await _db.Students.GetAllAsync();
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            return await _db.Students.GetByIdAsync(id);

        }

        public async Task AddAsync(Student obj)
        {
            await _db.Students.AddAsync(obj);

        }

        public async Task<bool> UpdateAsync(Student entity)
        {
            return await _db.Students.UpdateAsync(entity);

        }

        public async Task DeleteAsync(int id)
        {
            await _db.Students.DeleteAsync(id);

        }

        public async Task AddRangeAsync(List<Student> students)
        {
            await _db.Students.AddRangeAsync(students);

        }
    }
}
