using RecruitingChallenge.Domain.Enums;

namespace RecruitingChallenge.DAL.Filters
{
    public class OrderFilters : Filter
    {
        public EOrderSort SortBy { get; set; }
        public int? LastCursorId { get; set; }
        public string LastCursorValue { get; set; }
    }
}
