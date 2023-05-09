namespace DocumentsStore.Repositories.Queries;

public static class GroupsQueries
{
    public const string Create = @"
        INSERT INTO ""Group"" (""Name"")
        VALUES (@Name)
        RETURNING *
    ";

    public const string Delete = @"
        DELETE FROM ""Group""
        WHERE ""Id"" = @Id
        RETURNING *
    ";

    public const string ListAll = @"
        SELECT *
        FROM ""Group""
        ORDER BY ""Id""
        LIMIT @Take OFFSET @Skip
    ";

    public const string GetById = @"
        SELECT *
        FROM ""Group""
        WHERE ""Id"" = @Id
    ";

    public const string Update = @"
        UPDATE ""Group""
        SET ""Name"" = @Name
        WHERE ""Id"" = @Id
        RETURNING *
    ";
}