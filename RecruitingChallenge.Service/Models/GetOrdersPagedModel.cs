using RecruitingChallenge.Common.Enums;
using RecruitingChallenge.Domain.Enums;

namespace RecruitingChallenge.Service.Models
{
    public class GetOrdersPagedModel
    {
        public ESortOrderByProperty SortByProperty { get; set; }
        public ESortOrientation Orientation { get; set; }
        
        public DateTime? EntryDateFilter { get; set; }
        public decimal? AmountFilter { get; set; }
        public EOrderStatus? OrderStatusFilter { get; set; }
        public string ClientEmailFilter { get; set; }

        public int? LastCursorId { get; set; }
        public string LastCursorValue { get; set; }
    }
}
