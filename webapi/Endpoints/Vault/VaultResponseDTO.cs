namespace webapi.Endpoints.Vault
{
    public class VaultResponseDTO
    {
        public required string ID { get; init; }
        public required string Name { get; set; }
        public required string Description { get; set; }

        public static VaultResponseDTO ToVaultResponseDTO(VaultModel domain)
        {
            return new VaultResponseDTO() { ID = domain.ID, Name = domain.Name, Description = domain.Description };
        }
    }
}
