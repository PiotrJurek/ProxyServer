namespace ServerGui
{
    partial class serverGuiForm
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
            startServerButton = new Button();
            stopServerButton = new Button();
            serverInfoRichTextBox = new RichTextBox();
            topicsInfoRichTextBox = new RichTextBox();
            topicsInfoLabel = new Label();
            serverInfoLabel = new Label();
            debugInfoLabel = new Label();
            debugInfoRichTextBox = new RichTextBox();
            SuspendLayout();
            // 
            // startServerButton
            // 
            startServerButton.Location = new Point(512, 12);
            startServerButton.Name = "startServerButton";
            startServerButton.Size = new Size(131, 23);
            startServerButton.TabIndex = 0;
            startServerButton.Text = "Start";
            startServerButton.UseVisualStyleBackColor = true;
            startServerButton.Click += startServerButton_Click;
            // 
            // stopServerButton
            // 
            stopServerButton.Enabled = false;
            stopServerButton.Location = new Point(649, 12);
            stopServerButton.Name = "stopServerButton";
            stopServerButton.Size = new Size(131, 23);
            stopServerButton.TabIndex = 1;
            stopServerButton.Text = "Stop";
            stopServerButton.UseVisualStyleBackColor = true;
            stopServerButton.Click += stopServerButton_Click;
            // 
            // serverInfoRichTextBox
            // 
            serverInfoRichTextBox.Location = new Point(512, 65);
            serverInfoRichTextBox.Name = "serverInfoRichTextBox";
            serverInfoRichTextBox.ReadOnly = true;
            serverInfoRichTextBox.Size = new Size(268, 55);
            serverInfoRichTextBox.TabIndex = 3;
            serverInfoRichTextBox.TabStop = false;
            serverInfoRichTextBox.Text = "";
            // 
            // topicsInfoRichTextBox
            // 
            topicsInfoRichTextBox.Location = new Point(12, 65);
            topicsInfoRichTextBox.Name = "topicsInfoRichTextBox";
            topicsInfoRichTextBox.ReadOnly = true;
            topicsInfoRichTextBox.Size = new Size(494, 347);
            topicsInfoRichTextBox.TabIndex = 4;
            topicsInfoRichTextBox.TabStop = false;
            topicsInfoRichTextBox.Text = "";
            // 
            // topicsInfoLabel
            // 
            topicsInfoLabel.AutoSize = true;
            topicsInfoLabel.Location = new Point(12, 47);
            topicsInfoLabel.Name = "topicsInfoLabel";
            topicsInfoLabel.Size = new Size(147, 15);
            topicsInfoLabel.TabIndex = 5;
            topicsInfoLabel.Text = "Informations about topics:";
            // 
            // serverInfoLabel
            // 
            serverInfoLabel.AutoSize = true;
            serverInfoLabel.Location = new Point(512, 47);
            serverInfoLabel.Name = "serverInfoLabel";
            serverInfoLabel.Size = new Size(146, 15);
            serverInfoLabel.TabIndex = 6;
            serverInfoLabel.Text = "Informations about server:";
            // 
            // debugInfoLabel
            // 
            debugInfoLabel.AutoSize = true;
            debugInfoLabel.Location = new Point(512, 123);
            debugInfoLabel.Name = "debugInfoLabel";
            debugInfoLabel.Size = new Size(116, 15);
            debugInfoLabel.TabIndex = 7;
            debugInfoLabel.Text = "Debug informations:";
            // 
            // debugInfoRichTextBox
            // 
            debugInfoRichTextBox.Location = new Point(512, 141);
            debugInfoRichTextBox.Name = "debugInfoRichTextBox";
            debugInfoRichTextBox.ReadOnly = true;
            debugInfoRichTextBox.Size = new Size(268, 271);
            debugInfoRichTextBox.TabIndex = 8;
            debugInfoRichTextBox.TabStop = false;
            debugInfoRichTextBox.Text = "";
            // 
            // serverGuiForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(792, 428);
            Controls.Add(debugInfoRichTextBox);
            Controls.Add(debugInfoLabel);
            Controls.Add(serverInfoLabel);
            Controls.Add(topicsInfoLabel);
            Controls.Add(topicsInfoRichTextBox);
            Controls.Add(serverInfoRichTextBox);
            Controls.Add(stopServerButton);
            Controls.Add(startServerButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "serverGuiForm";
            Text = "Proxy Server";
            FormClosing += serverGuiForm_FormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button startServerButton;
        private Button stopServerButton;
        private RichTextBox serverInfoRichTextBox;
        private RichTextBox topicsInfoRichTextBox;
        private Label topicsInfoLabel;
        private Label serverInfoLabel;
        private Label debugInfoLabel;
        private RichTextBox debugInfoRichTextBox;
    }
}
