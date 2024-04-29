namespace Forum.Application.Exceptions
{
    public class UnauthorizedAccess : Exception
    {
        public string Code = "Access Denied!";
        public UnauthorizedAccess(string message) : base(message) { }

    }
}
