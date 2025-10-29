namespace OrderNowChallenge.Domain.Exceptions
{
	public class BadRequestException : Exception
	{
		public string Code { get; private set; } = "BAD_REQUEST";

		public BadRequestException(string message = null)
			: base(message) { }

		public BadRequestException(string message, Exception ex)
			: base(message, ex) { }
	}
}


