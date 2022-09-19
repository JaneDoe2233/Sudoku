namespace SudokuTest
{
    partial class HomePage
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
            this.buttonEasy = new System.Windows.Forms.Button();
            this.buttonMiddle = new System.Windows.Forms.Button();
            this.buttonDifficult = new System.Windows.Forms.Button();
            this.textBoxMain = new System.Windows.Forms.TextBox();
            this.buttonHistory = new System.Windows.Forms.Button();
            this.buttonSetting = new System.Windows.Forms.Button();
            this.buttonCollect = new System.Windows.Forms.Button();
            this.labelMain = new System.Windows.Forms.Label();
            this.buttonRenew = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSimple
            // 
            this.buttonEasy.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonEasy.Location = new System.Drawing.Point(148, 255);
            this.buttonEasy.Name = "buttonSimple";
            this.buttonEasy.Size = new System.Drawing.Size(190, 54);
            this.buttonEasy.TabIndex = 0;
            this.buttonEasy.Text = "简单";
            this.buttonEasy.UseVisualStyleBackColor = true;
            // 
            // buttonMiddle
            // 
            this.buttonMiddle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonMiddle.Location = new System.Drawing.Point(148, 334);
            this.buttonMiddle.Name = "buttonMiddle";
            this.buttonMiddle.Size = new System.Drawing.Size(190, 54);
            this.buttonMiddle.TabIndex = 1;
            this.buttonMiddle.Text = "中等";
            this.buttonMiddle.UseVisualStyleBackColor = true;
            // 
            // buttonDifficult
            // 
            this.buttonDifficult.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonDifficult.Location = new System.Drawing.Point(148, 413);
            this.buttonDifficult.Name = "buttonDifficult";
            this.buttonDifficult.Size = new System.Drawing.Size(190, 54);
            this.buttonDifficult.TabIndex = 2;
            this.buttonDifficult.Text = "困难";
            this.buttonDifficult.UseVisualStyleBackColor = true;
            // 
            // textBoxMain
            // 
            this.textBoxMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.textBoxMain.Font = new System.Drawing.Font("Bahnschrift Light", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxMain.ForeColor = System.Drawing.SystemColors.MenuText;
            this.textBoxMain.Location = new System.Drawing.Point(81, 12);
            this.textBoxMain.Multiline = true;
            this.textBoxMain.Name = "textBoxMain";
            this.textBoxMain.ReadOnly = true;
            this.textBoxMain.Size = new System.Drawing.Size(332, 150);
            this.textBoxMain.TabIndex = 3;
            this.textBoxMain.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // buttonHistory
            // 
            this.buttonHistory.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonHistory.Location = new System.Drawing.Point(412, 520);
            this.buttonHistory.Name = "buttonHistory";
            this.buttonHistory.Size = new System.Drawing.Size(70, 40);
            this.buttonHistory.TabIndex = 4;
            this.buttonHistory.Text = "历史记录";
            this.buttonHistory.UseVisualStyleBackColor = true;
            // 
            // buttonSetting
            // 
            this.buttonSetting.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSetting.Location = new System.Drawing.Point(12, 508);
            this.buttonSetting.Name = "buttonSetting";
            this.buttonSetting.Size = new System.Drawing.Size(70, 40);
            this.buttonSetting.TabIndex = 5;
            this.buttonSetting.Text = "设置";
            this.buttonSetting.UseVisualStyleBackColor = true;
            // 
            // buttonCollect
            // 
            this.buttonCollect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonCollect.Location = new System.Drawing.Point(412, 460);
            this.buttonCollect.Name = "buttonCollect";
            this.buttonCollect.Size = new System.Drawing.Size(70, 40);
            this.buttonCollect.TabIndex = 6;
            this.buttonCollect.Text = "收藏记录";
            this.buttonCollect.UseVisualStyleBackColor = true;
            // 
            // labelMain
            // 
            this.labelMain.AutoSize = true;
            this.labelMain.BackColor = System.Drawing.SystemColors.ButtonHighlight;
            this.labelMain.Font = new System.Drawing.Font("Arial Black", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelMain.Location = new System.Drawing.Point(203, 72);
            this.labelMain.Name = "labelMain";
            this.labelMain.Size = new System.Drawing.Size(88, 24);
            this.labelMain.TabIndex = 7;
            this.labelMain.Text = "SUDOKU";
            // 
            // buttonRenew
            // 
            this.buttonRenew.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonRenew.Location = new System.Drawing.Point(148, 176);
            this.buttonRenew.Name = "buttonRenew";
            this.buttonRenew.Size = new System.Drawing.Size(190, 54);
            this.buttonRenew.TabIndex = 8;
            this.buttonRenew.Text = "继续游戏";
            this.buttonRenew.UseVisualStyleBackColor = true;
            // 
            // HomePage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(494, 590);
            this.Controls.Add(this.buttonRenew);
            this.Controls.Add(this.labelMain);
            this.Controls.Add(this.buttonCollect);
            this.Controls.Add(this.buttonSetting);
            this.Controls.Add(this.buttonHistory);
            this.Controls.Add(this.textBoxMain);
            this.Controls.Add(this.buttonDifficult);
            this.Controls.Add(this.buttonMiddle);
            this.Controls.Add(this.buttonEasy);
            this.Name = "HomePage";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "HomePage";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonEasy;
        private System.Windows.Forms.Button buttonMiddle;
        private System.Windows.Forms.Button buttonDifficult;
        private System.Windows.Forms.TextBox textBoxMain;
        private System.Windows.Forms.Button buttonHistory;
        private System.Windows.Forms.Button buttonSetting;
        private System.Windows.Forms.Button buttonCollect;
        private System.Windows.Forms.Label labelMain;
        private System.Windows.Forms.Button buttonRenew;
    }
}