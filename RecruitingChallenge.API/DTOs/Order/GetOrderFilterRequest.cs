using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Service.Models;

namespace RecruitingChallenge.API.DTOs.Order
{
    public class GetOrderFilterRequest
    {
        public OrderSort? sortBy { get; set; }
        public SortOrientation orientation { get; set; }
        public string ClientEmail { get; set; }
        public string EntryDate { get; set; }
        public decimal Amount { get; set; }
        public OrderStatus? Status { get; set; }

        public int? lastCursorId  { get; set; }
        public string lastCursorValue { get; set; }

        public GetOrdersPagedModel ToServiceModel()
        {
            return new GetOrdersPagedModel
            {
                SortBy = this.sortBy,
                Orientation = this.orientation,
                Status = this.Status,
                LastCursorId = this.lastCursorId,
                LastCursorValue = this.lastCursorValue
            };
        }
    }
}
