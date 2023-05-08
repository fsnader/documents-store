using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class GetUserById : IGetUserById
{
    private readonly IUsersRepository _usersRepository;

    public GetUserById(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
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

        return UseCaseResult<User>.Success(user);
    }
}