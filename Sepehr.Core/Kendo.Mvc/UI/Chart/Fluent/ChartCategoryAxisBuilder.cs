namespace Kendo.Mvc.UI.Fluent
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.Resources;
    using Kendo.Mvc.UI;

    /// <summary>
    /// Configures the category axis for the <see cref="Chart{TModel}" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the data item to which the chart is bound to</typeparam>
    public class ChartCategoryAxisBuilder<TModel> : ChartAxisBuilderBase<IChartCategoryAxis, int, ChartCategoryAxisBuilder<TModel>>
        where TModel : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartCategoryAxisBuilder{TModel}"/> class.
        /// </summary>
        /// <param name="chart">The chart.</param>
        public ChartCategoryAxisBuilder(Chart<TModel> chart, IChartCategoryAxis axis)
            : base(axis)
        {
            Container = chart;
        }

        /// <summary>
        /// The parent Chart
        /// </summary>
        public Chart<TModel> Container
        {
            get;
            private set;
        }

        public ChartDateCategoryAxisBuilder<TModel> Date()
        {
            return new ChartDateCategoryAxisBuilder<TModel>(Container, Axis);
        }

        /// <summary>
        /// Defines bound categories.
        /// </summary>
        /// <param name="expression">
        /// The expression used to extract the categories value from the chart model
        /// </param>
        public ChartCategoryAxisBuilder<TModel> Categories<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            if (typeof(TModel).IsPlainType() && !expression.IsBindable())
            {
                throw new InvalidOperationException(Exceptions.MemberExpressionRequired);
            }

            var value = expression.Compile();

            if (Container.Data != null)
            {
                var dataList = new ArrayList();

                foreach (var dataPoint in Container.Data)
                {
                    dataList.Add(dataPoint != null ? value(dataPoint).ToString() : string.Empty);
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
        public ChartCategoryAxisBuilder<TModel> Categories(IEnumerable categories)
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
        public ChartCategoryAxisBuilder<TModel> Categories(params string[] categories)
        {
            Axis.Categories = categories;

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
        ///            .CategoryAxis(axis => axis.AxisCrossingValue(4))
        ///            .ValueAxis(axis => axis.Numeric().Title("Axis 1"))
        ///            .ValueAxis(axis => axis.Numeric("secondary").Title("Axis 2"))
        /// %&gt;
        /// </code>
        /// </example>
        public ChartCategoryAxisBuilder<TModel> AxisCrossingValue(double axisCrossingValue)
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
        ///            .CategoryAxis(axis => axis.AxisCrossingValue(0, 10))
        ///            .ValueAxis(axis => axis.Numeric().Title("Axis 1"))
        ///            .ValueAxis(axis => axis.Numeric("secondary").Title("Axis 2"))
        /// %&gt;
        /// </code>
        /// </example>
        public ChartCategoryAxisBuilder<TModel> AxisCrossingValue(params double[] axisCrossingValues)
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
        ///            .CategoryAxis(axis => axis.AxisCrossingValue(new double[] { 0, 10 }))
        ///            .ValueAxis(axis => axis.Numeric().Title("Axis 1"))
        ///            .ValueAxis(axis => axis.Numeric("secondary").Title("Axis 2"))
        /// %&gt;
        /// </code>
        /// </example>
        public ChartCategoryAxisBuilder<TModel> AxisCrossingValue(IEnumerable<double> axisCrossingValues)
        {
            Axis.AxisCrossingValues = axisCrossingValues;

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
        public ChartCategoryAxisBuilder<TModel> Justify(bool justified)
        {
            Axis.Justified = justified;

            return this;
        }

        /// <summary>
        /// Positions categories and series points on major ticks. This removes the empty space before and after the series.
        /// This option will be ignored if either Bar, Column, OHLC or Candlestick series are plotted on the axis.
        /// </summary>
        public ChartCategoryAxisBuilder<TModel> Justify()
        {
            Axis.Justified = true;

            return this;
        }

        /// <summary>
        /// Sets the selection range
        /// </summary>
        /// <param name="from">The selection range start.</param>
        /// <param name="to">The selection range end.
        /// *Note*: The category with the specified index is not included in the selected range
        /// unless the axis is justified. In order to select all categories specify
        /// a value larger than the last category index.
        /// </param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///           .Name("StockChart")
        ///           .CategoryAxis(axis => axis.Select(0, 3))
        /// %&gt;
        /// </code>
        /// </example>        
        public ChartCategoryAxisBuilder<TModel> Select(double from, double to)
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
        public ChartCategoryAxisBuilder<TModel> Select(Action<ChartAxisSelectionBuilder> configurator)
        {
            configurator(new ChartAxisSelectionBuilder(Axis.Select));

            return this;
        }
    }
}
