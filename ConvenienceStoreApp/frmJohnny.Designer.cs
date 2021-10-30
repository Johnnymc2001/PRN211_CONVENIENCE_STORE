
namespace ConvenienceStoreApp
{
    partial class frmJohnny
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
            this.ucSelfOrdersView1 = new ConvenienceStoreApp.ucSelfOrdersView();
            this.SuspendLayout();
            // 
            // ucSelfOrdersView1
            // 
            this.ucSelfOrdersView1.Location = new System.Drawing.Point(13, 27);
            this.ucSelfOrdersView1.loggedStaff = null;
            this.ucSelfOrdersView1.Name = "ucSelfOrdersView1";
            this.ucSelfOrdersView1.Size = new System.Drawing.Size(1085, 554);
            this.ucSelfOrdersView1.TabIndex = 0;
            // 

            // frmJohnny
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1169, 630);

            this.Controls.Add(this.ucSelfOrdersView1);

            this.Name = "frmJohnny";
            this.Text = "frmJohnny";
            this.Load += new System.EventHandler(this.frmJohnny_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ucSelfOrdersView ucSelfOrdersView1;
    }
}