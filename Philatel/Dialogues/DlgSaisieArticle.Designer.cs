namespace Philatel
{
    partial class DlgSaisieArticle
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
            this.textBoxMotif = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeParution = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPrixPayé = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "&Motif :";
            // 
            // textBoxMotif
            // 
            this.textBoxMotif.Location = new System.Drawing.Point(55, 12);
            this.textBoxMotif.Name = "textBoxMotif";
            this.textBoxMotif.Size = new System.Drawing.Size(175, 20);
            this.textBoxMotif.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "&Date de parution :";
            // 
            // dateTimeParution
            // 
            this.dateTimeParution.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeParution.Location = new System.Drawing.Point(111, 38);
            this.dateTimeParution.Name = "dateTimeParution";
            this.dateTimeParution.ShowCheckBox = true;
            this.dateTimeParution.Size = new System.Drawing.Size(119, 20);
            this.dateTimeParution.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "&Prix payé :";
            // 
            // textBoxPrixPayé
            // 
            this.textBoxPrixPayé.Location = new System.Drawing.Point(75, 64);
            this.textBoxPrixPayé.Name = "textBoxPrixPayé";
            this.textBoxPrixPayé.Size = new System.Drawing.Size(66, 20);
            this.textBoxPrixPayé.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 67);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "$";
            // 
            // DlgSaisieArticle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(241, 142);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxMotif);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxPrixPayé);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.dateTimeParution);
            this.Controls.Add(this.label2);
            this.Name = "DlgSaisieArticle";
            this.Text = "DlgSaisieArticle";
            this.Controls.SetChildIndex(this.label2, 0);
            this.Controls.SetChildIndex(this.dateTimeParution, 0);
            this.Controls.SetChildIndex(this.label3, 0);
            this.Controls.SetChildIndex(this.textBoxPrixPayé, 0);
            this.Controls.SetChildIndex(this.label4, 0);
            this.Controls.SetChildIndex(this.textBoxMotif, 0);
            this.Controls.SetChildIndex(this.label1, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxMotif;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimeParution;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPrixPayé;
        private System.Windows.Forms.Label label4;
    }
}