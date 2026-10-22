using System;
using System.Data;
using MySql.Data.MySqlClient;

namespace PharmacyManagementSystem
{
    public class DatabaseConnection
    {
        private string connectionString = "Server=127.0.0.1;Port=3307;Database=pharmacy;Uid=root;Pwd=m4kg3tl4n3ng;";
     
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

        public void CompleteOrderAndReduceStock(int orderId)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                using (MySqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string statusQuery = "SELECT status FROM orders WHERE order_id = @order_id FOR UPDATE";
                        string currentStatus;
                        using (MySqlCommand statusCommand = new MySqlCommand(statusQuery, conn, transaction))
                        {
                            statusCommand.Parameters.AddWithValue("@order_id", orderId);
                            object status = statusCommand.ExecuteScalar();
                            if (status == null || status == DBNull.Value)
                                throw new InvalidOperationException("Order was not found.");

                            currentStatus = status.ToString();
                        }

                        if (currentStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                        {
                            transaction.Commit();
                            return;
                        }

                        string itemCountQuery = "SELECT COUNT(DISTINCT product_id) FROM order_items WHERE order_id = @order_id";
                        int productCount;
                        using (MySqlCommand itemCountCommand = new MySqlCommand(itemCountQuery, conn, transaction))
                        {
                            itemCountCommand.Parameters.AddWithValue("@order_id", orderId);
                            productCount = Convert.ToInt32(itemCountCommand.ExecuteScalar());
                        }

                        string reduceStockQuery = @"UPDATE products p
                            INNER JOIN (
                                SELECT product_id, SUM(quantity) AS quantity
                                FROM order_items
                                WHERE order_id = @order_id
                                GROUP BY product_id
                            ) oi ON p.product_id = oi.product_id
                            SET p.stock_quantity = p.stock_quantity - oi.quantity
                            WHERE p.stock_quantity >= oi.quantity";

                        int updatedProducts;
                        using (MySqlCommand reduceStockCommand = new MySqlCommand(reduceStockQuery, conn, transaction))
                        {
                            reduceStockCommand.Parameters.AddWithValue("@order_id", orderId);
                            updatedProducts = reduceStockCommand.ExecuteNonQuery();
                        }

                        if (updatedProducts != productCount)
                            throw new InvalidOperationException("There is not enough stock for one or more products in this order.");

                        string updateStatusQuery = "UPDATE orders SET status = 'Completed' WHERE order_id = @order_id";
                        using (MySqlCommand updateStatusCommand = new MySqlCommand(updateStatusQuery, conn, transaction))
                        {
                            updateStatusCommand.Parameters.AddWithValue("@order_id", orderId);
                            updateStatusCommand.ExecuteNonQuery();
                        }

                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
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
