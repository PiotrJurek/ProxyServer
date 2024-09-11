using ClientLibrary;
using System.Collections.Generic;
using System.Linq.Expressions;
using static System.Net.Mime.MediaTypeNames;

namespace ClientGui
{
    public partial class Client : Form
    {
        private ClientApi _client = new ClientApi();
        private bool _logsOn = false;

        public Client()
        {
            InitializeComponent();
        }

        delegate void ServerStatusCallback(Dictionary<string, string> payload);
        private void ShowServerStatus(Dictionary<string, string> payload)
        {
            if (this.infoRichTextBox.InvokeRequired)
            {
                ServerStatusCallback d = new ServerStatusCallback(ShowServerStatus);
                this.Invoke(d, new object[] { payload });
            }
            else
            {
                this.infoRichTextBox.Text += "\nReceived server status:\n";
                foreach (KeyValuePair<string, string> kv in payload)
                {
                    this.infoRichTextBox.Text += $"  {kv.Key}:  {kv.Value}\n";
                }
            }
        }

        delegate void ServerLogsCallback(bool status, string text);
        private void ShowServerLogs(bool status, string text)
        {
            if (this.infoRichTextBox.InvokeRequired)
            {
                ServerLogsCallback d = new ServerLogsCallback(ShowServerLogs);
                this.Invoke(d, new object[] { status, text });
            }
            else
            {
                this.infoRichTextBox.Text += $"\n{(status ? "Acknowledge" : "Reject")}:  {text}\n";
            }
        }

        delegate void SubscriberCallback(string type, Dictionary<string, string> payload);
        private void ShowMessageOrFile(string type, Dictionary<string, string> payload)
        {
            if (this.infoRichTextBox.InvokeRequired)
            {
                SubscriberCallback d = new SubscriberCallback(ShowMessageOrFile);
                this.Invoke(d, new object[] { type, payload });
            }
            else
            {
                if (type == "file")
                {
                    this.infoRichTextBox.Text += "\nReceived file:\n";
                    try
                    {
                        string filePath = Path.Combine(Environment.CurrentDirectory, payload["fileName"]);
                        byte[] fileData = Convert.FromBase64String(payload["fileContent"]);
                        File.WriteAllBytes(filePath, fileData);

                        this.infoRichTextBox.Text += $"File '{payload["fileName"]}' saved successfully at {filePath}.\n";
                    }
                    catch (Exception ex)
                    {
                        this.infoRichTextBox.Text += $"Error saving file: {ex.Message}\n";
                    }
                }
                else if (type == "message")
                {
                    this.infoRichTextBox.Text += "\nReceived message:\n";
                    foreach (KeyValuePair<string, string> kv in payload)
                    {
                        this.infoRichTextBox.Text += $"  {kv.Key}:  {kv.Value}\n";
                    }
                }
            }
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            try
            {
                _client.Start(serverIPTextBox.Text, int.Parse(serverPortTextBox.Text), clientIDTextBox.Text);
            }
            catch (Exception ex)
            {
                this.infoRichTextBox.Text += $"\nError connecting to server: {ex.Message}\n";
            }

            if (_client.IsConnected())
            {
                serverIPTextBox.Enabled = false;
                serverPortTextBox.Enabled = false;
                clientIDTextBox.Enabled = false;
                startButton.Enabled = false;

                sendMessageGroupBox.Enabled = true;
                getStatusButton.Enabled = true;
                getServerStatusButton.Enabled = true;
                getServerLogsButton.Enabled = true;
                stopButton.Enabled = true;
            }
        }

        private void stopButton_Click(object sender, EventArgs e)
        {
            _client.Stop();

            if (!_client.IsConnected())
            {
                serverIPTextBox.Enabled = true;
                serverPortTextBox.Enabled = true;
                clientIDTextBox.Enabled = true;
                startButton.Enabled = true;

                sendMessageGroupBox.Enabled = false;
                getStatusButton.Enabled = false;
                getServerStatusButton.Enabled = false;
                getServerLogsButton.Enabled = false;
                stopButton.Enabled = false;

                _logsOn = false;
            }
        }

