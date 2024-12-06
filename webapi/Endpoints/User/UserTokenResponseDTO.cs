namespace webapi.Endpoints.User
{
    public class UserTokenResponseDTO
    {
        public required string ID { get; init; }
        public required string Name { get; init; }
        public required string Token { get; init; }

        public static UserTokenResponseDTO ToUserTokenResponseDTO(UserModel domain, string authToken)
        {
            return new UserTokenResponseDTO()
            {
                ID = domain.ID,
                Name = domain.Name,
                Token = authToken,
            };
        }

    }
}
