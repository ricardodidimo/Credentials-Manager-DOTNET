using UserUseCase = Core.User;

namespace Core.Vault
{
    public record Vault
    {
        public required string ID { get; init; }
        public required string Name { get; set;  }
        public required string Description { get; set; }
        public required string AccessCode { get; set; }
        public required string UserId { get; set; }
        public UserUseCase.User? User { get; init; }
    }
}
