using Core.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Core.Mvc.ViewModel
{
    [DataContract]
    public class MenuItemViewModel
    {
        public MenuItemViewModel(MenuItem menuItem)
        {
            SetText(menuItem.ViewElement.UniqueName, menuItem.ViewElement.Title);
            imageUrl = menuItem.ViewElement.XMLViewData;

            if (menuItem.Childeren.Count > 0)
            {
                items = new List<MenuItemViewModel>();
                foreach (var menu in menuItem.Childeren)
                {

                    items.Add(new MenuItemViewModel(menu));
                }
            }

        }
        [DataMember]
        public bool encoded { get; set; }
        [DataMember]
        public string text { get; private set; }
        [DataMember]
        public string imageUrl { get; set; }
        [DataMember]
        public List<MenuItemViewModel> items
        {
            get;
            set;
        }
        public void SetText(string uniqueName, string title)
        {
            text = string.Format("<span id={0}{1}{0} >{2}</span>", "'", uniqueName, title);
        }
    }
}