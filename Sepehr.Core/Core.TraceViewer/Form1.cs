using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace Core.TraceViewer
{
    public partial class Form1 : Form
    {
    
        private static string _logName = Assembly.GetExecutingAssembly().GetName().Name;
        private static string DeploymentFolder
        {
            get
            {
                return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), _logName);
            }
        }
        public Form1()
        {
            InitializeComponent();
          
        }

        
        private void btnCreateEventLog_Click(object sender, EventArgs e)
        {
            try
            {
                RegisterEventSourceWithOperatingSystem.SimulateInstall(DeploymentFolder);
                MessageBox.Show("عملیات با موفقیت انجام شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

      

        private void btnUnInstallEventLog_Click(object sender, EventArgs e)
        {
            var userAnswer = MessageBox.Show("آیا تمایل به حذف دارید؟ ","Question", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (userAnswer == DialogResult.Yes)
            {
                try
                {
                    RegisterEventSourceWithOperatingSystem.SimulateUninstall(DeploymentFolder);
                    MessageBox.Show("عملیات با موفقیت انجام شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }

                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }


    }
}

