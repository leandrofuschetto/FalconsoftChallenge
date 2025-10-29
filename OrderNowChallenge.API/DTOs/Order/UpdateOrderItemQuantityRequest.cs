using System.ComponentModel.DataAnnotations;

namespace OrderNowChallenge.API.DTOs.Order
{
	public class UpdateOrderItemQuantityRequest
	{
		[Range(1, int.MaxValue, ErrorMessage = "Quantity must be a positive number")]
		public int Quantity { get; set; }
	}
}


