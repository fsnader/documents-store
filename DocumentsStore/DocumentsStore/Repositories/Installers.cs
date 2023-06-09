using DocumentsStore.Repositories.Abstractions;
using DocumentsStore.Repositories.Database;

namespace DocumentsStore.Repositories;

public static class Installers
{
    public static IServiceCollection AddRepositories(
        this IServiceCollection serviceCollection)
    {
        return serviceCollection
            .AddSingleton<IDbConnectionFactory, DbConnectionFactory>()
            .AddSingleton<IQueryExecutor, QueryExecutor>()
            .AddScoped<IUsersRepository, UsersRepository>()
            .AddScoped<IGroupsRepository, GroupsRepository>()
            .AddScoped<IGroupUsersRepository, GroupUsersRepository>()
            .AddScoped<IDocumentsRepository, DocumentsRepository>();
    }
}