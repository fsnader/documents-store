namespace DocumentsStore.Repositories.Queries;

public static class UserQueries
{
    public static string Create = @"
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
    
    public static string GetById = @"
        SELECT *
        FROM ""User""
        WHERE ""Id"" = @Id
    ";
    
    public static string ListAll = @"
        SELECT *
        FROM ""User""
        ORDER BY ""Id""
        LIMIT @Take OFFSET @Skip
    ";
    
    public static string Update = @"
        UPDATE ""User""
        SET 
            ""Name"" = @Name,
            ""Email"" = @Email,
            ""Role"" = @Role::""UserRole""
        WHERE ""Id"" = @Id
        RETURNING *
    ";
    
    public static string Delete = @"
        DELETE FROM ""User""
        WHERE ""Id"" = @Id
        RETURNING *";
}