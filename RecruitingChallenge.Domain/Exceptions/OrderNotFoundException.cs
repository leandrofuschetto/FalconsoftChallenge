using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.Domain.Exceptions
{
    public class OrderNotFoundException : Exception
    {
        public string Code { get; private set; } = "ORDER_NOT_FOUND";

        public OrderNotFoundException(string message = null)
            : base(message) { }

        public OrderNotFoundException(
            string message,
            Exception ex = null)
            : base(message, ex) { }
    }
}
