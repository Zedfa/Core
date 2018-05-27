using Core.Cmn;
using Core.Cmn.Trace;
using Core.Service.Trace;
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
    /// Interaction logic for FilterBaseWindow.xaml
    /// </summary>
    public partial class FilterBaseWindow : Window
    {
        public List<string> WriterList { get; set; }
        public static FilterBaseModel FilterBase { get; set; }

        public FilterBaseWindow()
        {
            try
            {
                InitializeComponent();

                FilterBase = FilterBase ?? new FilterBaseModel();

                WriterList = WriterList ?? new List<string>();

                WriterList.Add(string.Empty);

                var templist = TraceListManager.GetWriters();

                if (templist != null)
                {
                    WriterList.AddRange(templist.OrderBy(w => w));
                }

                this.DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                MainWindow.FilterBase = FilterBase;

                TraceListManager.RefreshData(MainWindow.FilterBase);
                
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void cmbTraceWriters_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void cmbTraceKey_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                FilterBase = MainWindow.FilterBase;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnAddDataKey_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(FilterBase.Writer.Operand))
                MessageBox.Show("Select Writer");
            else
            {
                var dataFiltersWindow = new DataFiltersWindow();
                dataFiltersWindow.Show();
            }
        }
    }
}
