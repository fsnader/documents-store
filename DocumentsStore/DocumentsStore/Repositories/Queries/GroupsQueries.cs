namespace DocumentsStore.Repositories.Queries;

public static class GroupsQueries
{
    public const string Create = @"
        INSERT INTO ""Groups"" (""Name"")
        VALUES (@Name)
        RETURNING *
    ";

    public const string Delete = @"
        DELETE FROM ""Groups""
        WHERE ""Id"" = @Id
        RETURNING *
    ";

    public const string ListAll = @"
        SELECT *
        FROM ""Groups""
        ORDER BY ""Name""
        LIMIT @Take OFFSET @Skip
    ";

    public const string GetById = @"
        SELECT *
        FROM ""Groups""
        WHERE ""Id"" = @Id
    ";

    public const string Update = @"
        UPDATE ""Groups""
        SET ""Name"" = @Name
        WHERE ""Id"" = @Id
        RETURNING *
    ";
}