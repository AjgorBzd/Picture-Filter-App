
namespace cg_project1._1
{
    partial class FilterNameForm
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
            this.label1 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.OKNameButton = new System.Windows.Forms.Button();
            this.CancelNameButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(47, 29);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(157, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name your new filter...";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(47, 61);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(276, 27);
            this.NameTextBox.TabIndex = 1;
            // 
            // OKNameButton
            // 
            this.OKNameButton.Location = new System.Drawing.Point(47, 110);
            this.OKNameButton.Name = "OKNameButton";
            this.OKNameButton.Size = new System.Drawing.Size(94, 29);
            this.OKNameButton.TabIndex = 0;
            this.OKNameButton.Text = "Apply";
            this.OKNameButton.UseVisualStyleBackColor = true;
            this.OKNameButton.Click += new System.EventHandler(this.OKNameButton_Click);
            // 
            // CancelNameButton
            // 
            this.CancelNameButton.Location = new System.Drawing.Point(166, 110);
            this.CancelNameButton.Name = "CancelNameButton";
            this.CancelNameButton.Size = new System.Drawing.Size(94, 29);
            this.CancelNameButton.TabIndex = 3;
            this.CancelNameButton.Text = "Cancel";
            this.CancelNameButton.UseVisualStyleBackColor = true;
            this.CancelNameButton.Click += new System.EventHandler(this.CancelNameButton_Click);
            // 
            // FilterNameForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 169);
            this.Controls.Add(this.CancelNameButton);
            this.Controls.Add(this.OKNameButton);
            this.Controls.Add(this.NameTextBox);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FilterNameForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Filter Name";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.Button OKNameButton;
        private System.Windows.Forms.Button CancelNameButton;
    }
}