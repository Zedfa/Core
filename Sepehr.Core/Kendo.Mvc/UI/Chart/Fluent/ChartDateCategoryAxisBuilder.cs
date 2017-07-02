namespace Kendo.Mvc.UI.Fluent
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq.Expressions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.Resources;
    using Kendo.Mvc.UI;

    /// <summary>
    /// Configures date category axis for the <see cref="Chart{TModel}" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the data item to which the chart is bound to</typeparam>
    public class ChartDateCategoryAxisBuilder<TModel> : ChartAxisBuilderBase<IChartCategoryAxis, int, ChartDateCategoryAxisBuilder<TModel>>
        where TModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCategoryAxisBuilder{TModel}"/> class.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public ChartDateCategoryAxisBuilder(Chart<TModel> chart, IChartCategoryAxis axis)
            : base(axis)
        {
            Container = chart;
            axis.Type = ChartCategoryAxisType.Date;
        }

        /// <summary>
        /// The parent Chart
        /// </summary>
        public Chart<TModel> Container
        {
            get;
            private set;
        }

        /// <summary>
        /// Defines bound categories.
        /// </summary>
        /// <param name="expression">
        /// The expression used to extract the categories value from the chart model
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> Categories(Expression<Func<TModel, DateTime>> expression)
        {
            if (typeof(TModel).IsPlainType() && !expression.IsBindable())
            {
                throw new InvalidOperationException(Exceptions.MemberExpressionRequired);
            }

            var value = expression.Compile();

            if (Container.Data != null)
            {
                var dataList = new List<DateTime?>();

                foreach (var dataPoint in Container.Data)
                {
                    dataList.Add(dataPoint != null ? value(dataPoint) : new Nullable<DateTime>());
                }

                Axis.Categories = dataList;
            }
            else
            {
                Axis.Member = expression.MemberWithoutInstance();
            }

            return this;
        }

        /// <summary>
        /// Defines categories.
        /// </summary>
        /// <param name="categories">
        /// The list of categories
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> Categories(IEnumerable<DateTime> categories)
        {
            Axis.Categories = categories;

            return this;
        }

        /// <summary>
        /// Defines categories.
        /// </summary>
        /// <param name="categories">
        /// The list of categories
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> Categories(params DateTime[] categories)
        {
            Axis.Categories = categories;

            return this;
        }

        /// <summary>
        /// Sets the date category axis base unit.
        /// </summary>
        /// <param name="baseUnit">
        /// The date category axis base unit
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> BaseUnit(ChartAxisBaseUnit baseUnit)
        {
            Axis.BaseUnit = baseUnit;

            return this;
        }

        /// <summary>
        /// Sets the step (interval) between categories in base units.
        /// Specifiying 0 (auto) will set the step to such value that the total
        /// number of categories does not exceed MaxDateGroups.
        /// </summary>
        /// <remarks>
        /// This option is ignored if baseUnit is set to "fit".
        /// </remarks>
        /// <param name="baseUnitStep">
        /// the step (interval) between categories in base units.
        /// Set 0 for automatic step. The default value is 1.
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> BaseUnitStep(int baseUnitStep)
        {
            Axis.BaseUnitStep = baseUnitStep;

            return this;
        }

        /// <summary>
        /// Specifies the maximum number of groups (categories) that the chart will attempt to
        /// produce when either BaseUnit is set to Fit or BaseUnitStep is set to 0 (auto).
        /// This option is ignored in all other cases.
        /// </summary>
        /// <param name="maxDateGroups">
        /// the maximum number of groups (categories).
        /// The default value is 10.
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> MaxDateGroups(int maxDateGroups)
        {
            Axis.MaxDateGroups = maxDateGroups;

            return this;
        }

        /// <summary>
        /// If set to false, the min and max dates will not be rounded off to
        /// the nearest baseUnit. 
        /// This option is most useful in combination with explicit min and max dates.
        /// It will be ignored if either Bar, Column, OHLC or Candlestick series are plotted on the axis.
        /// </summary>
        /// <param name="roundToBaseUnit">
        /// A boolean value that indicates if the axis range should be rounded to the nearest base unit.
        /// The default value is true.
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> RoundToBaseUnit(bool roundToBaseUnit)
        {
            Axis.RoundToBaseUnit = roundToBaseUnit;

            return this;
        }

        /// <summary>
        /// Positions categories and series points on major ticks. This removes the empty space before and after the series.
        /// This option will be ignored if either Bar, Column, OHLC or Candlestick series are plotted on the axis.
        /// </summary>
        /// <param name="justified">
        /// A boolean value that indicates if the empty space before and after the series should be removed.
        /// The default value is false.
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> Justify(bool justified)
        {
            Axis.Justified = justified;

            return this;
        }

        /// <summary>
        /// Positions categories and series points on major ticks. This removes the empty space before and after the series.
        /// This option will be ignored if either Bar, Column, OHLC or Candlestick series are plotted on the axis.
        /// </summary>
        public ChartDateCategoryAxisBuilder<TModel> Justify()
        {
            Axis.Justified = true;

            return this;
        }

        /// <summary>
        /// Specifies the discrete baseUnitStep values when
        /// either BaseUnit is set to Fit or BaseUnitStep is set to 0 (auto).
        /// </summary>
        /// <param name="configurator">
        /// The configuration action.
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> AutoBaseUnitSteps(Action<ChartAxisBaseUnitStepsBuilder> configurator)
        {
            configurator(new ChartAxisBaseUnitStepsBuilder(Axis.AutoBaseUnitSteps));

            return this;
        }

        /// <summary>
        /// Sets the date category axis minimum (start) date.
        /// </summary>
        /// <param name="min">
        /// The date category axis minimum (start) date
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> Min(DateTime min)
        {
            Axis.Min = min;

            return this;
        }

        /// <summary>
        /// Sets the date category axis maximum (end) date.
        /// </summary>
        /// <param name="max">
        /// The date category axis maximum (end) date
        /// </param>
        public ChartDateCategoryAxisBuilder<TModel> Max(DateTime max)
        {
            Axis.Max = max;

            return this;
        }

        /// <summary>
        /// Sets value at which the first perpendicular axis crosses this axis.
        /// </summary>
        /// <param name="axisCrossingValue">The value at which the first perpendicular axis crosses this axis.</param>
        /// <example>
        /// <code lang="CS">
        /// &lt;%= Html.Kendo().Chart(Model)
        ///            .Name("Chart")
        ///            .CategoryAxis(axis => axis.Date().AxisCrossingValue(4))
        ///            .ValueAxis(axis => axis.Numeric().Title("Axis 1"))
        ///            .ValueAxis(axis => axis.Numeric("secondary").Title("Axis 2"))
        /// %&gt;
        /// </code>
        /// </example>
        public ChartDateCategoryAxisBuilder<TModel> AxisCrossingValue(double axisCrossingValue)
        {
            Axis.AxisCrossingValues = new double[] { axisCrossingValue };

            return this;
        }

        /// <summary>
        /// Sets value at which perpendicular axes cross this axis.
        /// </summary>
        /// <param name="axisCrossingValues">The values at which perpendicular axes cross this axis.</param>
        /// <example>
        /// <code lang="CS">
        /// &lt;%= Html.Kendo().Chart(Model)
        ///            .Name("Chart")
        ///            .CategoryAxis(axis => axis.Date().AxisCrossingValue(0, 10))
        ///            .ValueAxis(axis => axis.Numeric().Title("Axis 1"))
        ///            .ValueAxis(axis => axis.Numeric("secondary").Title("Axis 2"))
        /// %&gt;
        /// </code>
        /// </example>
        public ChartDateCategoryAxisBuilder<TModel> AxisCrossingValue(params double[] axisCrossingValues)
        {
            Axis.AxisCrossingValues = axisCrossingValues;

            return this;
        }

        /// <summary>
        /// Sets value at which perpendicular axes cross this axis.
        /// </summary>
        /// <param name="axisCrossingValues">The values at which perpendicular axes cross this axis.</param>
        /// <example>
        /// <code lang="CS">
        /// &lt;%= Html.Kendo().Chart(Model)
        ///            .Name("Chart")
        ///            .CategoryAxis(axis => axis.Date().AxisCrossingValue(new double[] { 0, 10 }))
        ///            .ValueAxis(axis => axis.Numeric().Title("Axis 1"))
        ///            .ValueAxis(axis => axis.Numeric("secondary").Title("Axis 2"))
        /// %&gt;
        /// </code>
        /// </example>
        public ChartDateCategoryAxisBuilder<TModel> AxisCrossingValue(IEnumerable<double> axisCrossingValues)
        {
            Axis.AxisCrossingValues = axisCrossingValues;

            return this;
        }

        /// <summary>
        /// Configures the axis labels.
        /// </summary>
        /// <param name="configurator">The configuration action.</param>
        /// <example>
        /// <code lang="CS">
        /// &lt;%= Html.Kendo().Chart()
        ///            .Name("Chart")
        ///            .CategoryAxis(axis => axis
        ///                .Date()
        ///                .Labels(labels => labels
        ///                    .Culture(new CultureInfo("es-ES")
        ///                    .Visible(true)
        ///                );
        ///            )
        /// %&gt;
        /// </code>
        /// </example>
        public ChartDateCategoryAxisBuilder<TModel> Labels(Action<ChartDateAxisLabelsBuilder> configurator)
        {
            configurator(new ChartDateAxisLabelsBuilder(Axis.Labels));

            return this;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public override ChartDateCategoryAxisBuilder<TModel> Labels(Action<ChartAxisLabelsBuilder> configurator)
        {
            return this;
        }

        /// <summary>
        /// Sets the selection range
        /// </summary>
        /// <param name="from">The selection range start.</param>
        /// <param name="to">The selection range end.
        /// *Note*: The specified date is not included in the selected range
        /// unless the axis is justified. In order to select all categories specify
        /// a value larger than the last date.
        /// </param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///           .Name("StockChart")
        ///           .CategoryAxis(axis => axis.Select(DateTime.Today.AddMonths(-1), DateTime.Today))
        /// %&gt;
        /// </code>
        /// </example>        
        public ChartDateCategoryAxisBuilder<TModel> Select(DateTime? from, DateTime? to)
        {
            Axis.Select.From = from;
            Axis.Select.To = to;

            return this;
        }

        /// <summary>
        /// Configures the selection
        /// </summary>
        /// <param name="configurator">The configuration action.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///           .Name("StockChart")
        ///           .CategoryAxis(axis => axis.Select(select =>
        ///               select.Mousewheel(mw => mw.Reverse())
        ///           )
        /// %&gt;
        /// </code>
        /// </example>        
        public ChartDateCategoryAxisBuilder<TModel> Select(Action<ChartAxisSelectionBuilder> configurator)
        {
            configurator(new ChartAxisSelectionBuilder(Axis.Select));

            return this;
        }
    }
}
