using DocumentsStore.Domain;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class CreateUser : ICreateUser
{
    public async Task<UseCaseResult<User>> ExecuteAsync(User user, CancellationToken cancellationToken)
    {
        if (!IsInputValid(user))
        {
            return UseCaseResult<User>.BadRequest();
        }

        return UseCaseResult<User>.Success(new User
        {
            Id = 0,
            Name = "Hello",
            Email = null,
            Role = Role.Regular,
            Groups = null
        });
    }
    
    private bool IsInputValid(User user) =>
        !string.IsNullOrWhiteSpace(user.Name) &&
        !string.IsNullOrWhiteSpace(user.Email);
}