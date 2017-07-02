namespace Kendo.Mvc.UI
{
    public class ChartElementBorder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartElementBorder" /> class.
        /// </summary>
        public ChartElementBorder(int? width, string color, ChartDashType? dashType)
        {
            Width = width;
            Color = color;
            DashType = dashType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChartElementBorder" /> class.
        /// </summary>
        public ChartElementBorder()
        {
        }

        /// <summary>
        /// Gets or sets the opacity of the border.
        /// </summary>
        public double? Opacity
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the width of the border.
        /// </summary>
        public int? Width
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the color of the border.
        /// </summary>
        public string Color
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the dash type of the border.
        /// </summary>
        public ChartDashType? DashType
        {
            get;
            set;
        }

        public IChartSerializer CreateSerializer()
        {
            return new ChartElementBorderSerializer(this);
        }
    }
}