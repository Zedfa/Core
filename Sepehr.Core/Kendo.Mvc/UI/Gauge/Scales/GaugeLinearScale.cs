namespace Kendo.Mvc.UI
{
    public class GaugeLinearScale : GaugeScaleBase, ILinearScale
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GaugeLinearScale" /> class.
        /// </summary>
        /// <value>The linear gauge.</value>
        public GaugeLinearScale(LinearGauge gauge)
            : base()
        {
            Labels = new GaugeLinearScaleLabels();
            lienarGauge = gauge;
        }

        /// <summary>
        /// Gets or sets the linear gauge.
        /// </summary>
        /// <value>The linear gauge.</value>
        public LinearGauge lienarGauge
        {
            get;
            private set;
        }

        /// <summary>
        /// The scale mirror.
        /// </summary>
        public bool? Mirror
        {
            get;
            set;
        }

        /// <summary>
        /// The scale orientation.
        /// </summary>
        public bool? Vertical
        {
            get;
            set;
        }

        /// <summary>
        /// The scale labels.
        /// </summary>
        public GaugeLinearScaleLabels Labels
        {
            get;
            set;
        }

        public override IChartSerializer CreateSerializer()
        {
            return new GaugeLinearScaleSerializer(this);
        }
    }
}