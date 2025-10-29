using System.ComponentModel.DataAnnotations;

namespace OrderNowChallenge.API.DTOs.Login
{
    public class LoginRequest
    {
        [Required(ErrorMessage = "Title is mandatory")]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Username max length is 15, min is 5")]
        public string UserName { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 5, ErrorMessage = "Password max length is 15, min is 5")]
        public string Password { get; set; }
    }
}
