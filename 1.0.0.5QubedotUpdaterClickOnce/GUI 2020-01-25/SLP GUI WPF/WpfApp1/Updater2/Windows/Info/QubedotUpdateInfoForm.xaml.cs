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



namespace WpfApp1.Updater2.Windows.Info
{
    /// <summary>
    /// Interaktionslogik für QubedotUpdateInfoForm.xaml
    /// </summary>
    public partial class QubedotUpdateInfoForm : Window
    {
        /// <summary>
        /// Creates a new QubedotUpdateInfoForm
        /// </summary>
      

        internal QubedotUpdateInfoForm(QubedotUpdateLocalAppInfo applicationInfo, QubedotUpdateXml updateInfo)
        {
            InitializeComponent();

            // Sets the icon if it's not null
           // if (applicationInfo.ApplicationIcon != null)
            //    this.Icon = applicationInfo.ApplicationIcon;

            // Fill in the UI
            
            this.lblVersions.Text = updateInfo.Tag == JobType.UPDATE ?
                string.Format("Current Version: {0}\nUpdate version: {1}", applicationInfo.Version.ToString(), updateInfo.Version.ToString()) :
                (updateInfo.Tag == JobType.ADD ? string.Format("Version: {0}", updateInfo.Version.ToString()) :
                "");
            this.txtDescription.Document.Blocks.Clear();
            this.txtDescription.Document.Blocks.Add(new Paragraph(new Run(updateInfo.Description)));
            
        }
      
        
        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void txtDescription_KeyDown(object sender, KeyEventArgs e)
        {
            // Only allow Cntrl - C to copy text
            if (!(e.Control && e.KeyCode == Keys.C))
                e.SuppressKeyPress = true;
        }

        private void pictureBox_Click(object sender, EventArgs e)
        {

        }
        private void txtDescription_TextChanged(object sender, EventArgs e)
        {

        }

        private void txtDescription_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}