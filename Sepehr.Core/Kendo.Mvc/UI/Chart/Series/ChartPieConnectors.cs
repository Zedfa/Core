namespace Kendo.Mvc.UI
{
    public class ChartPieConnectors
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartPieConnectors" /> class.
        /// </summary>
        public ChartPieConnectors()
        {
        }

        /// <summary>
        /// Defines the width of the line.
        /// </summary>
        public int? Width
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the color of the line.
        /// </summary>
        public string Color
        {
            get;
            set;
        }

        /// <summary>
        /// Defines the padding of the line.
        /// </summary>
        public int? Padding
        {
            get;
            set;
        }

        public IChartSerializer CreateSerializer()
        {
            return new ChartPieConnectorsSerializer(this);
        }
    }
}