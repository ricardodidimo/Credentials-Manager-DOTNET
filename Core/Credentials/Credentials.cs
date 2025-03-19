using Core.Vault;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Credentials
{
    public record Credentials
    {
        public required string ID { get; init; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string PrimaryCredential { get; set; }
        public required string SecondaryCredential { get; set; }
        public required string VaultId { get; set; }
        public Vault.Vault? Vault { get; set; }
        public string? CategoryId { get; set; }
        public Category.Category? Category { get; set; }
    }
}
