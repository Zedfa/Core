using Core.Entity;
using Core.Mvc.Attribute.Validation;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.ViewModel;
using Core.Mvc.ViewModels;
using Core.Rep.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "EventLogViewModel")]
    public class EventLogViewModel : ViewModelBase<LogDTO>
    {

        private static GridInfo _viewInfo = null;
        public EventLogViewModel()
            : base()
        {

        }

        public EventLogViewModel(LogDTO logDto)
            : base(logDto)
        {

        }

        [DataMember]
        public int Id
        {
            get
            {
                return Model.ID;
            }
            set
            {
                Model.ID = value;
            }
        }

        [DataMember]
        public string UserId
        {
            get
            {
                return Model.Source;
            }
            set
            {
                Model.Source = value;
            }
        }

        [DataMember]
        public string CustomMessage
        {
            get
            {
                return Model.CustomMessage;
            }
            set
            {
                Model.CustomMessage = value;
            }

        }

        [DataMember]
        public DateTime CreateDate
        {
            get
            {
                return Model.CreateDate;
            }
            set
            {
                Model.CreateDate = value;
            }
        }

        [DataMember]
        public string LogType
        {
            get
            {
                return Model.ApplicationName;
            }
            set
            {
                Model.ApplicationName = value;
            }
        }

        [DataMember]
        public int InnerExceptionCount
        {
            get
            {
                return Model.InnerExceptionCount;
            }
            set
            {
                Model.InnerExceptionCount = value;
            }
        }

        public static GridInfo ViewInfo
        {
            get
            {

                if (_viewInfo == null)
                {
                    var clientRel = new Core.Mvc.Helpers.CoreKendoGrid.ClientDependentFeature();
                    clientRel.Events.OnChange = "onLogChange";
                    clientRel.CssStyles.Add("font-size", "9pt;");
                    var dsConfig = new Core.Mvc.Helpers.CustomWrapper.DataModel.DataSourceInfo();
                    dsConfig.CrudCr.Read.Url = "api/Core/ExceptionLogApi/GetAllLogs";
                    //------------------------
                    var fConfig = new Features();
                    fConfig.ReadOnly = true;
                    fConfig.Selectability = Selectable.Row;
                    //---------------------
                    List<Column> colsInfo = new List<Column> { 
                                    new Column { Title="شناسه کاربر"   , Field="UserId"               , Width="110"  , Encoded=true  , Hidden=false }  ,
                                    new Column { Title="منشاء رویداد"  , Field="LogType"              , Width="90"  , Encoded=true  , Hidden=false } ,
                                    new Column { Title="پیام معادل"    , Field="CustomMessage"        , Template="# if( CustomMessage.length > 100 ) { #  #  CustomMessage = CustomMessage.substring(0,100); #   <span>#=kendo.toString(CustomMessage)#</span>   # } else {# <span>#=kendo.toString(CustomMessage)#</span> #} #"  , Width="800"  , Encoded=true  , Hidden=false }  ,
                                    new Column { Title="تاریخ وقوع"    , Field="CreateDate"           , Width="190"  , Encoded=true  , Hidden=false }  ,
                                    new Column { Title="خطاهای داخلی"  , Field="InnerExceptionCount"  , Width="70"   , Encoded=true  , Hidden=false },
                                    new Column { Title="مهر زمانی"     , Field="TimeStamp"            , Width="110"   , Encoded=true , Hidden=false }
                      };

                    _viewInfo = new Core.Mvc.Helpers.CoreKendoGrid.Settings.GridInfo(fConfig.CRUDOperation)
                     {
                         DataSource = dsConfig,
                         Features = fConfig,
                         ColumnsInfo = colsInfo
                     };
                }

                return _viewInfo;
            }
        }


    }

}
