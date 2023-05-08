using DocumentsStore.Repositories.Abstractions;

namespace DocumentsStore.Repositories;

public static class Installers
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IGroupsRepository, GroupsRepository>()
            .AddScoped<IGroupUsersRepository, GroupUsersRepository>();
    }
}