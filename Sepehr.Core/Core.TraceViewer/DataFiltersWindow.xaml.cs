using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;


namespace Core.TraceViewer
{
    /// <summary>
    /// Interaction logic for DataFiltersWindow.xaml
    /// </summary>
    public partial class DataFiltersWindow : Window
    {
        private ObservableCollection<DataKeyValuePair> _dataFilters;

        private ObservableCollection<DataKeyValuePair> DataFilters
        {
            get
            {
                if (_dataFilters == null)
                    _dataFilters = new ObservableCollection<DataKeyValuePair>();

                return _dataFilters;
            }
            set { _dataFilters = value; }
        }


        public DataFiltersWindow()
        {
            InitializeComponent();

            foreach (var item in FilterBaseWindow.FilterBase.Data)
            {
                DataFilters.Add(new DataKeyValuePair { Key = item.Key, Value = item.Value.Operand, Logic = item.Value.Logic });
            }

            ucDataFilters.Tag = DataFilters;
            ucDataFilters.ItemsSource = DataFilters;
            
            this.DataContext = this;

        }

        private void btnAddFilter_Click(object sender, RoutedEventArgs e)
        {
            DataFilters.Add(new DataKeyValuePair() { Key = "", Value = "" });

        }

        private void btnDeleteFilter_Click(object sender, RoutedEventArgs e)
        {
            //DataFilters.RemoveAt()

        }
        private void btnConfirm_Click(object sender, RoutedEventArgs e)
        {
            //foreach (var item in DataFilters)
            //{
            //   // KeyValuePair<string, FilterElements> existData;
            //   // if (!FilterBaseWindow.FilterBase.Data.Find(item=> item.Key.ToLower(), out existData))
            //    {
            //        FilterBaseWindow.FilterBase.Data.Add( new KeyValuePair<string, string> item.Key.ToLower(), item.Value.ToLower());
            //    }
            //}
            FilterBaseWindow.FilterBase.Data.Clear(); 

            foreach (var item in DataFilters)
            {
                //var existFilter = FilterBaseWindow.FilterBase.Data.Where(data =>
                //data.Key.ToLower().Equals(item.Key.ToLower())
                //&&
                //data.Value.Operand.ToLower().Equals(item.Value.ToLower())
                // &&
                // data.Value.Logic == item.Logic
                //).ToList();

                //if (existFilter.Count == 0)
                //{

                    FilterBaseWindow.FilterBase.Data.Add(new KeyValuePair<string, FilterElements>(item.Key.ToLower(), new FilterElements { Operand = item.Value.ToLower(), Logic = item.Logic }));
                //}
            }

            this.Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            DataFilters.Clear();

            foreach (var item in FilterBaseWindow.FilterBase.Data)
            {
                DataFilters.Add(new DataKeyValuePair { Key = item.Key, Value = item.Value.Operand.ToLower() });
            }
            this.Close();
        }

    }

}
