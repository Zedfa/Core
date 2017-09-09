using DevExpress.Xpf.Grid;
using Core.EventLogs.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Threading;
using Core.Cmn;

namespace Core.EventLogs
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Viewer _viewer;
        private Registrant _regisrant;
        // private List<LogEntry> _eventGridDataSource;
        public MainWindow()
        {
            GridControl.AllowInfiniteGridSize = true;
            InitializeComponent();
            cmbAutoRefrsh.DataContext = this;

            _viewer = new Viewer();
            _regisrant = new Registrant();
            LoadGridEventsData();
            this.Loaded += MainWindow_Loaded;
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


        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            (eventLogGrid.View as DevExpress.Xpf.Grid.TableView).BestFitArea = BestFitArea.All;

            (eventLogGrid.View as DevExpress.Xpf.Grid.TableView).BestFitColumns();
        }

        public string DefaultEventLog { get { return "TraceViewerService/Admin"; } }

        public void LoadGridEventsData()
        {
            var dataSource = _viewer.GetEventLogData(DefaultEventLog);
            eventLogGrid.ItemsSource = dataSource;

        }

        private void SaveLogGridConfig(object sender, RoutedEventArgs e)
        {
            var gridConfigPath = "EventLogGrid.txt";
            var layoutStream = new MemoryStream();
            eventLogGrid.SaveLayoutToStream(layoutStream);
            layoutStream.Position = 0;
            byte[] bytes = new byte[layoutStream.Length]; ;
            layoutStream.Read(bytes, 0, (int)layoutStream.Length);
            if (!File.Exists(gridConfigPath))
                File.Create(gridConfigPath);
            File.WriteAllBytes(gridConfigPath, bytes);
            MessageBox.Show("ذخیره شد!");

        }

        private void RefreshLogs(object sender, RoutedEventArgs e)
        {
            LoadGridEventsData();
        }

        private void InstallEventLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _regisrant.SimulateInstall(DeploymentFolder, DefaultEventLog);
                MessageBox.Show("عملیات با موفقیت انجام شد", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadGridEventsData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void UnInstallEventLogButton_Click(object sender, RoutedEventArgs e)
        {
            var userAnswer = MessageBox.Show("آیا تمایل به حذف دارید؟ ", "Question", MessageBoxButton.YesNo, MessageBoxImage.Warning);

            if (userAnswer == MessageBoxResult.Yes)
            {
                try
                {

                    _regisrant.SimulateUninstall(DeploymentFolder);
                    MessageBox.Show("عملیات با موفقیت انجام شد", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    LoadGridEventsData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
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
                _stateTimer = new PeriodicTaskFactory((task) => { this.Dispatcher.BeginInvoke(new Action(LoadGridEventsData)); },
                new TimeSpan(0, 0, delay),
                new TimeSpan(0, 0, delay));
                _stateTimer.Start();
            }
        }
    }
}
