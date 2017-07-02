namespace Kendo.Mvc.UI
{
    using System;
    using System.Collections.Generic;

    public class ChartDateAxis<T> : ChartAxisBase<T, DateTime>, IChartDateAxis where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartNumericAxis{T}" /> class.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public ChartDateAxis(Chart<T> chart)
            : base(chart)
        {
            AxisCrossingValues = new List<DateTime>();
            MajorGridLines = new ChartLine();
            MinorGridLines = new ChartLine();
            Labels = new ChartAxisLabels();
        }

        /// <summary>
        /// Specifies the date category axis base unit.
        /// </summary>
        public ChartAxisBaseUnit? BaseUnit
        {
            get;
            set;
        }

        /// <summary>
        /// The values at which perpendicular axes cross this axis.
        /// </summary>
        public IEnumerable<DateTime> AxisCrossingValues
        {
            get;
            set;
        }

        /// <summary>
        /// The minimum axis value.
        /// </summary>
        public DateTime? Min 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The axis maximum value.
        /// </summary>
        public DateTime? Max 
        { 
            get; 
            set; 
        }

        /// <summary>
        /// The interval between major divisions in base units.
        /// </summary>
        public double? MajorUnit
        {
            get;
            set;
        }

        /// <summary>
        /// The interval between minor divisions in base units.
        /// It defaults to 1/5th of the majorUnit.
        /// </summary>
        public double? MinorUnit
        {
            get;
            set;
        }

        public override IChartSerializer CreateSerializer()
        {
            return new ChartDateAxisSerializer(this);
        }
    }
}