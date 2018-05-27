using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;


namespace Core.TraceViewer
{

    /// <summary>
    /// Interaction logic for DataFilterItem.xaml
    /// </summary>
    public partial class DataFilterItem : UserControl
    {
        //public object ItemsSource
        //{
        //    get { return (object)GetValue(ItemsSourceProperty); }
        //    set { SetValue(ItemsSourceProperty, value); }
        //}

        //public static readonly DependencyProperty ItemsSourceProperty =
        //    DependencyProperty.Register("ItemsSource", typeof(object), typeof(DataFilterItem), new PropertyMetadata(null));


        public DataFilterItem()
        {

            InitializeComponent();

        }

        private void btnDeleteFilter_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<DataKeyValuePair> parentDataFilters = (ObservableCollection<DataKeyValuePair>)this.Tag;
            parentDataFilters.Remove((DataKeyValuePair)this.DataContext);
        }
    }

    public class DataKeyValuePair : INotifyPropertyChanged
    {
        private string _key;
        private string _value;

        public DataKeyValuePair()
        {
            Logics = new Dictionary<string, string> { { "And", " && " }, { "Or", " || " } };
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public string Key
        {
            get { return _key; }
            set
            {
                _key = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Key"));
            }
        }
        public Dictionary<string, string> Logics { get; }


        private string _logic;

        public string Logic
        {
            get { return _logic; }
            set
            {
                _logic = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Logic"));

            }
        }

        public string Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Value"));
            }
        }
    }
}
