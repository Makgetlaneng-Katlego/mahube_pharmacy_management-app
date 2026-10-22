using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PharmacyManagementSystem
{
    public class ProductsForm : Form
    {
        private DatabaseConnection _db = new DatabaseConnection();

        private Panel panelSide;
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblDate;
        private TextBox txtSearch;
        private Button btnAddProduct;
        private DataGridView dgvProducts;
        private Button btnDashboard;
        private Button btnProducts;
        private Button btnOrders;
        private Button btnConsultations;
        private Button btnPrescriptions;
        private Button btnLogout;

        public ProductsForm()
        {
            InitializeComponent();
            Load += ProductsForm_Load;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            Text = "Mahube Pharmacy - Products";
            Size = new Size(1200, 720);
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

            panelSide.Controls.AddRange(new Control[] { lblLogo, btnDashboard, btnProducts, btnOrders, btnConsultations, btnPrescriptions, btnUsers, btnLogout });

            // Header panel
            panelHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.WhiteSmoke };
            lblTitle = new Label
            {
                Text = "Products",
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
                Location = new Point(960, 20),
                AutoSize = true,
                ForeColor = Color.FromArgb(33, 33, 33)
            };
            var lblUserTitle = new Label
            {
                Text = "Pharmacist",
                Font = new Font("Segoe UI", 9),
                Location = new Point(985, 42),
                AutoSize = true,
                ForeColor = Color.Gray
            };

            panelHeader.Controls.AddRange(new Control[] { lblTitle, lblDate, lblUserRole, lblUserTitle });

            // Search and Add button
            txtSearch = new TextBox
            {
                Location = new Point(250, 130),
                Size = new Size(300, 35),
                Font = new Font("Segoe UI", 10),
                PlaceholderText = "Search products...",
                BorderStyle = BorderStyle.FixedSingle,
                Padding = new Padding(5)
            };
            txtSearch.TextChanged += TxtSearch_TextChanged;

            btnAddProduct = new Button
            {
                Text = "+ Add Product",
                Location = new Point(900, 130),
                Size = new Size(150, 35),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnAddProduct.FlatAppearance.BorderSize = 0;
            btnAddProduct.Click += BtnAddProduct_Click;

            // Product Inventory Label
            var lblInventory = new Label
            {
                Text = "Product Inventory",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                Location = new Point(220, 110),
                AutoSize = true,
                ForeColor = Color.FromArgb(46, 125, 50)
            };

            // DataGridView
            dgvProducts = new DataGridView
            {
                Location = new Point(220, 180),
                Size = new Size(940, 490),
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
            dgvProducts.Columns.Add("product_id", "Product ID");
            dgvProducts.Columns.Add("product_name", "Name");
            dgvProducts.Columns.Add("category_id", "Category");
            dgvProducts.Columns.Add("stock_quantity", "Stock");
            dgvProducts.Columns.Add("price", "Price");
            dgvProducts.Columns.Add("status", "Status");

            // Action buttons column
            var btnColumnEdit = new DataGridViewButtonColumn();
            btnColumnEdit.Name = "Edit";
            btnColumnEdit.HeaderText = "Actions";
            btnColumnEdit.Text = "✏️ Edit";
            btnColumnEdit.UseColumnTextForButtonValue = true;
            btnColumnEdit.Width = 80;
            btnColumnEdit.FlatStyle = FlatStyle.Flat;
            btnColumnEdit.DefaultCellStyle.BackColor = Color.White;
            dgvProducts.Columns.Add(btnColumnEdit);

            var btnColumnDelete = new DataGridViewButtonColumn();
            btnColumnDelete.Name = "Delete";
            btnColumnDelete.HeaderText = "";
            btnColumnDelete.Text = "🗑️ Delete";
            btnColumnDelete.UseColumnTextForButtonValue = true;
            btnColumnDelete.Width = 80;
            btnColumnDelete.FlatStyle = FlatStyle.Flat;
            dgvProducts.Columns.Add(btnColumnDelete);

            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvProducts.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(76, 175, 80);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProducts.CellClick += DgvProducts_CellClick;

            Controls.AddRange(new Control[] { panelSide, panelHeader, lblInventory, txtSearch, btnAddProduct, dgvProducts });
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

        private void ProductsForm_Load(object sender, EventArgs e)
        {
            try
            {
                LoadProducts();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadProducts(string searchTerm = "")
        {
            try
            {
                string query = @"SELECT product_id, product_name, category_id, stock_quantity, price FROM products";

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    query += " WHERE product_name LIKE @search OR product_id LIKE @search";
                }

                DataTable dt;
                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@search", "%" + searchTerm + "%")
                    };
                    dt = _db.ExecuteQuery(query, parameters);
                }
                else
                {
                    dt = _db.ExecuteQuery(query);
                }

                dgvProducts.Rows.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    int stock = Convert.ToInt32(row["stock_quantity"]);
                    decimal price = Convert.ToDecimal(row["price"]);
                    string status = stock <= 0 ? "Empty" : stock <= 10 ? "Low Stock" : "In Stock";

                    dgvProducts.Rows.Add(
                        row["product_id"].ToString(),
                        row["product_name"].ToString(),
                        row["category_id"].ToString(),
                        stock,
                        "$" + price.ToString("F2"),
                        status
                    );

                    // Color code the status
                    int rowIndex = dgvProducts.Rows.Count - 1;
                    var statusCell = dgvProducts.Rows[rowIndex].Cells["status"];
                    if (status == "Empty")
                    {
                        statusCell.Style.BackColor = Color.FromArgb(211, 47, 47);
                        statusCell.Style.ForeColor = Color.White;
                    }
                    else if (status == "Low Stock")
                    {
                        statusCell.Style.BackColor = Color.FromArgb(255, 193, 7);
                        statusCell.Style.ForeColor = Color.FromArgb(33, 33, 33);
                    }
                    else
                    {
                        statusCell.Style.BackColor = Color.FromArgb(76, 175, 80);
                        statusCell.Style.ForeColor = Color.White;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading products: " + ex.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void TxtSearch_TextChanged(object sender, EventArgs e)
        {
            LoadProducts(txtSearch.Text);
        }

        private void BtnAddProduct_Click(object sender, EventArgs e)
        {
            try
            {
                AddProductForm addForm = new AddProductForm();
                if (addForm.ShowDialog(this) == DialogResult.OK)
                {
                    LoadProducts();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open add product form: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            if (e.ColumnIndex == dgvProducts.Columns["Edit"].Index)
            {
                try
                {
                    int productId = Convert.ToInt32(dgvProducts.Rows[e.RowIndex].Cells["product_id"].Value);
                    using (AddProductForm editForm = new AddProductForm(productId))
                    {
                        if (editForm.ShowDialog(this) == DialogResult.OK)
                            LoadProducts(txtSearch.Text);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error opening product for update: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (e.ColumnIndex == dgvProducts.Columns["Delete"].Index)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this product?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    try
                    {
                        string productId = dgvProducts.Rows[e.RowIndex].Cells["product_id"].Value.ToString();
                        string query = "DELETE FROM products WHERE product_id = @product_id";
                        MySqlParameter[] parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@product_id", productId)
                        };
                        _db.ExecuteNonQuery(query, parameters);
                        MessageBox.Show("Product deleted successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        LoadProducts();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error deleting product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
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
            MessageBox.Show("Already on Products page", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
