namespace Kendo.Mvc.UI.Fluent
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Defines the fluent API for creating bindings for Kendo Menu, TreeView and PanelBar
    /// </summary>
    /// <typeparam name="TNavigationItem"></typeparam>
    public class NavigationBindingFactory<TNavigationItem> 
        where TNavigationItem : NavigationItem<TNavigationItem>
    {
        internal readonly IList<INavigationBinding<TNavigationItem>> container;

        public NavigationBindingFactory()
        {
            this.container = new List<INavigationBinding<TNavigationItem>>();
        }

        public NavigationBindingFactory<TNavigationItem> For<TParent>(Action<NavigationBindingBuilder<TNavigationItem, TParent>> action) 
            where TParent : class
        {
            NavigationBinding<TNavigationItem, TParent> item = new NavigationBinding<TNavigationItem, TParent>();
            NavigationBindingBuilder<TNavigationItem, TParent> builder = new NavigationBindingBuilder<TNavigationItem, TParent>(item);

            container.Add(item);

            action(builder);

            return this;
        }
    }
}
