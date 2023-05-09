namespace DocumentsStore.Repositories.Queries;

public static class UserQueries
{
    public static string CreateUser = @"INSERT INTO ""User"" (""Name"", ""Email"", ""Role"") VALUES (@Name, @Email, @Role::""UserRole"") RETURNING ""Id""";
    
    public static string GetUserById = @"SELECT * FROM ""User"" WHERE ""Id"" = @Id";
    
    public static string ListAllUsers = @"SELECT * FROM ""User"" ORDER BY ""Id"" OFFSET @Skip ROWS FETCH NEXT @Take ROWS ONLY";
    
    public static string UpdateUser = @"UPDATE ""User"" SET ""Name"" = @Name, ""Email"" = @Email, ""Role"" = @Role WHERE ""Id"" = @Id RETURNING *";
    
    public static string DeleteUser = @"DELETE FROM ""User"" WHERE ""Id"" = @Id RETURNING *";
}