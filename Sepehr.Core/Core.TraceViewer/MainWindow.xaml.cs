using Core.Cmn;
using Core.Cmn.Trace;
using DevExpress.Xpf.Editors.Settings;
using DevExpress.Xpf.Grid;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Core.TraceViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string GridConfigPath
        {
            get
            {
                try
                {
                    return System.IO.Path.GetDirectoryName(Assembly.GetEntryAssembly().Location) + "\\traceGrid.txt";
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
                return String.Empty;
            }
        }
        public static FilterBaseModel FilterBase { get; internal set; }
        // private static PeriodicTaskFactory _stateTimer;

        List<ExpandoObject> _currentTraceList;
        public List<ExpandoObject> CurrentTraceList
        {
            get
            {
                if (_currentTraceList == null)
                    _currentTraceList = new List<ExpandoObject>();
                return _currentTraceList;
            }
            set { _currentTraceList = value; }
        }

        //public Dictionary<int, string> AutoRefreshItems => new Dictionary<int, string> {
        //    {-1, "Select Auto Refresh..." },
        //    { 0, "Manual" },
        //    { 5, "5 Seconds" },
        //    { 30 , "30 Seconds"} ,
        //    { 60, "Minute"}
        //};

        public MainWindow()
        {
            try
            {
                CurrentTraceList = new List<ExpandoObject>();
                InitializeComponent();
                //TraceViewer = TraceViewer ?? new Core.Service.Trace.TraceViewerService();
                // _traceViewerService = new TraceViewerService();
                FilterBase = new FilterBaseModel();
                TraceListManager.DataChangedEvent += RefreshTraceData;
                //TraceListManager.ColumnChangedEvent += RefreshTraceData;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void refreshTraces_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TraceListManager.RefreshData(FilterBase);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void RefreshTraceData(object sender, EventArgs e)
        {
          
            CurrentTraceList = null;


            traceGrid.ItemsSource = null;
            traceGrid.Columns.Clear();


            List<string> columnNameList = new List<string>();
            //columnNameList.Add("Id");
            // columnNameList.Add("Level");
            columnNameList.Add("Message");
            columnNameList.Add("TraceKey");
            columnNameList.Add("SystemTime");
            columnNameList.Add("Writer");

            if (TraceListManager.Data != null)
            {
                for (int i = 0; i < TraceListManager.Data.Count; i++)
                {
                    var trace = new ExpandoObject();
                    //((IDictionary<string, object>)trace).Add("Id", TraceListManager.Data[i].Id);

                    // ((IDictionary<string, object>)trace).Add("Level", TraceListManager.Data[i].Level);

                    ((IDictionary<string, object>)trace).Add("Message", TraceListManager.Data[i].Message);

                    ((IDictionary<string, object>)trace).Add("TraceKey", TraceListManager.Data[i].TraceKey);

                    ((IDictionary<string, object>)trace).Add("SystemTime", TraceListManager.Data[i].SystemTime);

                    ((IDictionary<string, object>)trace).Add("Writer", TraceListManager.Data[i].Writer);


                    if (TraceListManager.Data[i].Data != null)
                    {
                        foreach (KeyValuePair<string, string> data in TraceListManager.Data[i].Data)
                        {
                            ((IDictionary<string, object>)trace).Add(data.Key, data.Value);

                            if (!columnNameList.Contains(data.Key))
                            {
                                columnNameList.Add(data.Key);
                            }
                        }
                    }
                    CurrentTraceList.Add(trace);

                }
                var firstRecord = CurrentTraceList.FirstOrDefault();
                foreach (var colName in columnNameList)
                {
                    //record haye ke in column ro nadarand empty mizaram ta mogheye group by dochare moshkel nasham
                    if (firstRecord != null && !((IDictionary<string, object>)firstRecord).ContainsKey(colName))
                    {
                        ((IDictionary<string, object>)firstRecord).Add(colName, "");
                    }
                    DevExpress.Xpf.Grid.GridColumn column = new DevExpress.Xpf.Grid.GridColumn { FieldName = colName };
                    if (colName.Equals("SystemTime"))
                    {

                        column.EditSettings = new TextEditSettings() { MaskUseAsDisplayFormat = true, Mask = "yyyy-MM-dd HH:mm:ss:ms", MaskType = DevExpress.Xpf.Editors.MaskType.DateTime };
                    }
                    column.AllowGrouping = DevExpress.Utils.DefaultBoolean.True;
                    Binding bindingExpression = new Binding(colName) { Mode = BindingMode.OneWay };
                    column.Binding = bindingExpression;
                    traceGrid.Columns.Add(column);

                }

                traceGrid.ItemsSource = CurrentTraceList;

            }



        }

        private void traceGrid_FilterChanged(object sender, RoutedEventArgs e)
        {
        }
       
       

        private void traceGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            { 
                if (traceGrid.SelectedItem != null)
                {
                    var selectedItem = ((IDictionary<string, object>)(traceGrid.SelectedItem));
                    TraceDto trace = new TraceDto
                    {
                        Message = ((dynamic)traceGrid.SelectedItem).Message,
                        SystemTime = ((dynamic)traceGrid.SelectedItem).SystemTime,
                        TraceKey = ((dynamic)traceGrid.SelectedItem).TraceKey,
                        Writer = ((dynamic)traceGrid.SelectedItem).Writer
                    };
                    string[] defaultCols = new string[] { "Message", "SystemTime", "TraceKey", "Writer" };
                    foreach (var key in selectedItem.Keys.Except(defaultCols))
                    {

                        trace.Data.Add(key, selectedItem[key]?.ToString());
                    }

                    OpenDetailsWindow(trace);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void OpenDetailsWindow(TraceDto selectedSource)
        {
            var detailsWindow = new TraceDetailsWindow(selectedSource);
            detailsWindow.Show();
        }

        private void btnCreateFilterBase_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var filterBaseWindow = new FilterBaseWindow();
                filterBaseWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
             

        private void SaveTraceConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var layoutStream = new MemoryStream();
                traceGrid.SaveLayoutToStream(layoutStream);
                layoutStream.Position = 0;
                byte[] bytes = new byte[layoutStream.Length];
                layoutStream.Read(bytes, 0, (int)layoutStream.Length);

                if (!File.Exists(GridConfigPath))
                    File.Create(GridConfigPath).Write(bytes, 0, bytes.Length);

                File.WriteAllBytes(GridConfigPath, bytes);
                 
                MessageBox.Show("ذخیره شد!");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ReloadGridConfig(GridControl grid)
        {
            if (File.Exists(GridConfigPath))
            {
                var stremReader = File.ReadAllBytes(GridConfigPath);
                grid.RestoreLayoutFromStream(new MemoryStream(stremReader));
            }
        }

        private void ReloadTraceConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ReloadGridConfig(traceGrid);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnDeleteRange_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var DeleteRangeWindow = new DeleteRangeWindow();
                DeleteRangeWindow.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
