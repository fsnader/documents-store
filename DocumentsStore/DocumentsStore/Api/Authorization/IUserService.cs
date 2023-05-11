using DocumentsStore.Domain;

namespace DocumentsStore.Api.Authorization;

public interface IUserService
{
    public Task<User> GetCurrentUserAsync(CancellationToken cancellationToken);

    public Task<string?> CreateTokenAsync(int userId, CancellationToken cancellationToken);
}