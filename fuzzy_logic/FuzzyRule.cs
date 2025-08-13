using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fuzzy_logic
{
    public class FuzzyRule
    {
        public string Sensitivity { get; set; }
        public string Amount { get; set; }
        public string Dirtiness { get; set; }

        public string SpinSpeed { get; set; }
        public string Duration { get; set; }
        public string Detergent { get; set; }
    }
}
