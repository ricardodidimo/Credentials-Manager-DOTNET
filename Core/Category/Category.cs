namespace Core.Category
{
    public record Category()
    {
        public required string ID { get; init; }
        public required string Name { get; set; }
        public required string UserId { get; set; }
        public User.User? User { get; set; }
    }
}