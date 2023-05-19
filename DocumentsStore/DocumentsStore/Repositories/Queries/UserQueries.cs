namespace DocumentsStore.Repositories.Queries;

public static class UserQueries
{
    public const string Create = @"
        INSERT INTO users 
            (
             name,
             email,
             role
             )
        VALUES
            (
             @Name, 
             @Email,
             @Role::user_role
            )
        RETURNING *
    ";
    
    public const string GetById = @"
        SELECT *
        FROM users
        WHERE id = @Id
    ";
    
    public const string ListAll = @"
        SELECT *
        FROM users
        ORDER BY id
        LIMIT @Take OFFSET @Skip
    ";
    
    public const string Update = @"
        UPDATE users
        SET 
            name = @Name,
            email = @Email,
            role = @Role::user_role
        WHERE id = @Id
        RETURNING *
    ";
    
    public const string Delete = @"
        DELETE FROM users
        WHERE id = @Id
        RETURNING *";
}