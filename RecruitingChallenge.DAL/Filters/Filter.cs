using RecruitingChallenge.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecruitingChallenge.DAL.Filters
{
    public class Filter
    {
        public ESortOrientation Orientation { get; set; }
        public EFilterOperator FilterOperator { get; set; }
        public string FilterValue { get; set; }
    }
}
