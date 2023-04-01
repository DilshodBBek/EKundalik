namespace EKundalik.Domain.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public required string SubjectName { get; set; }
        public override string ToString()
        {
            return $"SubjectId:{SubjectId}, SubjectName:{SubjectName}";
        }
    }
}