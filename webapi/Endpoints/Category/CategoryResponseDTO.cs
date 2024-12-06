namespace webapi.Endpoints.Vault
{
    public class CategoryResponseDTO
    {
        public required string ID { get; init; }
        public required string Name { get; set; }
        public required string UserId { get; set; }    

        public static CategoryResponseDTO ToCategoryResponseDTO(CategoryModel domain)
        {
            return new CategoryResponseDTO() { ID = domain.ID, Name = domain.Name, UserId = domain.UserId };
        }
    }
}
