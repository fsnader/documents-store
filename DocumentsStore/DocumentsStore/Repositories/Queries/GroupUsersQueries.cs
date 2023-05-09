namespace DocumentsStore.Repositories.Queries;

public static class GroupUsersQueries
{
    public static string AddUserToGroup { get; } = @"
        INSERT INTO ""UserGroup"" (""UserId"", ""GroupId"")
        VALUES (@UserId, @GroupId);";

    public static string RemoveUserFromGroup { get; } = @"
        DELETE FROM ""UserGroup""
        WHERE ""UserId"" = @UserId AND ""GroupId"" = @GroupId;";

    public static string GetUserById { get; } = @"
        SELECT *
        FROM ""Users""
        WHERE ""Id"" = @Id;";
}