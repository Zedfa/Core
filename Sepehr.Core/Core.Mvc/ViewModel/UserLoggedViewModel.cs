using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using Core.Cmn.FarsiUtils;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.ViewModel;
using Core.Cmn.Extensions;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Entity;

namespace Core.Mvc.ViewModel
{
    [DataContract(Name = "UserLoggedViewModel")]
    public class UserLoggedViewModel : ViewModelBase<CoreUserLog>
    {
        public UserLoggedViewModel()
            : base()
        {

        }

        public UserLoggedViewModel(CoreUserLog model)
            : base(model)
        {

        }

        [DataMember(IsRequired = true)]
        public int Id
        {
            get { return Model.Id; }

            set { Model.Id = value; }
        }

        [DataMember]
        [Display(Name = "شناسه کاربر")]
        public int UserIdInUserLog
        {
            get { return Model.UserId; }
            set { Model.UserId = value; }
        }

        [DataMember]
        private string _logType;
        [DataMember]
        public string LogType
        {
            get
            {
                if (string.IsNullOrEmpty(_logType))
                    return Model.LogType != null ? Model.LogType.GetDescriptionEnum() : null;
                return _logType;
            }
            set
            {
                _logType = value;
            }
        }

        public int? TableNameId
        {
            get { return Model.TableNameId; }
            set { Model.TableNameId = value; }
        }
        [DataMember]
        private string _tableName { get; set; }

        [DataMember]
        [Display(Name = "نام جدول")]
        public string TableName
        {
            get
            {
                if (string.IsNullOrEmpty(_tableName))
                    if (Model.TableNameId != null)
                    {
                        return Model.TableInfo != null ? Model.TableInfo.Caption : "";
                    }
                return _tableName;
            }
            set
            {
                _tableName = value;
            }
        }

        [DataMember]
        [Display(Name = "ActionDate")]
        public string DateTime
        {
            get
            {
                if (!Model.DateTime.Equals(System.DateTime.MinValue) && Model.DateTime != null)
                    return PersianDate.Parse(PersianDateConverter.ToPersianDate((DateTime)Model.DateTime).ToString()).ToString();
                return string.Empty;
            }
            set
            {
                if (value != null)
                    Model.DateTime = PersianDateConverter.ToGregorianDateTime(value);
            }
        }

        [DataMember]
        private string _fullName { get; set; }

        [DataMember]
        public string FullName
        {
            get
            {
                if (string.IsNullOrEmpty(_fullName))
                    return Model.User != null ? Model.User.FName + " " + Model.User.LName : null;
                return _fullName;
            }
            set
            {
                _fullName = value;
            }
        }

        [DataMember]
        private string _userForTrackInUserLog { get; set; }

        [DataMember]
        public string UserForTrackInUserLog
        {
            get;
            set;
        }

        [DataMember]
        public string Ip
        {
            get { return Model.Ip; }
            set { Model.Ip = value; }
        }

        private static GridInfo _viewInfo;

        public static GridInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {

                    var dsConfig = new DataSourceInfo();
                    dsConfig.CrudCr.Read.Url = "api/Core/UserLoggedApi";
                    
                    /*---------------------------------- */
                    var fConfig = new Features();

                    /*---------------------------------- */
                    fConfig.UserGuideIncluded = false;
                    _viewInfo = new GridInfo(fConfig.CRUDOperation)
                    {
                        ColumnsInfo = new List<Column> 
                            { 
                                new Column {Field="UserIdInUserLog", Visible=false , Hidden=true }, 
                                new Column { Title="نام کاربر" , Field="FullName"  , Encoded=true , Visible=true , Width="50px",Filterable = false,Sortable = false }, 
                                new Column { Title="نام جدول" , Field="TableName"  , Encoded=true , Visible=true , Width="50px",Filterable = false,Sortable = false }, 
                                new Column { Title="نوع عملیات" , Field="LogType"  , Encoded=true , Visible=true , Width="50px",Filterable = false,Sortable = false },
                                new Column { Title="تاریخ عملیات" , Field="DateTime"  , Encoded=true , Visible=true , Width="50px" ,Filterable = false,Sortable = false},
                                new Column { Title="IP" , Field="Ip"  , Encoded=true , Hidden=true , Width="50px" ,Filterable = false,Sortable = false},
                                new Column { Field="TableNameId" , Visible=false , Hidden=true} 

                            },
                        DataSource = dsConfig,
                        Features = fConfig,
                    };
                }
                return _viewInfo;
            }
        }
    }
}