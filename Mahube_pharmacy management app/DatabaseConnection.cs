using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PharmacyManagementSystem
{
    public class DatabaseConnection
    {
        private string connectionString = "Server=127.0.0.1;Port=3307;Database=pharmacy;Uid=root;Pwd=m4kg3tl4n3ng;";
        // Optional admin connection string used to apply GRANTs automatically when available.
        // Set via environment variable DB_ADMIN_CONNECTION when running with an account that can GRANT privileges.
        private readonly string adminConnectionString = Environment.GetEnvironmentVariable("DB_ADMIN_CONNECTION");

        public DataTable ExecuteQuery(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    return dt;
                }
            }
            catch (Exception ex)
            {
                // If it's a MySQL permission error, try to remediate automatically when possible
                if (ex is MySqlException mex && mex.Number == 1142) // ER_TABLEACCESS_DENIED_ERROR
                {
                    HandlePermissionErrorOrThrow(mex, query);
                }
                throw new Exception("Database query error: " + ex.Message);
            }
        }

        public void ExecuteNonQuery(string query, MySqlParameter[] parameters = null)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);

                    if (parameters != null)
                    {
                        cmd.Parameters.AddRange(parameters);
                    }

                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                if (ex is MySqlException mex && mex.Number == 1142) // ER_TABLEACCESS_DENIED_ERROR
                {
                    HandlePermissionErrorOrThrow(mex, query);
                }
                throw new Exception("Database execution error: " + ex.Message);
            }
        }

        private void HandlePermissionErrorOrThrow(MySqlException mex, string originalQuery)
        {
            try
            {
                // Parse current connection for database and user
                var builder = new MySqlConnectionStringBuilder(connectionString);
                string db = builder.Database;
                string appUser = builder.UserID;
                string appHost = builder.Server;

                // Determine which privileges are needed based on the SQL command
                string trimmed = originalQuery.TrimStart();
                string needed = "SELECT, INSERT, UPDATE, DELETE";
                if (trimmed.StartsWith("DELETE", StringComparison.OrdinalIgnoreCase)) needed = "DELETE";
                else if (trimmed.StartsWith("INSERT", StringComparison.OrdinalIgnoreCase)) needed = "INSERT";
                else if (trimmed.StartsWith("UPDATE", StringComparison.OrdinalIgnoreCase)) needed = "UPDATE";
                else if (trimmed.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)) needed = "SELECT";

                string grantSql = $"GRANT {needed} ON `{db}`.* TO '{appUser}'@'{appHost}'; FLUSH PRIVILEGES;";

                if (!string.IsNullOrWhiteSpace(adminConnectionString))
                {
                    // Attempt to apply the grant using the admin connection string
                    using (var adminConn = new MySqlConnection(adminConnectionString))
                    {
                        adminConn.Open();
                        using (var grantCmd = new MySqlCommand(grantSql, adminConn))
                        {
                            grantCmd.ExecuteNonQuery();
                        }
                    }

                    // After granting, return to let the caller retry the original operation.
                    return;
                }

                // If we cannot apply the grant, throw a clearer exception with remediation steps
                throw new Exception($"Database permission denied: {mex.Message}.\nSuggested SQL (run as DBA): {grantSql}");
            }
            catch (Exception inner)
            {
                // If remediation failed, rethrow an informative exception
                throw new Exception("Failed to remediate database permission error: " + inner.Message);
            }
        }
    }
}
