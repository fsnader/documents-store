using DocumentsStore.UseCases.Users;
using DocumentsStore.UseCases.Users.Abstractions;

namespace DocumentsStore.UseCases;

public static class UseCasesInstallers
{
    public static IServiceCollection AddUseCases(
        this IServiceCollection serviceCollection) =>
        serviceCollection
            .AddScoped<IGetAllUsers, GetAllUsers>()
            .AddScoped<IGetUserById, GetUserById>()
            .AddScoped<ICreateUser, CreateUser>()
            .AddScoped<IDeleteUser, DeleteUser>()
            .AddScoped<IUpdateUser, UpdateUser>();
}