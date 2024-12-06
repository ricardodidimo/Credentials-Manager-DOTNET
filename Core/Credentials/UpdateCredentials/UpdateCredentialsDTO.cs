using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Credentials.UpdateCredentials
{
    public record UpdateCredentialsDTO(string CredentialsId, string VaultAccessCode, string? Name, string? Description, string? PrimaryCredential, string? SecondaryCredential, string? VaultId, string? CategoryId);

}
