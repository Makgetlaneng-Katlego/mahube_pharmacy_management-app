namespace PharmacyManagementSystem
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblTitle = new System.Windows.Forms.Label();
            this.panelSide = new System.Windows.Forms.Panel();
            this.flowPanelSide = new System.Windows.Forms.FlowLayoutPanel();
            this.panelContent = new System.Windows.Forms.Panel();
            this.panelTop = new System.Windows.Forms.Panel();
            this.tblProductInputs = new System.Windows.Forms.TableLayoutPanel();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblPrice = new System.Windows.Forms.Label();
            this.lblStock = new System.Windows.Forms.Label();
            this.lblCategory = new System.Windows.Forms.Label();
            this.lblImage = new System.Windows.Forms.Label();
            this.lblProductId = new System.Windows.Forms.Label();
            this.lblProductName = new System.Windows.Forms.Label();
            this.btnShowOrders = new System.Windows.Forms.Button();
            this.btnShowProducts = new System.Windows.Forms.Button();
            this.btnRefreshProducts = new System.Windows.Forms.Button();
            this.btnConsultations = new System.Windows.Forms.Button();
            this.btnPrescriptions = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.btnExportProduct = new System.Windows.Forms.Button();
            this.btnEdit = new System.Windows.Forms.Button();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgvProducts = new System.Windows.Forms.DataGridView();
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.txtProductName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.txtPrice = new System.Windows.Forms.TextBox();
            this.txtStock = new System.Windows.Forms.TextBox();
            this.txtProductId = new System.Windows.Forms.TextBox();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.picProductImage = new System.Windows.Forms.PictureBox();
            this.btnUploadImage = new System.Windows.Forms.Button();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.panelHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProductImage)).BeginInit();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            this.panelHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.panelHeader.Controls.Add(this.lblTitle);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Padding = new System.Windows.Forms.Padding(12);
            this.panelHeader.Size = new System.Drawing.Size(1000, 70);
            this.panelHeader.TabIndex = 0;
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(20, 15);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(286, 30);
            this.lblTitle.TabIndex = 0;
            this.lblTitle.Text = "Pharmacy Management";
            // 
            // panelSide
            // 
            this.panelSide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(250)))), ((int)(((byte)(245)))));
            this.panelSide.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelSide.Location = new System.Drawing.Point(0, 70);
            this.panelSide.Name = "panelSide";
            this.panelSide.Padding = new System.Windows.Forms.Padding(12);
            this.panelSide.Size = new System.Drawing.Size(220, 630);
            this.panelSide.TabIndex = 1;
            // 
            // btnAdd
            // 
            // 
            // btnShowOrders
            // 
            this.btnShowOrders.Location = new System.Drawing.Point(16, 16);
            this.btnShowOrders.Name = "btnShowOrders";
            this.btnShowOrders.Size = new System.Drawing.Size(188, 36);
            this.btnShowOrders.TabIndex = 2;
            this.btnShowOrders.Text = "View Orders";
            this.btnShowOrders.UseVisualStyleBackColor = true;
            this.btnShowOrders.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnShowOrders.ForeColor = System.Drawing.Color.White;
            this.btnShowOrders.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowOrders.FlatAppearance.BorderSize = 0;
            this.btnShowOrders.Click += new System.EventHandler(this.btnShowOrders_Click);

            // 
            // btnShowProducts
            // 
            this.btnShowProducts.Location = new System.Drawing.Point(16, 64);
            this.btnShowProducts.Name = "btnShowProducts";
            this.btnShowProducts.Size = new System.Drawing.Size(188, 48);
            this.btnShowProducts.TabIndex = 3;
            this.btnShowProducts.Text = "View Products";
            this.btnShowProducts.UseVisualStyleBackColor = true;
            this.btnShowProducts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(143)))), ((int)(((byte)(0)))));
            this.btnShowProducts.ForeColor = System.Drawing.Color.White;
            this.btnShowProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShowProducts.FlatAppearance.BorderSize = 0;
            this.btnShowProducts.Click += new System.EventHandler(this.btnShowProducts_Click);

            // 
            // btnRefreshProducts
            // 
            this.btnRefreshProducts.Name = "btnRefreshProducts";
            this.btnRefreshProducts.Size = new System.Drawing.Size(188, 48);
            this.btnRefreshProducts.TabIndex = 20;
            this.btnRefreshProducts.Text = "Refresh Products";
            this.btnRefreshProducts.UseVisualStyleBackColor = true;
            this.btnRefreshProducts.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(175)))), ((int)(((byte)(80)))));
            this.btnRefreshProducts.ForeColor = System.Drawing.Color.White;
            this.btnRefreshProducts.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefreshProducts.FlatAppearance.BorderSize = 0;
            this.btnRefreshProducts.Click += new System.EventHandler(this.btnRefreshProducts_Click);

            // 
            // btnConsultations
            // 
            this.btnConsultations.Location = new System.Drawing.Point(16, 368);
            this.btnConsultations.Name = "btnConsultations";
            this.btnConsultations.Size = new System.Drawing.Size(188, 36);
            this.btnConsultations.TabIndex = 9;
            this.btnConsultations.Text = "Consultations";
            this.btnConsultations.UseVisualStyleBackColor = true;
            this.btnConsultations.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnConsultations.ForeColor = System.Drawing.Color.White;
            this.btnConsultations.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConsultations.FlatAppearance.BorderSize = 0;
            this.btnConsultations.Click += new System.EventHandler(this.btnConsultations_Click);

            // 
            // btnPrescriptions
            // 
            this.btnPrescriptions.Location = new System.Drawing.Point(16, 412);
            this.btnPrescriptions.Name = "btnPrescriptions";
            this.btnPrescriptions.Size = new System.Drawing.Size(188, 36);
            this.btnPrescriptions.TabIndex = 10;
            this.btnPrescriptions.Text = "Prescriptions";
            this.btnPrescriptions.UseVisualStyleBackColor = true;
            this.btnPrescriptions.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(143)))), ((int)(((byte)(0)))));
            this.btnPrescriptions.ForeColor = System.Drawing.Color.White;
            this.btnPrescriptions.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrescriptions.FlatAppearance.BorderSize = 0;
            this.btnPrescriptions.Click += new System.EventHandler(this.btnPrescriptions_Click);

            // 
            // btnExportProduct
            // 
            this.btnExportProduct.Location = new System.Drawing.Point(16, 456);
            this.btnExportProduct.Name = "btnExportProduct";
            this.btnExportProduct.Size = new System.Drawing.Size(188, 36);
            this.btnExportProduct.TabIndex = 11;
            this.btnExportProduct.Text = "Export Product";
            this.btnExportProduct.UseVisualStyleBackColor = true;
            this.btnExportProduct.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnExportProduct.ForeColor = System.Drawing.Color.White;
            this.btnExportProduct.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportProduct.FlatAppearance.BorderSize = 0;
            this.btnExportProduct.Click += new System.EventHandler(this.btnExportProduct_Click);

            // 
            // btnAdd
            // 
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(188, 48);
            this.btnAdd.TabIndex = 4;
            this.btnAdd.Text = "Add Product";
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnAdd.ForeColor = System.Drawing.Color.White;
            this.btnAdd.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAdd.FlatAppearance.BorderSize = 0;
            // 
            // btnEdit
            // 
            this.btnEdit.Name = "btnEdit";
            this.btnEdit.Size = new System.Drawing.Size(188, 48);
            this.btnEdit.TabIndex = 5;
            this.btnEdit.Text = "Edit Product";
            this.btnEdit.UseVisualStyleBackColor = true;
            this.btnEdit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(143)))), ((int)(((byte)(0)))));
            this.btnEdit.ForeColor = System.Drawing.Color.White;
            this.btnEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEdit.FlatAppearance.BorderSize = 0;
            // 
            // btnDelete
            // 
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(188, 48);
            this.btnDelete.TabIndex = 6;
            this.btnDelete.Text = "Delete Product";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.btnDelete.ForeColor = System.Drawing.Color.White;
            this.btnDelete.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDelete.FlatAppearance.BorderSize = 0;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnClear
            // 
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(188, 48);
            this.btnClear.TabIndex = 7;
            this.btnClear.Text = "Clear Fields";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSave
            // 
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(188, 48);
            this.btnSave.TabIndex = 8;
            this.btnSave.Text = "Save Product";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(125)))), ((int)(((byte)(50)))));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.FlatAppearance.BorderSize = 0;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // Use a flow layout panel inside the side panel to ensure consistent spacing
            this.flowPanelSide.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowPanelSide.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flowPanelSide.WrapContents = false;
            this.flowPanelSide.AutoScroll = true;
            this.flowPanelSide.Padding = new System.Windows.Forms.Padding(8);

            // Standardize margins for each button
            var btnMargin = new System.Windows.Forms.Padding(8);
            this.btnShowOrders.Margin = btnMargin;
            this.btnShowProducts.Margin = btnMargin;
            this.btnRefreshProducts.Margin = btnMargin;
            this.btnConsultations.Margin = btnMargin;
            this.btnPrescriptions.Margin = btnMargin;
            this.btnExportProduct.Margin = btnMargin;
            this.btnAdd.Margin = btnMargin;
            this.btnEdit.Margin = btnMargin;
            this.btnDelete.Margin = btnMargin;
            this.btnClear.Margin = btnMargin;
            this.btnSave.Margin = btnMargin;

            // Add the flow panel into the side panel and populate it with buttons
            this.panelSide.Controls.Add(this.flowPanelSide);
            this.flowPanelSide.Controls.Add(this.btnShowOrders);
            this.flowPanelSide.Controls.Add(this.btnShowProducts);
            this.flowPanelSide.Controls.Add(this.btnRefreshProducts);
            this.flowPanelSide.Controls.Add(this.btnAdd);
            this.flowPanelSide.Controls.Add(this.btnEdit);
            this.flowPanelSide.Controls.Add(this.btnDelete);
            this.flowPanelSide.Controls.Add(this.btnClear);
            this.flowPanelSide.Controls.Add(this.btnSave);
            this.flowPanelSide.Controls.Add(this.btnConsultations);
            this.flowPanelSide.Controls.Add(this.btnPrescriptions);
            this.flowPanelSide.Controls.Add(this.btnExportProduct);
            // 
            // dgvProducts
            // 
            this.dgvProducts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvProducts.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvProducts.Height = 220;
            this.dgvProducts.Location = new System.Drawing.Point(0, 0);
            this.dgvProducts.Name = "dgvProducts";
            this.dgvProducts.TabIndex = 7;
            this.dgvProducts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvProducts.ReadOnly = true;
            this.dgvProducts.AllowUserToAddRows = false;
            this.dgvProducts.AllowUserToDeleteRows = false;
            this.dgvProducts.RowTemplate.Height = 32;
            this.dgvProducts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvProducts.SelectionChanged += new System.EventHandler(this.dgvProducts_SelectionChanged);
            this.dgvProducts.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvProducts_CellClick);

            // dgvOrderItems - shows items for the selected order (hidden for product view)
            this.dgvOrderItems = new System.Windows.Forms.DataGridView();
            this.panelOrderToolbar = new System.Windows.Forms.Panel();
            this.cbOrderStatus = new System.Windows.Forms.ComboBox();
            this.btnUpdateStatus = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).BeginInit();
            this.dgvOrderItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOrderItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOrderItems.Location = new System.Drawing.Point(0, 220);
            this.dgvOrderItems.Name = "dgvOrderItems";
            this.dgvOrderItems.ReadOnly = true;
            this.dgvOrderItems.AllowUserToAddRows = false;
            this.dgvOrderItems.AllowUserToDeleteRows = false;
            this.dgvOrderItems.RowTemplate.Height = 28;
            this.dgvOrderItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvOrderItems.Visible = false;
            this.dgvOrderItems.TabIndex = 18;
            // 
            // panelOrderToolbar
            // 
            this.panelOrderToolbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelOrderToolbar.Height = 48;
            this.panelOrderToolbar.Location = new System.Drawing.Point(0, 220);
            this.panelOrderToolbar.Name = "panelOrderToolbar";
            this.panelOrderToolbar.Padding = new System.Windows.Forms.Padding(8);
            this.panelOrderToolbar.TabIndex = 19;
            // 
            // cbOrderStatus
            // 
            this.cbOrderStatus.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbOrderStatus.Location = new System.Drawing.Point(8, 12);
            this.cbOrderStatus.Name = "cbOrderStatus";
            this.cbOrderStatus.Size = new System.Drawing.Size(220, 23);
            this.cbOrderStatus.Enabled = false;
            // 
            // btnUpdateStatus
            // 
            this.btnUpdateStatus.Location = new System.Drawing.Point(240, 8);
            this.btnUpdateStatus.Name = "btnUpdateStatus";
            this.btnUpdateStatus.Size = new System.Drawing.Size(120, 30);
            this.btnUpdateStatus.TabIndex = 1;
            this.btnUpdateStatus.Text = "Update Status";
            this.btnUpdateStatus.UseVisualStyleBackColor = true;
            this.btnUpdateStatus.Click += new System.EventHandler(this.btnUpdateStatus_Click);

            this.panelOrderToolbar.Controls.Add(this.cbOrderStatus);
            this.panelOrderToolbar.Controls.Add(this.btnUpdateStatus);

            ((System.ComponentModel.ISupportInitialize)(this.dgvOrderItems)).EndInit();
            // 
            // lblProductId
            // 
            this.lblProductId.AutoSize = true;
            this.lblProductId.Location = new System.Drawing.Point(12, 14);
            this.lblProductId.Name = "lblProductId";
            this.lblProductId.Size = new System.Drawing.Size(63, 13);
            this.lblProductId.TabIndex = 8;
            this.lblProductId.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblProductId.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblProductId.Text = "Product ID";
            // 
            // lblProductName (was lblPassword)
            // 
            this.lblProductName.AutoSize = true;
            this.lblProductName.Location = new System.Drawing.Point(260, 14);
            this.lblProductName.Name = "lblProductName";
            this.lblProductName.Size = new System.Drawing.Size(82, 13);
            this.lblProductName.TabIndex = 9;
            this.lblProductName.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.lblProductName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.lblProductName.Text = "Product Name";
            // 
            // txtProductName
            // 
            this.txtProductName.Location = new System.Drawing.Point(340, 12);
            this.txtProductName.Name = "txtProductName";
            this.txtProductName.Size = new System.Drawing.Size(420, 23);
            this.txtProductName.TabIndex = 10;
            this.txtProductName.Font = new System.Drawing.Font("Segoe UI", 9F);
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(500, 120);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(200, 20);
            this.txtDescription.TabIndex = 11;
            // 
            // txtPrice
            // 
            this.txtPrice.Location = new System.Drawing.Point(500, 145);
            this.txtPrice.Name = "txtPrice";
            this.txtPrice.Size = new System.Drawing.Size(100, 20);
            this.txtPrice.TabIndex = 12;
            // 
            // txtStock
            // 
            this.txtStock.Location = new System.Drawing.Point(600, 145);
            this.txtStock.Name = "txtStock";
            this.txtStock.Size = new System.Drawing.Size(100, 20);
            this.txtStock.TabIndex = 13;
            // 
            // txtProductId
            // 
            this.txtProductId.Location = new System.Drawing.Point(108, 12);
            this.txtProductId.Name = "txtProductId";
            this.txtProductId.Size = new System.Drawing.Size(120, 23);
            this.txtProductId.TabIndex = 14;
            this.txtProductId.Font = new System.Drawing.Font("Segoe UI", 9F);
            // 
            // cmbCategory
            // 
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Location = new System.Drawing.Point(400, 120);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(100, 21);
            this.cmbCategory.TabIndex = 15;
            // 
            // picProductImage
            // 
            this.picProductImage.Location = new System.Drawing.Point(820, 16);
            this.picProductImage.Name = "picProductImage";
            this.picProductImage.Size = new System.Drawing.Size(100, 100);
            this.picProductImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picProductImage.TabIndex = 16;
            this.picProductImage.TabStop = false;
            // 
            // btnUploadImage
            // 
            this.btnUploadImage.Location = new System.Drawing.Point(820, 120);
            this.btnUploadImage.Name = "btnUploadImage";
            this.btnUploadImage.Size = new System.Drawing.Size(120, 34);
            this.btnUploadImage.TabIndex = 17;
            this.btnUploadImage.Text = "Upload Image";
            this.btnUploadImage.UseVisualStyleBackColor = true;
            this.btnUploadImage.Click += new System.EventHandler(this.btnUploadImage_Click);
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(250)))), ((int)(((byte)(245)))));
            this.ClientSize = new System.Drawing.Size(1160, 700);
            // panelTop holds filter/inputs and image, docked to top of content area
            this.panelTop.Location = new System.Drawing.Point(220, 70);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(940, 160);
            this.panelTop.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelTop.BackColor = System.Drawing.Color.Transparent;
            // build a table layout for product inputs (5 rows)
            this.tblProductInputs.ColumnCount = 3;
            this.tblProductInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 110F));
            this.tblProductInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProductInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 140F));
            this.tblProductInputs.RowCount = 6;
            this.tblProductInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tblProductInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tblProductInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tblProductInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            this.tblProductInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tblProductInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tblProductInputs.Dock = System.Windows.Forms.DockStyle.Fill;

            // Labels
            this.lblProductId.Text = "Product ID";
            this.lblProductId.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblProductName.Text = "Product Name";
            this.lblProductName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblDescription.Text = "Description";
            this.lblDescription.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblCategory.Text = "Category";
            this.lblCategory.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblPrice.Text = "Price / Stock";
            this.lblPrice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblImage.Text = "Image";
            this.lblImage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            // adjust existing controls to fill cells
            this.txtProductId.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtProductName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtDescription.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtPrice.Width = 120;
            this.txtStock.Width = 120;
            this.cmbCategory.Width = 140;
            this.btnUploadImage.Dock = System.Windows.Forms.DockStyle.Top;
            this.picProductImage.Size = new System.Drawing.Size(120, 96);
            this.picProductImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;

            // place controls into table (col 0 = label, col1 = input, col2 = image area)
            this.tblProductInputs.Controls.Add(this.lblProductId, 0, 0);
            this.tblProductInputs.Controls.Add(this.txtProductId, 1, 0);
            this.tblProductInputs.Controls.Add(this.lblProductName, 0, 1);
            this.tblProductInputs.Controls.Add(this.txtProductName, 1, 1);
            this.tblProductInputs.Controls.Add(this.lblDescription, 0, 2);
            this.tblProductInputs.Controls.Add(this.txtDescription, 1, 2);
            this.tblProductInputs.Controls.Add(this.lblCategory, 0, 3);
            this.tblProductInputs.Controls.Add(this.cmbCategory, 1, 3);
            // image path row
            this.tblProductInputs.Controls.Add(this.lblImage, 0, 4);
            // txtImagePath will be placed in column 1, row 4
            this.tblProductInputs.Controls.Add(this.txtImagePath, 1, 4);

            // price and stock on same row within a horizontal panel (last row)
            var pnlPriceStock = new System.Windows.Forms.FlowLayoutPanel();
            pnlPriceStock.Dock = System.Windows.Forms.DockStyle.Left;
            pnlPriceStock.AutoSize = true;
            pnlPriceStock.Controls.Add(this.txtPrice);
            pnlPriceStock.Controls.Add(this.txtStock);
            pnlPriceStock.Padding = new System.Windows.Forms.Padding(0);
            pnlPriceStock.Margin = new System.Windows.Forms.Padding(0);
            this.tblProductInputs.Controls.Add(this.lblPrice, 0, 5);
            this.tblProductInputs.Controls.Add(pnlPriceStock, 1, 5);

            // image column spans all rows on right
            this.tblProductInputs.SetRowSpan(this.picProductImage, 6);
            this.tblProductInputs.Controls.Add(this.picProductImage, 2, 0);
            this.tblProductInputs.Controls.Add(this.btnUploadImage, 2, 5);

            this.panelTop.Controls.Add(this.tblProductInputs);

            // panelContent will contain DataGridView and be docked fill
            this.panelContent.Location = new System.Drawing.Point(220, 230);
            this.panelContent.Name = "panelContent";
            this.panelContent.Size = new System.Drawing.Size(940, 450);
            this.panelContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panelContent.BackColor = System.Drawing.Color.Transparent;
            this.panelContent.Controls.Add(this.dgvProducts);
            this.panelContent.Controls.Add(this.panelOrderToolbar);
            this.panelContent.Controls.Add(this.dgvOrderItems);

            this.Controls.Add(this.panelContent);
            this.Controls.Add(this.panelTop);
            this.Controls.Add(this.panelSide);
            this.Controls.Add(this.panelHeader);
            this.Name = "frmMain";
            this.Text = "Mahube Pharmacy - Product Management";
            this.panelHeader.ResumeLayout(false);
            this.panelHeader.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvProducts)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picProductImage)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.Panel panelSide;
        private System.Windows.Forms.FlowLayoutPanel flowPanelSide;
        private System.Windows.Forms.TableLayoutPanel tblProductInputs;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.Label lblPrice;
        private System.Windows.Forms.Label lblStock;
        private System.Windows.Forms.Label lblCategory;
        private System.Windows.Forms.Label lblImage;
        private System.Windows.Forms.Panel panelContent;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Button btnShowOrders;
        private System.Windows.Forms.Button btnShowProducts;
        private System.Windows.Forms.Button btnRefreshProducts;
        private System.Windows.Forms.Button btnConsultations;
        private System.Windows.Forms.Button btnPrescriptions;
        private System.Windows.Forms.Button btnExportProduct;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.Button btnEdit;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgvProducts;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblProductId;
        private System.Windows.Forms.Label lblProductName;
        private System.Windows.Forms.TextBox txtProductName;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TextBox txtPrice;
        private System.Windows.Forms.TextBox txtStock;
        private System.Windows.Forms.TextBox txtProductId;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.PictureBox picProductImage;
        private System.Windows.Forms.Button btnUploadImage;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.DataGridView dgvOrderItems;
        private System.Windows.Forms.Panel panelOrderToolbar;
        private System.Windows.Forms.ComboBox cbOrderStatus;
        private System.Windows.Forms.Button btnUpdateStatus;
    }
}
