namespace Forum.Application.Exceptions
{
    public class TopicNotFound : Exception
    {
        public string Code = "Topic not found";
        public TopicNotFound(string message) : base(message) { }

    }
}
