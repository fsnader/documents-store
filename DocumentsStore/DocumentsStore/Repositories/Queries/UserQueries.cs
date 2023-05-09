namespace DocumentsStore.Repositories.Queries;

public static class UserQueries
{
    public static string CreateUser = @"INSERT INTO ""User"" (""Name"", ""Email"", ""Role"") VALUES (@Name, @Email, @Role::""UserRole"") RETURNING ""Id""";
    
    public static string GetUserById = @"SELECT * FROM ""User"" WHERE ""Id"" = @Id";
    
    
}