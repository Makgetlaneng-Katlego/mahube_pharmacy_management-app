using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public class DashboardForm : Form
    {
        private DatabaseConnection _db = new DatabaseConnection();

        private Panel panelSide;
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblDate;
        private FlowLayoutPanel cardsPanel;

        private Label lblTotalProductsValue;
        private Label lblPendingOrdersValue;
        private Label lblTodaysConsultsValue;
        private Label lblNewPrescriptionsValue;

        private DataGridView dgvRecentOrders;
        private ListBox lstLowStock;
        private ListBox lstRecentActivity;
        private Button btnDashboard;
        private Button btnProducts;
        private Button btnOrders;
        private Button btnConsultations;
        private Button btnPrescriptions;
        private Button btnUsers;
        private Button btnLogout;

        public DashboardForm()
        {
            InitializeComponent();
            Load += DashboardForm_Load;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            Text = "Mahube Pharmacy - Dashboard";
            Size = new Size(1200, 720);
            BackColor = Color.FromArgb(245, 250, 245);
            Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Side panel
            panelSide = new Panel { BackColor = Color.FromArgb(46, 125, 50), Dock = DockStyle.Left, Width = 200 };

            // Logo/Title on side
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

            // Navigation buttons
            btnDashboard = CreateSideButton("📊 Dashboard", 0);
            btnProducts = CreateSideButton("📦 Products", 52);
            btnOrders = CreateSideButton("🛒 Orders", 104);
            btnConsultations = CreateSideButton("💬 Consultations", 156);
            btnPrescriptions = CreateSideButton("💊 Prescriptions", 208);
            btnUsers = CreateSideButton("👥 Users", 260);

            btnDashboard.Click += BtnDashboard_Click;
            btnProducts.Click += BtnProducts_Click;
            btnOrders.Click += BtnOrders_Click;
            btnConsultations.Click += BtnConsultations_Click;
            btnPrescriptions.Click += BtnPrescriptions_Click;
            btnUsers.Click += BtnUsers_Click;   

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

            panelSide.Controls.AddRange(new Control[] { lblLogo, btnDashboard, btnProducts, btnOrders, btnConsultations, btnPrescriptions, btnUsers, btnLogout });

            // Header panel
            panelHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.WhiteSmoke };
            lblTitle = new Label
            {
                Text = "Dashboard",
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
            panelHeader.Controls.AddRange(new Control[] { lblTitle, lblDate });

            // Cards panel
            cardsPanel = new FlowLayoutPanel
            {
                Location = new Point(220, 120),
                Size = new Size(940, 120),
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false
            };

            var card1 = CreateCard(out lblTotalProductsValue, "Total Products");
            var card2 = CreateCard(out lblPendingOrdersValue, "Pending Orders");
            var card3 = CreateCard(out lblTodaysConsultsValue, "Today's Consultations");
            var card4 = CreateCard(out lblNewPrescriptionsValue, "New Prescriptions");

            cardsPanel.Controls.AddRange(new Control[] { card1, card2, card3, card4 });

            // Recent orders grid
            dgvRecentOrders = new DataGridView
            {
                Location = new Point(220, 260),
                Size = new Size(700, 360),
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White
            };

            // Low stock list
            var lblLowStockTitle = new Label { Text = "Low Stock Alerts", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(940, 260), Size = new Size(200, 25) };
            lstLowStock = new ListBox
            {
                Location = new Point(940, 285),
                Size = new Size(220, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            // Recent activity list
            var lblActivityTitle = new Label { Text = "Recent Activity", Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(940, 445), Size = new Size(200, 25) };
            lstRecentActivity = new ListBox
            {
                Location = new Point(940, 470),
                Size = new Size(220, 150),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            Controls.AddRange(new Control[] { panelSide, panelHeader, cardsPanel, dgvRecentOrders, lblLowStockTitle, lstLowStock, lblActivityTitle, lstRecentActivity });

            // DataGridView styling
            dgvRecentOrders.EnableHeadersVisualStyles = false;
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRecentOrders.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvRecentOrders.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
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

        private Panel CreateCard(out Label valueLabel, string subtitle)
        {
            var panel = new Panel { Size = new Size(220, 100), Margin = new Padding(10), BackColor = Color.White };
            panel.BorderStyle = BorderStyle.FixedSingle;

            valueLabel = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.FromArgb(46, 125, 50),
                Location = new Point(15, 10),
                AutoSize = true
            };
            var lblSub = new Label
            {
                Text = subtitle,
                Font = new Font("Segoe UI", 9),
                ForeColor = Color.Gray,
                Location = new Point(15, 60),
                AutoSize = true
            };
            panel.Controls.AddRange(new Control[] { valueLabel, lblSub });
            return panel;
        }

        private void DashboardForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadMetrics();
                LoadRecentOrders();
                LoadLowStock();
                LoadRecentActivity();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load dashboard: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadMetrics()
        {
            try
            {
                // Connects to 'products' table
                var dt = _db.ExecuteQuery("SELECT COUNT(*) AS total FROM products");
                lblTotalProductsValue.Text = (dt.Rows.Count > 0) ? dt.Rows[0]["total"].ToString() : "0";

                // Connects to 'orders' table
                dt = _db.ExecuteQuery("SELECT COUNT(*) AS total FROM orders WHERE status = 'Pending'");
                lblPendingOrdersValue.Text = (dt.Rows.Count > 0) ? dt.Rows[0]["total"].ToString() : "0";

                // Connects to 'consultation' table (removed filter for current date)
                dt = _db.ExecuteQuery("SELECT COUNT(*) AS total FROM consultation");
                lblTodaysConsultsValue.Text = (dt.Rows.Count > 0) ? dt.Rows[0]["total"].ToString() : "0";

                // Connects to 'prescription' table (removed filter for current date and fixed column name)
                dt = _db.ExecuteQuery("SELECT COUNT(*) AS total FROM prescription");
                lblNewPrescriptionsValue.Text = (dt.Rows.Count > 0) ? dt.Rows[0]["total"].ToString() : "0";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading metrics: " + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadRecentOrders()
        {
            try
            {
                // Connects to 'orders' table
                var dt = _db.ExecuteQuery(@"SELECT order_id AS 'Order ID', customer_id AS 'Customer', order_date AS 'Date', total_amount AS 'Amount', status AS 'Status'
                                            FROM orders ORDER BY order_date DESC LIMIT 10");
                dgvRecentOrders.DataSource = dt;

                if (dgvRecentOrders.Columns.Contains("Date"))
                    dgvRecentOrders.Columns["Date"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm";
                if (dgvRecentOrders.Columns.Contains("Amount"))
                    dgvRecentOrders.Columns["Amount"].DefaultCellStyle.Format = "N2";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading recent orders: " + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void LoadLowStock()
        {
            try
            {
                lstLowStock.Items.Clear();
                // Connects to 'products' table using 'product_name' and 'stock_quantity'
                var dt = _db.ExecuteQuery("SELECT product_name, stock_quantity FROM products WHERE stock_quantity <= 10 ORDER BY stock_quantity ASC LIMIT 8");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        lstLowStock.Items.Add($"⚠️ {r["product_name"]} — {r["stock_quantity"]} left");
                    }
                }
                else
                {
                    lstLowStock.Items.Add("No low-stock items");
                }
            }
            catch (Exception ex)
            {
                lstLowStock.Items.Clear();
                lstLowStock.Items.Add("Error loading low stock");
            }
        }

        private void LoadRecentActivity()
        {
            try
            {
                lstRecentActivity.Items.Clear();
                // Connects to 'orders' table
                var dt = _db.ExecuteQuery("SELECT order_id, order_date FROM orders ORDER BY order_date DESC LIMIT 8");
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow r in dt.Rows)
                    {
                        var dtVal = Convert.ToDateTime(r["order_date"]);
                        lstRecentActivity.Items.Add($"Order {r["order_id"]} — {dtVal.ToString("MMM dd HH:mm")}");
                    }
                }
                else
                {
                    lstRecentActivity.Items.Add("No recent activity");
                }
            }
            catch (Exception ex)
            {
                lstRecentActivity.Items.Clear();
                lstRecentActivity.Items.Add("Error loading activity");
            }
        }

        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Already on Dashboard", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
            try
            {
                OrdersForm ordersForm = new OrdersForm();
                ordersForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open Orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
        private void BtnUsers_Click(object sender, EventArgs e)
        {
            try
            {
                UserManagementForm userForm = new UserManagementForm();
                userForm.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open User Management: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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