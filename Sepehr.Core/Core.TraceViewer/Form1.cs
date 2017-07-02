using System;
using System.Configuration;
using System.Windows.Forms;

namespace Core.TraceViewer
{
    public partial class Form1 : Form
    {
        private static string _eventSourceName;
        public Form1()
        {
            InitializeComponent();
            //var eventListener = new TraceViewerEventListener(); 
            if (string.IsNullOrEmpty(ConfigurationManager.AppSettings["EventSourceName"]))
                throw new Exception("you must add 'EventSourceName' key in App.config ");

            _eventSourceName = ConfigurationManager.AppSettings["EventSourceName"].ToString();
        }

        private void btnCreateEventLog_Click(object sender, EventArgs e)
        {
            try
            {

                RegisterEventSourceWithOperatingSystem.SimulateInstall(_eventSourceName);
                MessageBox.Show("عملیات با موفقیت انجام شد", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnUnInstallEventLog_Click(object sender, EventArgs e)
        {
            var userAnswer = MessageBox.Show("در صورت حذف ، باید ویندوز را ریستارت کنید یا نام کلاس را تغییر دهید.\nآیا همچنان تمایل به حذف دارید؟ ",
                "Question", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (userAnswer == DialogResult.Yes)
            {
                try
                {
                    RegisterEventSourceWithOperatingSystem.SimulateUninstall(_eventSourceName);
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

