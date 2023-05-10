using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class GetUserById : IGetUserById
{
    private readonly IUsersRepository _usersRepository;
    private readonly IGroupUsersRepository _groupUsersRepository;

    public GetUserById(IUsersRepository usersRepository, IGroupUsersRepository groupUsersRepository)
    {
        _usersRepository = usersRepository;
        _groupUsersRepository = groupUsersRepository;
    }
    
    public async Task<UseCaseResult<User>> ExecuteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return UseCaseResult<User>.BadRequest("Please provide a valid id");
        }
    
        var user = await _usersRepository.GetByIdAsync(id, cancellationToken);

        if (user is null)
        {
            return UseCaseResult<User>.NotFound();
        }

        user.Groups = await _groupUsersRepository.GetGroupsByUserIdAsync(id, cancellationToken);

        return UseCaseResult<User>.Success(user);
    }
}