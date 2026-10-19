using System;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace PharmacyManagementSystem
{
    public class ConsultationForm : Form
    {
        private readonly ConsultationRepository _repo;
        private DataGridView dgvConsultations;
        private TextBox txtResponse;
        private Button btnSendResponse;
        private Label lblStatus;

        // Email configuration – replace with your own SMTP settings
        private const string SmtpHost = "smtp.gmail.com";
        private const int SmtpPort = 587;
        private const string SmtpUsername = "k3243730@gmail.com";
        private const string SmtpPassword = "vwtf juui rtft nlam";
        private const string FromEmail = "k3243730@gmail.com";
        private const string FromName = "Mahube Pharmacy";

        public ConsultationForm()
        {
            _repo = new ConsultationRepository(new DatabaseConnection());
            InitializeComponent();
            Load += ConsultationForm_LoadAsync;
            StartPosition = FormStartPosition.CenterParent;
        }

        #region UI Initialization

        private void InitializeComponent()
        {
            Text = "Consultations";
            Size = new Size(900, 600);
            MinimumSize = new Size(800, 450);
            BackColor = Color.White;

            var mainTable = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 3,
                Padding = new Padding(10)
            };
            mainTable.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 120));
            mainTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));

            dgvConsultations = new DataGridView
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
            dgvConsultations.SelectionChanged += DgvConsultations_SelectionChanged;
            mainTable.Controls.Add(dgvConsultations, 0, 0);

            var controlsPanel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.LeftToRight,
                Padding = new Padding(0, 10, 0, 0),
                BackColor = Color.Transparent,
                WrapContents = false
            };

            txtResponse = new TextBox
            {
                Size = new Size(500, 80),
                Font = new Font("Segoe UI", 9),
                BorderStyle = BorderStyle.FixedSingle,
                PlaceholderText = "Enter response (max 50 characters)",
                MaxLength = 50,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical
            };

            btnSendResponse = new Button
            {
                Text = "Send Response",
                Size = new Size(120, 80),
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(46, 125, 50),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand
            };
            btnSendResponse.FlatAppearance.BorderSize = 0;
            btnSendResponse.Click += BtnSendResponse_Click;

            controlsPanel.Controls.AddRange(new Control[] { txtResponse, btnSendResponse });
            mainTable.Controls.Add(controlsPanel, 0, 1);

            lblStatus = new Label
            {
                Dock = DockStyle.Fill,
                Text = "Ready",
                Font = new Font("Segoe UI", 9, FontStyle.Italic),
                ForeColor = Color.Gray,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(5, 0, 0, 0)
            };
            mainTable.Controls.Add(lblStatus, 0, 2);

            Controls.Add(mainTable);
        }

        #endregion

        #region Data Loading (Async)

        private async void ConsultationForm_LoadAsync(object sender, EventArgs e)
        {
            btnSendResponse.Enabled = false;
            lblStatus.Text = "Loading consultations...";

            try
            {
                DataTable dt = await Task.Run(() => _repo.GetAllConsultations());
                dgvConsultations.DataSource = dt;

                if (dgvConsultations.Columns.Contains("consultation_id")) dgvConsultations.Columns["consultation_id"].HeaderText = "ID";
                if (dgvConsultations.Columns.Contains("fullname")) dgvConsultations.Columns["fullname"].HeaderText = "Full Name";
                if (dgvConsultations.Columns.Contains("phone_number")) dgvConsultations.Columns["phone_number"].HeaderText = "Phone";
                if (dgvConsultations.Columns.Contains("preferred_date")) dgvConsultations.Columns["preferred_date"].HeaderText = "Preferred Date";
                if (dgvConsultations.Columns.Contains("consultation_type")) dgvConsultations.Columns["consultation_type"].HeaderText = "Type";
                if (dgvConsultations.Columns.Contains("Description")) dgvConsultations.Columns["Description"].HeaderText = "Description";
                if (dgvConsultations.Columns.Contains("response")) dgvConsultations.Columns["response"].HeaderText = "Response";
                if (dgvConsultations.Columns.Contains("email")) dgvConsultations.Columns["email"].HeaderText = "Email";

                dgvConsultations.ClearSelection();
                lblStatus.Text = $"Loaded {dt.Rows.Count} consultations.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to load consultations: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error loading data";
            }
            finally
            {
                btnSendResponse.Enabled = true;
            }
        }

        #endregion

        #region Selection Changed – Load Response

        private void DgvConsultations_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvConsultations.CurrentRow == null)
            {
                txtResponse.Clear();
                return;
            }

            if (dgvConsultations.Columns.Contains("response") && dgvConsultations.CurrentRow.Cells["response"].Value != null)
                txtResponse.Text = dgvConsultations.CurrentRow.Cells["response"].Value.ToString();
            else
                txtResponse.Clear();
        }

        #endregion

        #region Button Handler (Async)

        private async void BtnSendResponse_Click(object sender, EventArgs e)
        {
            if (dgvConsultations.CurrentRow == null)
            {
                MessageBox.Show("Please select a consultation.", "Validation", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var idObj = dgvConsultations.CurrentRow.Cells["consultation_id"].Value;
            if (idObj == null)
            {
                MessageBox.Show("Selected row has no ID.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            long consultationId = Convert.ToInt64(idObj);
            string response = txtResponse.Text.Trim();

            // Get the patient's email directly from the email column
            string patientEmail = null;
            if (dgvConsultations.Columns.Contains("email") && dgvConsultations.CurrentRow.Cells["email"].Value != null)
                patientEmail = dgvConsultations.CurrentRow.Cells["email"].Value.ToString();

            btnSendResponse.Enabled = false;
            lblStatus.Text = "Saving response...";

            try
            {
                await Task.Run(() => _repo.UpdateResponse(consultationId, response));

                if (!string.IsNullOrEmpty(patientEmail))
                {
                    await SendEmailAsync(patientEmail, consultationId, response);
                }
                else
                {
                    lblStatus.Text = "Response saved, but no email address found.";
                }

                MessageBox.Show("Response saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refresh grid
                DataTable dt = await Task.Run(() => _repo.GetAllConsultations());
                dgvConsultations.DataSource = dt;
                lblStatus.Text = "Response saved and email sent.";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save response: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblStatus.Text = "Error saving response.";
            }
            finally
            {
                btnSendResponse.Enabled = true;
            }
        }

        #endregion

        #region Email Sending

        private async Task SendEmailAsync(string toEmail, long consultationId, string response)
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
                        Subject = "Your Consultation Response from Mahube Pharmacy",
                        Body = $@"
Dear Patient,

We have updated the response for your consultation (ID: {consultationId}).

Response: {response}

If you have any questions, please contact us.

Best regards,
Mahube Pharmacy Team
",
                        IsBodyHtml = false
                    };
                    mail.To.Add(toEmail);

                    await client.SendMailAsync(mail);
                    lblStatus.Text = "Email sent to patient.";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Response saved, but email could not be sent: {ex.Message}", "Email Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                lblStatus.Text = "Response saved, email failed.";
            }
        }

        #endregion

        #region Repository

        private class ConsultationRepository
        {
            private readonly DatabaseConnection _db;

            public ConsultationRepository(DatabaseConnection db) => _db = db;

            public DataTable GetAllConsultations()
            {
                // Now email is directly in the consultation table – no join needed
                string query = @"
                    SELECT 
                        consultation_id, 
                        fullname, 
                        phone_number, 
                        preferred_date, 
                        consultation_type, 
                        Description, 
                        response, 
                        email 
                    FROM 
                        consultation 
                    ORDER BY 
                        preferred_date DESC";
                return _db.ExecuteQuery(query);
            }

            public void UpdateResponse(long id, string response)
            {
                string query = "UPDATE consultation SET response = @response WHERE consultation_id = @id";
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
                // Nothing to dispose
            }
            base.Dispose(disposing);
        }

        #endregion
    }
}