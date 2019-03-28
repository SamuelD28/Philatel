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
            this.label2 = new System.Windows.Forms.Label();
            this.dateTimeParution = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxPrixPayé = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMotif = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.comboBoxMotifs = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxTailleEtForme = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 1003;
            this.label1.Text = "&Motif :";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(92, 13);
            this.label2.TabIndex = 4001;
            this.label2.Text = "&Date de parution :";
            // 
            // dateTimeParution
            // 
            this.dateTimeParution.Format = System.Windows.Forms.DateTimePickerFormat.Short;
            this.dateTimeParution.Location = new System.Drawing.Point(111, 70);
            this.dateTimeParution.Name = "dateTimeParution";
            this.dateTimeParution.ShowCheckBox = true;
            this.dateTimeParution.Size = new System.Drawing.Size(119, 20);
            this.dateTimeParution.TabIndex = 4002;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 5001;
            this.label3.Text = "&Prix payé :";
            // 
            // textBoxPrixPayé
            // 
            this.textBoxPrixPayé.Location = new System.Drawing.Point(75, 96);
            this.textBoxPrixPayé.Name = "textBoxPrixPayé";
            this.textBoxPrixPayé.Size = new System.Drawing.Size(66, 20);
            this.textBoxPrixPayé.TabIndex = 5002;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(143, 99);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(13, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "$";
            // 
            // textBoxMotif
            // 
            this.textBoxMotif.Location = new System.Drawing.Point(252, 13);
            this.textBoxMotif.Name = "textBoxMotif";
            this.textBoxMotif.Size = new System.Drawing.Size(108, 20);
            this.textBoxMotif.TabIndex = 2002;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(187, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(60, 13);
            this.label5.TabIndex = 2001;
            this.label5.Text = "&Nouveau : ";
            // 
            // comboBoxMotifs
            // 
            this.comboBoxMotifs.FormattingEnabled = true;
            this.comboBoxMotifs.Location = new System.Drawing.Point(56, 13);
            this.comboBoxMotifs.Name = "comboBoxMotifs";
            this.comboBoxMotifs.Size = new System.Drawing.Size(121, 21);
            this.comboBoxMotifs.TabIndex = 1004;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 13);
            this.label6.TabIndex = 3001;
            this.label6.Text = "Taille et &forme :";
            // 
            // textBoxTailleEtForme
            // 
            this.textBoxTailleEtForme.Location = new System.Drawing.Point(97, 44);
            this.textBoxTailleEtForme.Name = "textBoxTailleEtForme";
            this.textBoxTailleEtForme.Size = new System.Drawing.Size(182, 20);
            this.textBoxTailleEtForme.TabIndex = 3002;
            // 
            // DlgSaisieArticle
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(372, 179);
            this.Controls.Add(this.textBoxTailleEtForme);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.comboBoxMotifs);
            this.Controls.Add(this.label5);
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
            this.Controls.SetChildIndex(this.label5, 0);
            this.Controls.SetChildIndex(this.comboBoxMotifs, 0);
            this.Controls.SetChildIndex(this.label6, 0);
            this.Controls.SetChildIndex(this.textBoxTailleEtForme, 0);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.DateTimePicker dateTimeParution;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox textBoxPrixPayé;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxMotif;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox comboBoxMotifs;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxTailleEtForme;
    }
}