        private void getStatusButton_Click(object sender, EventArgs e)
        {
            this.infoRichTextBox.Text += $"\n{_client.GetStatus()}\n";
        }

        private void getServerStatusButton_Click(object sender, EventArgs e)
        {
            try
            {
                _client.GetServerStatus(ShowServerStatus);
            }
            catch (Exception ex)
            {
                this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
            }
        }

        private void getServerLogsButton_Click(object sender, EventArgs e)
        {
            if (_logsOn)
            {
                try
                {
                    _client.GetServerLogs(null);
                    this.infoRichTextBox.Text += "\nGetting logs from the server turned off.\n";
                    _logsOn = !_logsOn;
                }
                catch (Exception ex)
                {
                    this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
                }
            }
            else
            {
                try
                {
                    _client.GetServerLogs(ShowServerLogs);
                    this.infoRichTextBox.Text += "\nGetting logs from the server turned on.\n";
                    _logsOn = !_logsOn;
                }
                catch (Exception ex)
                {
                    this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
                }
            }
        }

        private void createProducerButton_Click(object sender, EventArgs e)
        {
            try
            { 
                _client.CreateProducer(topicNameTextBox.Text);
            }
            catch (Exception ex)
            {
                this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
            }
        }

        private void createSubscriberButton_Click(object sender, EventArgs e)
        {
            try
            {
                _client.CreateSubscriber(topicNameTextBox.Text, ShowMessageOrFile);
            }
            catch (Exception ex)
            {
                this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
            }
        }

        private void withdrawProducerButton_Click(object sender, EventArgs e)
        {
            try
            {
                _client.WithdrawProducer(topicNameTextBox.Text);
            }
            catch (Exception ex)
            {
                this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
            }
        }

        private void withdrawSubscriberButton_Click(object sender, EventArgs e)
        {
            try
            {
                _client.WithdrawSubscriber(topicNameTextBox.Text);
            }
            catch (Exception ex)
            {
                this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
            }
        }

        private void sendFileButton_Click(object sender, EventArgs e)
        {
            try
            {
                _client.SendFile(topicNameTextBox.Text, pathToFileTextBox.Text);
            }
            catch (Exception ex)
            {
                this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
            }
        }

        private void produceButton_Click(object sender, EventArgs e)
        {
            Dictionary<string, string> payload = new Dictionary<string, string>();
            if (payloadTextBox1.Text != String.Empty && payloadTextBox2.Text != String.Empty)
            {
                payload.Add(payloadTextBox1.Text, payloadTextBox2.Text);
            }
            if (payloadTextBox3.Text != String.Empty && payloadTextBox4.Text != String.Empty)
            {
                payload.Add(payloadTextBox3.Text, payloadTextBox4.Text);
            }
            if (payloadTextBox5.Text != String.Empty && payloadTextBox6.Text != String.Empty)
            {
                payload.Add(payloadTextBox5.Text, payloadTextBox6.Text);
            }
            if (payloadTextBox7.Text != String.Empty && payloadTextBox8.Text != String.Empty)
            {
                payload.Add(payloadTextBox7.Text, payloadTextBox8.Text);
            }

            try
            {
                _client.Produce(topicNameTextBox.Text, payload);
            }
            catch (Exception ex)
            {
                this.infoRichTextBox.Text += $"\nError while sending message: {ex.Message}\n";
            }
        }

        private void serverConnectionTimer_Tick(object sender, EventArgs e)
        {
            if(!_client.IsConnected())
            {
                serverIPTextBox.Enabled = true;
                serverPortTextBox.Enabled = true;
                clientIDTextBox.Enabled = true;
                startButton.Enabled = true;

                sendMessageGroupBox.Enabled = false;
                getStatusButton.Enabled = false;
                getServerStatusButton.Enabled = false;
                getServerLogsButton.Enabled = false;
                stopButton.Enabled = false;

                _logsOn = false;
            }
        }
    }
}
