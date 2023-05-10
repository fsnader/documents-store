namespace DocumentsStore.Repositories.Queries;

public static class UserQueries
{
    public const string Create = @"
        INSERT INTO ""User"" 
            (
             ""Name"",
             ""Email"",
             ""Role""
             )
        VALUES
            (
             @Name, 
             @Email,
             @Role::""UserRole""
            )
        RETURNING *
    ";
    
    public const string GetById = @"
        SELECT *
        FROM ""User""
        WHERE ""Id"" = @Id
    ";
    
    public const string ListAll = @"
        SELECT *
        FROM ""User""
        ORDER BY ""Id""
        LIMIT @Take OFFSET @Skip
    ";
    
    public const string Update = @"
        UPDATE ""User""
        SET 
            ""Name"" = @Name,
            ""Email"" = @Email,
            ""Role"" = @Role::""UserRole""
        WHERE ""Id"" = @Id
        RETURNING *
    ";
    
    public const string Delete = @"
        DELETE FROM ""User""
        WHERE ""Id"" = @Id
        RETURNING *";
}