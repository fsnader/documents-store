using System.Data;

namespace DocumentsStore.Repositories.Database;

public interface IQueryExecutor
{
    public Task<T> ExecuteQueryAsync<T>(Func<IDbConnection, Task<T>> queryDelegate, CancellationToken cancellationToken);
}