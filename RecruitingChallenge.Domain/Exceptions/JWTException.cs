using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.Domain.Exceptions
{
    public class JWTException : Exception
    {
        public string Code { get; private set; } = "JWT_EXCEPTION";

        public JWTException(string message = null)
            : base(message) { }

        public JWTException(
            string message,
            Exception ex = null)
            : base(message, ex) { }
    }
}
