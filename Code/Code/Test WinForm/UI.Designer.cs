namespace WindowsFormsApplication1
{
    partial class UI
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
            this.ConnectButton = new System.Windows.Forms.Button();
            this.DisconnectButton = new System.Windows.Forms.Button();
            this.BaseTrack = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.BaseLabel = new System.Windows.Forms.Label();
            this.MoveButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.BaseTrack)).BeginInit();
            this.SuspendLayout();
            // 
            // ConnectButton
            // 
            this.ConnectButton.Location = new System.Drawing.Point(15, 12);
            this.ConnectButton.Name = "ConnectButton";
            this.ConnectButton.Size = new System.Drawing.Size(146, 28);
            this.ConnectButton.TabIndex = 0;
            this.ConnectButton.Text = "Connect";
            this.ConnectButton.UseVisualStyleBackColor = true;
            this.ConnectButton.Click += new System.EventHandler(this.ConnectToVrEp);
            // 
            // DisconnectButton
            // 
            this.DisconnectButton.Location = new System.Drawing.Point(12, 46);
            this.DisconnectButton.Name = "DisconnectButton";
            this.DisconnectButton.Size = new System.Drawing.Size(149, 28);
            this.DisconnectButton.TabIndex = 1;
            this.DisconnectButton.Text = "Disonnect";
            this.DisconnectButton.UseVisualStyleBackColor = true;
            this.DisconnectButton.Click += new System.EventHandler(this.DisconnectButton_Click);
            // 
            // BaseTrack
            // 
            this.BaseTrack.Location = new System.Drawing.Point(15, 93);
            this.BaseTrack.Maximum = 90;
            this.BaseTrack.Minimum = -90;
            this.BaseTrack.Name = "BaseTrack";
            this.BaseTrack.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.BaseTrack.Size = new System.Drawing.Size(45, 222);
            this.BaseTrack.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Base Angle";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(79, 77);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 13);
            this.label2.TabIndex = 4;
            this.label2.Text = "Base Feedback";
            // 
            // BaseLabel
            // 
            this.BaseLabel.AutoSize = true;
            this.BaseLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BaseLabel.Location = new System.Drawing.Point(77, 93);
            this.BaseLabel.Name = "BaseLabel";
            this.BaseLabel.Size = new System.Drawing.Size(42, 25);
            this.BaseLabel.TabIndex = 5;
            this.BaseLabel.Text = "0.0";
            // 
            // MoveButton
            // 
            this.MoveButton.Location = new System.Drawing.Point(76, 121);
            this.MoveButton.Name = "MoveButton";
            this.MoveButton.Size = new System.Drawing.Size(79, 28);
            this.MoveButton.TabIndex = 6;
            this.MoveButton.Text = "Move Joint";
            this.MoveButton.UseVisualStyleBackColor = true;
            this.MoveButton.Click += new System.EventHandler(this.MoveJoint);
            // 
            // UI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(167, 327);
            this.Controls.Add(this.MoveButton);
            this.Controls.Add(this.BaseLabel);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BaseTrack);
            this.Controls.Add(this.DisconnectButton);
            this.Controls.Add(this.ConnectButton);
            this.Name = "UI";
            this.Text = "Test";
            ((System.ComponentModel.ISupportInitialize)(this.BaseTrack)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ConnectButton;
        private System.Windows.Forms.Button DisconnectButton;
        private System.Windows.Forms.TrackBar BaseTrack;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label BaseLabel;
        private System.Windows.Forms.Button MoveButton;
    }
}

