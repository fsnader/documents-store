using DocumentsStore.Domain;
using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public class DeleteUser : IDeleteUser
{
    private readonly IUsersRepository _usersRepository;

    public DeleteUser(IUsersRepository usersRepository)
    {
        _usersRepository = usersRepository;
    }
    
    public async Task<UseCaseResult<User>> ExecuteAsync(int id, CancellationToken cancellationToken)
    {
        if (id <= 0)
        {
            return UseCaseResult<User>.BadRequest("Please provide a valid id");
        }
        
        var deleted = await _usersRepository.DeleteAsync(id, cancellationToken);

        if (deleted is null)
        {
            return UseCaseResult<User>.NotFound();
        }
        
        return UseCaseResult<User>.Success(deleted);
        
    }
}