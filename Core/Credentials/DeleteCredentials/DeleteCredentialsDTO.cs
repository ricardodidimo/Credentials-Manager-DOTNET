using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Credentials.DeleteCredentials
{
    public record DeleteCredentialsDTO(string CredentialsId, string VaultAccessCode);
}
