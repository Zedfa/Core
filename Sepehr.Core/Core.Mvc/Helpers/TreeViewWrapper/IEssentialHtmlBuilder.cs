using System;
using System.Collections.Generic;
using System.Linq;



namespace Core.Mvc.Helpers
{
    
    public interface IEssentialHtmlBuilder<TComponent, TItem>
        where TComponent : TreeViewBase,IEssentialItem<TItem>  where TItem : EssentialItem <TItem>
    {
        TComponent Component { get; }
    }
}
