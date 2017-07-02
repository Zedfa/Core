using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers
{
    public class StyleKind
    {
        public const string Div = "k-content";
        public const string InputBox = "k-input";
        public const string TextBox = "k-textbox";
        public const string Button = "k-button";
        public const string RighSpace = "k-space-right";
        public const string LeftSpace = "k-space-left";
        public const string CommandButton = "k-button k-button-icontext";
        public const string RequiredInput = "required-field";
        public const string OptionalInput = "optional-field";
        public const string HeaderRow = "k-grouping-header";
        public const string BoxBoarder = "k-widget k-reorderable";
        public const string Toolbar = "k-toolbar";
        public const string TreeToolbar = "k-toolbar tree-toolbar";
        public const string WindowContent = "k-window-content k-content";
        public const string checkBox = "k-checkbox";

        //for example to use TextBox With right Icon:
        //<span class="k-textbox k-space-right">
        //            <input type="text" value="Input with icon right" />
        //            <a href="#" class="k-icon k-i-search">&nbsp;</a>
        //        </span>
        public const string Pager = "k-pager-wrap";// example: <div id="pager" class="k-pager-wrap"></div>
        public const string Sprite = "k-sprite";
        //example to use sprite <ul>
        //   <li data-expanded="true">
        //   <span class="k-sprite folder"></span>images
        //---  .folder { background-position: 0 -16px; }
        //-------------ICons-------------------------------
        public class Icons
        {
            public const string Icon = "k-icon";
            public const string LookUP = "k-icon l-i-button";
            public const string Search = "k-icon k-i-search";
            public const string UpArrow = "k-icon k-i-arrow-n";
            public const string RightArrow = "k-icon k-i-arrow-e";
            public const string DownArrow = "k-icon k-i-arrow-s";
            public const string LeftArrow = "k-icon k-i-arrow-w";
            public const string RightExpand = "k-icon k-i-expand";
            public const string RightCollaps = "k-icon k-i-collapse";
            public const string LeftExpand = "k-icon k-i-expand-w";
            public const string LeftCollaps = "k-icon k-i-collapse-w";
            public const string Plus = "k-icon k-i-plus";
            public const string Tick = "k-icon k-i-tick";
            public const string Close = "k-icon k-i-close";
            public const string Cancel = "k-icon k-i-cancel";
            public const string Calendar = "k-icon k-i-calendar";
            public const string Refresh = "k-icon k-i-refresh";
            public const string Restore = "k-icon k-i-restore";
            public const string Maximize = "k-icon k-i-maximize";
            public const string Minimize = "k-icon k-i-minimize";
            public const string Setting = "k-icon k-i-custom";
            public const string Notify = "k-icon k-i-note";
            public const string SmallPlus = "k-icon k-si-plus";
            public const string SmallMinus = "k-icon k-si-minus";
            public const string Edit = "k-icon k-i-pencil";
            public const string Delete = "k-icon k-delete";
            public const string Add = "k-icon k-add";




        }




    }
}
