namespace DocumentsStore.Repositories.Queries;

public static class GroupsQueries
{
    public const string Create = @"
        INSERT INTO groups (name)
        VALUES (@Name)
        RETURNING *
    ";

    public const string Delete = @"
        DELETE FROM groups
        WHERE id = @Id
        RETURNING *
    ";

    public const string ListAll = @"
        SELECT *
        FROM groups
        ORDER BY id
        LIMIT @Take OFFSET @Skip
    ";

    public const string GetById = @"
        SELECT *
        FROM groups
        WHERE id = @Id
    ";

    public const string Update = @"
        UPDATE groups
        SET name = @Name
        WHERE id = @Id
        RETURNING *
    ";
}