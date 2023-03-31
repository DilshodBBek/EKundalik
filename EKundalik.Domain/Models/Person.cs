using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EKundalik.Domain.Models
{
    public abstract class Person 
    {
        public required string FullName { get; set; }
        public DateOnly BirthDate { get; set; } 
        /// <summary>
        /// True = Male
        /// False = Female 
        /// </summary>
        public bool Gender { get; set; } = true;
        

    }
}
