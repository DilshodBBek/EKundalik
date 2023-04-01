using EKundalik.Domain.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKundalik.Domain.Models
{
   public class Grade
    {
        public int GradeId { get; set; }
        public required StudentTeacher StudentTeacher { get; set; }
        public GradeEnum GradeEnum { get; set; }
        public DateTime  Date { get; set; }
        public override string ToString()
        {
            return $"GradeId:{GradeId},\n StudentTeacher:{StudentTeacher},\n GradeEnum:{GradeEnum},\n Date:{Date}";
        }
    }
}
