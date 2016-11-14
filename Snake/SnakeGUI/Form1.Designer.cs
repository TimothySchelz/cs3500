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
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.ServerLabel = new System.Windows.Forms.Label();
            this.NameLabel = new System.Windows.Forms.Label();
            this.ConnectButton = new System.Windows.Forms.Button();
            this.scoreBoardPanel1 = new SnakeGUI.ScoreBoardPanel();
            this.SuspendLayout();
            // 
            // gamePanel1
            // 
            this.gamePanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.gamePanel1.Cursor = System.Windows.Forms.Cursors.AppStarting;
            this.gamePanel1.Location = new System.Drawing.Point(12, 62);
            this.gamePanel1.MaximumSize = new System.Drawing.Size(800, 800);
            this.gamePanel1.Name = "gamePanel1";
            this.gamePanel1.Size = new System.Drawing.Size(750, 750);
            this.gamePanel1.TabIndex = 0;
            // 
            // ScoreBoardLabel
            // 
            this.ScoreBoardLabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ScoreBoardLabel.AutoSize = true;
            this.ScoreBoardLabel.Location = new System.Drawing.Point(902, 79);
            this.ScoreBoardLabel.Name = "ScoreBoardLabel";
            this.ScoreBoardLabel.Size = new System.Drawing.Size(83, 17);
            this.ScoreBoardLabel.TabIndex = 2;
            this.ScoreBoardLabel.Text = "ScoreBoard";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(120, 13);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(100, 22);
            this.textBox1.TabIndex = 3;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(465, 13);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(100, 22);
            this.textBox2.TabIndex = 4;
            // 
            // ServerLabel
            // 
            this.ServerLabel.AutoSize = true;
            this.ServerLabel.Location = new System.Drawing.Point(378, 13);
            this.ServerLabel.Name = "ServerLabel";
            this.ServerLabel.Size = new System.Drawing.Size(54, 17);
            this.ServerLabel.TabIndex = 5;
            this.ServerLabel.Text = "Server:";
            // 
            // NameLabel
            // 
            this.NameLabel.AutoSize = true;
            this.NameLabel.Location = new System.Drawing.Point(61, 13);
            this.NameLabel.Name = "NameLabel";
            this.NameLabel.Size = new System.Drawing.Size(49, 17);
            this.NameLabel.TabIndex = 6;
            this.NameLabel.Text = "Name:";
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(799, 13);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(75, 23);
            this.ConnectButton.TabIndex = 7;
            this.ConnectButton.Text = "Go";
            this.ConnectButton.UseVisualStyleBackColor = true;
            // 
            // scoreBoardPanel1
            // 
            this.scoreBoardPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.scoreBoardPanel1.Location = new System.Drawing.Point(772, 99);
            this.scoreBoardPanel1.Name = "scoreBoardPanel1";
            this.scoreBoardPanel1.Size = new System.Drawing.Size(350, 713);
            this.scoreBoardPanel1.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1134, 824);
            this.Controls.Add(this.ConnectButton);
            this.Controls.Add(this.NameLabel);
            this.Controls.Add(this.ServerLabel);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.ScoreBoardLabel);
            this.Controls.Add(this.scoreBoardPanel1);
            this.Controls.Add(this.gamePanel1);
            this.MinimumSize = new System.Drawing.Size(1152, 871);
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
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Label ServerLabel;
        private System.Windows.Forms.Label NameLabel;
        private System.Windows.Forms.Button ConnectButton;
    }
}

