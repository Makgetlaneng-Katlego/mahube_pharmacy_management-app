using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PharmacyManagementSystem
{
    public partial class frmMain : Form
    {
        private DatabaseConnection db = new DatabaseConnection();
        private string currentImagePath = null;
        // kept for compatibility with previous runtime edits - not used when image path is stored
        private byte[] currentImage = null;
        private bool isOrdersViewActive = false;

        public frmMain()
        {
            InitializeComponent();
            ApplyTheme();
            LoadProducts();
            LoadCategories();
        }

        private void btnRefreshProducts_Click(object sender, EventArgs e)
        {
            try
            {
                // If currently viewing orders, switch to products view before refresh
                isOrdersViewActive = false;
                dgvOrderItems.Visible = false;
                LoadProducts();
                dgvProducts.Height = panelTop.Height;
                MessageBox.Show("Products refreshed.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to refresh products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportProduct_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Please select a product to export.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var row = dgvProducts.CurrentRow;
                var productIdObj = row.Cells["product_id"].Value;
                if (productIdObj == null)
                {
                    MessageBox.Show("Selected row does not contain a product id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int productId = Convert.ToInt32(productIdObj);
                string name = dgvProducts.Columns.Contains("product_name") && row.Cells["product_name"].Value != null ? row.Cells["product_name"].Value.ToString() : string.Empty;
                string description = dgvProducts.Columns.Contains("description") && row.Cells["description"].Value != null ? row.Cells["description"].Value.ToString() : string.Empty;
                decimal price = 0;
                if (dgvProducts.Columns.Contains("price") && row.Cells["price"].Value != null)
                {
                    decimal.TryParse(row.Cells["price"].Value.ToString(), out price);
                }
                int stock = 0;
                if (dgvProducts.Columns.Contains("stock_quantity") && row.Cells["stock_quantity"].Value != null)
                {
                    int.TryParse(row.Cells["stock_quantity"].Value.ToString(), out stock);
                }
                int categoryId = 0;
                if (dgvProducts.Columns.Contains("category_id") && row.Cells["category_id"].Value != null)
                {
                    int.TryParse(row.Cells["category_id"].Value.ToString(), out categoryId);
                }

                // image path stored in Tag
                string imagePath = row.Tag as string;

                string query = @"INSERT INTO one_products_view (product_id, product_name, description, price, stock_quantity, category_id, image)
                                VALUES (@product_id, @product_name, @description, @price, @stock_quantity, @category_id, @image)
                                ON DUPLICATE KEY UPDATE product_name = VALUES(product_name), description = VALUES(description), price = VALUES(price), stock_quantity = VALUES(stock_quantity), category_id = VALUES(category_id), image = VALUES(image)";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", productId),
                    new MySqlParameter("@product_name", name),
                    new MySqlParameter("@description", description),
                    new MySqlParameter("@price", price),
                    new MySqlParameter("@stock_quantity", stock),
                    new MySqlParameter("@category_id", categoryId),
                    new MySqlParameter("@image", (object)imagePath ?? DBNull.Value)
                };

                db.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Product exported to one_products_view.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to export product: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnPrescriptions_Click(object sender, EventArgs e)
        {
            try
            {
                PrescriptionForm form = new PrescriptionForm();
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open prescriptions: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtProductName.ReadOnly = false;
            txtProductName.Enabled = true;
            txtProductName.Focus();
        }

        private void btnUpdateStatus_Click(object sender, EventArgs e)
        {
            if (!isOrdersViewActive)
            {
                MessageBox.Show("Switch to Orders view to update status.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (dgvProducts.CurrentRow == null)
            {
                MessageBox.Show("Please select an order.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (cbOrderStatus.SelectedItem == null)
            {
                MessageBox.Show("Please select a status.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idObj = dgvProducts.CurrentRow.Cells["order_id"].Value;
            if (idObj == null)
            {
                MessageBox.Show("Selected row has no order id.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int orderId = Convert.ToInt32(idObj);
            string newStatus = cbOrderStatus.SelectedItem.ToString();

            try
            {
                if (newStatus.Equals("Completed", StringComparison.OrdinalIgnoreCase))
                {
                    db.CompleteOrderAndReduceStock(orderId);
                    MessageBox.Show("Order completed and stock updated successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadOrders();
                    return;
                }

                string query = "UPDATE orders SET status = @status WHERE order_id = @id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@status", newStatus),
                    new MySqlParameter("@id", orderId)
                };

                db.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Order status updated.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // If order moved to a state where stock should be reduced, call stored procedure
                try
                {
                    if (!string.IsNullOrWhiteSpace(newStatus) &&
                        (newStatus.Equals("Shipped", StringComparison.OrdinalIgnoreCase)
                         || newStatus.Equals("Ready for collection", StringComparison.OrdinalIgnoreCase)))
                    {
                        string procCall = "CALL ReduceStockForOrder(@order_id)";
                        MySqlParameter[] procParams = new MySqlParameter[]
                        {
                            new MySqlParameter("@order_id", orderId)
                        };

                        db.ExecuteNonQuery(procCall, procParams);
                        MessageBox.Show("Stock reduced for order " + orderId + ".", "Inventory Updated", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                catch (Exception exProc)
                {
                    // Inform the user but do not roll back the status update here
                    MessageBox.Show("Failed to reduce stock for order: " + exProc.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                // reload orders to reflect change
                LoadOrders();
                // reselect the updated order
                foreach (DataGridViewRow row in dgvProducts.Rows)
                {
                    if (row.Cells["order_id"].Value != null && Convert.ToInt32(row.Cells["order_id"].Value) == orderId)
                    {
                        row.Selected = true;
                        dgvProducts.CurrentCell = row.Cells[0];
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to update status: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnConsultations_Click(object sender, EventArgs e)
        {
            try
            {
                ConsultationForm form = new ConsultationForm();
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog(this);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to open consultations: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowOrders_Click(object sender, EventArgs e)
        {
            try
            {
                LoadOrders();
                isOrdersViewActive = true;
                dgvOrderItems.Visible = true;
                dgvProducts.Height = 220;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load orders: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnShowProducts_Click(object sender, EventArgs e)
        {
            try
            {
                LoadProducts();
                isOrdersViewActive = false;
                dgvOrderItems.Visible = false;
                dgvProducts.Height = panelTop.Height; // expand to fill top area
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load products: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Load order items for selected order from the consolidated view (order_items_full_view)
        private void LoadOrderItems(int orderId)
        {
            // order_items_full_view should expose: order_item_id, order_id, product_id, product_name (optional), quantity
            string query = @"SELECT order_item_id, order_id, product_id, product_name, quantity FROM order_items_full_view WHERE order_id = @order_id ORDER BY order_id, order_item_id";
            MySqlParameter[] parameters = new MySqlParameter[]
            {
                new MySqlParameter("@order_id", orderId)
            };

            DataTable dt = db.ExecuteQuery(query, parameters);
            dgvOrderItems.DataSource = dt;

            // header adjustments
            if (dgvOrderItems.Columns.Contains("order_item_id")) dgvOrderItems.Columns["order_item_id"].HeaderText = "Item ID";
            if (dgvOrderItems.Columns.Contains("order_id")) dgvOrderItems.Columns["order_id"].HeaderText = "Order ID";
            if (dgvOrderItems.Columns.Contains("product_name"))
                dgvOrderItems.Columns["product_name"].HeaderText = "Product";
            else if (dgvOrderItems.Columns.Contains("product_id"))
                dgvOrderItems.Columns["product_id"].HeaderText = "Product ID";
            if (dgvOrderItems.Columns.Contains("quantity")) dgvOrderItems.Columns["quantity"].HeaderText = "Quantity";

            foreach (DataGridViewColumn col in dgvOrderItems.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        // New: Load orders view
        private void LoadOrders()
        {
            // Use the consolidated view which includes order and item details
            string query = @"SELECT order_id, customer_id, order_date, total_amount, status, shipping_address, payment_method,
                                   order_item_id, quantity, product_id, category_id, product_name
                            FROM order_items_full_view
                            ORDER BY order_date DESC, order_id, order_item_id";

            DataTable dt = db.ExecuteQuery(query);
            dgvProducts.DataSource = dt;

            // Adjust header text and formatting to match requested view
            if (dgvProducts.Columns.Contains("order_id")) dgvProducts.Columns["order_id"].HeaderText = "Order ID";
            if (dgvProducts.Columns.Contains("customer_id")) dgvProducts.Columns["customer_id"].HeaderText = "Customer ID";
            if (dgvProducts.Columns.Contains("order_date"))
            {
                dgvProducts.Columns["order_date"].HeaderText = "Order Date";
                dgvProducts.Columns["order_date"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm";
            }
            if (dgvProducts.Columns.Contains("total_amount"))
            {
                dgvProducts.Columns["total_amount"].HeaderText = "Total";
                dgvProducts.Columns["total_amount"].DefaultCellStyle.Format = "N2";
                dgvProducts.Columns["total_amount"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleRight;
            }
            if (dgvProducts.Columns.Contains("status")) dgvProducts.Columns["status"].HeaderText = "Status";
            if (dgvProducts.Columns.Contains("shipping_address")) dgvProducts.Columns["shipping_address"].HeaderText = "Shipping Address";
            if (dgvProducts.Columns.Contains("payment_method")) dgvProducts.Columns["payment_method"].HeaderText = "Payment";
            if (dgvProducts.Columns.Contains("order_item_id")) dgvProducts.Columns["order_item_id"].HeaderText = "Item ID";
            if (dgvProducts.Columns.Contains("quantity")) dgvProducts.Columns["quantity"].HeaderText = "Quantity";
            if (dgvProducts.Columns.Contains("product_id")) dgvProducts.Columns["product_id"].HeaderText = "Product ID";
            if (dgvProducts.Columns.Contains("category_id")) dgvProducts.Columns["category_id"].HeaderText = "Category ID";
            if (dgvProducts.Columns.Contains("product_name")) dgvProducts.Columns["product_name"].HeaderText = "Product";

            foreach (DataGridViewColumn col in dgvProducts.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }

            dgvProducts.Height = 220;
            dgvProducts.ClearSelection();
            if (dgvProducts.Rows.Count > 0)
            {
                dgvProducts.Rows[0].Selected = true;
                if (dgvProducts.Columns.Contains("order_id") && dgvProducts.Rows[0].Cells["order_id"].Value != null)
                {
                    int firstOrderId = Convert.ToInt32(dgvProducts.Rows[0].Cells["order_id"].Value);
                    LoadOrderItems(firstOrderId);
                    dgvOrderItems.Visible = true;
                }
            }

            // Populate status dropdown with common statuses and enable; keep in sync with selected order
            cbOrderStatus.Items.Clear();
            cbOrderStatus.Items.AddRange(new string[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" });
            cbOrderStatus.Enabled = dgvProducts.Rows.Count > 0;
        }

        private void ApplyTheme()
        {
            // Green and Orange Theme matching the mortar & pestle image
            this.BackColor = Color.FromArgb(245, 250, 245);

            // Header Panel - Dark Green
            panelHeader.BackColor = Color.FromArgb(46, 125, 50);
            lblTitle.ForeColor = Color.White;

            // Side Panel - Medium Green
            panelSide.BackColor = Color.FromArgb(76, 175, 80);

            // Buttons - Orange
            btnAdd.BackColor = Color.FromArgb(255, 143, 0);
            btnEdit.BackColor = Color.FromArgb(255, 143, 0);
            btnDelete.BackColor = Color.FromArgb(211, 47, 47);
            btnCreate.BackColor = Color.FromArgb(46, 125, 50);
            btnSave.BackColor = Color.FromArgb(46, 125, 50);

            // Navigation buttons
            btnShowOrders.BackColor = Color.FromArgb(46, 125, 50);
            btnShowProducts.BackColor = Color.FromArgb(255, 143, 0);
            btnShowOrders.ForeColor = Color.White;
            btnShowProducts.ForeColor = Color.White;
            btnShowOrders.FlatStyle = FlatStyle.Flat;
            btnShowProducts.FlatStyle = FlatStyle.Flat;
            btnShowOrders.FlatAppearance.BorderSize = 0;
            btnShowProducts.FlatAppearance.BorderSize = 0;

            foreach (Button btn in new Button[] { btnAdd, btnEdit, btnDelete, btnCreate, btnSave })
            {
                btn.ForeColor = Color.White;
                btn.FlatStyle = FlatStyle.Flat;
                btn.FlatAppearance.BorderSize = 0;
            }

            // DataGridView styling
            dgvProducts.BackgroundColor = Color.White;
            dgvProducts.EnableHeadersVisualStyles = false;
            dgvProducts.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvProducts.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvProducts.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvProducts.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            dgvProducts.DefaultCellStyle.SelectionBackColor = Color.FromArgb(76, 175, 80);
            dgvProducts.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvProducts.RowHeadersVisible = false;
            dgvProducts.GridColor = Color.FromArgb(200, 230, 201);

            // Order items grid styling (when visible)
            dgvOrderItems.BackgroundColor = Color.White;
            dgvOrderItems.EnableHeadersVisualStyles = false;
            dgvOrderItems.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvOrderItems.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvOrderItems.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvOrderItems.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            dgvOrderItems.DefaultCellStyle.SelectionBackColor = Color.FromArgb(76, 175, 80);
            dgvOrderItems.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvOrderItems.RowHeadersVisible = false;
            dgvOrderItems.GridColor = Color.FromArgb(200, 230, 201);
        }

        private void LoadCategories()
        {
            string query = "SELECT category_id, category_name FROM product_categories";
            DataTable dt = db.ExecuteQuery(query);
            cmbCategory.DisplayMember = "category_name";
            cmbCategory.ValueMember = "category_id";
            cmbCategory.DataSource = dt;
        }

        private void LoadProducts()
        {
            // products.image now stores a file path to the image
            string query = @"SELECT product_id, product_name, description, price, 
                           stock_quantity, category_id, image FROM products";
            DataTable dt = db.ExecuteQuery(query);
            dgvProducts.DataSource = dt;
            if (dgvProducts.Columns.Contains("category_id")) dgvProducts.Columns["category_id"].Visible = false;
            if (dgvProducts.Columns.Contains("image")) dgvProducts.Columns["image"].Visible = false;

            // assign image path to row.Tag to avoid showing path in UI
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var row = dgvProducts.Rows[i];
                if (dt.Rows[i]["image"] != DBNull.Value)
                {
                    row.Tag = dt.Rows[i]["image"].ToString();
                }
                else
                {
                    row.Tag = null;
                }
            }

            // Auto-size columns
            foreach (DataGridViewColumn col in dgvProducts.Columns)
            {
                col.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            }
        }

        private void dgvProducts_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow == null) return;

            if (isOrdersViewActive)
            {
                // When on Orders view, treat dgvProducts as orders grid and load order items
                if (dgvProducts.Columns.Contains("order_id") && dgvProducts.CurrentRow.Cells["order_id"].Value != null)
                {
                    int orderId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["order_id"].Value);
                    LoadOrderItems(orderId);
                    // set status combobox to the current order's status if available
                    if (dgvProducts.Columns.Contains("status") && dgvProducts.CurrentRow.Cells["status"].Value != null)
                    {
                        string status = dgvProducts.CurrentRow.Cells["status"].Value.ToString();
                        if (cbOrderStatus.Items.Contains(status)) cbOrderStatus.SelectedItem = status;
                        else cbOrderStatus.SelectedIndex = -1;
                    }
                    cbOrderStatus.Enabled = true;
                    btnUpdateStatus.Enabled = true;
                }
                return;
            }

            // Otherwise treat as product selection
            if (dgvProducts.Columns.Contains("product_name") && dgvProducts.CurrentRow.Cells["product_name"].Value != null)
                txtProductName.Text = dgvProducts.CurrentRow.Cells["product_name"].Value.ToString();
            if (dgvProducts.Columns.Contains("description") && dgvProducts.CurrentRow.Cells["description"].Value != null)
                txtDescription.Text = dgvProducts.CurrentRow.Cells["description"].Value.ToString();
            if (dgvProducts.Columns.Contains("price") && dgvProducts.CurrentRow.Cells["price"].Value != null)
                txtPrice.Text = dgvProducts.CurrentRow.Cells["price"].Value.ToString();
            if (dgvProducts.Columns.Contains("stock_quantity") && dgvProducts.CurrentRow.Cells["stock_quantity"].Value != null)
                txtStock.Text = dgvProducts.CurrentRow.Cells["stock_quantity"].Value.ToString();

            if (dgvProducts.Columns.Contains("category_id") && dgvProducts.CurrentRow.Cells["category_id"].Value != null)
            {
                int categoryId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["category_id"].Value);
                cmbCategory.SelectedValue = categoryId;
            }

            // Load image path from Tag
            var imagePath = dgvProducts.CurrentRow.Tag as string;
            if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
            {
                try
                {
                    using (var img = Image.FromFile(imagePath))
                    {
                        picProductImage.Image = new Bitmap(img);
                    }
                    currentImagePath = imagePath;
                }
                catch
                {
                    picProductImage.Image = null;
                    currentImagePath = null;
                }
            }
            else
            {
                picProductImage.Image = null;
                currentImagePath = null;
            }
        }

        private void btnUploadImage_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            ofd.Title = "Select Product Image";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                // Load image without locking file and store path
                try
                {
                    using (var img = Image.FromFile(ofd.FileName))
                    {
                        picProductImage.Image = new Bitmap(img);
                    }
                    currentImagePath = ofd.FileName;
                    txtImagePath.Text = currentImagePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to load image: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    currentImagePath = null;
                }
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                string query;
                MySqlParameter[] parameters;

                int parsedId;
                bool hasId = int.TryParse(txtProductId.Text, out parsedId);

                if (hasId)
                {
                    // Check if product with this ID already exists
                    string existsQuery = "SELECT COUNT(*) FROM products WHERE product_id = @product_id";
                    MySqlParameter[] existsParams = new MySqlParameter[] { new MySqlParameter("@product_id", parsedId) };
                    DataTable dtExists = db.ExecuteQuery(existsQuery, existsParams);
                    int count = 0;
                    if (dtExists.Rows.Count > 0)
                    {
                        count = Convert.ToInt32(dtExists.Rows[0][0]);
                    }

                    if (count > 0)
                    {
                        // Update existing
                        query = @"UPDATE products SET category_id = @category_id, 
                                 product_name = @product_name, description = @description, 
                                 price = @price, stock_quantity = @stock_quantity, 
                                 image = @image WHERE product_id = @product_id";

                        parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@product_id", parsedId),
                            new MySqlParameter("@category_id", cmbCategory.SelectedValue),
                            new MySqlParameter("@product_name", txtProductName.Text),
                            new MySqlParameter("@description", txtDescription.Text),
                            new MySqlParameter("@price", float.Parse(txtPrice.Text)),
                            new MySqlParameter("@stock_quantity", int.Parse(txtStock.Text)),
                            new MySqlParameter("@image", (object)currentImagePath ?? DBNull.Value)
                        };
                    }
                    else
                    {
                        // Insert explicit ID
                        query = @"INSERT INTO products (product_id, category_id, product_name, description, price, stock_quantity, image)
                                  VALUES (@product_id, @category_id, @product_name, @description, @price, @stock_quantity, @image)";

                        parameters = new MySqlParameter[]
                        {
                            new MySqlParameter("@product_id", parsedId),
                            new MySqlParameter("@category_id", cmbCategory.SelectedValue),
                            new MySqlParameter("@product_name", txtProductName.Text),
                            new MySqlParameter("@description", txtDescription.Text),
                            new MySqlParameter("@price", float.Parse(txtPrice.Text)),
                            new MySqlParameter("@stock_quantity", int.Parse(txtStock.Text)),
                            new MySqlParameter("@image", (object)currentImagePath ?? DBNull.Value)
                        };
                    }
                }
                else
                {
                    // No ID provided - insert with auto-increment
                    query = @"INSERT INTO products (category_id, product_name, description, 
                                 price, stock_quantity, image) 
                                 VALUES (@category_id, @product_name, @description, 
                                 @price, @stock_quantity, @image)";

                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@category_id", cmbCategory.SelectedValue),
                        new MySqlParameter("@product_name", txtProductName.Text),
                        new MySqlParameter("@description", txtDescription.Text),
                        new MySqlParameter("@price", float.Parse(txtPrice.Text)),
                        new MySqlParameter("@stock_quantity", int.Parse(txtStock.Text)),
                        new MySqlParameter("@image", (object)(string.IsNullOrWhiteSpace(txtImagePath.Text) ? null : txtImagePath.Text) ?? DBNull.Value)
                    };
                }

                db.ExecuteNonQuery(query, parameters);
                MessageBox.Show("Product saved successfully!", "Success",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);

                LoadProducts();
                ClearForm();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message, "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvProducts.CurrentRow != null)
            {
                DialogResult result = MessageBox.Show("Are you sure you want to delete this product?",
                    "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result == DialogResult.Yes)
                {
                    int productId = Convert.ToInt32(dgvProducts.CurrentRow.Cells["product_id"].Value);
                    string query = "DELETE FROM products WHERE product_id = @product_id";
                    MySqlParameter[] parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@product_id", productId)
                    };

                    db.ExecuteNonQuery(query, parameters);
                    MessageBox.Show("Product deleted successfully!", "Success",
                                  MessageBoxButtons.OK, MessageBoxIcon.Information);
                    LoadProducts();
                    ClearForm();
                }
            }
            else
            {
                MessageBox.Show("Please select a product to delete.", "Warning",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            ClearForm();
            txtProductName.Focus();
        }

        private bool ValidateInputs()
        {
            // product_name required
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Product name is required.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // price must be a positive float
            if (string.IsNullOrWhiteSpace(txtPrice.Text) || !float.TryParse(txtPrice.Text, out float price) || price < 0)
            {
                MessageBox.Show("Valid non-negative price is required.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // stock must be integer >= 0
            if (string.IsNullOrWhiteSpace(txtStock.Text) || !int.TryParse(txtStock.Text, out int stock) || stock < 0)
            {
                MessageBox.Show("Valid non-negative stock quantity is required.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // if product id provided, ensure numeric
            if (!string.IsNullOrWhiteSpace(txtProductId.Text) && !int.TryParse(txtProductId.Text, out _))
            {
                MessageBox.Show("Product ID must be a valid integer.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            // category should be selected
            if (cmbCategory.SelectedValue == null || cmbCategory.Items.Count == 0)
            {
                MessageBox.Show("Please select a category.", "Validation Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            txtProductId.Clear();
            txtProductName.Clear();
            txtDescription.Clear();
            txtPrice.Clear();
            txtStock.Clear();
            cmbCategory.SelectedIndex = 0;
            picProductImage.Image = null;
            currentImagePath = null;
            txtImagePath.Text = string.Empty;
            dgvProducts.ClearSelection();
        }

        private void dgvProducts_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvProducts.Rows[e.RowIndex];
                txtProductId.Text = row.Cells["product_id"].Value.ToString();

                // Store image path in Tag for later use
                string query = "SELECT image FROM products WHERE product_id = @product_id";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_id", int.Parse(txtProductId.Text))
                };

                DataTable dt = db.ExecuteQuery(query, parameters);
                if (dt.Rows.Count > 0 && dt.Rows[0]["image"] != DBNull.Value)
                {
                    string path = dt.Rows[0]["image"].ToString();
                    dgvProducts.CurrentRow.Tag = path;
                    txtImagePath.Text = path;
                }
            }
        }
    }
}
