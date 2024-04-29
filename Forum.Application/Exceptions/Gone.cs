namespace Forum.Application.Exceptions
{
    public class Gone : Exception
    {
        public string Code = "Operation not available!";
        public Gone(string message) : base(message) { }

    }
}
