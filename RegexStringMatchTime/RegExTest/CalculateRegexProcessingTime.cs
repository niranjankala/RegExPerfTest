using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;


namespace RegExTest
{
    public class CalculateRegexProcessingTime
    {
        public void GetRegexTimeCaculations()
        {
            string pattern = "CheckConvexity: Zone=*, Surface=* is non-convex.  ...vertex * to vertex * to vertex *  ...vertex *=[*]  ...vertex *=[*]  ...vertex *=[*]";
            string patternString = pattern;
            patternString = Regex.Escape(patternString);
            if (patternString.Contains("\\*]"))
                patternString = patternString.Replace("\\*]", "\\*\\]");

            string sampleMsgToMatch = "CheckConvexity: Zone=CEILING VOID ZONE STORY 1, Surface=CEILING VOID STAFF BEDROOM:SLAB 2.2 is non-convex.  ...vertex 2 to vertex 3 to vertex 4  ...vertex 2=[-9.59,21.59,3.90]  ...vertex 3=[-6.73,24.45,3.90]  ...vertex 4=[-8.13,25.85,3.90]";
            MatchRegExGreedyPattern(patternString, sampleMsgToMatch);
            MatchRegExWithBoundaryConditon(patternString, sampleMsgToMatch);
            MatchRegExWithLazyPattern(patternString, sampleMsgToMatch);
            MatchRegExWithLazyPattern1(patternString, sampleMsgToMatch);
            MatchStringWithLikeService(pattern, sampleMsgToMatch);

        }

        private void MatchRegExGreedyPattern(string patternString, string sampleMsgToMatch)
        {
            Console.WriteLine("Greedy method");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            patternString = patternString.Replace("\\*", ".*");
            Console.WriteLine(String.Format("Matched:{0}", Regex.IsMatch(sampleMsgToMatch, patternString, RegexOptions.IgnorePatternWhitespace)));
            watch.Stop();
            Console.WriteLine(string.Format("Parse Time:{0:0.########} sec", watch.Elapsed.TotalSeconds));

        }

        private void MatchRegExWithBoundaryConditon(string patternString, string sampleMsgToMatch)
        {
            //It is failing to match becuase msg contain space between variable section of message, 
            // Need to correct it but what should be the conditions to make it generic
            Console.WriteLine("Matching RegEx with Boundary Conditon");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            patternString = patternString.Replace("\\*", "[^ ]*");
            //Console.WriteLine(string.Format("Pattern:{0}", patternString));
            Console.WriteLine(String.Format("Matched:{0}", Regex.IsMatch(sampleMsgToMatch, patternString, RegexOptions.IgnorePatternWhitespace)));
            watch.Stop();
            Console.WriteLine(string.Format("Parse Time:{0:0.########} sec", watch.Elapsed.TotalSeconds));
        }

        private void MatchRegExWithLazyPattern(string patternString, string sampleMsgToMatch)
        {
            //It is failing to match becuase msg contain space between variable section of message, 
            // Need to correct it but what should be the conditions to make it generic
            Console.WriteLine("Matching RegEx with Lazy pattern matching");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            patternString = patternString.Replace("\\*", "[^$]*");
            //Console.WriteLine(string.Format("Pattern:{0}", patternString));
            Console.WriteLine(String.Format("Matched:{0}", Regex.IsMatch(sampleMsgToMatch, patternString, RegexOptions.IgnorePatternWhitespace)));
            watch.Stop();
            Console.WriteLine(string.Format("Parse Time:{0:0.########} sec", watch.Elapsed.TotalSeconds));
        }

        private void MatchRegExWithLazyPattern1(string patternString, string sampleMsgToMatch)
        {
            //It is failing to match becuase msg contain space between variable section of message, 
            // Need to correct it but what should be the conditions to make it generic
            Console.WriteLine("Matching RegEx with Lazy pattern matching 1");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            patternString = patternString.Replace("\\*", ".*?");
            int lastIndexOnVarSymbol = patternString.LastIndexOf(".*?");
            if (lastIndexOnVarSymbol > 3 && patternString.LastIndexOf(".*?") == patternString.Length - 3)
                patternString = patternString.Substring(0, patternString.Length - 1);


            //Console.WriteLine(string.Format("Pattern:{0}", patternString));
            Console.WriteLine(String.Format("Matched:{0}", Regex.IsMatch(sampleMsgToMatch, patternString, RegexOptions.IgnorePatternWhitespace)));
            watch.Stop();
            Console.WriteLine(string.Format("Parse Time:{0:0.########} sec", watch.Elapsed.TotalSeconds));
        }

        private void MatchStringWithLikeService(string patternString, string sampleMsgToMatch)
        {
            //It is failing to match becuase msg contain space between variable section of message, 
            // Need to correct it but what should be the conditions to make it generic
            Console.WriteLine("Using VB Like Service");
            Stopwatch watch = new Stopwatch();
            watch.Start();
            patternString = patternString.Replace("*]", "*\\]");
            patternString = patternString.Replace("[*", "\\[*");
            patternString = patternString.Replace("\\*", ".*?");
            //Console.WriteLine(string.Format("Pattern:{0}", patternString));
            Console.WriteLine(String.Format("Matched:{0}", LikeOperator.LikeString(sampleMsgToMatch, patternString, Microsoft.VisualBasic.CompareMethod.Text)));
            watch.Stop();
            Console.WriteLine(string.Format("Parse Time:{0:0.########} sec", watch.Elapsed.TotalSeconds));
        }
    }
}
