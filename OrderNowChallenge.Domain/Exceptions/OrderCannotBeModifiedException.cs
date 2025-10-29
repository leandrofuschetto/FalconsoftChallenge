using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderNowChallenge.Domain.Exceptions
{
    public class OrderCannotBeModifiedException : Exception
    {
        public string Code { get; private set; } = "ORDER_CANNOT_BE_MODIFIED";

        public OrderCannotBeModifiedException(string message = null)
            : base(message) { }

        public OrderCannotBeModifiedException(
            string message,
            Exception ex = null)
            : base(message, ex) { }
    }
}
