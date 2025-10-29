namespace OrderNowChallenge.Domain.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public string Code { get; private set; } = "USER_NOT_FOUND_EXCEPTION";

        public UserNotFoundException(string message = null) 
            : base(message) { }

        public UserNotFoundException(
            string message,
            Exception ex = null)
            : base(message, ex) { }
    }
}
