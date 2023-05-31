namespace API.Helpers
{
    public class MessageParams : PaginationParams
    {
        public string Name { get; set; }

        public string Container { get; set; } = "unread";
    }
}