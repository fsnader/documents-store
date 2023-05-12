namespace DocumentsStore.Repositories.Queries;

public static class DocumentQueries
{
    public const string GetUsersPermissions = @"
        SELECT ""UserId""
        FROM ""DocumentUserPermission""
        WHERE ""DocumentId"" = @DocumentId
    ";
    
    public const string GetGroupsPermissions = @"
        SELECT ""GroupId""
        FROM ""DocumentGroupPermission""
        WHERE ""DocumentId"" = @DocumentId
    ";
    
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
            WHERE ""Id"" = @DocumentId
            AND (
                ""UserId"" = @UserId
                OR EXISTS (
                    SELECT 1 FROM ""DocumentUserPermission""
                    WHERE ""DocumentId"" = @DocumentId
                    AND ""UserId"" = @UserId
                )
                OR EXISTS (
                    SELECT 1 FROM ""DocumentGroupPermission""
                    INNER JOIN ""UserGroup"" ON ""UserGroup"".""GroupId"" = ""DocumentGroupPermission"".""GroupId""
                    WHERE ""DocumentGroupPermission"".""DocumentId"" = @DocumentId
                    AND ""UserGroup"".""UserId"" = @UserId
                )
            )
        );
";

    public static readonly string ListUserAuthorizedDocuments = @"
    SELECT d.*
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