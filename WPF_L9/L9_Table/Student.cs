using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using L9_Table;

namespace L9_Table
{
    public class Student
    {
        public string FirstName { get; set; }
        public string SurName { get; set; }
        public string Patronymic { get; set; }
        public DateTime BirthDate { get; set; }
        public double Height { get; set; }
        public string City { get; set; }
        public int DistanceFromMoscow { get; set; }
        public int BirthYear => BirthDate.Year;

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - BirthDate.Year;
                if (BirthDate.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
