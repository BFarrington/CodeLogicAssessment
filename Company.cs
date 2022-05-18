using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeLogicAssessment
{
    public class Company
    {
        private readonly string name;
        public string Name { get { return name; } }
        public List<MonthLimit> Overall { get; set; }
        public Dictionary<string, List<MonthLimit>> SubLimits { get; set; }
        public Company(string company)
        {
            name = company;
            Overall = new List<MonthLimit>();
            SubLimits = new Dictionary<string, List<MonthLimit>>();
        }
    }
    public class MonthLimit
    {
        public int Month { get; set; }
        public float Limit { get; set; }

        public MonthLimit(int Month, float Limit)
        {
            this.Month = Month;
            this.Limit = Limit;
        }
    }
}