namespace DocumentsStore.Repositories.Queries;

public static class GroupUsersQueries
{
    public const string AddUserToGroup = @"
        INSERT INTO user_groups (user_id, group_id)
        VALUES (@UserId, @GroupId);
    ";

    public const string RemoveUserFromGroup = @"
        DELETE FROM user_groups
        WHERE user_id = @UserId AND group_id = @GroupId;
    ";

    public const string GetGroupsByUserId = @"
            SELECT g.*
            FROM groups g
            JOIN user_groups ug ON g.id = ug.group_id
            WHERE ug.user_id = @UserId
        ";
}