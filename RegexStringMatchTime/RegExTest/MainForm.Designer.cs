namespace RegExTest
{
    partial class MainForm
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
            this.btnLoadResolutionMessages = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblStatus = new System.Windows.Forms.Label();
            this.grdErrorsViewer = new DevExpress.XtraGrid.GridControl();
            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdErrorsViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // btnLoadResolutionMessages
            // 
            this.btnLoadResolutionMessages.Dock = System.Windows.Forms.DockStyle.Right;
            this.btnLoadResolutionMessages.Location = new System.Drawing.Point(180, 2);
            this.btnLoadResolutionMessages.Name = "btnLoadResolutionMessages";
            this.btnLoadResolutionMessages.Size = new System.Drawing.Size(101, 27);
            this.btnLoadResolutionMessages.TabIndex = 0;
            this.btnLoadResolutionMessages.Text = "Parse messages";
            this.btnLoadResolutionMessages.UseVisualStyleBackColor = true;
            this.btnLoadResolutionMessages.Click += new System.EventHandler(this.btnLoadResolutionMessages_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblStatus);
            this.panel1.Controls.Add(this.btnLoadResolutionMessages);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(0, 230);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(0, 2, 3, 2);
            this.panel1.Size = new System.Drawing.Size(284, 31);
            this.panel1.TabIndex = 1;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(3, 9);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 13);
            this.lblStatus.TabIndex = 1;
            // 
            // grdErrorsViewer
            // 
            this.grdErrorsViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdErrorsViewer.Location = new System.Drawing.Point(0, 0);
            this.grdErrorsViewer.MainView = this.gridView1;
            this.grdErrorsViewer.Name = "grdErrorsViewer";
            this.grdErrorsViewer.Size = new System.Drawing.Size(284, 230);
            this.grdErrorsViewer.TabIndex = 2;
            this.grdErrorsViewer.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            this.gridView1});
            // 
            // gridView1
            // 
            this.gridView1.GridControl = this.grdErrorsViewer;
            this.gridView1.Name = "gridView1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.grdErrorsViewer);
            this.Controls.Add(this.panel1);
            this.Name = "MainForm";
            this.Text = "MainForm";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdErrorsViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnLoadResolutionMessages;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblStatus;
        private DevExpress.XtraGrid.GridControl grdErrorsViewer;
        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;

    }
}