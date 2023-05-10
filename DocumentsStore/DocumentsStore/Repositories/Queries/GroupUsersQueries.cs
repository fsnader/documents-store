namespace DocumentsStore.Repositories.Queries;

public static class GroupUsersQueries
{
    public const string AddUserToGroup = @"
        INSERT INTO ""UserGroup"" (""UserId"", ""GroupId"")
        VALUES (@UserId, @GroupId);
    ";

    public const string RemoveUserFromGroup = @"
        DELETE FROM ""UserGroup""
        WHERE ""UserId"" = @UserId AND ""GroupId"" = @GroupId;
    ";

    public const string GetGroupsByUserId = @"
            SELECT g.*
            FROM ""Group"" g
            JOIN ""UserGroup"" ug ON g.""Id"" = ug.""GroupId""
            WHERE ug.""UserId"" = @UserId
        ";
}