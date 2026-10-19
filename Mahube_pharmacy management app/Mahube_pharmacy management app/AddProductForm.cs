using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PharmacyManagementSystem
{
    public class AddProductForm : Form
    {
        private readonly DatabaseConnection _db = new DatabaseConnection();
        private readonly ProductRepository _productRepo;

        // UI controls
        private TextBox txtProductName;
        private ComboBox cmbCategory;
        private TextBox txtPrice;
        private TextBox txtStockQuantity;
        private TextBox txtDescription;
        private TextBox txtImageUrl;
        private Button btnCancel;
        private Button btnAddProduct;
        private PictureBox picPreview;
        private Label lblStatus;

        // For image loading cancellation
        private CancellationTokenSource _imageLoadCts;

        public AddProductForm()
        {
            _productRepo = new ProductRepository(_db);
            InitializeComponent();
            LoadCategoriesAsync().ConfigureAwait(false); // fire-and-forget, but we'll handle errors
            StartPosition = FormStartPosition.CenterParent;
        }

        #region UI Initialization

        private void InitializeComponent()
        {
            Text = "Add New Product";
            Size = new Size(720, 650);
            MinimumSize = new Size(720, 600);
            BackColor = Color.White;
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;

            // Title bar
            var panelTitle = CreateTitlePanel();

            // Main layout using TableLayoutPanel
            var table = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoScroll = true,
                Padding = new Padding(20, 10, 20, 10),
                BackColor = Color.White,
                ColumnCount = 2,
                RowCount = 9, // we'll add rows dynamically
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
            };
            // Set column widths: 50% / 50%
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));

            // Row styles (auto-size for most, but some fixed)
            for (int i = 0; i < table.RowCount; i++)
                table.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            // Build rows
            int row = 0;

            // 1. Product Name
            table.Controls.Add(CreateLabel("Product Name *", FontStyle.Bold), 0, row);
            txtProductName = CreateTextBox(placeholder: "Enter product name");
            table.Controls.Add(txtProductName, 1, row);
            row++;

            // 2. Category
            table.Controls.Add(CreateLabel("Category *", FontStyle.Bold), 0, row);
            cmbCategory = new ComboBox
            {
                DropDownStyle = ComboBoxStyle.DropDownList,
                Font = new Font("Segoe UI", 10),
                Height = 30,
                Margin = new Padding(3, 3, 3, 3),
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
            };
            table.Controls.Add(cmbCategory, 1, row);
            row++;

            // 3. Price
            table.Controls.Add(CreateLabel("Price *", FontStyle.Bold), 0, row);
            txtPrice = CreateTextBox(placeholder: "0.00");
            table.Controls.Add(txtPrice, 1, row);
            row++;

            // 4. Stock Quantity
            table.Controls.Add(CreateLabel("Stock Quantity *", FontStyle.Bold), 0, row);
            txtStockQuantity = CreateTextBox(placeholder: "0");
            table.Controls.Add(txtStockQuantity, 1, row);
            row++;

            // 5. Description (spans both columns)
            table.Controls.Add(CreateLabel("Description", FontStyle.Bold), 0, row);
            txtDescription = new TextBox
            {
                Multiline = true,
                Height = 80,
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                ScrollBars = ScrollBars.Vertical,
                PlaceholderText = "Enter product description...",
                Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Top,
                Margin = new Padding(3, 3, 3, 3),
            };
            table.SetColumnSpan(txtDescription, 2);
            table.Controls.Add(txtDescription, 0, row);
            row++;

            // 6. Image URL (spans both columns)
            table.Controls.Add(CreateLabel("Product Image URL", FontStyle.Bold), 0, row);
            txtImageUrl = CreateTextBox(placeholder: "https://example.com/product-image.jpg");
            txtImageUrl.TextChanged += TxtImageUrl_TextChanged;
            table.SetColumnSpan(txtImageUrl, 2);
            table.Controls.Add(txtImageUrl, 0, row);
            row++;

            // 7. Image Preview (spans both columns)
            table.Controls.Add(CreateLabel("Image Preview:", FontStyle.Bold), 0, row);
            picPreview = new PictureBox
            {
                Size = new Size(200, 200),
                BorderStyle = BorderStyle.FixedSingle,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.FromArgb(240, 240, 240),
                Anchor = AnchorStyles.Top | AnchorStyles.Left,
                Margin = new Padding(3, 3, 3, 3),
            };
            table.SetColumnSpan(picPreview, 2);
            table.Controls.Add(picPreview, 0, row);
            row++;

            // 8. Status label (spans both columns)
            lblStatus = new Label
            {
                AutoSize = false,
                Height = 20,
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(3, 3, 3, 3),
            };
            table.SetColumnSpan(lblStatus, 2);
            table.Controls.Add(lblStatus, 0, row);
            row++;

            // 9. Buttons (spans both columns)
            var panelButtons = new Panel
            {
                Height = 50,
                Dock = DockStyle.Fill,
                BackColor = Color.Transparent,
            };
            btnCancel = new Button
            {
                Text = "Cancel",
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(200, 200, 200),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.None,
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => Close();

            btnAddProduct = new Button
            {
                Text = "Add Product",
                Size = new Size(120, 35),
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.None,
            };
            btnAddProduct.FlatAppearance.BorderSize = 0;
            btnAddProduct.Click += BtnAddProduct_Click;

            // Center buttons horizontally in panel
            panelButtons.Controls.Add(btnCancel);
            panelButtons.Controls.Add(btnAddProduct);
            btnCancel.Location = new Point(panelButtons.Width / 2 - 130, 5);
            btnAddProduct.Location = new Point(panelButtons.Width / 2 + 10, 5);
            panelButtons.Resize += (s, e) =>
            {
                btnCancel.Location = new Point(panelButtons.Width / 2 - 130, 5);
                btnAddProduct.Location = new Point(panelButtons.Width / 2 + 10, 5);
            };

            table.SetColumnSpan(panelButtons, 2);
            table.Controls.Add(panelButtons, 0, row);
            row++;

            // Set the row count to match
            table.RowCount = row;

            // Add everything to the form
            Controls.AddRange(new Control[] { panelTitle, table });
        }

        private Panel CreateTitlePanel()
        {
            var panel = new Panel { Dock = DockStyle.Top, Height = 50, BackColor = Color.FromArgb(46, 125, 50) };
            var lblTitle = new Label
            {
                Text = "Add New Product",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Left,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(15, 0, 0, 0)
            };
            var btnClose = new Button
            {
                Text = "×",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Dock = DockStyle.Right,
                FlatStyle = FlatStyle.Flat,
                Width = 50
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.Click += (s, e) => Close();
            panel.Controls.AddRange(new Control[] { lblTitle, btnClose });
            return panel;
        }

        private Label CreateLabel(string text, FontStyle style = FontStyle.Regular)
        {
            return new Label
            {
                Text = text,
                Font = new Font("Segoe UI", 10, style),
                AutoSize = true,
                Anchor = AnchorStyles.Left,
                Margin = new Padding(3, 8, 3, 3),
            };
        }

        private TextBox CreateTextBox(string placeholder = "")
        {
            return new TextBox
            {
                Font = new Font("Segoe UI", 10),
                BorderStyle = BorderStyle.FixedSingle,
                Height = 30,
                PlaceholderText = placeholder,
                Anchor = AnchorStyles.Left | AnchorStyles.Right,
                Margin = new Padding(3, 3, 3, 3),
            };
        }

        #endregion

        #region Data Loading

        private async Task LoadCategoriesAsync()
        {
            try
            {
                // Offload synchronous DB call to a thread-pool thread
                DataTable dt = await Task.Run(() => _db.ExecuteQuery("SELECT category_id, category_name FROM product_categories"));
                cmbCategory.DataSource = dt;
                cmbCategory.DisplayMember = "category_name";
                cmbCategory.ValueMember = "category_id";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading categories: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnAddProduct.Enabled = false;
            }
        }

        #endregion

        #region Image Preview (Async with cancellation)

        private async void TxtImageUrl_TextChanged(object sender, EventArgs e)
        {
            _imageLoadCts?.Cancel();
            _imageLoadCts = new CancellationTokenSource();
            var token = _imageLoadCts.Token;

            string url = txtImageUrl.Text.Trim();
            if (string.IsNullOrWhiteSpace(url) || !(url.StartsWith("http://") || url.StartsWith("https://")))
            {
                picPreview.Image = null;
                return;
            }

            try
            {
                using var client = new HttpClient();
                client.Timeout = TimeSpan.FromSeconds(10);
                byte[] bytes = await client.GetByteArrayAsync(url, token);
                token.ThrowIfCancellationRequested();

                using var ms = new MemoryStream(bytes);
                var img = Image.FromStream(ms);
                // Dispose old image to avoid GDI leaks
                picPreview.Image?.Dispose();
                picPreview.Image = img;
            }
            catch (OperationCanceledException)
            {
                // ignore
            }
            catch (Exception ex)
            {
                // Could log the exception
                picPreview.Image = null;
                lblStatus.Text = "Image could not be loaded.";
            }
        }

        #endregion

        #region Add Product (Async)

        private async void BtnAddProduct_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs())
                return;

            btnAddProduct.Enabled = false;
            btnAddProduct.Text = "Adding...";
            lblStatus.Text = "Adding product...";

            try
            {
                var product = new Product
                {
                    Name = txtProductName.Text.Trim(),
                    CategoryId = (int)cmbCategory.SelectedValue,
                    Price = decimal.Parse(txtPrice.Text.Trim(), CultureInfo.InvariantCulture),
                    Stock = int.Parse(txtStockQuantity.Text.Trim(), CultureInfo.InvariantCulture),
                    Description = txtDescription.Text.Trim(),
                    ImageUrl = string.IsNullOrWhiteSpace(txtImageUrl.Text) ? null : txtImageUrl.Text.Trim()
                };

                await _productRepo.AddAsync(product);
                MessageBox.Show("Product added successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding product: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error adding product.";
            }
            finally
            {
                btnAddProduct.Enabled = true;
                btnAddProduct.Text = "Add Product";
                if (lblStatus.Text == "Adding product...")
                    lblStatus.Text = string.Empty;
            }
        }

        #endregion

        #region Validation

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtProductName.Text))
            {
                MessageBox.Show("Product Name is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtProductName.Focus();
                return false;
            }

            if (cmbCategory.SelectedValue == null)
            {
                MessageBox.Show("Category is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbCategory.Focus();
                return false;
            }

            if (!decimal.TryParse(txtPrice.Text.Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out decimal price) || price < 0)
            {
                MessageBox.Show("Valid Price (non-negative) is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPrice.Focus();
                return false;
            }

            if (!int.TryParse(txtStockQuantity.Text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out int stock) || stock < 0)
            {
                MessageBox.Show("Valid Stock Quantity (non-negative integer) is required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtStockQuantity.Focus();
                return false;
            }

            return true;
        }

        #endregion

        #region Helper Classes (Repository & DTO)

        private class Product
        {
            public string Name { get; set; }
            public int CategoryId { get; set; }
            public decimal Price { get; set; }
            public int Stock { get; set; }
            public string Description { get; set; }
            public string ImageUrl { get; set; }
        }

        private class ProductRepository
        {
            private readonly DatabaseConnection _db;

            public ProductRepository(DatabaseConnection db)
            {
                _db = db;
            }

            public async Task AddAsync(Product product)
            {
                string query = @"INSERT INTO products (product_name, category_id, price, stock_quantity, description, image)
                                 VALUES (@product_name, @category_id, @price, @stock_quantity, @description, @image)";

                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@product_name", product.Name),
                    new MySqlParameter("@category_id", product.CategoryId),
                    new MySqlParameter("@price", product.Price),
                    new MySqlParameter("@stock_quantity", product.Stock),
                    new MySqlParameter("@description", product.Description ?? (object)DBNull.Value),
                    new MySqlParameter("@image", product.ImageUrl ?? (object)DBNull.Value)
                };

                // Offload the synchronous DB call to a thread-pool thread
                await Task.Run(() => _db.ExecuteNonQuery(query, parameters));
            }
        }

        #endregion
    }
}