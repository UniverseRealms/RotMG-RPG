namespace wServer
{
    partial class WorldManager
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
            this.btnGetList = new System.Windows.Forms.Button();
            this.txtPlayerList = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // btnGetList
            // 
            this.btnGetList.Location = new System.Drawing.Point(13, 13);
            this.btnGetList.Name = "btnGetList";
            this.btnGetList.Size = new System.Drawing.Size(196, 51);
            this.btnGetList.TabIndex = 0;
            this.btnGetList.Text = "Get Player List";
            this.btnGetList.UseVisualStyleBackColor = true;
            this.btnGetList.Click += new System.EventHandler(this.btnGetList_Click);
            // 
            // txtPlayerList
            // 
            this.txtPlayerList.Location = new System.Drawing.Point(13, 87);
            this.txtPlayerList.Multiline = true;
            this.txtPlayerList.Name = "txtPlayerList";
            this.txtPlayerList.Size = new System.Drawing.Size(476, 365);
            this.txtPlayerList.TabIndex = 1;
            // 
            // WorldManager
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(501, 464);
            this.Controls.Add(this.txtPlayerList);
            this.Controls.Add(this.btnGetList);
            this.Name = "WorldManager";
            this.Text = "WorldManager";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGetList;
        private System.Windows.Forms.TextBox txtPlayerList;
    }
}