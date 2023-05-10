namespace DocumentsStore.Repositories.Queries;

public static class GroupUsersQueries
{
    public static string AddUserToGroup = @"
        INSERT INTO ""UserGroup"" (""UserId"", ""GroupId"")
        VALUES (@UserId, @GroupId);
    ";

    public static string RemoveUserFromGroup = @"
        DELETE FROM ""UserGroup""
        WHERE ""UserId"" = @UserId AND ""GroupId"" = @GroupId;
    ";

    public static string GetUserById = @"
        SELECT *
        FROM ""User""
        WHERE ""Id"" = @Id;
    ";

    public static string GetGroupsByUserId = @"
            SELECT g.*
            FROM ""Group"" g
            JOIN ""UserGroup"" ug ON g.""Id"" = ug.""GroupId""
            WHERE ug.""UserId"" = @UserId
        ";
}