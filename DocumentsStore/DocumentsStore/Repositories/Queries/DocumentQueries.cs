namespace DocumentsStore.Repositories.Queries;

public static class DocumentQueries
{
    public const string GetUsersPermissions = @"
        SELECT user_id
        FROM document_user_permissions
        WHERE document_id = @DocumentId
    ";
    
    public const string GetGroupsPermissions = @"
        SELECT group_id
        FROM document_group_permissions
        WHERE document_id = @DocumentId
    ";
    
    public const string InsertUsePermission = @"
        INSERT INTO document_user_permissions (document_id, user_id)
        VALUES (@DocumentId, @UserId);";
    
    public const string RemoveUserPermission = @"
        DELETE FROM document_user_permissions
        WHERE document_id = @DocumentId AND user_id = @UserId;";

    public const string InsertGroupPermission = @"
        INSERT INTO document_group_permissions (document_id, group_id)
        VALUES (@DocumentId, @GroupId);";
    
    public const string RemoveGroupPermission = @"
        DELETE FROM document_group_permissions
        WHERE document_id = @DocumentId AND group_id = @GroupId;";

    public const string GetDocumentById = @"
        SELECT *
        FROM documents
        WHERE id = @Id;
    ";

    public const string CheckUserDocumentPermission = @"
        SELECT EXISTS (
            SELECT 1 FROM documents
            WHERE id = @DocumentId
            AND (
                user_id = @UserId
                OR EXISTS (
                    SELECT 1 FROM document_user_permissions
                    WHERE document_id = @DocumentId
                    AND user_id = @UserId
                )
                OR EXISTS (
                    SELECT 1 FROM document_group_permissions
                    INNER JOIN user_groups ON user_groups.group_id = document_group_permissions.group_id
                    WHERE document_group_permissions.document_id = @DocumentId
                    AND user_groups.user_id = @UserId
                )
            )
        );
";

    public static readonly string ListUserAuthorizedDocuments = @"
    SELECT d.*
        FROM documents d
        INNER JOIN document_user_permissions dup ON d.id = dup.document_id
        INNER JOIN users u ON dup.user_id = u.id
        WHERE dup.user_id = @UserId
    UNION
        SELECT *
        FROM documents
        WHERE user_id = @UserId
    UNION
        SELECT d.*
        FROM documents d
            INNER JOIN document_group_permissions dgp ON d.id = dgp.document_id
            INNER JOIN user_groups gu ON dgp.group_id = gu.group_id
            INNER JOIN users u ON gu.user_id = u.id
        WHERE gu.user_id = @UserId
    ORDER BY posted_date DESC
    LIMIT @Take OFFSET @Skip;
";
}