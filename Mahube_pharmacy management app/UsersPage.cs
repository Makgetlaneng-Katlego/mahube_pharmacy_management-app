
using System;
using System.Data;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PharmacyManagementSystem
{
    public class UserManagementForm : Form
    {
        private DatabaseConnection _db = new DatabaseConnection();

        private Panel panelSide;
        private Panel panelHeader;
        private Label lblTitle;
        private Label lblDate;
        private DataGridView dgvUsers;
        private Button btnAddUser;

        private Button btnDashboard;
        private Button btnProducts;
        private Button btnOrders;
        private Button btnConsultations;
        private Button btnPrescriptions;
        private Button btnUsers;
        private Button btnLogout;

        public UserManagementForm()
        {
            InitializeComponent();
            Load += UserManagementForm_Load;
            StartPosition = FormStartPosition.CenterScreen;
        }

        private void InitializeComponent()
        {
            this.Text = "Mahube Pharmacy - User Management";
            this.Size = new Size(1200, 720);
            this.BackColor = Color.FromArgb(245, 250, 245);
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            panelSide = new Panel { BackColor = Color.FromArgb(46, 125, 50), Dock = DockStyle.Left, Width = 200 };

            var lblLogo = new Label
            {
                Text = "☘ Mahube Pharmacy",
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                ForeColor = Color.White,
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };

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

            panelSide.Controls.AddRange(new Control[] { lblLogo, btnDashboard, btnProducts, btnOrders,
                                                        btnConsultations, btnPrescriptions, btnUsers,
                                                        btnLogout });

            panelHeader = new Panel { Dock = DockStyle.Top, Height = 100, BackColor = Color.WhiteSmoke };
            lblTitle = new Label
            {
                Text = "User Management",
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

            btnAddUser = new Button
            {
                Text = "+ Add User",
                Location = new Point(220, 115),
                Size = new Size(140, 35),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold)
            };
            btnAddUser.FlatAppearance.BorderSize = 0;
            btnAddUser.Click += BtnAddUser_Click;

            dgvUsers = new DataGridView
            {
                Location = new Point(220, 160),
                Size = new Size(940, 480),
                ReadOnly = true,
                AllowUserToAddRows = false,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };
            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 125, 50);
            dgvUsers.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvUsers.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            dgvUsers.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(232, 245, 233);
            dgvUsers.CellClick += DgvUsers_CellClick;

            Controls.AddRange(new Control[] { panelSide, panelHeader, btnAddUser, dgvUsers });
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

        private void UserManagementForm_Load(object sender, EventArgs e)
        {
            LoadUsers();
        }

        private void LoadUsers()
        {
            try
            {
                string query = "SELECT user_id, fullname, email, role, phone_number FROM users ORDER BY user_id";
                DataTable dt = _db.ExecuteQuery(query);
                dgvUsers.DataSource = dt;

                if (dgvUsers.Columns["Edit"] == null)
                {
                    DataGridViewButtonColumn colEdit = new DataGridViewButtonColumn
                    {
                        Name = "Edit",
                        Text = "Edit",
                        UseColumnTextForButtonValue = true,
                        HeaderText = "Actions",
                        Width = 60
                    };
                    DataGridViewButtonColumn colDelete = new DataGridViewButtonColumn
                    {
                        Name = "Delete",
                        Text = "Delete",
                        UseColumnTextForButtonValue = true,
                        HeaderText = "",
                        Width = 60
                    };
                    dgvUsers.Columns.Add(colEdit);
                    dgvUsers.Columns.Add(colDelete);
                }

                if (dgvUsers.Columns.Contains("user_id"))
                    dgvUsers.Columns["user_id"].HeaderText = "User ID";
                if (dgvUsers.Columns.Contains("fullname"))
                    dgvUsers.Columns["fullname"].HeaderText = "Name";
                if (dgvUsers.Columns.Contains("email"))
                    dgvUsers.Columns["email"].HeaderText = "Email";
                if (dgvUsers.Columns.Contains("role"))
                    dgvUsers.Columns["role"].HeaderText = "Role";
                if (dgvUsers.Columns.Contains("phone_number"))
                    dgvUsers.Columns["phone_number"].HeaderText = "Phone";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Failed to load users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void DgvUsers_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            int userId = Convert.ToInt32(dgvUsers.Rows[e.RowIndex].Cells["user_id"].Value);
            string colName = dgvUsers.Columns[e.ColumnIndex].Name;

            if (colName == "Edit")
                EditUser(userId);
            else if (colName == "Delete")
            {
                if (MessageBox.Show("Are you sure you want to delete this user?", "Confirm Delete",
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    DeleteUser(userId);
            }
        }

        private void EditUser(int userId)
        {
            DataTable dt = _db.ExecuteQuery("SELECT user_id, fullname, email, role, phone_number FROM users WHERE user_id = @id",
                new MySqlParameter[] { new MySqlParameter("@id", userId) });
            if (dt.Rows.Count == 0) return;
            DataRow row = dt.Rows[0];

            User user = new User
            {
                UserId = userId,
                FullName = row["fullname"].ToString(),
                Email = row["email"].ToString(),
                Role = row["role"].ToString(),
                PhoneNumber = row["phone_number"].ToString()
            };

            using (AddEditUserForm editForm = new AddEditUserForm(user))
            {
                if (editForm.ShowDialog() == DialogResult.OK)
                {
                    string newPassword = string.IsNullOrEmpty(editForm.Password) ? null : editForm.Password;
                    UpdateUser(editForm.UserData, newPassword);
                    LoadUsers();
                }
            }
        }

        private void DeleteUser(int userId)
        {
            try
            {
                _db.ExecuteNonQuery("DELETE FROM users WHERE user_id = @id",
                    new MySqlParameter[] { new MySqlParameter("@id", userId) });
                LoadUsers();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Delete failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUser(User user, string newPassword)
        {
            try
            {
                string query;
                MySqlParameter[] parameters;
                if (!string.IsNullOrEmpty(newPassword))
                {
                    query = @"UPDATE users SET fullname = @fullname, email = @email, role = @role,
                              phone_number = @phone, password = @password WHERE user_id = @id";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@fullname", user.FullName),
                        new MySqlParameter("@email", user.Email),
                        new MySqlParameter("@role", user.Role),
                        new MySqlParameter("@phone", user.PhoneNumber),
                        new MySqlParameter("@password", HashPassword(newPassword)),
                        new MySqlParameter("@id", user.UserId)
                    };
                }
                else
                {
                    query = @"UPDATE users SET fullname = @fullname, email = @email, role = @role,
                              phone_number = @phone WHERE user_id = @id";
                    parameters = new MySqlParameter[]
                    {
                        new MySqlParameter("@fullname", user.FullName),
                        new MySqlParameter("@email", user.Email),
                        new MySqlParameter("@role", user.Role),
                        new MySqlParameter("@phone", user.PhoneNumber),
                        new MySqlParameter("@id", user.UserId)
                    };
                }
                _db.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Update failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void AddUser(User user, string plainPassword)
        {
            try
            {
                string query = @"INSERT INTO users (fullname, email, password, role, phone_number)
                                 VALUES (@fullname, @email, @password, @role, @phone)";
                MySqlParameter[] parameters = new MySqlParameter[]
                {
                    new MySqlParameter("@fullname", user.FullName),
                    new MySqlParameter("@email", user.Email),
                    new MySqlParameter("@password", HashPassword(plainPassword)),
                    new MySqlParameter("@role", user.Role),
                    new MySqlParameter("@phone", user.PhoneNumber)
                };
                _db.ExecuteNonQuery(query, parameters);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Add user failed: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string HashPassword(string password)
        {
            using (SHA256 sha = SHA256.Create())
            {
                byte[] bytes = sha.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private void BtnAddUser_Click(object sender, EventArgs e)
        {
            using (AddEditUserForm addForm = new AddEditUserForm())
            {
                if (addForm.ShowDialog() == DialogResult.OK)
                {
                    AddUser(addForm.UserData, addForm.Password);
                    LoadUsers();
                }
            }
        }

        // Navigation methods
        private void BtnDashboard_Click(object sender, EventArgs e)
        {
            DashboardForm dashboard = new DashboardForm();
            dashboard.Show();
            this.Close();
        }

        private void BtnProducts_Click(object sender, EventArgs e)
        {
            ProductsForm products = new ProductsForm();
            products.Show();
            this.Close();
        }

        private void BtnOrders_Click(object sender, EventArgs e)
        {
            OrdersForm orders = new OrdersForm();
            orders.Show();
            this.Close();
        }

        private void BtnConsultations_Click(object sender, EventArgs e)
        {
            ConsultationForm consult = new ConsultationForm();
            consult.StartPosition = FormStartPosition.CenterParent;
            consult.ShowDialog(this);
        }

        private void BtnPrescriptions_Click(object sender, EventArgs e)
        {
            PrescriptionForm presc = new PrescriptionForm();
            presc.StartPosition = FormStartPosition.CenterParent;
            presc.ShowDialog(this);
        }

        private void BtnUsers_Click(object sender, EventArgs e)
        {
            LoadUsers(); // refresh
        }

        private void BtnLogout_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Are you sure you want to logout?", "Confirm Logout",
                                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                LoginForm login = new LoginForm();
                login.Show();
                this.Close();
            }
        }
    }

    // User entity (no IsActive)
    public class User
    {
        public int UserId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string PhoneNumber { get; set; }
    }

    // Add/Edit dialog (no Active checkbox)
    public class AddEditUserForm : Form
    {
        public User UserData { get; private set; }
        public string Password { get; private set; }
        private bool isEdit;

        private TextBox txtFullName, txtEmail, txtPassword, txtPhone;
        private ComboBox cmbRole;
        private Button btnSave, btnCancel;

        public AddEditUserForm(User user = null)
        {
            InitializeComponent();
            if (user != null)
            {
                isEdit = true;
                UserData = user;
                txtFullName.Text = user.FullName;
                txtEmail.Text = user.Email;
                txtPhone.Text = user.PhoneNumber;
                cmbRole.Text = user.Role;
                txtPassword.Visible = false;
                this.Text = "Edit User";
            }
            else
            {
                isEdit = false;
                UserData = new User();
                this.Text = "Add User";
            }
        }

        private void InitializeComponent()
        {
            this.Text = "Add/Edit User";
            this.Size = new Size(420, 340);
            this.StartPosition = FormStartPosition.CenterParent;
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.BackColor = Color.White;

            int y = 20;
            int spacing = 35;
            int labelWidth = 90;
            int controlWidth = 260;

            Label lblFullName = new Label { Text = "Full Name:", Location = new Point(20, y), Size = new Size(labelWidth, 25) };
            txtFullName = new TextBox { Location = new Point(120, y), Size = new Size(controlWidth, 25) };
            y += spacing;

            Label lblEmail = new Label { Text = "Email:", Location = new Point(20, y), Size = new Size(labelWidth, 25) };
            txtEmail = new TextBox { Location = new Point(120, y), Size = new Size(controlWidth, 25) };
            y += spacing;

            Label lblPassword = new Label { Text = "Password:", Location = new Point(20, y), Size = new Size(labelWidth, 25) };
            txtPassword = new TextBox { Location = new Point(120, y), Size = new Size(controlWidth, 25), PasswordChar = '*' };
            y += spacing;

            Label lblPhone = new Label { Text = "Phone:", Location = new Point(20, y), Size = new Size(labelWidth, 25) };
            txtPhone = new TextBox { Location = new Point(120, y), Size = new Size(controlWidth, 25) };
            y += spacing;

            Label lblRole = new Label { Text = "Role:", Location = new Point(20, y), Size = new Size(labelWidth, 25) };
            cmbRole = new ComboBox
            {
                Location = new Point(120, y),
                Size = new Size(controlWidth, 25),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            cmbRole.Items.AddRange(new string[] { "Admin", "Pharmacist", "Cashier", "Consultant" });
            cmbRole.SelectedIndex = 0;
            y += spacing + 10;

            btnSave = new Button
            {
                Text = "Save",
                Location = new Point(120, y),
                Size = new Size(90, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White
            };
            btnSave.FlatAppearance.BorderSize = 0;
            btnSave.Click += BtnSave_Click;

            btnCancel = new Button
            {
                Text = "Cancel",
                Location = new Point(220, y),
                Size = new Size(90, 30),
                FlatStyle = FlatStyle.Flat,
                BackColor = Color.FromArgb(158, 158, 158),
                ForeColor = Color.White
            };
            btnCancel.FlatAppearance.BorderSize = 0;
            btnCancel.Click += (s, e) => { DialogResult = DialogResult.Cancel; Close(); };

            Controls.AddRange(new Control[] { lblFullName, txtFullName, lblEmail, txtEmail, lblPassword, txtPassword,
                                              lblPhone, txtPhone, lblRole, cmbRole, btnSave, btnCancel });
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtFullName.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                cmbRole.SelectedItem == null)
            {
                MessageBox.Show("Full Name, Email, and Role are required.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!isEdit && string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Password is required for new users.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!txtEmail.Text.Contains("@") || !txtEmail.Text.Contains("."))
            {
                MessageBox.Show("Please enter a valid email address.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UserData.FullName = txtFullName.Text.Trim();
            UserData.Email = txtEmail.Text.Trim();
            UserData.Role = cmbRole.SelectedItem.ToString();
            UserData.PhoneNumber = txtPhone.Text.Trim();
            Password = txtPassword.Text;

            DialogResult = DialogResult.OK;
            Close();
        }
    }
}