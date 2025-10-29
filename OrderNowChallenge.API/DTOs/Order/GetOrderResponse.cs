using OrderNowChallenge.Domain.Enums;

namespace OrderNowChallenge.API.DTOs.Order
{
    public class GetOrderResponse
    {
        public int Id { get; set; }
        public string ClientEmail { get; set; }
        public string OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();

        public static GetOrderResponse FromDomain(Domain.Models.Order order)
        {
            var getOrderResponse = new GetOrderResponse
            {
                Id = order.Id,
                ClientEmail = order.Client.Email,
                OrderDate = order.EntryDate.ToString("yyyy-MM-dd"),
                OrderStatus = order.Status.ToString(),
                TotalAmount = order.TotalAmount
            };

            foreach (var item in order.OrderItems)
            {
                getOrderResponse.Items.Add(new OrderItemResponse()
                {
                    Id= item.Id,
                    ProductName = item.ProductName,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    Subtotal = item.Subtotal
                });
            }

            return getOrderResponse;
        }
    }

    public class OrderItemResponse
    { 
        public Guid Id { get; set; }
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
