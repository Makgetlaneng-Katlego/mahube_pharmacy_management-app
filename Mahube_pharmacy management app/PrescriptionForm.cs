using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PharmacyManagementSystem
{
    public class PrescriptionForm : Form
    {
        private readonly PrescriptionRepository _repo;
        private DataGridView dgvPrescriptions;
        private PictureBox picPreview;
        private Button btnOpenFile;
        private TextBox txtResponse;
        private Button btnSaveResponse;
        private Label lblStatus;
        private Panel previewPanel;

        // Email configuration – replace with your own SMTP settings
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SmtpUsername = "k3243730@gmail.com";
        private const string SmtpPassword = "vwtf juui rtft nlam";
        private const string FromEmail = "k3243730@gmail.com";
        private const string FromName = "Mahube Pharmacy";

        public PrescriptionForm()
        {
            _repo = new PrescriptionRepository(new DatabaseConnection());
            InitializeComponent();
            Load += PrescriptionForm_LoadAsync;
            StartPosition = FormStartPosition.CenterParent;
        }

        #region UI Initialization (simple, no split container)

        private void InitializeComponent()
        {
            Text = "Prescriptions";
            Size = new Size(1000, 650);
            MinimumSize = new Size(800, 500);
            BackColor = Color.White;

            // Main TableLayoutPanel: 3 rows – grid, preview, controls+status
            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4,
                Padding = new Padding(10)
            };
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // grid
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 220)); // preview
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 50));  // controls
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));  // status

            // DataGridView
            dgvPrescriptions = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                BackgroundColor = Color.White,
                RowHeadersVisible = false,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                BorderStyle = BorderStyle.None,
                AlternatingRowsDefaultCellStyle = { BackColor = Color.FromArgb(240, 248, 240) }
            };
            dgvPrescriptions.SelectionChanged += DgvPrescriptions_SelectionChangedAsync;
            mainTable.Controls.Add(dgvPrescriptions, 0, 0);

            // Preview Panel
            previewPanel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.FromArgb(245, 245, 245),
                BorderStyle = BorderStyle.FixedSingle,
                Height = 220
            };
            var lblPreviewTitle = new Label
            {
                Text = "Image Preview",
                Dock = DockStyle.Top,
                Height = 30,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White
            };
            picPreview = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.White,
                BorderStyle = BorderStyle.None
            };
            previewPanel.Controls.Add(picPreview);
            previewPanel.Controls.Add(lblPreviewTitle);
            mainTable.Controls.Add(previewPanel, 0, 1);

            // Controls panel
            var controlsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 10, 0, 0),
                BackColor = Color.Transparent,
                WrapContents = false
            };

            btnOpenFile = new Button
            {
                Text = "Open File",
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnOpenFile.FlatAppearance.BorderSize = 0;
            btnOpenFile.Click += BtnOpenFile_Click;

            txtResponse = new TextBox
            {
                Size = new Size(300, 30),
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Enter response (max 50)",
                MaxLength = 50
            };

            btnSaveResponse = new Button
            {
                Text = "Save Response",
                Size = new Size(110, 30),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSaveResponse.FlatAppearance.BorderSize = 0;
            btnSaveResponse.Click += BtnSaveResponse_Click;

            controlsPanel.Controls.AddRange(new Control[] { btnOpenFile, txtResponse, btnSaveResponse });
            mainTable.Controls.Add(controlsPanel, 0, 2);

            // Status label
            lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Ready",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0)
            };
            mainTable.Controls.Add(lblStatus, 0, 3);

            Controls.Add(mainTable);
        }

        #endregion

        #region Load Data

        private async void PrescriptionForm_LoadAsync(object sender, EventArgs e)
        {
            btnOpenFile.Enabled = false;
            btnSaveResponse.Enabled = false;
            lblStatus.Text = "Loading prescriptions...";

            try
            {
                DataTable dt = await Task.Run(() => _repo.GetAllPrescriptions());
                dgvPrescriptions.DataSource = dt;

                if (dgvPrescriptions.Columns.Contains("id")) dgvPrescriptions.Columns["id"].HeaderText = "ID";
                if (dgvPrescriptions.Columns.Contains("user_email")) dgvPrescriptions.Columns["user_email"].HeaderText = "User Email";
                if (dgvPrescriptions.Columns.Contains("doctor_name")) dgvPrescriptions.Columns["doctor_name"].HeaderText = "Doctor";
                if (dgvPrescriptions.Columns.Contains("date_prescribed")) dgvPrescriptions.Columns["date_prescribed"].HeaderText = "Date";
                if (dgvPrescriptions.Columns.Contains("type")) dgvPrescriptions.Columns["type"].HeaderText = "Type";
                if (dgvPrescriptions.Columns.Contains("notes")) dgvPrescriptions.Columns["notes"].HeaderText = "Notes";
                if (dgvPrescriptions.Columns.Contains("response")) dgvPrescriptions.Columns["response"].HeaderText = "Response";

                dgvPrescriptions.ClearSelection();
                picPreview.Image = null;
                lblStatus.Text = $"Loaded {dt.Rows.Count} prescriptions.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load prescriptions: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error loading data";
            }
            finally
            {
                btnOpenFile.Enabled = true;
                btnSaveResponse.Enabled = true;
            }
        }

        #endregion

        #region Selection Changed – Load Preview

        private async void DgvPrescriptions_SelectionChangedAsync(object sender, EventArgs e)
        {
            if (dgvPrescriptions.CurrentRow == null)
            {
                txtResponse.Clear();
                picPreview.Image = null;
                return;
            }

            // Load response text
            if (dgvPrescriptions.Columns.Contains("response") && dgvPrescriptions.CurrentRow.Cells["response"].Value != null)
                txtResponse.Text = dgvPrescriptions.CurrentRow.Cells["response"].Value.ToString();
            else
                txtResponse.Clear();

            // Load image preview
            var idObj = dgvPrescriptions.CurrentRow.Cells["id"].Value;
            if (idObj == null) return;

            int id = Convert.ToInt32(idObj);
            btnOpenFile.Enabled = false;
            lblStatus.Text = "Loading preview...";

            try
            {
                byte[] fileData = await Task.Run(() => _repo.GetFileData(id));
                if (fileData == null || fileData.Length == 0)
                {
                    picPreview.Image = null;
                    lblStatus.Text = "No file attached.";
                    return;
                }

                string ext = GetFileExtension(fileData);
                if (ext == ".jpg" || ext == ".png")
                {
                    using (var ms = new MemoryStream(fileData))
                    {
                        var img = Image.FromStream(ms);
                        picPreview.Image?.Dispose();
                        picPreview.Image = img;
                    }
                    lblStatus.Text = "Image preview loaded.";
                }
                else
                {
                    picPreview.Image = null;
                    lblStatus.Text = $"File type: {ext} (use 'Open File' to view)";
                }
            }
            catch (Exception ex)
            {
                picPreview.Image = null;
                lblStatus.Text = "Preview error: " + ex.Message;
            }
            finally
            {
                btnOpenFile.Enabled = true;
            }
        }

        #endregion

        #region Button Handlers

        private async void BtnOpenFile_Click(object sender, EventArgs e)
        {
            if (dgvPrescriptions.CurrentRow == null)
            {
                MessageBox.Show("Please select a prescription.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idObj = dgvPrescriptions.CurrentRow.Cells["id"].Value;
            if (idObj == null)
            {
                MessageBox.Show("Selected row has no ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = Convert.ToInt32(idObj);
            btnOpenFile.Enabled = false;
            lblStatus.Text = "Opening file...";

            try
            {
                byte[] fileData = await Task.Run(() => _repo.GetFileData(id));
                if (fileData == null || fileData.Length == 0)
                {
                    MessageBox.Show("No file attached for this prescription.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    lblStatus.Text = "No file to open.";
                    return;
                }

                string extension = GetFileExtension(fileData);
                string tempPath = Path.Combine(Path.GetTempPath(), $"prescription_{id}_{Guid.NewGuid():N}{extension}");
                File.WriteAllBytes(tempPath, fileData);

                ProcessStartInfo psi = new ProcessStartInfo(tempPath) { UseShellExecute = true };
                Process.Start(psi);
                lblStatus.Text = $"Opened file: {Path.GetFileName(tempPath)}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to open file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error opening file.";
            }
            finally
            {
                btnOpenFile.Enabled = true;
            }
        }

        private async void BtnSaveResponse_Click(object sender, EventArgs e)
        {
            if (dgvPrescriptions.CurrentRow == null)
            {
                MessageBox.Show("Please select a prescription.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idObj = dgvPrescriptions.CurrentRow.Cells["id"].Value;
            if (idObj == null)
            {
                MessageBox.Show("Selected row has no ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int id = Convert.ToInt32(idObj);
            string response = txtResponse.Text.Trim();
            string patientEmail = dgvPrescriptions.CurrentRow.Cells["user_email"].Value?.ToString();

            btnSaveResponse.Enabled = false;
            lblStatus.Text = "Saving response...";

            try
            {
                await Task.Run(() => _repo.UpdateResponse(id, response));

                if (!string.IsNullOrEmpty(patientEmail))
                    await SendEmailAsync(patientEmail, id, response);
                else
                    lblStatus.Text = "Response saved, but no email address found.";

                MessageBox.Show("Response saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh grid
                DataTable dt = await Task.Run(() => _repo.GetAllPrescriptions());
                dgvPrescriptions.DataSource = dt;
                lblStatus.Text = "Response saved and email sent.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save response: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error saving response.";
            }
            finally
            {
                btnSaveResponse.Enabled = true;
            }
        }

        #endregion

        #region Email Sending

        private async Task SendEmailAsync(string toEmail, int prescriptionId, string response)
        {
            try
            {
                using (var client = new SmtpClient(SmtpHost, SmtpPort))
                {
                    client.EnableSsl = true;
                    client.Credentials = new NetworkCredential(SmtpUsername, SmtpPassword);

                    var mail = new MailMessage
                    {
                        From = new MailAddress(FromEmail, FromName),
                        Subject = "Your Prescription Response from Mahube Pharmacy",
                        Body = $@"
Dear Patient,

We have updated the response for your prescription (ID: {prescriptionId}).

Response: {response}

If you have any questions, please contact us.

Best regards,
Mahube Pharmacy Team
",
                        IsBodyHtml = false
                    };
                    mail.To.Add(toEmail);

                    await client.SendMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Response saved, but email could not be sent: {ex.Message}", "Email Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Response saved, email failed.";
            }
        }

        #endregion

        #region Helper Methods

        private string GetFileExtension(byte[] blob)
        {
            if (blob.Length > 4 && blob[0] == 0x25 && blob[1] == 0x50 && blob[2] == 0x44 && blob[3] == 0x46)
                return ".pdf";
            if (blob.Length > 3 && blob[0] == 0xFF && blob[1] == 0xD8)
                return ".jpg";
            if (blob.Length > 8 && blob[0] == 0x89 && blob[1] == 0x50 && blob[2] == 0x4E && blob[3] == 0x47)
                return ".png";
            return ".bin";
        }

        #endregion

        #region Repository

        private class PrescriptionRepository
        {
            private readonly DatabaseConnection _db;

            public PrescriptionRepository(DatabaseConnection db) => _db = db;

            public DataTable GetAllPrescriptions()
            {
                string query = "SELECT id, user_email, doctor_name, date_prescribed, type, notes, response FROM prescription ORDER BY date_prescribed DESC";
                return _db.ExecuteQuery(query);
            }

            public byte[] GetFileData(int id)
            {
                string query = "SELECT file_data FROM prescription WHERE id = @id LIMIT 1";
                MySqlParameter[] parameters = new[] { new MySqlParameter("@id", id) };
                using (DataTable dt = _db.ExecuteQuery(query, parameters))
                {
                    if (dt.Rows.Count == 0 || dt.Rows[0]["file_data"] == DBNull.Value)
                        return null;
                    return (byte[])dt.Rows[0]["file_data"];
                }
            }

            public void UpdateResponse(int id, string response)
            {
                string query = "UPDATE prescription SET response = @response WHERE id = @id";
                MySqlParameter[] parameters = new[]
                {
                    new MySqlParameter("@response", string.IsNullOrEmpty(response) ? (object)DBNull.Value : response),
                    new MySqlParameter("@id", id)
                };
                _db.ExecuteNonQuery(query, parameters);
            }
        }

        #endregion

        #region Cleanup

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                picPreview?.Image?.Dispose();
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}