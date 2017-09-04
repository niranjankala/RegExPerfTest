using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace RegExTest
{
    public class Error
    {        
        public bool Resolved { get; set; }
        public string ErrorMessagePattern { get; set; }
        public int SimulationIndex { get; set; }
        public ErrorType ErrorType { get; set; }
        [DisplayName("Possible Resolution")]
        public string PossibleResolution { get; set; }
        [DisplayName("Error Message")]
        public string ErrorMessage { get; set; }
        [DisplayName("Error Count")]
        public int ErrorCount { get; set; }
        public string ErrorCode { get; set; }
        public string SourceType { get; set; }
        public int ErrorIndex { get; set; }
        public string OriginalErrorMessage { get; set; }
        public bool IsErrorLogged { get; set; }

        public List<Error> Errors { get; set; }       
    }

    public class ErrorComparer : IEqualityComparer<Error>
    {
        public bool Equals(Error x, Error y)
        {
            if (Regex.IsMatch(x.ErrorMessage, y.ErrorMessage, System.Text.RegularExpressions.RegexOptions.CultureInvariant)) return true;
            return false;
        }

        public int GetHashCode(Error obj)
        {
            return obj.ErrorMessage.GetHashCode();
        }
    }
}
