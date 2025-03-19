namespace webapi.Endpoints.User
{
    public class UserResponseDTO
    {
        public required string ID { get; init; }
        public required string Name { get; init; }
        public required string Email { get; init; }

        public static UserResponseDTO ToUserResponseDTO(UserModel domain)
        {
            return new UserResponseDTO()
            {
                ID = domain.ID,
                Name = domain.Name,
                Email = domain.Email,
            };
        }

    }
}
