namespace RecruitingChallenge.Domain.Exceptions
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

    public class UserNotFoundAnonymousException : Exception
    {
        public string Code { get; private set; } = "AUTH_GENERAL_ERROR";

        public UserNotFoundAnonymousException(string message = null)
            : base(message) { }

        public UserNotFoundAnonymousException(
            string message,
            Exception ex = null)
            : base(message, ex) { }
    }
}
