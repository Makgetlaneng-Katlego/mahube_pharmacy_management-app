using Microsoft.VisualBasic.ApplicationServices;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public class OrdersForm : Form
    {
        private DatabaseConnection _db = new DatabaseConnection();

        private Panel panelSide;
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblDate;
        private ComboBox cmbStatus;
        private TextBox txtSearch;
        private DataGridView dgvOrders;
        private Button btnDashboard;
        private Button btnProducts;
        private Button btnOrders;
        private Button btnConsultations;
        private Button btnPrescriptions;
        private Button btnLogout;

        public OrdersForm()
        {
            InitializeComponent();
            Load += OrdersForm_Load;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            Text = "Mahube Pharmacy - Orders";
            Size = new Size(1400, 720);
            BackColor = Color.FromArgb(245, 250, 245);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Side panel
            panelSide = new Panel { BackColor = Color.FromArgb(46, 125, 50), Dock = DockStyle.Left, Width = 200 };

            var lblLogo = new Label
            {
                Text = "☘ Mahube Pharmacy",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Location = new Point(10, 15),
                Size = new Size(180, 40),
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Top,
                Height = 60
            };

            btnDashboard = CreateSideButton("📊 Dashboard", 0);
            btnProducts = CreateSideButton("📦 Products", 52);
            btnOrders = CreateSideButton("🛒 Orders", 104);
            btnConsultations = CreateSideButton("💬 Consultations", 156);
            btnPrescriptions = CreateSideButton("💊 Prescriptions", 208);
            var btnUsers = CreateSideButton("👥 Users", 260);
            var btnSettings = CreateSideButton("⚙️ Settings", 312);

            btnDashboard.Click += BtnDashboard_Click;
            btnProducts.Click += BtnProducts_Click;
            btnOrders.Click += BtnOrders_Click;
            btnConsultations.Click += BtnConsultations_Click;
            btnPrescriptions.Click += BtnPrescriptions_Click;

            btnLogout = new Button
            {
                Text = "🚪 Logout",
                Dock = DockStyle.Bottom,
                Height = 52,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.FromArgb(211, 47, 47),
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnLogout.FlatAppearance.BorderSize = 0;
            btnLogout.Click += BtnLogout_Click;

            panelSide.Controls.AddRange(new Control[] { lblLogo, btnDashboard, btnProducts, btnOrders, btnConsultations, btnPrescriptions, btnUsers, btnSettings, btnLogout });

            // Header panel
            panelHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.WhiteSmoke };
            lblTitle = new Label
            {
                Text = "Orders",
                Font = new Font("Segoe UI", 26, FontStyle.Bold),
                Location = new Point(220, 12),
                AutoSize = true,
                ForeColor = Color.FromArgb(46, 125, 50)
            };
            lblDate = new Label
            {
                Text = DateTime.Now.ToString("dddd, MMM d, yyyy"),
                Font = new Font("Segoe UI", 10),
                Location = new Point(220, 58),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            // User info in top right
            var lblUserRole = new Label
            {
                Text = "Admin User",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(1260, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(33, 33, 33)
            };
            var lblUserTitle = new Label
            {
                Text = "Pharmacist",
                Font = new Font("Segoe UI", 9),
                Location = new Point(1285, 42),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            panelHeader.Controls.AddRange(new Control[] { lblTitle, lblDate, lblUserRole, lblUserTitle });

            // All Orders Label
            var lblAllOrders = new Label
            {
                Text = "All Orders",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(220, 120),
                AutoSize = true,
                ForeColor = Color.FromArgb(46, 125, 50)
            };

            // Status Filter
            var lblStatus = new Label
            {
                Text = "All Status",
                Font = new Font("Segoe UI", 9),
                Location = new Point(1150, 125),
                AutoSize = true,
                ForeColor = Color.FromArgb(66, 133, 66)
            };
            cmbStatus = new ComboBox
            {
                Location = new Point(1210, 120),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Padding = new Padding(5)
            };
            cmbStatus.Items.AddRange(new string[] { "All Status", "Pending", "Processing", "Shipped", "Delivered", "Completed", "Cancelled" });
            cmbStatus.SelectedIndex = 0;
            cmbStatus.SelectedIndexChanged += CmbStatus_SelectedIndexChanged;

            // Search box
            txtSearch = new TextBox
            {
                Location = new Point(1050, 130),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Search orders...",
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            // DataGridView
            dgvOrders = new DataGridView
            {
                Location = new Point(220, 170),
                Size = new Size(1140, 510),
                ReadOnly = false,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                GridColor = Color.FromArgb(200, 230, 201),
                RowTemplate = { Height = 40 }
            };

            // Add columns
            dgvOrders.Columns.Add("order_id", "Order ID");
            dgvOrders.Columns.Add("customer_id", "Customer");
            dgvOrders.Columns.Add("order_date", "Date");
            dgvOrders.Columns.Add("item_count", "Items");
            dgvOrders.Columns.Add("total_amount", "Total");
            dgvOrders.Columns.Add("status", "Status");

            // Action button column (View)
            var btnColumnView = new DataGridViewButtonColumn();
            btnColumnView.Name = "View";
            btnColumnView.HeaderText = "Actions";
            btnColumnView.Text = "👁️ View";
            btnColumnView.UseColumnTextForButtonValue = true;
            btnColumnView.Width = 80;
            btnColumnView.FlatStyle = FlatStyle.Flat;
            dgvOrders.Columns.Add(btnColumnView);

            // NEW: Update Status button column
            var btnColumnUpdate = new DataGridViewButtonColumn();
            btnColumnUpdate.Name = "UpdateStatus";
            btnColumnUpdate.HeaderText = "Update Status";
            btnColumnUpdate.Text = "🔄 Update";
            btnColumnUpdate.UseColumnTextForButtonValue = true;
            btnColumnUpdate.Width = 100;
            btnColumnUpdate.FlatStyle = FlatStyle.Flat;
            dgvOrders.Columns.Add(btnColumnUpdate);

            dgvOrders.EnableHeadersVisualStyles = false;
            dgvOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvOrders.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            dgvOrders.DefaultCellStyle.SelectionBackColor = Color.FromArgb(76, 175, 80);
            dgvOrders.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvOrders.CellClick += DgvOrders_CellClick;

            Controls.AddRange(new Control[] { panelSide, panelHeader, lblAllOrders, lblStatus, cmbStatus, txtSearch, dgvOrders });
        }

        private Button CreateSideButton(string text, int yOffset)
        {
            var btn = new Button
            {
                Text = text,
                Location = new Point(0, yOffset + 60),
                Width = 200,
                Height = 52,
                FlatStyle = FlatStyle.Flat,
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            btn.FlatAppearance.BorderSize = 0;
            btn.MouseEnter += (s, e) => btn.BackColor = Color.FromArgb(56, 142, 60);
            btn.MouseLeave += (s, e) => btn.BackColor = Color.Transparent;
            return btn;
        }

        private void OrdersForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadOrders();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadOrders(string searchTerm = "", string statusFilter = "All Status")
        {
            try
            {

                string query = @"SELECT 
                            o.order_id,
                            COALESCE(u.fullname, CAST(o.customer_id AS CHAR)) AS customer_id,
                            o.order_date,
                            COUNT(DISTINCT oi.order_item_id) AS item_count,
                            o.total_amount,
                            o.status
                        FROM orders o
                        LEFT JOIN order_items oi ON o.order_id = oi.order_id
                        LEFT JOIN users u ON o.customer_id = u.user_id
                        WHERE 1 = 1 ";


                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query += " AND (CAST(o.order_id AS CHAR) LIKE @search OR u.fullname LIKE @search)";
                }

                if (statusFilter != "All Status")
                {
                    query += " AND o.status = @status";
                }

                query += " GROUP BY o.order_id, u.fullname, o.order_date, o.total_amount, o.status";
                query += " ORDER BY o.order_date DESC";

                DataTable dt;
                if (!string.IsNullOrWhiteSpace(searchTerm) && statusFilter != "All Status")
                {
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@search", "%" + searchTerm + "%"),
                        new MySqlParameter("@status", statusFilter)
                    };
                    dt = _db.ExecuteQuery(query, parameters);
                }
                else if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@search", "%" + searchTerm + "%")
                    };
                    dt = _db.ExecuteQuery(query, parameters);
                }
                else if (statusFilter != "All Status")
                {
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@status", statusFilter)
                    };
                    dt = _db.ExecuteQuery(query, parameters);
                }
                else
                {
                    dt = _db.ExecuteQuery(query);
                }

                dgvOrders.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    string orderId = "#ORD-" + row["order_id"].ToString().PadLeft(3, '0');
                    string customerId = row["customer_id"].ToString();
                    DateTime orderDate = Convert.ToDateTime(row["order_date"]);
                    int itemCount = Convert.ToInt32(row["item_count"]);
                    decimal total = Convert.ToDecimal(row["total_amount"]);
                    string status = row["status"].ToString();

                    dgvOrders.Rows.Add(
                        orderId,
                        customerId,
                        orderDate.ToString("MMM d, yyyy"),
                        itemCount + " item" + (itemCount != 1 ? "s" : ""),
                        "$" + total.ToString("F2"),
                        status
                    );

                    // Color code the status
                    int rowIndex = dgvOrders.Rows.Count - 1;
                    var statusCell = dgvOrders.Rows[rowIndex].Cells["status"];

                    switch (status.ToLower())
                    {
                        case "completed":
                            statusCell.Style.BackColor = Color.FromArgb(76, 175, 80);
                            statusCell.Style.ForeColor = Color.White;
                            break;
                        case "pending":
                            statusCell.Style.BackColor = Color.FromArgb(255, 193, 7);
                            statusCell.Style.ForeColor = Color.FromArgb(33, 33, 33);
                            break;
                        case "processing":
                            statusCell.Style.BackColor = Color.FromArgb(33, 150, 243);
                            statusCell.Style.ForeColor = Color.White;
                            break;
                        case "shipped":
                            statusCell.Style.BackColor = Color.FromArgb(156, 39, 176);
                            statusCell.Style.ForeColor = Color.White;
                            break;
                        case "delivered":
                            statusCell.Style.BackColor = Color.FromArgb(76, 175, 80);
                            statusCell.Style.ForeColor = Color.White;
                            break;
                        case "cancelled":
                            statusCell.Style.BackColor = Color.FromArgb(211, 47, 47);
                            statusCell.Style.ForeColor = Color.White;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading orders: " + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadOrders(txtSearch.Text, cmbStatus.SelectedItem?.ToString() ?? "All Status");
        }

        private void CmbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadOrders(txtSearch.Text, cmbStatus.SelectedItem?.ToString() ?? "All Status");
        }

        private void DgvOrders_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvOrders.Columns["View"].Index)
            {
                try
                {
                    string orderId = dgvOrders.Rows[e.RowIndex].Cells["order_id"].Value.ToString();
                    // Remove the "#ORD-" prefix to get the actual ID
                    orderId = orderId.Replace("#ORD-", "");
                    MessageBox.Show($"Order Details for {orderId}\n\nFull order details view coming soon.", "Order Details", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error viewing order: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            // NEW: Handle Update Status Button Click
            else if (e.ColumnIndex == dgvOrders.Columns["UpdateStatus"].Index)
            {
                try
                {
                    string orderIdStr = dgvOrders.Rows[e.RowIndex].Cells["order_id"].Value.ToString();
                    int orderId = int.Parse(orderIdStr.Replace("#ORD-", ""));
                    string currentStatus = dgvOrders.Rows[e.RowIndex].Cells["status"].Value.ToString();

                    // Show dialog to get new status
                    string newStatus = ShowStatusUpdateDialog(currentStatus);

                    // If a new status was selected and it's different from the current one
                    if (!string.IsNullOrEmpty(newStatus) && newStatus != currentStatus)
                    {
                        UpdateOrderStatus(orderId, newStatus);
                        // Refresh the grid to show the updated status and colors
                        LoadOrders(txtSearch.Text, cmbStatus.SelectedItem?.ToString() ?? "All Status");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error updating status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // NEW: Method to display the status update dialog
        private string ShowStatusUpdateDialog(string currentStatus)
        {
            Form dialog = new Form()
            {
                Text = "Update Order Status",
                Size = new Size(320, 180),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false,
                BackColor = Color.White
            };

            Label lbl = new Label()
            {
                Text = "Select New Status:",
                Location = new Point(20, 20),
                AutoSize = true,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };

            ComboBox cmb = new ComboBox()
            {
                Location = new Point(20, 50),
                Width = 260,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10)
            };
            cmb.Items.AddRange(new string[] { "Pending", "Processing", "Shipped", "Delivered", "Completed", "Cancelled" });
            cmb.SelectedItem = currentStatus;

            Button btnOk = new Button()
            {
                Text = "Update",
                DialogResult = DialogResult.OK,
                Location = new Point(110, 100),
                Width = 80,
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat
            };
            btnOk.FlatAppearance.BorderSize = 0;

            Button btnCancel = new Button()
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Location = new Point(200, 100),
                Width = 80,
                FlatStyle = FlatStyle.Flat
            };
            btnCancel.FlatAppearance.BorderColor = Color.Gray;

            dialog.Controls.Add(lbl);
            dialog.Controls.Add(cmb);
            dialog.Controls.Add(btnOk);
            dialog.Controls.Add(btnCancel);
            dialog.AcceptButton = btnOk;
            dialog.CancelButton = btnCancel;

            if (dialog.ShowDialog(this) == DialogResult.OK)
            {
                return cmb.SelectedItem?.ToString();
            }
            return null;
        }

        // NEW: Method to execute the database update
        private void UpdateOrderStatus(int orderId, string newStatus)
        {
            try
            {
                string query = "UPDATE orders SET status = @status WHERE order_id = @orderId";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@status", newStatus),
                    new MySqlParameter("@orderId", orderId)
                };

                // Note: Ensure your DatabaseConnection class has an ExecuteNonQuery method.
                // If it uses a different method name for updates, adjust it here.
                _db.ExecuteNonQuery(query, parameters);

                MessageBox.Show("Order status updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            this.Close();
        }

        private void BtnProducts_Click(object sender, EventArgs e)
        {
            try
            {
                ProductsForm productsForm = new ProductsForm();
                productsForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnOrders_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Already on Orders page", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnConsultations_Click(object sender, EventArgs e)
        {
            try
            {
                ConsultationForm form = new ConsultationForm();
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Consultations: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnPrescriptions_Click(object sender, EventArgs e)
        {
            try
            {
                PrescriptionForm form = new PrescriptionForm();
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Prescriptions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                LoginForm loginForm = new LoginForm();
                loginForm.Show();
                this.Close();
            }
        }
    }
}
