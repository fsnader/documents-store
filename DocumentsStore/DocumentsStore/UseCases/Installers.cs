using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases.Users;

public static class UseCasesInstallers
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddScoped<IGetAllUsers, GetAllUsers>()
            .AddScoped<IGetUserById, GetUserById>()
            .AddScoped<ICreateUser, CreateUser>();
}