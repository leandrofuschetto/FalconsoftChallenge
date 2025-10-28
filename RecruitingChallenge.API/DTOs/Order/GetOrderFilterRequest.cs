using RecruitingChallenge.Domain.Enums;
using RecruitingChallenge.Service.Models;
using System.Globalization;
using System.Reflection;

namespace RecruitingChallenge.API.DTOs.Order
{
    public class GetOrderFilterRequest
    {
        public EOrderSort SortBy { get; set; } = EOrderSort.Id;
        public ESortOrientation Orientation { get; set; } = ESortOrientation.Asc;
        public EFilterOperator FilterOperator { get; set; } = EFilterOperator.Equal;
        public string FilterValue { get; set; }

        public int? lastCursorId  { get; set; }
        public string lastCursorValue { get; set; }

        public void ValidateAndNormalize() 
        {
            if (string.IsNullOrEmpty(this.FilterValue)) 
                return;

            if (SortBy.Equals(EOrderSort.Id))
                return;

            // to do hacer custom y poner en el handler
            if (SortBy.Equals(EOrderSort.EntryDate))
            {
                if (!DateTime.TryParseExact(this.FilterValue, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
                    throw new ArgumentException($"Valid dates should have 'yyyy-MM-dd' format");
            }

            if (SortBy.Equals(EOrderSort.TotalAmount))
            {
                if (!decimal.TryParse(this.FilterValue.Replace(',', '.'), NumberStyles.Any, CultureInfo.InvariantCulture, out _))
                    throw new ArgumentException($"TotalAmout should be a decimal value");
            }
        }

        public GetOrdersPagedModel ToServiceModel()
        {
            return new GetOrdersPagedModel
            {
                SortBy = this.SortBy,
                Orientation = this.Orientation,
                LastCursorId = this.lastCursorId,
                LastCursorValue = this.lastCursorValue,
                FilterOperator = this.FilterOperator,
                FilterValue = this.FilterValue
            };
        }
    }
}
