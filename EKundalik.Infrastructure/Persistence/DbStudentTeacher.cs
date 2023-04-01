using EKundalik.Application.Interfaces;
using EKundalik.Domain.Models;
using Npgsql;

namespace EKundalik.Infrastructure.Persistence
{
    public class DbStudentTeacher : IRepository<StudentTeacher>
    {
        private readonly string _connectionString = EKundalikDbContext.conString;

        public async Task AddAsync(StudentTeacher obj)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string cmdText = @"insert into student_teacher(student_id, teacher_id, subject_id)
                               values(@s_id, @t_id, @sub_id)";
            NpgsqlCommand command = new NpgsqlCommand(cmdText, connection);
            command.Parameters.AddWithValue("@s_id", obj.Student.StudentId);
            command.Parameters.AddWithValue("@t_id", obj.Teacher.TeacherId);
            command.Parameters.AddWithValue("@sub_id", obj.Subject.SubjectId);

            int res = await command.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine("Added Succesfully");
            }
        }

        public async Task AddRangeAsync(List<StudentTeacher> studentTeachers)
        {
            foreach (StudentTeacher studentTeacher in studentTeachers)
            {
                await AddAsync(studentTeacher);
            }
        }

        public async Task DeleteAsync(int id)
        {
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string deleteQuery = @"delete from student_teacher where id = @id";
            NpgsqlCommand command = new NpgsqlCommand(deleteQuery, connection);
            command.Parameters.AddWithValue("@id", id);
            int res = await command.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine("Deleted Succesfully");
            }
            else Console.WriteLine("Deleted faild");
        }

        public async Task<IEnumerable<StudentTeacher>> GetAllAsync()
        {
            ICollection<StudentTeacher>? studentTeachers = new List<StudentTeacher>();
            using NpgsqlConnection connection = new NpgsqlConnection(_connectionString);
            connection.Open();
            string getAllQuery = @"select * from student_teacher";
            NpgsqlCommand command = new NpgsqlCommand(getAllQuery, connection);

            NpgsqlDataReader reader = await command.ExecuteReaderAsync();
            while (reader.Read())
            {
                studentTeachers.Add(new()
                {
                    Id = (int)reader[0],
                    Student = await new DbStudents().GetByIdAsync((int)reader[1]),
                    Teacher = await new DbTeacher().GetByIdAsync((int)reader[2]),
                    Subject = await new DbSubject().GetByIdAsync((int)reader[3])
                });
            }
            return studentTeachers;
        }

        public async Task<StudentTeacher> GetByIdAsync(int id)
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"select * from student_teacher where id=@id";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@id", id);

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();

            StudentTeacher student_teachers = new()
            {
                Id = (int)reader["id"],
                Student = await new DbStudents().GetByIdAsync((int)reader["student_id"]),
                Teacher = await new DbTeacher().GetByIdAsync((int)reader["teacher_id"]),
                Subject = await new DbSubject().GetByIdAsync((int)reader["subject_id"])
            };

            return student_teachers;
        }

        public async Task<bool> UpdateAsync(StudentTeacher entity)
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"update student_teacher set student_id=@StudentId, teacher_id=@TeacherId, subject_id=@SubjectId
                               where  id= @id;";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@StudentId", entity.Student.StudentId);
            cmd.Parameters.AddWithValue("@TeacherId", entity.Teacher.TeacherId);
            cmd.Parameters.AddWithValue("@SubjectId", entity.Subject.SubjectId);
            cmd.Parameters.AddWithValue("@id", entity.Id);

            int res = await cmd.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine(" updated succesfully");
                return true;
            }
            Console.WriteLine(" update failed");
            return false;
        }
    }
}
