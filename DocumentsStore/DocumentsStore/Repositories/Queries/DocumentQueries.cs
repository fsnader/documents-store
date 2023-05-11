namespace DocumentsStore.Repositories.Queries;

public static class DocumentQueries
{
    public const string CreateDocument = @"
        INSERT INTO ""Document"" (""UserId"", ""PostedDate"", ""Name"", ""Description"", ""Category"", ""Content"")
        VALUES (@UserId, @PostedDate, @Name, @Description, @Category, @Content)
        RETURNING ""Id"";";

    public const string InsertAuthorizedUser = @"
        INSERT INTO ""DocumentUserPermission"" (""DocumentId"", ""UserId"")
        VALUES (@DocumentId, @UserId);";

    public const string InsertAuthorizedGroup = @"
        INSERT INTO ""DocumentGroupPermission"" (""DocumentId"", ""GroupId"")
        VALUES (@DocumentId, @GroupId);";

    public const string GetDocumentById = @"
        SELECT ""Id"", ""UserId"", ""PostedDate"", ""Name"", ""Description"", ""Category"", ""Content""
        FROM ""Document""
        WHERE ""Id"" = @Id;";

    public const string CheckUserDocumentPermission = @"
        SELECT COUNT(*) 
        FROM ""DocumentPermission""
        WHERE ""DocumentId"" = @Id;";

    public static readonly string ListUserAuthorizedDocuments = @"
        SELECT d.*
        FROM ""Document"" d
        INNER JOIN ""DocumentUserPermission"" dup ON d.""Id"" = dup.""DocumentId""
        INNER JOIN ""User"" u ON dup.""UserId"" = u.""Id""
        WHERE dup.""UserId"" = @UserId
    UNION
        SELECT d.*
        FROM ""Document"" d
        INNER JOIN ""DocumentGroupPermission"" dgp ON d.""Id"" = dgp.""DocumentId""
        INNER JOIN ""UserGroup"" gu ON dgp.""GroupId"" = gu.""GroupId""
        INNER JOIN ""User"" u ON gu.""UserId"" = u.""Id""
        WHERE gu.""UserId"" = @UserId
    ORDER BY ""PostedDate"" DESC
    LIMIT @Take OFFSET @Skip;
";
}