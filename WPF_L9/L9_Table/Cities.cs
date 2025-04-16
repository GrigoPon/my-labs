using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L9_Table
{
    public class Cities
    {

        public string City { get; set; } = "";
        public int DistanceFromMoscow { get; set; }
        public override string ToString() => $"{City}";
    }
}
