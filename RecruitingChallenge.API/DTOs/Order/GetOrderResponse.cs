using RecruitingChallenge.Domain.Enums;

namespace RecruitingChallenge.API.DTOs.Order
{
    public class GetOrderResponse
    {
        public int Id { get; set; }
        public string ClientFullName { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderStatus { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemResponse> Items { get; set; } = new List<OrderItemResponse>();

        public static GetOrderResponse FromDomain(Domain.Models.Order order)
        {
            var getOrderResponse = new GetOrderResponse
            {
                Id = order.Id,
                ClientFullName = order.Client.FullName,
                OrderDate = order.EntryDate,
                OrderStatus = order.Status.ToString(),
                TotalAmount = order.TotalAmount
            };

            foreach (var item in order.OrderItems)
            {
                getOrderResponse.Items.Add(new OrderItemResponse()
                {
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
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Subtotal { get; set; }
    }
}
