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
        public required StudentTeacher StudentTeacherId { get; set; }
        public GradeEnum GradeEnum { get; set; }
        public DateTime  Date { get; set; }
    }
}
