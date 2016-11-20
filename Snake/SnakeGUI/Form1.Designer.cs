namespace SnakeGUI
{
    partial class Form1
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
            this.gamePanel1 = new SnakeGUI.GamePanel();
            this.ScoreBoardLabel = new System.Windows.Forms.Label();
            this.NameBox = new System.Windows.Forms.TextBox();
            this.ServerBox = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.scoreBoardPanel1 = new SnakeGUI.ScoreBoardPanel();
            this.SuspendLayout();
            // 
            // gamePanel1
            // 
            this.gamePanel1.BackColor = System.Drawing.Color.White;
            this.gamePanel1.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.gamePanel1.Location = new System.Drawing.Point(15, 55);
            this.gamePanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.gamePanel1.MaximumSize = new System.Drawing.Size(750, 750);
            this.gamePanel1.MinimumSize = new System.Drawing.Size(750, 750);
            this.gamePanel1.Name = "gamePanel1";
            this.gamePanel1.Size = new System.Drawing.Size(750, 750);
            this.gamePanel1.TabIndex = 0;
            // 
            // ScoreBoardLabel
            // 
            this.ScoreBoardLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.ScoreBoardLabel.AutoSize = true;
            this.ScoreBoardLabel.Location = new System.Drawing.Point(949, 55);
            this.ScoreBoardLabel.Name = "ScoreBoardLabel";
            this.ScoreBoardLabel.Size = new System.Drawing.Size(83, 17);
            this.ScoreBoardLabel.TabIndex = 2;
            this.ScoreBoardLabel.Text = "ScoreBoard";
            // 
            // NameBox
            // 
            this.NameBox.Location = new System.Drawing.Point(120, 14);
            this.NameBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.NameBox.Name = "NameBox";
            this.NameBox.Size = new System.Drawing.Size(100, 22);
            this.NameBox.TabIndex = 3;
            this.NameBox.Text = "InputName";
            // 
            // ServerBox
            // 
            this.ServerBox.Location = new System.Drawing.Point(465, 14);
            this.ServerBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ServerBox.Name = "ServerBox";
            this.ServerBox.Size = new System.Drawing.Size(100, 22);
            this.ServerBox.TabIndex = 4;
            this.ServerBox.Text = "localhost";
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(379, 14);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(54, 17);
            this.ServerLabel.TabIndex = 5;
            this.ServerLabel.Text = "Server:";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(61, 14);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(49, 17);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "Name:";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(799, 14);
            this.ConnectButton.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 7;
            this.ConnectButton.Text = "Go";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.Connect);
            // 
            // scoreBoardPanel1
            // 
            this.scoreBoardPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.scoreBoardPanel1.BackColor = System.Drawing.Color.LightGray;
            this.scoreBoardPanel1.Location = new System.Drawing.Point(786, 74);
            this.scoreBoardPanel1.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.scoreBoardPanel1.Name = "scoreBoardPanel1";
            this.scoreBoardPanel1.Size = new System.Drawing.Size(362, 590);
            this.scoreBoardPanel1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1160, 822);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.ServerBox);
            this.Controls.Add(this.NameBox);
            this.Controls.Add(this.ScoreBoardLabel);
            this.Controls.Add(this.scoreBoardPanel1);
            this.Controls.Add(this.gamePanel1);
            this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.MinimumSize = new System.Drawing.Size(1151, 869);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.OnStartUp);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private GamePanel gamePanel1;
        private ScoreBoardPanel scoreBoardPanel1;
        private System.Windows.Forms.Label ScoreBoardLabel;
        private System.Windows.Forms.TextBox NameBox;
        private System.Windows.Forms.TextBox ServerBox;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Button ConnectButton;
    }
}

