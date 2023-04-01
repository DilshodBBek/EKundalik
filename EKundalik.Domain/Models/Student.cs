using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKundalik.Domain.Models
{
    public class Student : Person
    {
        public int StudentId { get; set; }
        public override string ToString()
        {
            return $"StudentId:{StudentId}, FullName:{FullName}, BirthDate:{BirthDate}, Gender:{Gender}";
        }



    }
}
