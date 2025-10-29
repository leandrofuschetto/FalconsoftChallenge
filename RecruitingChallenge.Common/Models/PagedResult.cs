using RecruitingChallenge.Common.Extensions;

namespace RecruitingChallenge.Common.Models
{
    public class PagedResult<T>
    {
        public IReadOnlyList<T> Items { get; }
        public bool HasNextPage { get; }
        public string? NextCursor { get; }
        public string? NextCursorValue { get; }

        public PagedResult(IReadOnlyList<T> items, string? nextCursorValue, string? nextCursor, bool hasNextPage)
        {
            Items = items;
            NextCursorValue = nextCursorValue?.ToString().TryFormatAsDate();
            NextCursor = nextCursor;
            HasNextPage = hasNextPage;
        }
    }
}
