using System.Collections.Generic;
using Core.Entity;
using System.Linq;
using System;
using Core.Cmn.Monitoring;
using Core.Entity.Enum;
using Core.Service.Models;
using System.Collections.Concurrent;
using Core.Cmn;
using System.Globalization;

namespace Core.Service
{


    public class AppBase
    {

        private static List<IUserProfile> _onlineUsers;


        private static List<IViewElement> _allViewElements;


        private static int _companyId;


        private static int _userId;


        private static string _companyName;

        private static string _companyChartName;


        private static string _nationalId { get; set; }


        private static ConcurrentDictionary<int, UserViewElement> _viewElementsGrantedToUser;
        //private static List<UserViewElement> _viewElementsGrantedToUser;

        private static UserViewElement _viewElementsGrantedToAnonymousUser;


        //private static string _companyDomainName { get; set; }

        private string _userPortName { get; set; }
        private bool _showAllData { get; set; }

        private int? _companyChartId { get; set; }
        private static ResourceMonitoring _monitor;
        public static ResourceMonitoring Monitor
        {
            get
            {
                if (_monitor == null)
                    _monitor = new ResourceMonitoring();
                return _monitor;
            }
        }



        public ConcurrentDictionary<int, UserViewElement> ViewElementsGrantedToUser
        {
            get { return _viewElementsGrantedToUser ?? (_viewElementsGrantedToUser = new ConcurrentDictionary<int, UserViewElement>()); }
            set
            {
                _viewElementsGrantedToUser = value;
            }
        }

        public UserViewElement ViewElementsGrantedToAnonymousUser
        {
            get { return _viewElementsGrantedToAnonymousUser; }
            set { _viewElementsGrantedToAnonymousUser = value; }
        }






        public int CompanyId
        {
            get { return _companyId; }
            set { _companyId = value; }
        }


        public int UserId
        {
            get { return _userId; }
            set { _userId = value; }
        }

        public string NationalId
        {
            get { return _nationalId; }
            set { _nationalId = value; }
        }


        public string CompanyName
        {
            get { return _companyName; }
            set { _companyName = value; }
        }


        public string CompanyChartName
        {
            get { return _companyChartName; }
            set { _companyChartName = value; }
        }


        public int? CompanyChartId
        {
            get { return _companyChartId; }
            set { _companyChartId = value; }
        }


        public List<IUserProfile> OnlineUsers
        {
            get { return _onlineUsers ?? (_onlineUsers = new List<IUserProfile>()); }
        }


        public List<IViewElement> AllViewElements
        {
            get { return _allViewElements ?? (_allViewElements = new List<IViewElement>()); }
            set { _allViewElements = value; }
        }

        public string UserPortName
        {
            get { return _userPortName; }
            set { _userPortName = value; }
        }


        public bool ShowAllData
        {
            get { return _showAllData; }
            set { _showAllData = value; }
        }


        public static ViewElementInfo GetMenuItemPathByUniqueName(int userId, string uniqueName)
        {
         
            var commonViewElement = AppBase._viewElementsGrantedToAnonymousUser.ViewElements
                                   .FirstOrDefault(element => element.ConceptualName.Split('#')[0].ToLower() == uniqueName.ToLower());

       
            if (commonViewElement != null)
                return commonViewElement;
          
            UserViewElement currentUser = null;
            if (AppBase._viewElementsGrantedToUser !=null &&  AppBase._viewElementsGrantedToUser.TryGetValue(userId, out currentUser))
            {

                var viewElement = currentUser.ViewElements
                                        .FirstOrDefault(element => element.ConceptualName.Split('#')[0].ToLower() == uniqueName.ToLower());

           
                return viewElement;
             
            }
            return null;
            //throw new Exception("The entered url does not exist in any Role");

        }


        public static bool HasCurrentUserAccess(int userId, string url = null, string uniqueName = null)
        {
            bool hasAcces = false;


            if (url == null && uniqueName == null)
            {
                throw new Exception("url &  also uniqueName can't be null.");
            }

            UserViewElement currentUser = null;

            if (AppBase._viewElementsGrantedToUser.TryGetValue(userId, out currentUser))
            {
                if (uniqueName == null)
                {
                    var tempUrl = url.StartsWith("api/") ? url.ToLower().Remove(0, 4) : url.ToLower();

                    var viewElementGrantedToUser = currentUser.ViewElements;
                    hasAcces = viewElementGrantedToUser.Any(element => element.Url.ToLower() == tempUrl.ToLower());
                }
                else
                {

                    var viewElementGrantedToUser = currentUser.ViewElements;

                    hasAcces = viewElementGrantedToUser.Any(element => $"{element.ConceptualName.ToLower()}#{element.Url.ToLower()}" == uniqueName.ToLower());


                }
            }
            else
            {
                hasAcces = false;

            }
            return hasAcces;
        }
    }
}
