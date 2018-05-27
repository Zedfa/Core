using Core.Cmn.Trace;
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
    /// Interaction logic for TraceDetailsWindow.xaml
    /// </summary>
    public partial class TraceDetailsWindow : Window
    {
        public TraceDto Detail { get; set; }


        public TraceDetailsWindow(TraceDto data)
        {
            InitializeComponent();

            this.Detail = data;

            this.DataContext = this;
        }


    }
}
