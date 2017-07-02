namespace Kendo.Mvc.UI.Fluent
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the fluent interface for configuring the <see cref="Chart{T}"/> component.
    /// </summary>
    public class StockChartBuilder<T> : WidgetBuilderBase<StockChart<T>, StockChartBuilder<T>>, IHideObjectMembers where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChartBuilder{T}"/> class.
        /// </summary>
        /// <param name="component">The component.</param>
        public StockChartBuilder(StockChart<T> component)
            : base(component)
        {
        }

        /// <summary>
        /// Sets the field used by all date axes (including the navigator).
        /// </summary>
        /// <param name="dateField">The date field.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .DateField("Date")
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> DateField(string dateField)
        {
            Component.DateField = dateField;
            return this;
        }

        /// <summary>
        /// Enables or disables automatic binding.
        /// </summary>
        /// <param name="autoBind">
        /// Gets or sets a value indicating if the chart
        /// should be data bound during initialization.
        /// The default value is true.
        /// </param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .DataSource(ds =>
        ///             {
        ///                 ds.Ajax().Read(r => r.Action("SalesData", "Chart"));
        ///             })
        ///             .AutoBind(false)
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> AutoBind(bool autoBind)
        {
            Component.AutoBind = autoBind;
            return this;
        }

        /// <summary>
        /// Configures the stock chart navigator.
        /// </summary>
        /// <param name="configurator">The navigator configuration action.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("StockChart")
        ///             .Navigator(nav => nav
        ///                 .Series(series =>
        ///                 {
        ///                     series.Line(s => s.Volume);
        ///                 }
        ///             )
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Navigator(Action<ChartNavigatorBuilder<T>> configurator)
        {

            configurator(new ChartNavigatorBuilder<T>(Component.Navigator));

            return this;
        }

        /// <summary>
        /// Configures the client-side events.
        /// </summary>
        /// <param name="configurator">The client events configuration action.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Events(events => events
        ///                 .OnLoad("onLoad")
        ///             )
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Events(Action<ChartEventBuilder> configurator)
        {

            configurator(new ChartEventBuilder(Component.Events));

            return this;
        }

        /// <summary>
        /// Sets the theme of the chart.
        /// </summary>
        /// <param name="theme">The Chart theme.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Theme("Telerik")
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Theme(string theme)
        {
            Component.Theme = theme;
            return this;
        }

        /// <summary>
        /// Sets the Chart area.
        /// </summary>
        /// <param name="configurator">The Chart area.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .ChartArea(chartArea => chartArea.margin(20))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> ChartArea(Action<ChartAreaBuilder> configurator)
        {

            configurator(new ChartAreaBuilder(Component.ChartArea));
            return this;
        }

        /// <summary>
        /// Sets the Plot area.
        /// </summary>
        /// <param name="configurator">The Plot area.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .PlotArea(plotArea => plotArea.margin(20))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> PlotArea(Action<PlotAreaBuilder> configurator)
        {

            configurator(new PlotAreaBuilder(Component.PlotArea));
            return this;
        }

        /// <summary>
        /// Sets the title of the chart.
        /// </summary>
        /// <param name="title">The Chart title.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Title("Yearly sales")
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Title(string title)
        {
            Component.Title.Text = title;
            return this;
        }

        /// <summary>
        /// Defines the title of the chart.
        /// </summary>
        /// <param name="configurator">The configuration action.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Title(title => title.Text("Yearly sales"))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Title(Action<ChartTitleBuilder> configurator)
        {

            configurator(new ChartTitleBuilder(Component.Title));

            return this;
        }

        /// <summary>
        /// Sets the legend visibility.
        /// </summary>
        /// <param name="visible">A value indicating whether to show the legend.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Legend(false)
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Legend(bool visible)
        {
            Component.Legend.Visible = visible;

            return this;
        }

        /// <summary>
        /// Configures the legend.
        /// </summary>
        /// <param name="configurator">The configuration action.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Legend(legend => legend.Visible(true).Position(ChartLegendPosition.Bottom))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Legend(Action<ChartLegendBuilder> configurator)
        {

            configurator(new ChartLegendBuilder(Component.Legend));

            return this;
        }

        /// <summary>
        /// Defines the chart series.
        /// </summary>
        /// <param name="configurator">The add action.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .Series(series =>
        ///             {
        ///                 series.Bar(s => s.SalesAmount);
        ///             })
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Series(Action<ChartSeriesFactory<T>> configurator)
        {

            ChartSeriesFactory<T> factory = new ChartSeriesFactory<T>(Component);

            configurator(factory);

            return this;
        }

        /// <summary>
        /// Defines the options for all chart series of the specified type.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .SeriesDefaults(series => series.Bar().Stack(true))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> SeriesDefaults(Action<ChartSeriesDefaultsBuilder<T>> configurator)
        {

            configurator(new ChartSeriesDefaultsBuilder<T>(Component));

            return this;
        }

        /// <summary>
        /// Defines the chart panes.
        /// </summary>
        /// <param name="configurator">The add action.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .Panes(panes =>
        ///             {
        ///                 panes.Add("volume");
        ///             })
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Panes(Action<ChartPanesFactory> configurator)
        {
            ChartPanesFactory factory = new ChartPanesFactory(Component);

            configurator(factory);

            return this;
        }

        /// <summary>
        /// Defines the options for all chart axes of the specified type.
        /// </summary>
        /// <param name="configurator">The configurator.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .AxisDefaults(axisDefaults => axisDefaults.MinorTickSize(5))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> AxisDefaults(Action<ChartAxisDefaultsBuilder<T>> configurator)
        {

            configurator(new ChartAxisDefaultsBuilder<T>(Component));

            return this;
        }

        /// <summary>
        /// Configures the category axis
        /// </summary>
        /// <param name="configurator">The configurator</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .CategoryAxis(axis => axis
        ///                 .Categories(s => s.DateString)
        ///             )
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> CategoryAxis(Action<ChartCategoryAxisBuilder<T>> configurator)
        {
            var categoryAxis = new ChartCategoryAxis<T>(Component);

            configurator(new ChartCategoryAxisBuilder<T>(Component, categoryAxis));
            Component.CategoryAxes.Add(categoryAxis);

            return this;
        }

        /// <summary>
        /// Defines value axis options
        /// </summary>
        /// <param name="configurator">The configurator</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .ValueAxis(a => a.Numeric().TickSize(4))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> ValueAxis(Action<ChartValueAxisFactory<T>> configurator)
        {

            ChartValueAxisFactory<T> factory = new ChartValueAxisFactory<T>(Component, Component.ValueAxes);
            configurator(factory);

            return this;
        }

        /// <summary>
        /// Defines X-axis options for scatter charts
        /// </summary>
        /// <param name="configurator">The configurator</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .XAxis(a => a.Numeric().Max(4))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> XAxis(Action<ChartValueAxisFactory<T>> configurator)
        {

            var factory = new ChartValueAxisFactory<T>(Component, Component.XAxes);
            configurator(factory);

            return this;
        }

        /// <summary>
        /// Configures Y-axis options for scatter charts.
        /// </summary>
        /// <param name="configurator">The configurator</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart(Model)
        ///             .Name("Chart")
        ///             .YAxis(a => a.Numeric().Max(4))
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> YAxis(Action<ChartValueAxisFactory<T>> configurator)
        {

            var factory = new ChartValueAxisFactory<T>(Component, Component.YAxes);
            configurator(factory);

            return this;
        }

        /// <summary>
        /// Data Source configuration
        /// </summary>
        /// <param name="configurator">Use the configurator to set different data binding options.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .DataSource(ds =>
        ///             {
        ///                 ds.Ajax().Read(r => r.Action("SalesData", "Chart"));
        ///             })
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> DataSource(Action<ReadOnlyAjaxDataSourceBuilder<T>> configurator)
        {
            configurator(new ReadOnlyAjaxDataSourceBuilder<T>(Component.DataSource, this.Component.ViewContext, this.Component.UrlGenerator));

            return this;
        }

        /// <summary>
        /// Sets the series colors.
        /// </summary>
        /// <param name="colors">A list of the series colors.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .SeriesColors(new string[] { "#f00", "#0f0", "#00f" })
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> SeriesColors(IEnumerable<string> colors)
        {
            Component.SeriesColors = colors;

            return this;
        }

        /// <summary>
        /// Sets the series colors.
        /// </summary>
        /// <param name="colors">The series colors.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .SeriesColors("#f00", "#0f0", "#00f")
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> SeriesColors(params string[] colors)
        {
            Component.SeriesColors = colors;

            return this;
        }

        /// <summary>
        /// Use it to configure the data point tooltip.
        /// </summary>
        /// <param name="configurator">Use the configurator to set data tooltip options.</param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Tooltip(tooltip =>
        ///             {
        ///                 tooltip.Visible(true).Format("{0:C}");
        ///             })
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Tooltip(Action<ChartTooltipBuilder> configurator)
        {

            configurator(new ChartTooltipBuilder(Component.Tooltip));

            return this;
        }

        /// <summary>
        /// Sets the data point tooltip visibility.
        /// </summary>
        /// <param name="visible">
        /// A value indicating if the data point tooltip should be displayed.
        /// The tooltip is not visible by default.
        /// </param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Tooltip(true)
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Tooltip(bool visible)
        {
            Component.Tooltip.Visible = visible;

            return this;
        }

        /// <summary>
        /// Enables or disabled animated transitions on initial load and refresh. 
        /// </summary>
        /// <param name="transitions">
        /// A value indicating if transition animations should be played.
        /// </param>
        /// <example>
        /// <code lang="CS">
        ///  &lt;%= Html.Kendo().StockChart()
        ///             .Name("Chart")
        ///             .Transitions(false)
        /// %&gt;
        /// </code>
        /// </example>
        public StockChartBuilder<T> Transitions(bool transitions)
        {
            Component.Transitions = transitions;
            return this;
        }
    }
}