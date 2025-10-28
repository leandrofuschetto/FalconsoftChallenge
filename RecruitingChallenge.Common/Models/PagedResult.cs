using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            NextCursorValue = nextCursorValue;
            NextCursor = nextCursor;
            HasNextPage = hasNextPage;
        }
    }
}
