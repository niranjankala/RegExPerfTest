using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RegExTest
{
    class Program
    {
        static void Main(string[] args)
        {
            CalculateRegexProcessingTime regexTestCal = new CalculateRegexProcessingTime();
            regexTestCal.GetRegexTimeCaculations();
            Console.ReadKey();
           
        }
    }
}
