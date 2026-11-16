using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomelabAPI.Core.Exceptions
{
    public abstract class AppException : Exception
    {
        public ErrorCategory Category { get; }
        public string ErrorCode { get; }
        public string? Field { get; }
        public string? Title { get; }

        protected AppException(string message, ErrorCategory category, string errorCode, string? field = null, string? title = null)
            : base(message)
        {
            ErrorCode = errorCode;
            Category = category;
            Field = field;
            Title = title;
        }

    }
}
