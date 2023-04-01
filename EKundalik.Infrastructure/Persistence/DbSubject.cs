using EKundalik.Application.Interfaces;
using EKundalik.Domain.Models;
using Npgsql;

namespace EKundalik.Infrastructure.Persistence
{
    public class DbSubject : IRepository<Subject>
    {
        private readonly string _connectionString = EKundalikDbContext.conString;

        public async Task AddAsync(Subject subject)
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"insert into subject(subject_name) 
                                values (@name)";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@name", subject.SubjectName);

            int res = await cmd.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine(subject.SubjectName + " added succesfully");
            }
        }

        public async Task AddRangeAsync(List<Subject> subjects)
        {
            foreach (Subject subject in subjects)
            {
                await AddAsync(subject);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string deleteQuery = @"delete from subject where subject_id = @id";
            NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection);
            command.Parameters.AddWithValue("@id", id);
            int res = await command.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine("Deleted Succesfully");
            }
            else Console.WriteLine("Deleted faild");
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            ICollection<Subject>? subjects = new List<Subject>();
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string getAllQuery = @"select * from subject";
            NpgsqlCommand command = new NpgsqlCommand(getAllQuery, connection);

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                subjects.Add(new()
                {
                    SubjectId = (int)reader[0],
                    SubjectName = (string)reader[1]
                });
            }
            return subjects;
        }

        public async Task<Subject> GetByIdAsync(int id)
        {
            Subject? subject = null;
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string getAllQuery = @"select * from subject where subject_id = @id";
            NpgsqlCommand command = new NpgsqlCommand(getAllQuery, connection);
            command.Parameters.AddWithValue("@id", id);
            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                subject = new Subject()
                {
                    SubjectId = (int)reader[0],
                    SubjectName = (string)reader[1]
                };
            }
            return subject;
        }

        public async Task<bool> UpdateAsync(Subject entity)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string updateQuery = @"update subject set subject_name = @name where subject_id = @id";
            NpgsqlCommand command = new(updateQuery, connection);
            command.Parameters.AddWithValue("@name", entity.SubjectName);
            command.Parameters.AddWithValue("@id", entity.SubjectId);
            int res = await command.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine(entity.SubjectName + " Update Succesfully");
                return true;
            }
            Console.WriteLine(entity.SubjectName + " update failed");
            return false;
        }
    }
}
