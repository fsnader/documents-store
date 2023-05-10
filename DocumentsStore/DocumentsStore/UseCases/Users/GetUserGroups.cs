using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class GetUserGroups : IGetUserGroups
{
    private readonly IGroupUsersRepository _groupUsersRepository;
    private readonly IUsersRepository _usersRepository;

    public GetUserGroups(IGroupUsersRepository groupUsersRepository, IUsersRepository usersRepository)
    {
        _groupUsersRepository = groupUsersRepository;
        _usersRepository = usersRepository;
    }

    public async Task<UseCaseResult<IEnumerable<Group>>> ExecuteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return UseCaseResult<IEnumerable<Group>>.BadRequest("Please provide a valid id");
        }

        var user = await _usersRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return UseCaseResult<IEnumerable<Group>>.NotFound("User not found");
        }
        
        var results = await _groupUsersRepository.GetGroupsByUserIdAsync(id, cancellationToken);

        return UseCaseResult<IEnumerable<Group>>.Success(results);
    }
}