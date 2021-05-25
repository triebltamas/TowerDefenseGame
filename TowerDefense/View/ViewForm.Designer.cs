
namespace TowerDefense
{
    partial class ViewForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip = new System.Windows.Forms.MenuStrip();
            this.fileMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.towerButton = new System.Windows.Forms.Button();
            this.minerButton = new System.Windows.Forms.Button();
            this.enemiesLabelPic = new System.Windows.Forms.Label();
            this.goldLabelPic = new System.Windows.Forms.Label();
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
            this.shooterButton = new System.Windows.Forms.Button();
            this.trapButton = new System.Windows.Forms.Button();
            this.upgradeButton = new System.Windows.Forms.Button();
            this.cannonButton = new System.Windows.Forms.Button();
            this.breakDownButton = new System.Windows.Forms.Button();
            this.goldLabel = new System.Windows.Forms.Label();
            this.enemiesLabel = new System.Windows.Forms.Label();
            this.menuStrip.SuspendLayout();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip
            // 
            this.menuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileMenuItem,
            this.newGameMenuItem});
            this.menuStrip.Location = new System.Drawing.Point(0, 0);
            this.menuStrip.Name = "menuStrip";
            this.menuStrip.Size = new System.Drawing.Size(484, 24);
            this.menuStrip.TabIndex = 0;
            this.menuStrip.Text = "menuStrip1";
            // 
            // fileMenuItem
            // 
            this.fileMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveGameMenuItem,
            this.loadGameMenuItem});
            this.fileMenuItem.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.fileMenuItem.Name = "fileMenuItem";
            this.fileMenuItem.Size = new System.Drawing.Size(31, 20);
            this.fileMenuItem.Text = "Fájl";
            // 
            // saveGameMenuItem
            // 
            this.saveGameMenuItem.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.saveGameMenuItem.Name = "saveGameMenuItem";
            this.saveGameMenuItem.Size = new System.Drawing.Size(104, 22);
            this.saveGameMenuItem.Text = "Mentés";
            // 
            // loadGameMenuItem
            // 
            this.loadGameMenuItem.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.loadGameMenuItem.Name = "loadGameMenuItem";
            this.loadGameMenuItem.Size = new System.Drawing.Size(104, 22);
            this.loadGameMenuItem.Text = "Betöltés";
            // 
            // newGameMenuItem
            // 
            this.newGameMenuItem.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.newGameMenuItem.Name = "newGameMenuItem";
            this.newGameMenuItem.Size = new System.Drawing.Size(49, 20);
            this.newGameMenuItem.Text = "Új játék";
            // 
            // towerButton
            // 
            this.towerButton.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.towerButton.Location = new System.Drawing.Point(9, 25);
            this.towerButton.Name = "towerButton";
            this.towerButton.Size = new System.Drawing.Size(43, 32);
            this.towerButton.TabIndex = 1;
            this.towerButton.Text = "Torony";
            this.towerButton.UseVisualStyleBackColor = true;
            // 
            // minerButton
            // 
            this.minerButton.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.minerButton.Location = new System.Drawing.Point(107, 25);
            this.minerButton.Name = "minerButton";
            this.minerButton.Size = new System.Drawing.Size(49, 32);
            this.minerButton.TabIndex = 2;
            this.minerButton.Text = "Bányász";
            this.minerButton.UseVisualStyleBackColor = true;
            // 
            // enemiesLabelPic
            // 
            this.enemiesLabelPic.AutoSize = true;
            this.enemiesLabelPic.Font = new System.Drawing.Font("Segoe UI", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enemiesLabelPic.Location = new System.Drawing.Point(377, 28);
            this.enemiesLabelPic.Name = "enemiesLabelPic";
            this.enemiesLabelPic.Size = new System.Drawing.Size(39, 28);
            this.enemiesLabelPic.TabIndex = 3;
            this.enemiesLabelPic.Text = "👺";
            // 
            // goldLabelPic
            // 
            this.goldLabelPic.AutoSize = true;
            this.goldLabelPic.Font = new System.Drawing.Font("Microsoft Sans Serif", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.goldLabelPic.Location = new System.Drawing.Point(434, 31);
            this.goldLabelPic.Name = "goldLabelPic";
            this.goldLabelPic.Size = new System.Drawing.Size(24, 25);
            this.goldLabelPic.TabIndex = 4;
            this.goldLabelPic.Text = "💲";
            // 
            // statusStrip
            // 
            this.statusStrip.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusLabel});
            this.statusStrip.Location = new System.Drawing.Point(0, 659);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(484, 22);
            this.statusStrip.TabIndex = 5;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusLabel
            // 
            this.statusLabel.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.statusLabel.Name = "statusLabel";
            this.statusLabel.Size = new System.Drawing.Size(0, 17);
            // 
            // tableLayoutPanel
            // 
            this.tableLayoutPanel.AutoSize = true;
            this.tableLayoutPanel.ColumnCount = 8;
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel.Location = new System.Drawing.Point(36, 60);
            this.tableLayoutPanel.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel.Name = "tableLayoutPanel";
            this.tableLayoutPanel.RowCount = 12;
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel.Size = new System.Drawing.Size(392, 588);
            this.tableLayoutPanel.TabIndex = 6;
            // 
            // shooterButton
            // 
            this.shooterButton.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.shooterButton.Location = new System.Drawing.Point(56, 25);
            this.shooterButton.Margin = new System.Windows.Forms.Padding(1);
            this.shooterButton.Name = "shooterButton";
            this.shooterButton.Size = new System.Drawing.Size(47, 32);
            this.shooterButton.TabIndex = 7;
            this.shooterButton.Text = "Puskás";
            this.shooterButton.UseVisualStyleBackColor = true;
            // 
            // trapButton
            // 
            this.trapButton.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.trapButton.Location = new System.Drawing.Point(162, 25);
            this.trapButton.Name = "trapButton";
            this.trapButton.Size = new System.Drawing.Size(39, 32);
            this.trapButton.TabIndex = 8;
            this.trapButton.Text = "Akna";
            this.trapButton.UseVisualStyleBackColor = true;
            // 
            // upgradeButton
            // 
            this.upgradeButton.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.upgradeButton.Location = new System.Drawing.Point(205, 25);
            this.upgradeButton.Margin = new System.Windows.Forms.Padding(1);
            this.upgradeButton.Name = "upgradeButton";
            this.upgradeButton.Size = new System.Drawing.Size(55, 32);
            this.upgradeButton.TabIndex = 9;
            this.upgradeButton.Text = "Fejlesztés";
            this.upgradeButton.UseVisualStyleBackColor = true;
            // 
            // cannonButton
            // 
            this.cannonButton.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.cannonButton.Location = new System.Drawing.Point(327, 25);
            this.cannonButton.Name = "cannonButton";
            this.cannonButton.Size = new System.Drawing.Size(44, 32);
            this.cannonButton.TabIndex = 10;
            this.cannonButton.Text = "Ágyú";
            this.cannonButton.UseVisualStyleBackColor = true;
            // 
            // breakDownButton
            // 
            this.breakDownButton.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.breakDownButton.Location = new System.Drawing.Point(264, 25);
            this.breakDownButton.Name = "breakDownButton";
            this.breakDownButton.Size = new System.Drawing.Size(57, 32);
            this.breakDownButton.TabIndex = 11;
            this.breakDownButton.Text = "Elbontás";
            this.breakDownButton.UseVisualStyleBackColor = true;
            // 
            // goldLabel
            // 
            this.goldLabel.AutoSize = true;
            this.goldLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.goldLabel.Location = new System.Drawing.Point(455, 41);
            this.goldLabel.Name = "goldLabel";
            this.goldLabel.Size = new System.Drawing.Size(15, 12);
            this.goldLabel.TabIndex = 12;
            this.goldLabel.Text = ": 0";
            // 
            // enemiesLabel
            // 
            this.enemiesLabel.AutoSize = true;
            this.enemiesLabel.Font = new System.Drawing.Font("Segoe UI", 7F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.enemiesLabel.Location = new System.Drawing.Point(413, 41);
            this.enemiesLabel.Name = "enemiesLabel";
            this.enemiesLabel.Size = new System.Drawing.Size(15, 12);
            this.enemiesLabel.TabIndex = 13;
            this.enemiesLabel.Text = ": 0";
            // 
            // ViewForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(484, 681);
            this.Controls.Add(this.enemiesLabel);
            this.Controls.Add(this.goldLabel);
            this.Controls.Add(this.breakDownButton);
            this.Controls.Add(this.cannonButton);
            this.Controls.Add(this.upgradeButton);
            this.Controls.Add(this.trapButton);
            this.Controls.Add(this.shooterButton);
            this.Controls.Add(this.goldLabelPic);
            this.Controls.Add(this.tableLayoutPanel);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.enemiesLabelPic);
            this.Controls.Add(this.minerButton);
            this.Controls.Add(this.towerButton);
            this.Controls.Add(this.menuStrip);
            this.MainMenuStrip = this.menuStrip;
            this.MaximumSize = new System.Drawing.Size(500, 720);
            this.MinimumSize = new System.Drawing.Size(500, 720);
            this.Name = "ViewForm";
            this.Text = "Xdd Tower Defense";
            this.menuStrip.ResumeLayout(false);
            this.menuStrip.PerformLayout();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip;
        private System.Windows.Forms.ToolStripMenuItem fileMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveGameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadGameMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameMenuItem;
        private System.Windows.Forms.Button towerButton;
        private System.Windows.Forms.Button minerButton;
        private System.Windows.Forms.Label enemiesLabelPic;
        private System.Windows.Forms.Label goldLabelPic;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel statusLabel;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel;
        private System.Windows.Forms.Button shooterButton;
        private System.Windows.Forms.Button trapButton;
        private System.Windows.Forms.Button upgradeButton;
        private System.Windows.Forms.Button cannonButton;
        private System.Windows.Forms.Button breakDownButton;
        private System.Windows.Forms.Label goldLabel;
        private System.Windows.Forms.Label enemiesLabel;
    }
}

