namespace Kendo.Mvc.UI.Fluent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using Kendo.Mvc.Extensions;
    using Kendo.Mvc.UI;
    using System.Collections;
    using System.Web.Mvc;
    /// <summary>
    /// Creates columns for the <see cref="Grid{TModel}" />.
    /// </summary>
    /// <typeparam name="TModel">The type of the data item to which the grid is bound to</typeparam>
    public class GridColumnFactory<TModel> : IHideObjectMembers 
        where TModel : class
    {
        private bool hasGeneratedColumn;

        /// <summary>
        /// Initializes a new instance of the <see cref="GridColumnFactory{TModel}"/> class.
        /// </summary>
        /// <param name="container">The container.</param>
        public GridColumnFactory(Grid<TModel> container)
        {
            Container = container;
        }

        public Grid<TModel> Container
        {
            get;
            private set;
        }

        public void LoadSettings(IEnumerable<GridColumnSettings> settings)
        {
            var generator = new GridColumnGenerator<TModel>(Container);

            foreach (var setting in settings)
            {
                Container.Columns.Add(generator.CreateColumn(setting));
            }
        }

        /// <summary>
        /// Defines a bound column.
        /// </summary>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public virtual GridBoundColumnBuilder<TModel> Bound<TValue>(Expression<Func<TModel, TValue>> expression)
        {
            GridBoundColumn<TModel, TValue> column = new GridBoundColumn<TModel, TValue>(Container, expression);

            Container.Columns.Add(column);

            return new GridBoundColumnBuilder<TModel>(column);
        }

        /// <summary>
        /// Defines a bound column.
        /// </summary>
        public virtual GridBoundColumnBuilder<TModel> Bound(string memberName)
        {
            return Bound(null, memberName);
        }

        /// <summary>
        /// Defines a bound column.
        /// </summary>
        public virtual GridBoundColumnBuilder<TModel> Bound(Type memberType, string memberName)
        {
            const bool liftMemberAccess = false;

            var lambdaExpression = ExpressionBuilder.Lambda<TModel>(memberType, memberName, liftMemberAccess);

            if (typeof(TModel).IsDynamicObject() && memberType != null && lambdaExpression.Body.Type.GetNonNullableType() != memberType.GetNonNullableType())
            {
                lambdaExpression = Expression.Lambda(Expression.Convert(lambdaExpression.Body, memberType), lambdaExpression.Parameters);
            }
            var columnType = typeof(GridBoundColumn<,>).MakeGenericType(new[] { typeof(TModel), lambdaExpression.Body.Type });

            var constructor = columnType.GetConstructor(new[] { Container.GetType(), lambdaExpression.GetType() });

            var column = (IGridBoundColumn)constructor.Invoke(new object[] { Container, lambdaExpression });
            
            column.Member = memberName;

            if (!column.Title.HasValue())
            {
                column.Title = memberName.AsTitle();
            }
            
            if (memberType != null)
            {
                column.MemberType = memberType;
            }

            Container.Columns.Add((GridColumnBase<TModel>)column);

            return new GridBoundColumnBuilder<TModel>(column);
        }
                
        /// <summary>
        /// Defines a foreign key column.
        /// </summary>
        /// <typeparam name="TValue">Member type</typeparam>
        /// <param name="expression">The member which matches the selected item</param>
        /// <param name="data">The foreign data</param>
        /// <param name="dataFieldValue">The data value field</param>
        /// <param name="dataFieldText">The data text field</param>
        /// <returns></returns>
        public virtual GridBoundColumnBuilder<TModel> ForeignKey<TValue>(Expression<Func<TModel, TValue>> expression, IEnumerable data, 
            string dataFieldValue, string dataFieldText)
        {
            return ForeignKey(expression, new SelectList(data, dataFieldValue, dataFieldText));
        }

        /// <summary>
        /// Defines a foreign key column.
        /// </summary>
        /// <typeparam name="TValue">Member type</typeparam>
        /// <param name="expression">The member which matches the selected item</param>
        /// <param name="data">The foreign data</param>
        /// <returns></returns>
        public virtual GridBoundColumnBuilder<TModel> ForeignKey<TValue>(Expression<Func<TModel, TValue>> expression, SelectList data)
        {            
            GridForeignKeyColumn<TModel, TValue> column = new GridForeignKeyColumn<TModel, TValue>(Container, expression, data);

            column.Data = data;

            Container.Columns.Add(column);

            return new GridBoundColumnBuilder<TModel>(column);
        }

        public virtual GridBoundColumnBuilder<TModel> ForeignKey(string memberName, IEnumerable data,
            string dataFieldValue, string dataFieldText)
        {
            return ForeignKey(null, memberName, new SelectList(data, dataFieldValue, dataFieldText));
        }

        public virtual GridBoundColumnBuilder<TModel> ForeignKey(string memberName, SelectList data)
        {
            return ForeignKey(null, memberName, data);
        }

        public virtual GridBoundColumnBuilder<TModel> ForeignKey(Type memberType, string memberName, IEnumerable data,
            string dataFieldValue, string dataFieldText)
        {
            return ForeignKey(memberType, memberName, new SelectList(data, dataFieldValue, dataFieldText));
        }

        public virtual GridBoundColumnBuilder<TModel> ForeignKey(Type memberType, string memberName, SelectList data)
        {
            const bool liftMemberAccess = false;

            var lambdaExpression = ExpressionBuilder.Lambda<TModel>(memberType, memberName, liftMemberAccess);

            if (typeof(TModel).IsDynamicObject() && memberType != null && lambdaExpression.Body.Type.GetNonNullableType() != memberType.GetNonNullableType())
            {
                lambdaExpression = Expression.Lambda(Expression.Convert(lambdaExpression.Body, memberType), lambdaExpression.Parameters);
            }

            var columnType = typeof(GridForeignKeyColumn<,>).MakeGenericType(new[] { typeof(TModel), lambdaExpression.Body.Type });

            var constructor = columnType.GetConstructor(new[] { Container.GetType(), lambdaExpression.GetType(), data.GetType() });

            var column = (IGridBoundColumn)constructor.Invoke(new object[] { Container, lambdaExpression, data });

            column.Member = memberName;

            if (!column.Title.HasValue())
            {
                column.Title = memberName.AsTitle();
            }

            if (memberType != null)
            {
                column.MemberType = memberType;
            }

            Container.Columns.Add((GridColumnBase<TModel>)column);

            return new GridBoundColumnBuilder<TModel>(column);
        }

        protected virtual void AutoGenerate(bool shouldGenerate, Action<GridColumnBase<TModel>> columnAction)
        {
            if (hasGeneratedColumn) return;

            if (shouldGenerate)
            {
                new GridColumnGenerator<TModel>(Container)
                    .GetColumns()
                    .Each(c =>
                              {
                                  if (columnAction != null)
                                  {
                                      columnAction(c);
                                  }
                                  Container.Columns.Add(c);
                              });
                hasGeneratedColumn = true;
            }
            Container.AutoGenerateColumns = shouldGenerate;
        }

        /// <summary>
        /// Determines if columns should be automatically generated.
        /// </summary>
        /// <param name="shouldGenerate">If true columns should be generated, otherwise false.</param>
        public virtual void AutoGenerate(bool shouldGenerate)
        {
            AutoGenerate(shouldGenerate, null);
        }

        /// <summary>
        /// Determines if columns should be automatically generated.
        /// </summary>
        /// <param name="columnAction">Action which will be executed for each generated column.</param>
        public virtual void AutoGenerate(Action<GridColumnBase<TModel>> columnAction)
        {
            AutoGenerate(true, columnAction);
        }

        /// <summary>
        /// Defines a template column.
        /// </summary>
        /// <param name="templateAction"></param>
        /// <returns></returns>
        public virtual GridTemplateColumnBuilder<TModel> Template(Action<TModel> templateAction)
        {
            GridTemplateColumn<TModel> column = new GridTemplateColumn<TModel>(Container, templateAction);
            Container.Columns.Add(column);

            return new GridTemplateColumnBuilder<TModel>(column);
        }

        public virtual GridTemplateColumnBuilder<TModel> Template(Func<TModel, object> template)
        {
            GridTemplateColumn<TModel> column = new GridTemplateColumn<TModel>(Container, template);
            Container.Columns.Add(column);

            return new GridTemplateColumnBuilder<TModel>(column);
        }

        /// <summary>
        /// Defines a command column.
        /// </summary>
        /// <param name="commandAction"></param>
        /// <returns></returns>
        public virtual GridActionColumnBuilder Command(Action<GridActionCommandFactory<TModel>> commandAction)
        {
            GridActionColumn<TModel> column = new GridActionColumn<TModel>(Container);
            
            commandAction(new GridActionCommandFactory<TModel>(column));

            Container.Columns.Add(column);

            return new GridActionColumnBuilder(column);
        }
    }
}
