using Core.Cmn;
using Core.Cmn.Extensions;
using Core.Cmn.SharedMemory;
using Core.Cmn.Trace;
using DevExpress.Xpf.Grid;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Linq.Dynamic;
using DevExpress.Data.Filtering;
using Core.EventLogs.ViewModels;
using Core.Service;

namespace Core.EventLogs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private List<TraceViewModel> logs;
        private ISharedMemoryService _sharedMemoryService;
        private Reader<TraceDto> Reader;

        public MainWindow()
        {
            GridControl.AllowInfiniteGridSize = true;

            InitializeComponent();

            logs = new List<TraceViewModel>();
            logs.Add(new TraceViewModel(new TraceDto { Level = 1, Message = "viewer was started!", TraceKey = "viewer" }));

            cmbAutoRefrsh.DataContext = this;
      
            this.Loaded += MainWindow_Loaded;

            _sharedMemoryService = new SharedMemoryService();

        }

        private static string _logName = Assembly.GetExecutingAssembly().GetName().Name;

        private static string DeploymentFolder
        {
            get
            {
                return System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), _logName);
            }
        }

        public Dictionary<int, string> AutoRefreshItems => new Dictionary<int, string> {
            {-1, "Select Auto Refresh..." },
            { 0, "Manual" },
            { 5, "5 Seconds" },
            { 30 , "30 Seconds"} ,
            { 60, "Minute"}
        };
        private List<Config> _allSharedMemories;
        public List<Config> AllSharedMemories
        {
            get
            {
                _allSharedMemories = _allSharedMemories ?? new List<Config>();
                return _allSharedMemories;
            }
            set
            {

                _allSharedMemories = value;
            }
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SetSharedMemoriesList();


            (eventLogGrid.View as DevExpress.Xpf.Grid.TableView).BestFitArea = BestFitArea.All;

            (eventLogGrid.View as DevExpress.Xpf.Grid.TableView).BestFitColumns();
        }


        private void SaveLogGridConfig(object sender, RoutedEventArgs e)
        {
            var gridConfigPath = "EventLogGrid.txt";

            var layoutStream = new MemoryStream();

            eventLogGrid.SaveLayoutToStream(layoutStream);

            layoutStream.Position = 0;

            byte[] bytes = new byte[layoutStream.Length];

            layoutStream.Read(bytes, 0, (int)layoutStream.Length);

            if (!File.Exists(gridConfigPath))
                File.Create(gridConfigPath);

            File.WriteAllBytes(gridConfigPath, bytes);

            MessageBox.Show("ذخیره شد!");
        }

        private void RefreshLogs(object sender, RoutedEventArgs e)
        {
            InsertNewDataToGrid();
        }
         
        private static PeriodicTaskFactory _stateTimer;

        private void cmbAutoRefrsh_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var delay = (int)cmbAutoRefrsh.SelectedValue;
            if (_stateTimer != null)
            {
                _stateTimer.CancelToken = new CancellationToken(true);
            }

            if (delay > 0)
            {
                _stateTimer = new PeriodicTaskFactory((task) =>
                {
                    //var dataSource = _viewer.GetEventLogData();
                    this.Dispatcher.BeginInvoke(new Action(InsertNewDataToGrid));
                },
                new TimeSpan(0, 0, delay),
                new TimeSpan(0, 0, delay));
                _stateTimer.Start();
            }
        } 

        private void eventLogGrid_FilterChanged(object sender, RoutedEventArgs e)
        {
            FilterGridDataSource();
        }

        private void FilterGridDataSource()
        {
            var gridFilter = eventLogGrid.FilterCriteria;

            if (gridFilter != null)
            {
                var filters = new Filters();

                var temp = logs.Where(filters.ConvertToWhereClause(eventLogGrid.FilterCriteria));

                eventLogGrid.ItemsSource = temp.TakeLast(1000);
            }

            else
            {
                if (logs.Count >= 1000)
                    eventLogGrid.ItemsSource = logs.TakeLast(1000);
                else
                    eventLogGrid.ItemsSource = logs;
            }
        }

        #region 1000 ta 1000ta be grid ezafe mikone
        //private void InsertNewDataToGrid()
        //{
        //    Stopwatch stopwatch = new Stopwatch();
        //    stopwatch.Start();


        //    logs.AddRange(Reader.Memory.Data);
        //    var cat = logs.Count / 1000;
        //    var remain = logs.Count % 1000;
        //    var counterCat = 0;
        //    List<Source> datasource = new List<Source>();
        //    _stateTimer1 = new PeriodicTaskFactory((task) =>
        //          {

        //              Action action = () =>
        //              {
        //                  var startPoint = datasource.Count ;
        //                  var length =1000;
        //                  if (counterCat > cat)
        //                  {
        //                      _stateTimer.CancelToken = new CancellationToken(true);
        //                  }
        //                  else
        //                  {
        //                      if (cat == counterCat)
        //                          length += remain > 0 ? remain : 1000;

        //                      for (int i = startPoint; i < (length + startPoint ); i++)
        //                      {
        //                          datasource.Add(logs[i]);
        //                      }

        //                      eventLogGrid.ItemsSource = datasource;
        //                      counterCat += 1;
        //                  }


        //              };
        //              this.Dispatcher.BeginInvoke(action);
        //          },
        //    new TimeSpan(0, 0, 1),
        //    new TimeSpan(0, 0, 1));
        //    _stateTimer1.Start();

        //    stopwatch.Stop();
        //    // MessageBox.Show($"time for assign grid data source: { stopwatch.Elapsed.ToString()}");
        //}
        #endregion

        private void InsertNewDataToGrid()
        {
            //Stopwatch stopwatch = new Stopwatch();
            //stopwatch.Start();
            if (Reader != null)
            {
               // Thread.Sleep(20000);

                var sharedMemoryData = Reader.Memory.Data;
                foreach (var item in sharedMemoryData)
                {
                    if (item != null)
                        logs.Add(new TraceViewModel(item));
                }
                //for (int i = 0; i < sharedMemoryData.Count-1 ; i++)
                //{
                //    logs.Add(new TraceViewModel(sharedMemoryData[i]));
                //}
                FilterGridDataSource();
            }

            // stopwatch.Stop();
            // MessageBox.Show($"time for assign grid data source: { stopwatch.Elapsed.ToString()}");
        }



        private void cmbAllSharedMemories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedItem = (Config)cmbAllSharedMemories.SelectedItem;

            if (selectedItem != null)
            {
                Reader = Reader<TraceDto>.GetReader(selectedItem.Name, selectedItem.Size);

                InsertNewDataToGrid();
            }

        }

        private void sharedMemoriesRefresh_Click(object sender, RoutedEventArgs e)
        {
            SetSharedMemoriesList();

        }

        private void SetSharedMemoriesList()
        {

            AllSharedMemories.Clear();

            AllSharedMemories.AddRange(Core.Cmn.AppBase.SharedMemoryList);
            cmbAllSharedMemories.ItemsSource = AllSharedMemories;

        }
        private void sharedMemoriesDispose_Click(object sender, RoutedEventArgs e)
        {

            _sharedMemoryService.Dispose<TraceDto>(((Config)cmbAllSharedMemories.SelectedItem).Name);

            cmbAllSharedMemories.ItemsSource = Core.Cmn.AppBase.SharedMemoryList;
        }
    }
}