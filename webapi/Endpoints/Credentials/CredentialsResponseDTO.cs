namespace webapi.Endpoints.Credentials
{
    public class CredentialsResponseDTO
    {
        public required string ID { get; init; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string PrimaryCredential { get; set; }
        public required string SecondaryCredential { get; set; }
        public required string VaultId { get; set; }
        public string? CategoryId { get; set; }

        public static CredentialsResponseDTO ToCredentialsResponseDTO(CredentialsModel domain)
        {
            return new CredentialsResponseDTO()
            {
                ID = domain.ID,
                Name = domain.Name,
                Description = domain.Description,
                VaultId = domain.VaultId,
                CategoryId = domain.CategoryId,
                PrimaryCredential = domain.PrimaryCredential,
                SecondaryCredential = domain.SecondaryCredential
            };
        }
    }
}
