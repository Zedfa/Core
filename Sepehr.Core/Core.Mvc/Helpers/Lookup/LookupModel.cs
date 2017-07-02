using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Helpers.Lookup
{
    public class Grid : ILookupView
    {
        public Grid()
        {
            this.Width = 900;
            this.Height = 500;

        }
        public CoreKendoGrid.Settings.GridInfo GridInfo { get; set; }

        public CoreKendoGrid.ClientDependentFeature ClientDependentFeatures { get; set; }

        public string GridID { get; set; }

        public string PropertyNameForValue { get; set; }

        public string PropertyNameForDisplay { get; set; }

        public string PropertyNameForBinding { get; set; }

        public bool UseMultiSelect { get; set; }

        public string LookupName { get; set; }

        public string Title
        {
            get;

            set;

        }
         
        public int Width
        {
            get;

            set;

        }

        public int Height
        {
            get;

            set;

        }


        public string ViewModel
        {
            get;
            set;
        }

        public string ViewInfoKey
        {
            get;
            set;
        }

        //private CoreKendoGrid.ScrollPosition _position =CoreKendoGrid.ScrollPosition.Top;
        public CoreKendoGrid.ScrollPosition Position { get; set; }
    }

    public interface ILookupView
    {
        string Title { get; set; }

        string LookupName { get; set; }

        string ViewModel { get; set; }

        string ViewInfoKey { get; set; }

        string PropertyNameForBinding { get; set; }

        string PropertyNameForValue { get; set; }

        string PropertyNameForDisplay { get; set; }

        bool UseMultiSelect { get; set; }

        int Width { get; set; }

        int Height { get; set; }



    }

    //public class LookupViewInfo : ILookupView
    //{
    //    public string Title
    //    {
    //        get ;
    //        set ;
    //    }

    //    public string LookupName
    //    {
    //        get;
    //        set;
    //    }

    //    public string ViewModel
    //    {
    //        get ;
    //        set ;
    //    }

    //    public string ViewInfoKey
    //    {
    //        get;
    //        set;
    //    }

    //    public string PropertyNameForBinding
    //    {
    //        get ;
    //        set ;
    //    }

    //    public string PropertyNameForValue
    //    {
    //        get;
    //        set;
    //    }

    //    public string PropertyNameForDisplay
    //    {
    //        get;
    //        set;
    //    }

    //    public bool UseMultiSelect
    //    {
    //        get;
    //        set;
    //    }

    //    public int Width
    //    {
    //        get;
    //        set;
    //    }

    //    public int Height
    //    {
    //        get;
    //        set;
    //    }
    //}

    public class Tree : ILookupView
    {
        public Tree()
        {
            this.Width = 500;
            this.Height = 300;

            // this.Width = 300;
            // this.Height = 200;
        }

        public TreeInfo TreeInfo { get; set; }

        public string TreeID { get; set; }

        public string PropertyNameForBinding { get; set; }

        public bool UseMultiSelect { get; set; }

        public string LookupName { get; set; }

        public string Title
        {
            get;

            set;

        }

        public int Width
        {
            get;

            set;

        }

        public int Height
        {
            get;

            set;

        }



        public string ViewModel
        {
            get;

            set;

        }

        public string ViewInfoKey
        {
            get;

            set;

        }


        public string PropertyNameForValue
        {
            get;

            set;

        }

        public string PropertyNameForDisplay
        {
            get;

            set;

        }
    }
}
