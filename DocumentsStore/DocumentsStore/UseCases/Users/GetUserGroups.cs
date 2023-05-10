using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class GetUserGroups : IGetUserGroups
{
    private readonly IGroupUsersRepository _groupUsersRepository;

    public GetUserGroups(IGroupUsersRepository groupUsersRepository) => _groupUsersRepository = groupUsersRepository;

    public async Task<UseCaseResult<IEnumerable<Group>>> ExecuteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return UseCaseResult<IEnumerable<Group>>.BadRequest("Please provide a valid id");
        }
        
        var results = await _groupUsersRepository.GetGroupsByUserIdAsync(id, cancellationToken);

        return UseCaseResult<IEnumerable<Group>>.Success(results);
    }
}