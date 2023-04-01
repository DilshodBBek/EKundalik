using EKundalik.Application.Interfaces;
using EKundalik.Domain.Models;
using EKundalik.Domain.States;
using Npgsql;

namespace EKundalik.Infrastructure.Persistence
{
    public class DbGrade : IRepository<Grade>
    {
        private readonly string _connectionString = EKundalikDbContext.conString;

        public async Task AddAsync(Grade grade)
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"insert into grade(grade_rate,grade_date, student_teacher_id) 
                    values (@name, @birth_date, @gender)";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@grade_rate", grade.GradeEnum);
            cmd.Parameters.AddWithValue("@grade_date", grade.Date);
            cmd.Parameters.AddWithValue("@student_teacher_id", grade.StudentTeacher.Id);

            int res = await cmd.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine("grade added succesfully");
            }
        }

        public async Task AddRangeAsync(List<Grade> grades)
        {
            foreach (Grade grade in grades)
            {
                await AddAsync(grade);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"delete from grade where grade_id=@id";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@id", id);

            int res = await cmd.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine("Deleted succesfully");
            }
            else Console.WriteLine("Deleted failed");
        }

        public async Task<IEnumerable<Grade>> GetAllAsync()
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"select * from grade";
            NpgsqlCommand cmd = new(cmdText, connection);

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            ICollection<Grade> grades = new List<Grade>();
            while (reader.Read())
            {
                grades.Add(new()
                {
                    GradeId = (int)reader["grade_id"],
                    GradeEnum = Enum.Parse<GradeEnum>(reader["grade_rate"]?.ToString()),
                    StudentTeacher = (StudentTeacher)reader["student_teacher_id"],
                    Date = (DateTime)reader["grade_date"]
                });
            }
            return grades;
        }

        public async Task<Grade> GetByIdAsync(int id)
        {
            using NpgsqlConnection connection = new(_connectionString);
            await connection.OpenAsync();
            string cmdText = @"select * from grade where grade_id=@id";
            using NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@id", id);

            using NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            if (await reader.ReadAsync())
            {
                Grade grade = new()
                {
                    GradeId = (int)reader["grade_id"],
                    GradeEnum = Enum.Parse<GradeEnum>(reader["grade_rate"].ToString()),
                    StudentTeacher = (StudentTeacher)reader["student_teacher_id"],
                    Date = (DateTime)reader["grade_date"]
                };
                return grade;
            }
            return null;
        }

        public async Task<bool> UpdateAsync(Grade entity)
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"update grade set grade_rate=@grade_rate, grade_date=@grade_date, student_teacher_id=@student_teacher_id
                               where teacher_id = @id;";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@grade_rate", entity.GradeEnum);
            cmd.Parameters.AddWithValue("@grade_date", entity.Date);
            cmd.Parameters.AddWithValue("@student_teacher_id", entity.StudentTeacher.Id);
            int res = await cmd.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine("grade updated succesfully");
                return true;
            }
            Console.WriteLine("grade update failed");
            return false;
        }
    }
}
