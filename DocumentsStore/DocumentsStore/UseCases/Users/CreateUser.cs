using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Exceptions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class CreateUser : ICreateUser
{
    private readonly IUsersRepository _usersRepository;

    public CreateUser(IUsersRepository usersRepository) => _usersRepository = usersRepository;

    public async Task<UseCaseResult<User>> ExecuteAsync(User user, CancellationToken cancellationToken)
    {
        if (!IsInputValid(user))
        {
            return UseCaseResult<User>.BadRequest();
        }

        try
        {
            var created = await _usersRepository.CreateAsync(user, cancellationToken);

            return UseCaseResult<User>.Success(created);
        }
        catch (UniqueException)
        {
            return UseCaseResult<User>.BadRequest("Email needs to be unique");
        }
    }
    
    private bool IsInputValid(User user) =>
        !string.IsNullOrWhiteSpace(user.Name) &&
        !string.IsNullOrWhiteSpace(user.Email);
}