using DocumentsStore.Domain;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class GetAllUsers : IGetAllUsers
{
    public Task<UseCaseResult<IEnumerable<User>>> ExecuteAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}