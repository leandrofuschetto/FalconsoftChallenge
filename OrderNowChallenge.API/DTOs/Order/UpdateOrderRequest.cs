using OrderNowChallenge.API.Filters;
using OrderNowChallenge.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace OrderNowChallenge.API.DTOs.Order
{
    public class UpdateOrderRequest
    {
        [EnumValidationAttribute(typeof(EOrderStatus))]
        public EOrderStatus NewStatus { get; set; }
    }
}
