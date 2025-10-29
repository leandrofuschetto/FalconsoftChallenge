using RecruitingChallenge.API.Filters;
using RecruitingChallenge.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace RecruitingChallenge.API.DTOs.Order
{
    public class UpdateOrderRequest
    {
        [EnumValidationAttribute(typeof(EOrderStatus))]
        public EOrderStatus NewStatus { get; set; }
    }
}
