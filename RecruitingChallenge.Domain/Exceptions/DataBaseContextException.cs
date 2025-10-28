namespace RecruitingChallenge.Domain.Exceptions
{
    public class DataBaseContextException : Exception
    {
        public string Code { get; private set; } = "DATABASE_GENERAL_EXCEPTION";

        public DataBaseContextException(string message = null) 
            : base(message) { }

        public DataBaseContextException(
            string message,
            Exception ex = null)
            : base(message, ex) { }
    }
}
