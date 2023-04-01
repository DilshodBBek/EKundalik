namespace EKundalik.Domain.Models
{
    public class StudentTeacher
    {
        public int Id { get; set; }
        public required Student Student { get; set; }
        public required Teacher Teacher { get; set; }
        public required Subject Subject { get; set; }

        public override string ToString()
        {
            return $"Id:{Id},\n Student:{Student},\n Teacher:{Teacher},\n Subject:{Subject}";
        }
    }
}
