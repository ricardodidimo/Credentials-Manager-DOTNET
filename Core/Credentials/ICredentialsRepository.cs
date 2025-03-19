using Core.Credentials.ListCredentials;
using FluentResults;

namespace Core.Credentials
{
    public interface ICredentialsRepository
    {

        public Task<IResult<List<Credentials>>> ListCredentials(ListCredentialsFiltersDTO ownerID);
        public Task<IResult<Credentials>> FindByIdentifier(string uuid);
        public Task<IResult<Credentials>> CreateCredentials(Credentials credentials);
        public Task<IResult<Credentials>> UpdateCredentials(Credentials credentials);
        public Task<IResult<Credentials>> DeleteCredentials(Credentials credentials);
    }
}
