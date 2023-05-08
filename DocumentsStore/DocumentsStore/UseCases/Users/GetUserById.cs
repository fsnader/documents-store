using DocumentsStore.Domain;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class GetUserById : IGetUserById
{
    public Task<UseCaseResult<User>> ExecuteAsync(int id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}