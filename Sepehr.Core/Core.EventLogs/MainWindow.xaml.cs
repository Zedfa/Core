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
            _viewer = new Viewer();
            _regisrant = new Registrant();
            LoadTreeViewEventLogsData();
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

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            (eventLogGrid.View as DevExpress.Xpf.Grid.TableView).BestFitArea = BestFitArea.All;

            (eventLogGrid.View as DevExpress.Xpf.Grid.TableView).BestFitColumns();
        }

        public string DefaultEventLog { get { return "TraceViewerService/Admin"; } }

        private void LoadTreeViewEventLogsData()
        {

            var eventLogs = _viewer.GetAllEventLogsName().Select(str => new TreeNodeEventLogViewModel()
            { Name = str, IsSelected = str == DefaultEventLog });
            treeViewEventLogs.ItemsSource = eventLogs;
        }

        private void treeViewEventLogs_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            var selectedItem = (TreeNodeEventLogViewModel)((System.Windows.Controls.TreeView)sender).SelectedValue;

            LoadGridEventsData(selectedItem.Name);
        }

        private void LoadGridEventsData(string logName)
        {
            if (!string.IsNullOrEmpty(logName))
            {
                var dataSource = _viewer.GetEventLogData(logName);
                eventLogGrid.ItemsSource = dataSource;
            }

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
            if (treeViewEventLogs.SelectedValue != null)
            {
                var selectedEventlog = ((TreeNodeEventLogViewModel)treeViewEventLogs.SelectedValue).Name;
                LoadGridEventsData(selectedEventlog);
            }
        }

        private void InstallEventLogButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _regisrant.SimulateInstall(DeploymentFolder,DefaultEventLog);
                MessageBox.Show("عملیات با موفقیت انجام شد", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                LoadTreeViewEventLogsData();
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
                    LoadTreeViewEventLogsData();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void Image_Loaded(object sender, RoutedEventArgs e)
        {
            ((Image)sender).Source = new BitmapImage(new Uri(((LogEntry)(((EditGridCellData)((Image)sender).DataContext)).RowData.Row).System.LevelIcon, UriKind.Relative));
        }
    }
}
