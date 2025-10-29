using RecruitingChallenge.API.Filters;
using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Service.Models;
using System.ComponentModel.DataAnnotations;

namespace RecruitingChallenge.API.DTOs.Order
{
    public class GetOrderFilterRequest
    {
        public ESortOrderByProperty SortOrderByProperty { get; set; } = ESortOrderByProperty.Id;
        public ESortOrientation Orientation { get; set; } = ESortOrientation.Asc;

        [DateFormatValidation("yyyy-MM-dd")]
        public string EntryDateFilter { get; set; }

        public decimal? AmountFilter { get; set; }
        public EOrderStatus? OrderStatusFilter { get; set; }

        [EmailAddress]
        public string ClientEmailFilter { get; set; }

        public int? lastCursorId  { get; set; }
        public string lastCursorValue { get; set; }

        public GetOrdersPagedModel ToServiceModel()
        {
            return new GetOrdersPagedModel
            {
                SortByProperty = this.SortOrderByProperty,
                Orientation = this.Orientation,
                LastCursorId = this.lastCursorId,
                LastCursorValue = this.lastCursorValue,
                AmountFilter = this.AmountFilter,
                OrderStatusFilter = this.OrderStatusFilter,
                ClientEmailFilter = this.ClientEmailFilter,

                EntryDateFilter = !string.IsNullOrWhiteSpace(this.EntryDateFilter)
                    ? Convert.ToDateTime(this.EntryDateFilter)
                    : null
            };
        }
    }
}
