using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class GetAllUsers : IGetAllUsers
{
    private readonly IUsersRepository _usersRepository;

    public GetAllUsers(IUsersRepository usersRepository) => _usersRepository = usersRepository;

    public async Task<UseCaseResult<IEnumerable<User>>> ExecuteAsync(int take, int skip, CancellationToken cancellationToken)
    {
        var results = await _usersRepository.ListAllAsync(take, skip, cancellationToken);

        return UseCaseResult<IEnumerable<User>>.Success(results);
    }
}

