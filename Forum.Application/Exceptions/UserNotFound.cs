namespace Forum.Application.Exceptions
{
    public class UserNotFound : Exception
    {
        public string Code = "User not found";
        public UserNotFound(string message) : base(message) { }

    }
}
