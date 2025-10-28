using RecruitingChallenge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.Service.Models
{
    public class GetOrdersPagedModel
    {
        public OrderSort? SortBy { get; set; }
        public SortOrientation Orientation { get; set; }
        public string FilterValue { get; set; }
        public OrderStatus? Status { get; set; }

        public int? LastCursorId { get; set; }
        public string LastCursorValue { get; set; }
    }
}
