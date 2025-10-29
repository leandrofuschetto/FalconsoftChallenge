using OrderNowChallenge.API.Filters;
using OrderNowChallenge.Common.Enums;
using OrderNowChallenge.Domain.Enums;
using OrderNowChallenge.Service.Models;
using System.ComponentModel.DataAnnotations;

namespace OrderNowChallenge.API.DTOs.Order
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
                OrderStatusFilter = this.OrderStatusFilter,
                ClientEmailFilter = this.ClientEmailFilter,
                AmountFilter = this.AmountFilter,
                LastCursorId = this.lastCursorId,
                
                EntryDateFilter = !string.IsNullOrWhiteSpace(this.EntryDateFilter)
                    ? Convert.ToDateTime(this.EntryDateFilter)
                    : null,
                
                LastCursorValue = this.SortOrderByProperty.Equals(ESortOrderByProperty.TotalAmount)
                    ? this.lastCursorValue?.Replace(".", ",")
                    : this.lastCursorValue
            };
        }
    }
}
