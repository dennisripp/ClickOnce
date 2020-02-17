using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.ComponentModel;
using System.IO;
using System.Net;
using System.Diagnostics;
using MessageBoxResult = System.Windows.MessageBoxResult;


namespace WpfApp1.Updater2.Windows.Download
{
    /// <summary>
    /// Interaktionslogik für QubedotUpdateDownloadForm.xaml
    /// </summary>
    public partial class QubedotUpdateDownloadForm : Window
    {
        /// <summary>
        /// The web client to download the update
        /// </summary>
        private WebClient webClient;

     

        /// <summary>
        /// The thread to hash the file on
        /// </summary>
        private BackgroundWorker bgWorker;

        /// <summary>
        /// The MD5 hash of the file to download
        /// </summary>
        private string md5;

        /// <summary>
        /// Gets the temp file path for the downloaded file
        /// </summary>
        internal string TempFilePath { get; }

        /// <summary>
        /// Creates a new QubedotUpdateDownloadForm
        /// </summary>
        internal QubedotUpdateDownloadForm(Uri location, string md5, ImageSource programIcon)
        {
            InitializeComponent();

            if (programIcon != null)
                this.Icon = programIcon;

            // Set the temp file name and create new 0-byte file
            TempFilePath = System.IO.Path.GetTempFileName();

            this.md5 = md5;

            // Set up WebClient to download file
            webClient = new WebClient();
            webClient.DownloadProgressChanged += new DownloadProgressChangedEventHandler(webClient_DownloadProgressChanged);
            webClient.DownloadFileCompleted += new AsyncCompletedEventHandler(webClient_DownloadFileCompleted);

            // Set up backgroundworker to hash file
            bgWorker = new BackgroundWorker();
            bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
            bgWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bgWorker_RunWorkerCompleted);

            // Download file
            try { webClient.DownloadFileAsync(location, this.TempFilePath); }
            catch { this.DialogResult = MessageBoxResult.No; this.Close(); }
        }

        /// <summary>
        /// Downloads file from server
        /// </summary>
        private void webClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            // Update progressbar on download
            this.lblProgress.Text = string.Format("Downloaded {0} of {1}", FormatBytes(e.BytesReceived, 1, true), FormatBytes(e.TotalBytesToReceive, 1, true));
            this.progressBar.Value = e.ProgressPercentage;
        }

        private void webClient_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                this.DialogResult = MessageBoxResult.No;
                this.Close();
            }
            else if (e.Cancelled)
            {
                this.DialogResult = MessageBoxResult.Abort;
                this.Close();
            }
            else
            {
                // Show the "Hashing" label and set the progressbar to marquee
                //  this.lblProgress.Text = "Verifying Download...";
                // this.progressBar.Style = ProgressBarStyle.Marquee;

                // Start the hashing
                bgWorker.RunWorkerAsync(new string[] { this.TempFilePath, this.md5 });
            }
        }

        /// <summary>
        /// Formats the byte count to closest byte type
        /// </summary>
        /// <param name="bytes">The amount of bytes</param>
        /// <param name="decimalPlaces">How many decimal places to show</param>
        /// <param name="showByteType">Add the byte type on the end of the string</param>
        /// <returns>The bytes formatted as specified</returns>
        private string FormatBytes(long bytes, int decimalPlaces, bool showByteType)
        {
            double newBytes = bytes;
            string formatString = "{0";
            string byteType = "B";

            // Check if best size in KB
            if (newBytes > 1024 && newBytes < 1048576)
            {
                newBytes /= 1024;
                byteType = "KB";
            }
            else if (newBytes > 1048576 && newBytes < 1073741824)
            {
                // Check if best size in MB
                newBytes /= 1048576;
                byteType = "MB";
            }
            else
            {
                // Best size in GB
                newBytes /= 1073741824;
                byteType = "GB";
            }

            // Show decimals
            if (decimalPlaces > 0)
                formatString += ":0.";

            // Add decimals
            for (int i = 0; i < decimalPlaces; i++)
                formatString += "0";

            // Close placeholder
            formatString += "}";

            // Add byte type
            if (showByteType)
                formatString += byteType;

            return string.Format(formatString, newBytes);
        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            string file = ((string[])e.Argument)[0];
            string updateMD5 = ((string[])e.Argument)[1];

            // Hash the file and compare to the hash in the update xml
            if (Hasher.HashFile(file, HashType.MD5).ToUpper() != updateMD5.ToUpper())
                e.Result = MessageBoxResult.No;
            else
                e.Result = MessageBoxResult.OK;
        }

        private void bgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            this.DialogResult = (MessageBoxResult)e.Result;
            this.Close();
        }

        private void QubedotUpdateDownloadForm_FormClosed(object sender, EventArgs e)
        {
            if (webClient.IsBusy)
            {
                webClient.CancelAsync();
                this.DialogResult = MessageBoxResult.Abort;
            }

            if (bgWorker.IsBusy)
            {
                bgWorker.CancelAsync();
                this.DialogResult = MessageBoxResult.Abort;
            }
        }

        private void QubedotUpdateDownloadForm_Load(object sender, EventArgs e)
        {

        }
    }
}