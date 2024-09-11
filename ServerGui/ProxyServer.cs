using ServerLibrary;
using ServerLibrary.Config;
using System.Collections.ObjectModel;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace ServerGui
{
    public partial class serverGuiForm : Form
    {
        private ProxyServer _proxyServer;

        public serverGuiForm()
        {
            InitializeComponent();
            _proxyServer = new ProxyServer();
            _proxyServer.OnUpdateServerStatus += _proxyServer_OnUpdateServerStatus;
            SetTopicsInfoText(_proxyServer.GetTopicsInfo());
        }

        private void _proxyServer_OnUpdateServerStatus(object sender, ServerStatusEventArgs e)
        {
            switch (e.UpdateType)
            {
                case UpdateType.Info:
                case UpdateType.Error:
                    AddDebugInfoText(e.Message + "\n");
                    break;
                case UpdateType.TopicsChange:
                    SetTopicsInfoText(((ProxyServer)sender).GetTopicsInfo());
                    break;
                case UpdateType.ServerStarted:
                    AddServerInfoText(e.Message + "\n");
                    break;
                case UpdateType.ServerStoped:
                    if(e.Message == "*")
                    {
                        ResetServerInfoText();
                    }
                    else
                    {
                        RemoveServerInfoText(e.Message + "\n");
                    }
                    break;
            }
        }

        private void LT_CollectionChanged(object? sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs e)
        {
            SetTopicsInfoText(_proxyServer.GetTopicsInfo());
        }

        private void AddDebugInfoText(string text)
        {
            if (serverInfoRichTextBox.InvokeRequired)
            {
                Invoke(AddDebugInfoText, [text]);
            }
            else
            {
                debugInfoRichTextBox.Text += text;
            }
        }

        private void AddServerInfoText(string text)
        {
            if (serverInfoRichTextBox.InvokeRequired)
            {
                Invoke(AddServerInfoText, [text]);
            }
            else
            {
                serverInfoRichTextBox.Text += text;
                startServerButton.Enabled = false;
                stopServerButton.Enabled = true;
            }
        }

        private void ResetServerInfoText()
        {
            if (serverInfoRichTextBox.InvokeRequired)
            {
                Invoke(ResetServerInfoText);
            }
            else
            {
                serverInfoRichTextBox.Text = "";
                startServerButton.Enabled = true;
                stopServerButton.Enabled = false;
            }
        }

        private void RemoveServerInfoText(string text)
        {
            if (serverInfoRichTextBox.InvokeRequired)
            {
                Invoke(RemoveServerInfoText, [text]);
            }
            else
            {
                string serverInfoText = serverInfoRichTextBox.Text;
                int index = serverInfoText.IndexOf(text, StringComparison.Ordinal);
                serverInfoRichTextBox.Text = (index < 0) ? serverInfoText : serverInfoText.Remove(index, text.Length);
            }
        }

        private void SetTopicsInfoText(string text)
        {
            if (topicsInfoRichTextBox.InvokeRequired)
            {
                Invoke(SetTopicsInfoText, [text]);
            }
            else
            {
                topicsInfoRichTextBox.Text = text;
            }
        }

        private void startServerButton_Click(object sender, EventArgs e)
        {
            _proxyServer.Start();
        }

        private void stopServerButton_Click(object sender, EventArgs e)
        {
            _proxyServer.Stop();
        }

        private void serverGuiForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            _proxyServer?.Stop();
        }
    }
}
