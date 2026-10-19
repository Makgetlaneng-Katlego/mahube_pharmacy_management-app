using System;
using System.Drawing;
using System.Windows.Forms;

namespace PharmacyManagementSystem
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
            ApplyTheme();
            this.StartPosition = FormStartPosition.CenterScreen;
            this.DoubleBuffered = true;
        }

        private void ApplyTheme()
        {
            // Light background
            this.BackColor = Color.FromArgb(245, 250, 245);
            this.ForeColor = Color.FromArgb(33, 33, 33);

            // Title label
            lblTitle.ForeColor = Color.FromArgb(46, 125, 50);
            lblTitle.Font = new Font("Segoe UI", 28, FontStyle.Bold);

            // Subtitle
            lblSubtitle.ForeColor = Color.FromArgb(255, 143, 0);
            lblSubtitle.Font = new Font("Segoe UI", 14, FontStyle.Regular);

            // Username label and textbox
            lblUsername.ForeColor = Color.FromArgb(66, 133, 66);
            lblUsername.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            txtUsername.BackColor = Color.White;
            txtUsername.ForeColor = Color.FromArgb(33, 33, 33);
            txtUsername.BorderStyle = BorderStyle.None;
            txtUsername.Font = new Font("Segoe UI", 11);

            // Password label and textbox
            lblPassword.ForeColor = Color.FromArgb(66, 133, 66);
            lblPassword.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            txtPassword.BackColor = Color.White;
            txtPassword.ForeColor = Color.FromArgb(33, 33, 33);
            txtPassword.BorderStyle = BorderStyle.None;
            txtPassword.Font = new Font("Segoe UI", 11);
            txtPassword.PasswordChar = '●';

            // Login button - Green
            btnLogin.BackColor = Color.FromArgb(46, 125, 50);
            btnLogin.ForeColor = Color.White;
            btnLogin.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnLogin.FlatStyle = FlatStyle.Flat;
            btnLogin.FlatAppearance.BorderSize = 0;
            btnLogin.Cursor = Cursors.Hand;

            // Clear button - Light gray
            btnClear.BackColor = Color.FromArgb(224, 224, 224);
            btnClear.ForeColor = Color.FromArgb(33, 33, 33);
            btnClear.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            btnClear.FlatStyle = FlatStyle.Flat;
            btnClear.FlatAppearance.BorderSize = 0;
            btnClear.Cursor = Cursors.Hand;

            // Exit button - Orange
            btnExit.BackColor = Color.FromArgb(255, 143, 0);
            btnExit.ForeColor = Color.White;
            btnExit.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            btnExit.FlatStyle = FlatStyle.Flat;
            btnExit.FlatAppearance.BorderSize = 0;
            btnExit.Cursor = Cursors.Hand;

            // Checkbox
            chkRemember.ForeColor = Color.FromArgb(66, 133, 66);
            chkRemember.Font = new Font("Segoe UI", 9);
            chkRemember.BackColor = Color.FromArgb(245, 250, 245);

            // Info label
            lblInfo.ForeColor = Color.FromArgb(158, 158, 158);
            lblInfo.Font = new Font("Segoe UI", 9, FontStyle.Italic);
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            if (ValidateLogin())
            {
                // Open dashboard after successful login
                DashboardForm dashboard = new DashboardForm();
                dashboard.StartPosition = FormStartPosition.CenterScreen;
                dashboard.Show();
                this.Hide();
            }
        }

        private bool ValidateLogin()
        {
            if (string.IsNullOrWhiteSpace(txtUsername.Text))
            {
                MessageBox.Show("Please enter a username.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtUsername.Focus();
                return false;
            }

            if (string.IsNullOrWhiteSpace(txtPassword.Text))
            {
                MessageBox.Show("Please enter a password.", "Validation Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtPassword.Focus();
                return false;
            }

            // Placeholder validation - replace with actual authentication
            if (txtUsername.Text == "admin" && txtPassword.Text == "admin")
            {
                return true;
            }

            MessageBox.Show("Invalid username or password.", "Login Failed",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
            txtPassword.Clear();
            txtUsername.Focus();
            return false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtUsername.Clear();
            txtPassword.Clear();
            txtUsername.Focus();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}
