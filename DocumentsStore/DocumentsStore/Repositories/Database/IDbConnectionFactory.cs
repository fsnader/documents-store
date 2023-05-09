using System.Data;

namespace DocumentsStore.Repositories.Database;

public interface IDbConnectionFactory
{
    public IDbConnection GenerateConnection();
}