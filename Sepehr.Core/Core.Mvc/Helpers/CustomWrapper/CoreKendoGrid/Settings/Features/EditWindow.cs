using Core.Mvc.Helpers.CustomWrapper.Infrastructure;
using Core.Mvc.Helpers.CoreKendoGrid.Infrastructure;
using Kendo.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;


namespace Core.Mvc.Helpers.CoreKendoGrid.Settings.Features
{
    [Serializable()]
    public class EditWindow : JsonObjectBase
    {
        public string WindowTitle { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool? Resizable { get; set; }
        public bool? Draggable { get; set; }
        public int TemplateMaxHeight { get; set; }
        

        protected override void Serialize(IDictionary<string, object> json)
        {
            json["title"] = WindowTitle;
            json["width"] = Width == 0 ? "auto" : Width.ToString(); //Width;
            json["height"] = Height == 0 ? "auto" : Height.ToString(); //Height;
            json["resizable"] = Resizable ?? true;
            json["draggable"] = Draggable ?? true;
            json["open"] = GetOnOpenHandlerManipulation(); 
        }
        
        private object GetOnOpenHandlerManipulation()
        {
            return new ClientHandlerDescriptor { TemplateDelegate = obj => "function(e) {{ ns_Grid.GridOperations.onEditWindowOpen(e); }}" }; 

            //var tickButton = IconButtonHelper.IconButton(string.Empty, "ذخیره",  StyleKind.CommandButton , StyleKind.Icons.Tick, string.Empty);
            //var cancelButton = IconButtonHelper.IconButton(string.Empty, "انصراف",StyleKind.CommandButton, StyleKind.Icons.Cancel , string.Empty);

            //var actionDiv = new TagBuilder("div");
            //actionDiv.MergeAttribute("class", "grd-popup-action");
            //var actionTopSeperator = new TagBuilder("div");
            //actionTopSeperator.MergeAttribute("class", "grd-popup-action-top-border");
            ////var templateMaxHeightApply = TemplateMaxHeight == 0 ? string.Empty : string.Format("winWrapper.children('.k-edit-form-container').find('form').css({{ 'max-height' : '{0}px' , 'overflow' : 'auto' }});", TemplateMaxHeight);
            //var onOpenHandlerScript = string.Format("function(ev) {{ var actionElem = $('{0}') , winWrapper= ev.sender.element , actionTopBorder = $('{1}') , actButtonElems = ev.sender.element.children('div').children('a');  actionElem.append(actButtonElems); winWrapper.append(actionTopBorder); winWrapper.append(actionElem); {2} }} ", actionDiv.ToString(), actionTopSeperator.ToString(), string.Empty);//, SelectionViaStyle.WindowContent, SelectionViaStyle.FormInWindow, SelectionViaStyle.CommandButton
            //return new ClientHandlerDescriptor { TemplateDelegate = obj => onOpenHandlerScript };
        }
        //"function(e) {{ alert($('.k-window-content .k-content >  .k-button .k-button-icontext').innerHTML); }}"
        //var kWindowContainer = string.Format(
        //actionElem.css('top' , ); actionElem.css('top' , \"'\" + eval(actDivTopOffset) + \"'\"); , $('div.k-edit-form-container').find('form').css('width')
        //parseInt(kWinHeight.substring(0 , kWinHeight.length -2))-(1-0.22)*100
        //var actionElem = $('{0}') ,  winContent= $('{1}') , kWinHeight = $('div.k-widget.k-window.k-rtl').css('height') , actDivTopOffset = parseInt(kWinHeight.substring(0 , kWinHeight.length -2)) - $('div.k-edit-form-container').find('form').css('height') +  -(1-0.22)*100  + 'px' ,  actButtonElems = $('{1}').find('{2}').find('a{3}');  actionElem.css('margin-top' , actDivTopOffset);  actionElem.append(actButtonElems); winContent.append(actionElem);
    }
}
