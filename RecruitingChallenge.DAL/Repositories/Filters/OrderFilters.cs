using RecruitingChallenge.Domain.Enums;

namespace RecruitingChallenge.DAL.Repositories.Filters
{
    public class OrderFilters
    {
        public OrderSort? SortBy { get; set; }
        public SortOrientation Orientation { get; set; }
        public string ClientEmail { get; set; }
        public string EntryDate { get; set; }
        public decimal Amount { get; set; }
        public OrderStatus? Status { get; set; }

        public int? LastCursorId { get; set; }
        public string LastCursorValue { get; set; }
    }
}
