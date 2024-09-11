namespace ClientGui
{
    partial class Client
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
            components = new System.ComponentModel.Container();
            startButton = new Button();
            getStatusButton = new Button();
            getServerStatusButton = new Button();
            getServerLogsButton = new Button();
            stopButton = new Button();
            infoRichTextBox = new RichTextBox();
            topicNameLabel = new Label();
            sendMessageGroupBox = new GroupBox();
            produceButton = new Button();
            payloadTextBox8 = new TextBox();
            payloadTextBox7 = new TextBox();
            payloadTextBox6 = new TextBox();
            payloadTextBox5 = new TextBox();
            payloadTextBox4 = new TextBox();
            payloadTextBox3 = new TextBox();
            payloadTextBox2 = new TextBox();
            payloadTextBox1 = new TextBox();
            payloadLabel = new Label();
            sendFileButton = new Button();
            pathToFileTextBox = new TextBox();
            pathToFileLabel = new Label();
            withdrawSubscriberButton = new Button();
            createSubscriberButton = new Button();
            withdrawProducerButton = new Button();
            createProducerButton = new Button();
            topicNameTextBox = new TextBox();
            connectOptionsGroupBox = new GroupBox();
            clientIDLabel = new Label();
            clientIDTextBox = new TextBox();
            serverPortTextBox = new TextBox();
            serverPortLabel = new Label();
            serverIPTextBox = new TextBox();
            serverIPLabel = new Label();
            serverConnectionTimer = new System.Windows.Forms.Timer(components);
            sendMessageGroupBox.SuspendLayout();
            connectOptionsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // startButton
            // 
            startButton.Location = new Point(85, 113);
            startButton.Name = "startButton";
            startButton.Size = new Size(130, 23);
            startButton.TabIndex = 0;
            startButton.Text = "Start";
            startButton.UseVisualStyleBackColor = true;
            startButton.Click += startButton_Click;
            // 
            // getStatusButton
            // 
            getStatusButton.Enabled = false;
            getStatusButton.Location = new Point(12, 12);
            getStatusButton.Name = "getStatusButton";
            getStatusButton.Size = new Size(130, 23);
            getStatusButton.TabIndex = 1;
            getStatusButton.Text = "Get Status";
            getStatusButton.UseVisualStyleBackColor = true;
            getStatusButton.Click += getStatusButton_Click;
            // 
            // getServerStatusButton
            // 
            getServerStatusButton.Enabled = false;
            getServerStatusButton.Location = new Point(148, 12);
            getServerStatusButton.Name = "getServerStatusButton";
            getServerStatusButton.Size = new Size(130, 23);
            getServerStatusButton.TabIndex = 2;
            getServerStatusButton.Text = "Get Server Status";
            getServerStatusButton.UseVisualStyleBackColor = true;
            getServerStatusButton.Click += getServerStatusButton_Click;
            // 
            // getServerLogsButton
            // 
            getServerLogsButton.Enabled = false;
            getServerLogsButton.Location = new Point(284, 12);
            getServerLogsButton.Name = "getServerLogsButton";
            getServerLogsButton.Size = new Size(130, 23);
            getServerLogsButton.TabIndex = 3;
            getServerLogsButton.Text = "Get Server Logs";
            getServerLogsButton.UseVisualStyleBackColor = true;
            getServerLogsButton.Click += getServerLogsButton_Click;
            // 
            // stopButton
            // 
            stopButton.Enabled = false;
            stopButton.Location = new Point(221, 113);
            stopButton.Name = "stopButton";
            stopButton.Size = new Size(130, 23);
            stopButton.TabIndex = 4;
            stopButton.Text = "Stop";
            stopButton.UseVisualStyleBackColor = true;
            stopButton.Click += stopButton_Click;
            // 
            // infoRichTextBox
            // 
            infoRichTextBox.Location = new Point(12, 41);
            infoRichTextBox.Name = "infoRichTextBox";
            infoRichTextBox.ReadOnly = true;
            infoRichTextBox.Size = new Size(402, 451);
            infoRichTextBox.TabIndex = 5;
            infoRichTextBox.TabStop = false;
            infoRichTextBox.Text = "";
            // 
            // topicNameLabel
            // 
            topicNameLabel.AutoSize = true;
            topicNameLabel.Location = new Point(6, 29);
            topicNameLabel.Name = "topicNameLabel";
            topicNameLabel.Size = new Size(73, 15);
            topicNameLabel.TabIndex = 6;
            topicNameLabel.Text = "Topic Name:";
            // 
            // sendMessageGroupBox
            // 
            sendMessageGroupBox.Controls.Add(produceButton);
            sendMessageGroupBox.Controls.Add(payloadTextBox8);
            sendMessageGroupBox.Controls.Add(payloadTextBox7);
            sendMessageGroupBox.Controls.Add(payloadTextBox6);
            sendMessageGroupBox.Controls.Add(payloadTextBox5);
            sendMessageGroupBox.Controls.Add(payloadTextBox4);
            sendMessageGroupBox.Controls.Add(payloadTextBox3);
            sendMessageGroupBox.Controls.Add(payloadTextBox2);
            sendMessageGroupBox.Controls.Add(payloadTextBox1);
            sendMessageGroupBox.Controls.Add(payloadLabel);
            sendMessageGroupBox.Controls.Add(sendFileButton);
            sendMessageGroupBox.Controls.Add(pathToFileTextBox);
            sendMessageGroupBox.Controls.Add(pathToFileLabel);
            sendMessageGroupBox.Controls.Add(withdrawSubscriberButton);
            sendMessageGroupBox.Controls.Add(createSubscriberButton);
            sendMessageGroupBox.Controls.Add(withdrawProducerButton);
            sendMessageGroupBox.Controls.Add(createProducerButton);
            sendMessageGroupBox.Controls.Add(topicNameTextBox);
            sendMessageGroupBox.Controls.Add(topicNameLabel);
            sendMessageGroupBox.Enabled = false;
            sendMessageGroupBox.Location = new Point(420, 169);
            sendMessageGroupBox.Name = "sendMessageGroupBox";
            sendMessageGroupBox.Size = new Size(368, 323);
            sendMessageGroupBox.TabIndex = 7;
            sendMessageGroupBox.TabStop = false;
            sendMessageGroupBox.Text = "Send Message";
            // 
            // produceButton
            // 
            produceButton.Location = new Point(221, 287);
            produceButton.Name = "produceButton";
            produceButton.Size = new Size(130, 23);
            produceButton.TabIndex = 24;
            produceButton.Text = "Produce";
            produceButton.UseVisualStyleBackColor = true;
            produceButton.Click += produceButton_Click;
            // 
            // payloadTextBox8
            // 
            payloadTextBox8.Location = new Point(221, 258);
            payloadTextBox8.Name = "payloadTextBox8";
            payloadTextBox8.Size = new Size(130, 23);
            payloadTextBox8.TabIndex = 23;
            // 
            // payloadTextBox7
            // 
            payloadTextBox7.Location = new Point(85, 258);
            payloadTextBox7.Name = "payloadTextBox7";
            payloadTextBox7.Size = new Size(130, 23);
            payloadTextBox7.TabIndex = 22;
            // 
            // payloadTextBox6
            // 
            payloadTextBox6.Location = new Point(221, 229);
            payloadTextBox6.Name = "payloadTextBox6";
            payloadTextBox6.Size = new Size(130, 23);
            payloadTextBox6.TabIndex = 21;
            // 
            // payloadTextBox5
            // 
            payloadTextBox5.Location = new Point(85, 229);
            payloadTextBox5.Name = "payloadTextBox5";
            payloadTextBox5.Size = new Size(130, 23);
            payloadTextBox5.TabIndex = 20;
            // 
            // payloadTextBox4
            // 
            payloadTextBox4.Location = new Point(221, 200);
            payloadTextBox4.Name = "payloadTextBox4";
            payloadTextBox4.Size = new Size(130, 23);
            payloadTextBox4.TabIndex = 19;
            // 
            // payloadTextBox3
            // 
            payloadTextBox3.Location = new Point(85, 200);
            payloadTextBox3.Name = "payloadTextBox3";
            payloadTextBox3.Size = new Size(130, 23);
            payloadTextBox3.TabIndex = 18;
            // 
            // payloadTextBox2
            // 
            payloadTextBox2.Location = new Point(221, 171);
            payloadTextBox2.Name = "payloadTextBox2";
            payloadTextBox2.Size = new Size(130, 23);
            payloadTextBox2.TabIndex = 17;
            // 
            // payloadTextBox1
            // 
            payloadTextBox1.Location = new Point(85, 171);
            payloadTextBox1.Name = "payloadTextBox1";
            payloadTextBox1.Size = new Size(130, 23);
            payloadTextBox1.TabIndex = 16;
            // 
            // payloadLabel
            // 
            payloadLabel.AutoSize = true;
            payloadLabel.Location = new Point(6, 174);
            payloadLabel.Name = "payloadLabel";
            payloadLabel.Size = new Size(52, 15);
            payloadLabel.TabIndex = 15;
            payloadLabel.Text = "Payload:";
            // 
            // sendFileButton
            // 
            sendFileButton.Location = new Point(221, 142);
            sendFileButton.Name = "sendFileButton";
            sendFileButton.Size = new Size(130, 23);
            sendFileButton.TabIndex = 14;
            sendFileButton.Text = "Send File";
            sendFileButton.UseVisualStyleBackColor = true;
            sendFileButton.Click += sendFileButton_Click;
            // 
            // pathToFileTextBox
            // 
            pathToFileTextBox.Location = new Point(85, 113);
            pathToFileTextBox.Name = "pathToFileTextBox";
            pathToFileTextBox.Size = new Size(266, 23);
            pathToFileTextBox.TabIndex = 13;
            // 
            // pathToFileLabel
            // 
            pathToFileLabel.AutoSize = true;
            pathToFileLabel.Location = new Point(6, 116);
            pathToFileLabel.Name = "pathToFileLabel";
            pathToFileLabel.Size = new Size(70, 15);
            pathToFileLabel.TabIndex = 12;
            pathToFileLabel.Text = "Path To File:";
            // 
            // withdrawSubscriberButton
            // 
            withdrawSubscriberButton.Location = new Point(221, 84);
            withdrawSubscriberButton.Name = "withdrawSubscriberButton";
            withdrawSubscriberButton.Size = new Size(130, 23);
            withdrawSubscriberButton.TabIndex = 11;
            withdrawSubscriberButton.Text = "Withdraw Subscriber";
            withdrawSubscriberButton.UseVisualStyleBackColor = true;
            withdrawSubscriberButton.Click += withdrawSubscriberButton_Click;
            // 
            // createSubscriberButton
            // 
            createSubscriberButton.Location = new Point(221, 55);
            createSubscriberButton.Name = "createSubscriberButton";
            createSubscriberButton.Size = new Size(130, 23);
            createSubscriberButton.TabIndex = 10;
            createSubscriberButton.Text = "Create Subscriber";
            createSubscriberButton.UseVisualStyleBackColor = true;
            createSubscriberButton.Click += createSubscriberButton_Click;
            // 
            // withdrawProducerButton
            // 
            withdrawProducerButton.Location = new Point(85, 84);
            withdrawProducerButton.Name = "withdrawProducerButton";
            withdrawProducerButton.Size = new Size(130, 23);
            withdrawProducerButton.TabIndex = 9;
            withdrawProducerButton.Text = "Withdraw Producer";
            withdrawProducerButton.UseVisualStyleBackColor = true;
            withdrawProducerButton.Click += withdrawProducerButton_Click;
            // 
            // createProducerButton
            // 
            createProducerButton.Location = new Point(85, 55);
            createProducerButton.Name = "createProducerButton";
            createProducerButton.Size = new Size(130, 23);
            createProducerButton.TabIndex = 8;
            createProducerButton.Text = "Create Producer";
            createProducerButton.UseVisualStyleBackColor = true;
            createProducerButton.Click += createProducerButton_Click;
            // 
            // topicNameTextBox
            // 
            topicNameTextBox.Location = new Point(85, 26);
            topicNameTextBox.Name = "topicNameTextBox";
            topicNameTextBox.Size = new Size(266, 23);
            topicNameTextBox.TabIndex = 7;
            // 
            // connectOptionsGroupBox
            // 
            connectOptionsGroupBox.Controls.Add(clientIDLabel);
            connectOptionsGroupBox.Controls.Add(clientIDTextBox);
            connectOptionsGroupBox.Controls.Add(serverPortTextBox);
            connectOptionsGroupBox.Controls.Add(serverPortLabel);
            connectOptionsGroupBox.Controls.Add(serverIPTextBox);
            connectOptionsGroupBox.Controls.Add(serverIPLabel);
            connectOptionsGroupBox.Controls.Add(startButton);
            connectOptionsGroupBox.Controls.Add(stopButton);
            connectOptionsGroupBox.Location = new Point(420, 12);
            connectOptionsGroupBox.Name = "connectOptionsGroupBox";
            connectOptionsGroupBox.Size = new Size(368, 151);
            connectOptionsGroupBox.TabIndex = 8;
            connectOptionsGroupBox.TabStop = false;
            connectOptionsGroupBox.Text = "Connect Options";
            // 
            // clientIDLabel
            // 
            clientIDLabel.AutoSize = true;
            clientIDLabel.Location = new Point(6, 87);
            clientIDLabel.Name = "clientIDLabel";
            clientIDLabel.Size = new Size(55, 15);
            clientIDLabel.TabIndex = 29;
            clientIDLabel.Text = "Client ID:";
            // 
            // clientIDTextBox
            // 
            clientIDTextBox.Location = new Point(85, 84);
            clientIDTextBox.Name = "clientIDTextBox";
            clientIDTextBox.Size = new Size(266, 23);
            clientIDTextBox.TabIndex = 28;
            clientIDTextBox.Text = "Client1";
            // 
            // serverPortTextBox
            // 
            serverPortTextBox.Location = new Point(85, 55);
            serverPortTextBox.Name = "serverPortTextBox";
            serverPortTextBox.Size = new Size(266, 23);
            serverPortTextBox.TabIndex = 27;
            serverPortTextBox.Text = "1234";
            // 
            // serverPortLabel
            // 
            serverPortLabel.AutoSize = true;
            serverPortLabel.Location = new Point(6, 58);
            serverPortLabel.Name = "serverPortLabel";
            serverPortLabel.Size = new Size(67, 15);
            serverPortLabel.TabIndex = 26;
            serverPortLabel.Text = "Server Port:";
            // 
            // serverIPTextBox
            // 
            serverIPTextBox.Location = new Point(85, 26);
            serverIPTextBox.Name = "serverIPTextBox";
            serverIPTextBox.Size = new Size(266, 23);
            serverIPTextBox.TabIndex = 25;
            serverIPTextBox.Text = "127.0.0.1";
            // 
            // serverIPLabel
            // 
            serverIPLabel.AutoSize = true;
            serverIPLabel.Location = new Point(6, 29);
            serverIPLabel.Name = "serverIPLabel";
            serverIPLabel.Size = new Size(55, 15);
            serverIPLabel.TabIndex = 25;
            serverIPLabel.Text = "Server IP:";
            // 
            // serverConnectionTimer
            // 
            serverConnectionTimer.Enabled = true;
            serverConnectionTimer.Interval = 1000;
            serverConnectionTimer.Tick += serverConnectionTimer_Tick;
            // 
            // Client
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 504);
            Controls.Add(connectOptionsGroupBox);
            Controls.Add(sendMessageGroupBox);
            Controls.Add(infoRichTextBox);
            Controls.Add(getServerLogsButton);
            Controls.Add(getServerStatusButton);
            Controls.Add(getStatusButton);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Client";
            Text = "Client";
            sendMessageGroupBox.ResumeLayout(false);
            sendMessageGroupBox.PerformLayout();
            connectOptionsGroupBox.ResumeLayout(false);
            connectOptionsGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private Button startButton;
        private Button getStatusButton;
        private Button getServerStatusButton;
        private Button getServerLogsButton;
        private Button stopButton;
        private RichTextBox infoRichTextBox;
        private Label topicNameLabel;
        private GroupBox sendMessageGroupBox;
        private TextBox topicNameTextBox;
        private Button createProducerButton;
        private Button withdrawProducerButton;
        private Button createSubscriberButton;
        private Button withdrawSubscriberButton;
        private TextBox pathToFileTextBox;
        private Label pathToFileLabel;
        private Button sendFileButton;
        private Label payloadLabel;
        private TextBox payloadTextBox1;
        private Button produceButton;
        private TextBox payloadTextBox8;
        private TextBox payloadTextBox7;
        private TextBox payloadTextBox6;
        private TextBox payloadTextBox5;
        private TextBox payloadTextBox4;
        private TextBox payloadTextBox3;
        private TextBox payloadTextBox2;
        private GroupBox connectOptionsGroupBox;
        private Label serverIPLabel;
        private TextBox serverPortTextBox;
        private Label serverPortLabel;
        private TextBox serverIPTextBox;
        private TextBox clientIDTextBox;
        private Label clientIDLabel;
        private System.Windows.Forms.Timer serverConnectionTimer;
    }
}
