using EKundalik.Application.Interfaces;
using EKundalik.Domain.Models;
using EKundalik.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKundalik.Application.Handlers
{
    public class StudentCommandHandlers:IRepository<Student>
    {
        private readonly EKundalikDbContext _db = new();

        public Task<IEnumerable<Student>> GetAllAsync()
        {
            _db.Students.GetAllAsync();
        }

        public Task<Student> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task Create(Student obj)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAsync(Student entity)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(int id)
        {
            throw new NotImplementedException();
        }
    }
}
