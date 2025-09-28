using System.Data;

namespace AccessControl.DataAccess;

public interface IDbConnectionFactory
{
    IDbConnection GetConnection();
}
