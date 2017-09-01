using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RegExTest
{
    public enum ErrorType
    {
        None = 0,
        Warning = 1,
        Fatal = 2,
        Severe = 4,
        Error = 8,
        All = Warning | Fatal | Severe | Error

    }
}
