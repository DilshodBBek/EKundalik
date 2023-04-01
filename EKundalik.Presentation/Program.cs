using EKundalik.Domain.Models;
using EKundalik.Infrastructure.Persistence;

namespace EKundalik.Presentation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //  Start();
            //  CheckTeacher();
            TestGrade();
            //TestSubject();

            // TestStudentTeacher();
            Console.ReadKey();
        }

        private static async void TestStudentTeacher()
        {
            EKundalikDbContext _dbContext = new();

            List<StudentTeacher> studentTeachers = new()
            {
                new()
                {
                    //"Tom Kerry"
                    Student=await _dbContext.Students.GetByIdAsync(32),

                    //"Physics"
                    Subject=await _dbContext.Subjects.GetByIdAsync(3),

                    //"Jamshid"
                    Teacher= await _dbContext.Teachers.GetByIdAsync(4)
                },
                 new()
                {
                    //"Jim Kerry"
                    Student=await _dbContext.Students.GetByIdAsync(34),

                    //"Math"
                    Subject=await _dbContext.Subjects.GetByIdAsync(2),

                    //""John""
                    Teacher= await _dbContext.Teachers.GetByIdAsync(3)
                },
                  new()
                {
                    //"Jerry Kerry"
                    Student=await _dbContext.Students.GetByIdAsync(33),

                    //"History"
                    Subject=await _dbContext.Subjects.GetByIdAsync(4),

                    //""Dilshod""
                    Teacher= await _dbContext.Teachers.GetByIdAsync(2)
                }

            };

            await _dbContext.StudentTeachers.AddAsync(studentTeachers[0]);

            await _dbContext.StudentTeachers.AddRangeAsync(studentTeachers);
            List<StudentTeacher> res = (List<StudentTeacher>)await _dbContext.StudentTeachers.GetAllAsync();
            foreach (StudentTeacher item in res)
            {
                Console.WriteLine(item);
            }
        }

        private static async void TestSubject()
        {
            EKundalikDbContext _dbContext = new();

            List<Subject> subjects = new()
            {
               new(){SubjectName="Math"},
               new(){SubjectName="Biology"},
               new(){SubjectName="History"}
            };

            //await _dbContext.Subjects.AddAsync(subjects[0]);
            //await _dbContext.Subjects.AddRangeAsync(subjects);

            List<Subject> resSubjects = (List<Subject>)await _dbContext.Subjects.GetAllAsync();

            foreach (Subject subject in resSubjects)
            {
                Console.WriteLine(subject);
            }
            //Subject s = await _dbContext.Subjects.GetByIdAsync(3);
            //s.SubjectName = "Physics";
            //await _dbContext.Subjects.UpdateAsync(s);
            await _dbContext.Subjects.DeleteAsync(_dbContext.Subjects.GetByIdAsync(1).Id);

        }

        private static async void TestGrade()
        {
            EKundalikDbContext _dbContext = new();

            List<Grade> grades = new()
            {

                 new()
                {
                    StudentTeacher= await _dbContext.StudentTeachers.GetByIdAsync(2),
                    Date=DateTime.Now.AddDays(-1),
                    GradeEnum=Domain.States.GradeEnum.F
                },
                  new()
                {
                    StudentTeacher= await _dbContext.StudentTeachers.GetByIdAsync(3),
                    Date=DateTime.Now,
                    GradeEnum=Domain.States.GradeEnum.A
                },
                    new()
                {
                    StudentTeacher= await _dbContext.StudentTeachers.GetByIdAsync(4),
                    Date=DateTime.Now.AddDays(-2),
                    GradeEnum=Domain.States.GradeEnum.B
                }
            };

           await _dbContext.Grades.AddAsync(
                new()
                {
                    StudentTeacher = await _dbContext.StudentTeachers.GetByIdAsync(2),
                    Date = DateTime.Now,
                    GradeEnum = Domain.States.GradeEnum.D
                });
            await _dbContext.Grades.AddRangeAsync(grades);

            List<Grade> res =(List<Grade>)await _dbContext.Grades.GetAllAsync();

            foreach (var item in res)
            {
                Console.WriteLine(item);
            }

        }

        private static async void CheckTeacher()
        {
            EKundalikDbContext _dbContext = new();

            //List<Teacher> teachers = new()
            //{
            //    new Teacher()
            //    {
            //        FullName="Dilshod",
            //        BirthDate=DateTime.Now
            //    },
            //     new Teacher()
            //    {
            //        FullName="Xondamir",
            //        BirthDate=DateTime.Now
            //    },
            //    new Teacher()
            //    {
            //        FullName="Jamshid",
            //        BirthDate=DateTime.Now
            //    }
            //};

            //await _dbContext.Teachers.AddAsync(teachers[0]);
            //await _dbContext.Teachers.AddRangeAsync(teachers);

            List<Teacher> result = (List<Teacher>)await _dbContext.Teachers.GetAllAsync();


            foreach (var item in result)
            {
                Console.WriteLine(item);
            }
            int max = result.Max(x => x.FullName.Length);
            Teacher? teacher = result.FirstOrDefault(x => x.FullName.Length == max);
            teacher.FullName = "John";
            //await _dbContext.Teachers.UpdateAsync(teacher);

            Teacher? Dilshod = result.Distinct().FirstOrDefault();

            await _dbContext.Teachers.DeleteAsync(Dilshod.TeacherId);

        }

        private static async Task Start()
        {
            try
            {
                EKundalikDbContext _dbContext = new();
                _dbContext.CreateDb();

                Student Jim = new()
                {
                    FullName = "Jim Kerry",
                    BirthDate = DateTime.Now.AddYears(-25),
                    Gender = true
                };

                Student Tom = new()
                {
                    FullName = "Tom Kerry",
                    BirthDate = DateTime.Now.AddYears(-30),
                    Gender = true
                };

                Student Jerry = new()
                {
                    FullName = "Jerry Kerry",
                    BirthDate = DateTime.Now.AddYears(-40),
                    Gender = false
                };
                List<Student> students = new() { Tom, Jerry, Jim };
                //await _dbContext.Students.AddAsync(Jim);
                await _dbContext.Students.AddRangeAsync(students);

                //Console.WriteLine("GetAllAsync");
                //List<Student> result = (List<Student>)await _dbContext.Students.GetAllAsync();

                //foreach (Student student in result)
                //{
                //    //Console.WriteLine("Deleted:\n" + !_dbContext.Students.DeleteAsync(student.StudentId).IsFaulted);

                //}
                //Jim =await _dbContext.Students.GetByIdAsync(5);
                //Jim.FullName = "Tomas Edison";

                //bool IsUpdated = await _dbContext.Students.UpdateAsync(Jim);
                //Console.WriteLine("IsUpdated:" + IsUpdated);

                //Console.WriteLine("GetByIdAsync:\n" +await _dbContext.Students.GetByIdAsync(Jim.StudentId));

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }
}