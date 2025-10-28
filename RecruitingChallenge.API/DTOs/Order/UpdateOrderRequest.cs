using RecruitingChallenge.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecruitingChallenge.API.DTOs.Order
{
    public class UpdateOrderRequest
    {
        [JsonConverter(typeof(JsonStringEnumConverter<EOrderStatus>))]
        public EOrderStatus NewStatus { get; set; }
    }
}
