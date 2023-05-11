using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Exceptions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class UpdateUser : IUpdateUser
{
    private readonly IUsersRepository _usersRepository;

    public UpdateUser(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<UseCaseResult<User>> ExecuteAsync(int id, User user, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return UseCaseResult<User>.BadRequest("Please provide a valid id");
        }

        try
        {
            var updated = await _usersRepository.UpdateAsync(id, user, cancellationToken);

            if (updated is null)
            {
                return UseCaseResult<User>.NotFound();
            }

            return UseCaseResult<User>.Success(updated);
        }
        catch (UniqueException)
        {
            return UseCaseResult<User>.BadRequest("Email needs to be unique");
        }
    }
}