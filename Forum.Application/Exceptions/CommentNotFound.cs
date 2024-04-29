namespace Forum.Application.Exceptions
{
    public class CommentNotFound : Exception
    {
        public string Code = "Comment not found";
        public CommentNotFound(string message) : base(message) { }

    }
}
