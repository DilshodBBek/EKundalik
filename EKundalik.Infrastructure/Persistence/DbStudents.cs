using EKundalik.Application.Interfaces;
using EKundalik.Domain.Models;
using Npgsql;
using System.Data;

namespace EKundalik.Infrastructure.Persistence
{
    public class DbStudents : IRepository<Student>
    {
        private readonly string _connectionString = EKundalikDbContext.conString;

        public async Task AddAsync(Student obj)
        {
            try
            {
                using NpgsqlConnection connection = new(_connectionString);
                connection.Open();
                string cmdText = @"insert into student(student_name,birth_date, gender) 
                    values (@name, @birth_date, @gender)";
                NpgsqlCommand cmd = new(cmdText, connection);
                cmd.Parameters.AddWithValue("@name", obj.FullName);
                cmd.Parameters.AddWithValue("@birth_date", obj.BirthDate);
                cmd.Parameters.AddWithValue("@gender", obj.Gender);

                int res = await cmd.ExecuteNonQueryAsync();
                if (res > 0)
                {
                    Console.WriteLine(obj.FullName + " added succesfully");
                }
               // return Task.CompletedTask;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
               // return Task.CompletedTask;
            }
        }

        public Task AddRangeAsync(List<Student> students)
        {
            DataColumn name = new()
            {
                ColumnName = "student_name",
                DataType = typeof(string)
            };
            DataColumn birth_date = new()
            {
                ColumnName = "birth_date",
                DataType = typeof(DateTime)
            };
            DataColumn gender = new()
            {
                ColumnName = "gender",
                DataType = typeof(bool)
            };

            DataTable dataTable = new("student");
            dataTable.Columns.Add(name);
            dataTable.Columns.Add(birth_date);
            dataTable.Columns.Add(gender);

            foreach (Student student in students)
            {
                DataRow row = dataTable.NewRow();

                row["student_name"] = student.FullName;
                row["birth_date"] = student.BirthDate;
                row["gender"] = student.Gender;

                dataTable.Rows.Add(row);
            }

            using NpgsqlConnection connection = new(_connectionString);
            NpgsqlDataAdapter dataAdapter = new()
            {
                InsertCommand = new NpgsqlCommand(
                "INSERT INTO student (student_name, birth_date, gender) VALUES (@name, @date, @gender)",
                connection)
            };
            dataAdapter.InsertCommand.Parameters.Add("@name", NpgsqlTypes.NpgsqlDbType.Varchar, 50, "student_name");
            dataAdapter.InsertCommand.Parameters.Add("@date", NpgsqlTypes.NpgsqlDbType.Date, 0, "birth_date");
            dataAdapter.InsertCommand.Parameters.Add("@gender", NpgsqlTypes.NpgsqlDbType.Boolean, 0, "gender");

            dataAdapter.Update(dataTable);
            return Task.CompletedTask;
        }

        public async Task DeleteAsync(int id)
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"delete from student where student_id=@id";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@id", id);

            int res = await cmd.ExecuteNonQueryAsync();
            if (res > 0)
            {
                Console.WriteLine("Deleted succesfully");
            }
            else Console.WriteLine("Deleted failed");
        }

        public async Task<IEnumerable<Student>> GetAllAsync()
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"select * from student";
            NpgsqlCommand cmd = new(cmdText, connection);

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            ICollection<Student> students = new List<Student>();
            while (reader.Read())
            {
                students.Add(new()
                {
                    StudentId = (int)reader["student_id"],
                    FullName = reader["student_name"]?.ToString(),
                    BirthDate = DateTime.Parse(reader["birth_date"]?.ToString()),
                    Gender = (bool)reader["gender"]
                });
            }
            return students;
        }

        public async Task<Student> GetByIdAsync(int id)
        {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"select * from student where student_id=@id";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@id", id);

            NpgsqlDataReader reader = await cmd.ExecuteReaderAsync();
            Student student = null;
            while (reader.Read())
            {
                student = new()
                {
                    StudentId = (int)reader["student_id"],
                    FullName = reader["student_name"]?.ToString(),
                    BirthDate = DateTime.Parse(reader["birth_date"]?.ToString()),
                    Gender = (bool)reader["gender"]
                };
            }
            return student;

        }

        public async Task<bool> UpdateAsync(Student entity)
        {
            try
            {
            using NpgsqlConnection connection = new(_connectionString);
            connection.Open();
            string cmdText = @"update student set student_name=@name, birth_date=@birth_date, gender=@gender
                               where student_id = @id;";
            NpgsqlCommand cmd = new(cmdText, connection);
            cmd.Parameters.AddWithValue("@name", entity.FullName);
            cmd.Parameters.AddWithValue("@birth_date", entity.BirthDate);
            cmd.Parameters.AddWithValue("@gender", entity.Gender);
            cmd.Parameters.AddWithValue("@id", entity.StudentId);

            int res = cmd.ExecuteNonQuery();
            if (res > 0)
            {
                Console.WriteLine(entity.FullName + " updated succesfully");
                return true;
            }
            Console.WriteLine(entity.FullName + " update failed");
            return false;
            }
            catch (Exception e)
            {
                Console.WriteLine("Update Time:"+e.Message);
                return false;
            }
        }
    }
}
