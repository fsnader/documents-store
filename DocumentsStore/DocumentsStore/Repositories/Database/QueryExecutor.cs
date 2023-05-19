using System.Data;
using DocumentsStore.Repositories.Exceptions;
using Npgsql;

namespace DocumentsStore.Repositories.Database;

public class QueryExecutor : IQueryExecutor
{
    private readonly IDbConnectionFactory _dbConnectionFactory;
    private readonly ILogger<IDbConnectionFactory> _logger;

    public QueryExecutor(IDbConnectionFactory dbConnectionFactory, ILogger<IDbConnectionFactory> logger)
    {
        _dbConnectionFactory = dbConnectionFactory;
        _logger = logger;
    }

    public async Task<T> ExecuteQueryAsync<T>(Func<IDbConnection, Task<T>> queryDelegate, CancellationToken cancellationToken)
    {
        using var connection = _dbConnectionFactory.GenerateConnection();

        try
        {
            return await queryDelegate(connection);
        }
        catch (PostgresException ex)
        {
            _logger.LogError(ex, "Error executing query");
            
            if (ex.SqlState == PostgresErrorCodes.UniqueViolation)
            {
                throw new UniqueException();
            }

            throw;
        }
    }
}