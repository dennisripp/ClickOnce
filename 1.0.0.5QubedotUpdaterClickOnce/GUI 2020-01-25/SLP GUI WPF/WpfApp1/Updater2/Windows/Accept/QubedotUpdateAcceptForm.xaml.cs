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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WpfApp1.Updater2.Windows.Info;
using System.IO;
using WpfApp1.Updater2.Windows;
using WpfApp1.Updater2;
using MessageBoxResult = System.Windows.MessageBoxResult;

namespace WpfApp1.Updater2.Windows.Accept
{
    /// <summary>
    /// Interaktionslogik für QubedotUpdateAcceptForm.xaml
    /// </summary>
    internal partial class QubedotUpdateAcceptForm : Window
    {
        /// <summary>
        /// The program to update's info
        /// </summary>
        private QubedotUpdateLocalAppInfo applicationInfo;

        /// <summary>
        /// The update info from the update.xml
        /// </summary>
        private QubedotUpdateXml updateInfo;

        /// <summary>
        /// The update info display form
        /// </summary>
        private QubedotUpdateInfoForm updateInfoForm;

        /// <summary>
        /// Creates a new QubedotUpdateAcceptForm
        /// </summary>
        /// <param name="applicationInfo"></param>
        /// <param name="updateInfo"></param>
        

        internal QubedotUpdateAcceptForm(QubedotUpdateLocalAppInfo applicationInfo, QubedotUpdateXml updateInfo, int num_cur_update, int num_total_update)
        {
            InitializeComponent();

            this.applicationInfo = applicationInfo;
            this.updateInfo = updateInfo;
            this.txtDescription2.Document.Blocks.Clear();
            this.txtDescription2.Document.Blocks.Add(new Paragraph(new Run(string.Format("{0} - ({1}/{2}) Available Update", this.applicationInfo.ApplicationName, num_cur_update, num_total_update))));
            
            //this.lblUpdateAvail.Text = "An update for \"" + this.applicationInfo.ApplicationID + "\" is available.\r\nWould you like to update?";

            // Assigns the icon if it isn't null
           // if (this.applicationInfo.ApplicationIcon != null)
           //    this.Icon = this.applicationInfo.ApplicationIcon;

            // Adds the update version # to the form
            this.lblNewVersion.Text = updateInfo.Tag != JobType.REMOVE ?
                string.Format(updateInfo.Tag == JobType.UPDATE ? "Update: {0}\nNew Version: {1}" : "New: {0}\nVersion: {1}", System.IO.Path.GetFileName(this.applicationInfo.ApplicationPath), this.updateInfo.Version.ToString()) :
                string.Format("Remove: {0}", System.IO.Path.GetFileName(this.applicationInfo.ApplicationPath));
        }

        private void btnYes_Click(object sender, EventArgs e)
        {
            this.DialogResult = MessageBoxResult.Yes;
            this.Close();
        }

        private void btnNo_Click(object sender, EventArgs e)
        {
            this.DialogResult = MessageBoxResult.No;
            this.Close();
        }

        private void btnDetails_Click(object sender, EventArgs e)
        {
            if (this.updateInfoForm == null)
                this.updateInfoForm = new QubedotUpdateInfoForm(this.applicationInfo, this.updateInfo);

            // Shows the details form
            this.updateInfoForm.ShowDialog(this);
        }

        private void QubedotUpdateAcceptForm_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }

        private void btnDetails_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
