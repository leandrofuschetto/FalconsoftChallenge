using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderNowChallenge.Domain.Exceptions
{
    public class OrderItemNotFoundException : Exception
    {
        public string Code { get; private set; } = "ORDER_ITEM_NOT_FOUND";

        public OrderItemNotFoundException(string message = null)
            : base(message) { }

        public OrderItemNotFoundException(
            string message,
            Exception ex = null)
            : base(message, ex) { }
    }
}
