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
            this.scoreBoardPanel1 = new SnakeGUI.ScoreBoardPanel();
            this.ScoreBoardLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // gamePanel1
            // 
            this.gamePanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.gamePanel1.Location = new System.Drawing.Point(12, 55);
            this.gamePanel1.Name = "gamePanel1";
            this.gamePanel1.Size = new System.Drawing.Size(452, 491);
            this.gamePanel1.TabIndex = 0;
            // 
            // scoreBoardPanel1
            // 
            this.scoreBoardPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.scoreBoardPanel1.Location = new System.Drawing.Point(471, 100);
            this.scoreBoardPanel1.Name = "scoreBoardPanel1";
            this.scoreBoardPanel1.Size = new System.Drawing.Size(238, 446);
            this.scoreBoardPanel1.TabIndex = 1;
            // 
            // ScoreBoardLabel
            // 
            this.ScoreBoardLabel.AutoSize = true;
            this.ScoreBoardLabel.Location = new System.Drawing.Point(551, 71);
            this.ScoreBoardLabel.Name = "ScoreBoardLabel";
            this.ScoreBoardLabel.Size = new System.Drawing.Size(83, 17);
            this.ScoreBoardLabel.TabIndex = 2;
            this.ScoreBoardLabel.Text = "ScoreBoard";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(721, 558);
            this.Controls.Add(this.ScoreBoardLabel);
            this.Controls.Add(this.scoreBoardPanel1);
            this.Controls.Add(this.gamePanel1);
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
    }
}

