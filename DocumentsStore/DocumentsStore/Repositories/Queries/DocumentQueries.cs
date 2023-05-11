namespace DocumentsStore.Repositories.Queries;

public static class DocumentQueries
{
    public const string InsertAuthorizedUser = @"
        INSERT INTO ""DocumentUserPermission"" (""DocumentId"", ""UserId"")
        VALUES (@DocumentId, @UserId);";

    public const string InsertAuthorizedGroup = @"
        INSERT INTO ""DocumentGroupPermission"" (""DocumentId"", ""GroupId"")
        VALUES (@DocumentId, @GroupId);";

    public const string GetDocumentById = @"
        SELECT *
        FROM ""Document""
        WHERE ""Id"" = @Id;
    ";

    public const string CheckUserDocumentPermission = @"
        SELECT EXISTS (
            SELECT 1 FROM ""Document""
            WHERE ""Id"" = @documentId
            AND (
                ""UserId"" = @userId
                OR EXISTS (
                    SELECT 1 FROM ""DocumentUserPermission""
                    WHERE ""DocumentId"" = @documentId
                    AND ""UserId"" = @userId
                )
                OR EXISTS (
                    SELECT 1 FROM ""DocumentGroupPermission""
                    INNER JOIN ""UserGroup"" ON ""UserGroup"".""GroupId"" = ""DocumentGroupPermission"".""GroupId""
                    WHERE ""DocumentGroupPermission"".""DocumentId"" = @documentId
                    AND ""UserGroup"".""UserId"" = @userId
                )
            )
        );
";

    public static readonly string ListUserAuthorizedDocuments = @"
    SELECT DISTINCT d.*
        FROM ""Document"" d
        INNER JOIN ""DocumentUserPermission"" dup ON d.""Id"" = dup.""DocumentId""
        INNER JOIN ""User"" u ON dup.""UserId"" = u.""Id""
        WHERE dup.""UserId"" = @UserId
    UNION
        SELECT *
        FROM ""Document""
        WHERE ""UserId"" = @UserId
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