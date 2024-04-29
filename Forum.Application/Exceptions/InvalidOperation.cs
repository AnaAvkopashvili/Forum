namespace Forum.Application.Exceptions
{
    public class InvalidOperation : Exception
    {
        public string Code = "Invalid Operation!";
        public InvalidOperation(string message) : base(message) { }

    }
}
