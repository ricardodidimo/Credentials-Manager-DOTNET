using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Credentials.ListCredentials
{
    public record ListCredentialsFiltersDTO(string VaultId, string VaultAccessCode, string? CategoryId, int CurrentPage = 1, int PageSize = 10);
}
