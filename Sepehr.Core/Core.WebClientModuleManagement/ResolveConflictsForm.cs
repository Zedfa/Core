using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Core.WebClientModuleManagement
{
    public partial class ResolveConflictsForm : Form
    {
        #region Fields

        private static string _filesVersionInfoPath = Path.GetFullPath(AppDomain.CurrentDomain.BaseDirectory + "FilesVersionInfo.txt");
        #endregion

        #region Constructors

        public ResolveConflictsForm()
        {
            InitializeComponent();
            FillForm();
        }
        #endregion

        #region Event Handlers

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var newSource = new List<ResourceInfo>();
            newSource.AddRange(ResourceManagementBase._conflictResourceInfoList);
            lbConflicts.DataSource = newSource;
            lbConflicts.Refresh();
        }
        private void btnMerge_Click(object sender, EventArgs e)
        {
            RunMergeTool();
        }
        private void btnKeepLocal_Click(object sender, EventArgs e)
        {
            foreach (var selectedItem in lbConflicts.SelectedItems)
            {
                var resourceInfo = (ResourceInfo)selectedItem;
                var sourceVersionFile = File.ReadAllText(resourceInfo.SourceVersionFilePath);
                var versionFile = File.ReadAllText(_filesVersionInfoPath);
                ResourceManagementBase.SyncFile(resourceInfo, true);

                ResourceManagementBase._conflictResourceInfoList.Remove(resourceInfo);
            }
            var newSource = new List<ResourceInfo>();
            newSource.AddRange(ResourceManagementBase._conflictResourceInfoList);
            lbConflicts.DataSource = newSource;
            lbConflicts.Refresh();
        }
        private void btnTakeSource_Click(object sender, EventArgs e)
        {
            foreach (var selectedItem in lbConflicts.SelectedItems)
            {
                var resourceInfo = (ResourceInfo)selectedItem;
                var sourceVersionFile = ResourceManagementBase.TryReadAllText(resourceInfo.SourceVersionFilePath);
                var versionFile = ResourceManagementBase.TryReadAllText(_filesVersionInfoPath);
                ResourceManagementBase.SyncFile(resourceInfo, false);

                ResourceManagementBase._conflictResourceInfoList.Remove(resourceInfo);
            }
            var newSource = new List<ResourceInfo>();
            newSource.AddRange(ResourceManagementBase._conflictResourceInfoList);
            lbConflicts.DataSource = newSource;
            lbConflicts.Refresh();
        }
        private void lbConflicts_DoubleClick(object sender, EventArgs e)
        {
            RunMergeTool();
        }        private void lbConflicts_SelectedIndexChanged(object sender, EventArgs e)
        {
            btnMerge.Enabled = lbConflicts.SelectedItems.Count == 1;
        }
        #endregion

        #region Helper Methods

        private void FillForm()
        {
            lbConflicts.DataSource = ResourceManagementBase._conflictResourceInfoList;
            lbConflicts.DisplayMember = "Key";
        }
        private void RunMergeTool()
        {
            if (lbConflicts.SelectedItems.Count == 0) { return; }
            try
            {
                var selected = (ResourceInfo)lbConflicts.SelectedItem;
                System.Diagnostics.Process.Start(@"C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\IDE\vsDiffMerge.exe",
                    String.Format("{0} {1} {2} {3} /m", selected.FullPath, selected.SourcePath, selected.FullPath, selected.FullPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
