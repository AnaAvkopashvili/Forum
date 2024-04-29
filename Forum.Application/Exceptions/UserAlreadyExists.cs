namespace Forum.Application.Exceptions
{
    public class UserAlreadyExists : Exception
    {
        public string Code = "User already exists";
        public UserAlreadyExists(string message) : base(message) { }

    }
}
