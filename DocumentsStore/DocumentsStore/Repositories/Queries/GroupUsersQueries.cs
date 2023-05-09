namespace DocumentsStore.Repositories.Queries;

public static class GroupUsersQueries
{
    public static string AddUserToGroup = @"
        INSERT INTO ""UserGroup"" (""UserId"", ""GroupId"")
        VALUES (@UserId, @GroupId);
    ";

    public static string RemoveUserFromGroup= @"
        DELETE FROM ""UserGroup""
        WHERE ""UserId"" = @UserId AND ""GroupId"" = @GroupId;
    ";

    public static string GetUserById = @"
        SELECT *
        FROM ""Users""
        WHERE ""Id"" = @Id;
    ";
}