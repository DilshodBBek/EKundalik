using Npgsql;
namespace EKundalik.Infrastructure.Persistence
{
    public class EKundalikDbContext
    {
        public static string conString = File.ReadAllText(@"..\..\..\..\..\EKundalik.Presentation\EKundalik.Infrastructure\Appconfig.txt");

        public static void InitializeTables()
        {
            try
            {
                using NpgsqlConnection connection = new NpgsqlConnection(conString);
                connection.Open();
                string query = @"CREATE TABLE student
                                (
                                    student_id serial NOT NULL,
                                    student_name character varying(50) NOT NULL,
                                    birth_date date,
                                    gender boolean DEFAULT true,
                                    PRIMARY KEY (student_id)
                                );
                                CREATE TABLE teacher
                                (
                                    teacher_id serial NOT NULL,
                                    teacher_name character varying(50) NOT NULL,
                                    birth_date date,
                                    gender boolean DEFAULT true,
                                    PRIMARY KEY (teacher_id)
                                );
                                CREATE TABLE subject
                                (
                                    subject_id serial NOT NULL,
                                    subject_name character varying(50) NOT NULL,
                                    PRIMARY KEY (subject_id)
                                );
                                CREATE TABLE student_teacher
                                (
                                    id serial NOT NULL,
                                    student_id integer references student(student_id) NOT NULL,
                                    teacher_id integer references teacher(teacher_id) NOT NULL,
                                    subject_id integer references subject(subject_id) NOT NULL,
                                    PRIMARY KEY (id)
                                );
                                CREATE TABLE grade
                                (
                                    grade_id serial NOT NULL,
                                    grade_rate integer NOT NULL,
                                    grade_date date,
                                    student_teacher_id integer references student_teacher(id) NOT NULL,
                                    PRIMARY KEY (grade_id)
                                );";

                NpgsqlCommand command = connection.CreateCommand();
                connection.Close();
            };
        }
        public static void CreateDb()
        {
            try
            {
                using NpgsqlConnection connection = new NpgsqlConnection(conString);
                connection.Open();
                connection.Close();
            }
            catch (NpgsqlException e)
            {
                if (e.Message.Contains("does not exist", StringComparison.OrdinalIgnoreCase))
                {
                    conString = conString.Replace("ekundalik", "postgres");
                    using NpgsqlConnection connection = new(conString);
                    connection.Open();
                    string query = "create database ekundalik";
                    NpgsqlCommand command = new(query, connection);
                    command.ExecuteNonQuery();
                    InitializeTables();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

        }

        public DbStudents Students { get; set; }
    }
}
