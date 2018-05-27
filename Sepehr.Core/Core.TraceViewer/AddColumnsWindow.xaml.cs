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

namespace Core.TraceViewer
{
    /// <summary>
    /// Interaction logic for AddColumnsWindow.xaml
    /// </summary>
    public partial class AddColumnsWindow : Window
    {
        public Dictionary<string, string> ColumnsList { get; set; }
        public Dictionary<string, string> DisplayColumnsList { get; set; }
        public AddColumnsWindow(Dictionary<string, string> columns)
        {
            InitializeComponent();

            ColumnsList = columns;

            DisplayColumnsList = TraceListManager.DisplayCoumns;

            this.DataContext = this;

        }


        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (KeyValuePair<string, string>)lstAllColumns.SelectedItem;

            DisplayColumnsList.Add(selectedItem.Key, selectedItem.Value);

            ColumnsList.Remove(selectedItem.Key.ToString());

            lstAllColumns.Items.Refresh();

            lstDisplayColumns.Items.Refresh();

        }

        private void btnADelete_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (KeyValuePair<string, string>)lstDisplayColumns.SelectedItem;

            DisplayColumnsList.Remove(selectedItem.Key.ToString());

            ColumnsList.Add(selectedItem.Key, selectedItem.Value);

            lstAllColumns.Items.Refresh();

            lstDisplayColumns.Items.Refresh();
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            TraceListManager.DisplayCoumns = DisplayColumnsList;
            this.Close();
        }




    }
}
