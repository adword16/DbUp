using System;
using DbUp.Builder;
using DbUp.Engine.Locking;
using DbUp.MySql;
using DbUp.Support.MySql;
using MySql.Data.MySqlClient;

public class MySqlLockManager : ILockManager
{
    private UpgradeConfiguration _UpgradeConfiguration;
    private string _SchemaName;
    private MySqlConnection _MySqlLockConnection;
    

    public MySqlLockManager(UpgradeConfiguration upgradeConfiguration)
    {
        _UpgradeConfiguration = upgradeConfiguration;
    }

    public void Lock()
    {
        string lockTableName  = "mysql.plugin"; //its easier to use a existing table instead of creating one (in case of running two updates at same time you will still get an 'already exists' error)

        var conStr = ((MySqlConnectionManager) _UpgradeConfiguration.ConnectionManager).ConnectionString;

        _MySqlLockConnection = new MySqlConnection(conStr);;
        _MySqlLockConnection.Open();
        _UpgradeConfiguration.Log.WriteInformation("Waiting for lock...");
        
        using (var cmd = new MySqlCommand("LOCK TABLES {LOCK_TABLE_NAME} WRITE", _MySqlLockConnection))
        {
            cmd.ExecuteNonQuery();
        }

        _UpgradeConfiguration.Log.WriteInformation("Got lock.");

    }

    public void UnLock()
    {
        _UpgradeConfiguration.Log.WriteInformation("Releasing lock...");
        using (var cmd = new MySqlCommand("UNLOCK TABLES", _MySqlLockConnection))
        {
            cmd.ExecuteNonQuery();
        }
        _MySqlLockConnection.Close();
        _MySqlLockConnection.Dispose();

        _UpgradeConfiguration.Log.WriteInformation("Lock released.");

    }
}