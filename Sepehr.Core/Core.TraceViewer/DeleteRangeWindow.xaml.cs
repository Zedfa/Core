using System;
using System.Collections.Generic;
using System.Globalization;
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
    /// Interaction logic for DeleteRangeWindow.xaml
    /// </summary>
    public partial class DeleteRangeWindow : Window
    {
        public DeleteRangeWindow()
        {
            InitializeComponent();
            try
            {
                FilterBase = new FilterBaseModel();

                WriterList = TraceListManager.GetWriters() ?? new List<string>();

                this.DataContext = this;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }
        public List<string> WriterList { get; set; }
        public FilterBaseModel FilterBase { get; set; }

        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            DateTime startDate, endDate;
         
            try
            {
                if (string.IsNullOrEmpty(FilterBase.StartDate.Operand) && string.IsNullOrEmpty(FilterBase.EndDate.Operand))
                {
                    MessageBox.Show("you must set start date and end date");
                    return;
                }

                if (string.IsNullOrEmpty(FilterBase.Writer.Operand))
                {
                    MessageBox.Show("Select a Writer");
                    return;
                }

                if (DateTime.TryParse(FilterBase.StartDate.Operand, out startDate)
                    && DateTime.TryParse(FilterBase.EndDate.Operand, out endDate))
                {
                    TraceListManager.DeleteRange(startDate, endDate, FilterBase.Writer.Operand);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());

            }
            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
