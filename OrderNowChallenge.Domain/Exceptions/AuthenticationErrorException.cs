namespace OrderNowChallenge.Domain.Exceptions
{
    public class AuthenticationErrorException : Exception
    {
        public string Code { get; private set; } = "AUTHENTICATION_ERROR_EXCEPTION";

        public AuthenticationErrorException(string message = null) 
            : base(message) { }

        public AuthenticationErrorException(
            string message,
            Exception ex = null)
            : base(message, ex) { }
    }
}
