using Core.Cmn.Cache;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings;
using Core.Mvc.Helpers.CustomWrapper.DataModel;
using Core.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Core.Mvc.ViewModel
{
    public class CacheInfoViewModel:CacheInfo
    {


        private static GridInfo _viewInfo;

        public static GridInfo ViewInfo
        {
            get
            {
                if (_viewInfo == null)
                {
                    var dsConfig = new DataSourceInfo();
                    dsConfig.CrudCr.Read.Url = "api/Core/CacheInfoApi/GetEntities";
                    dsConfig.ModelCr.ModelIdName = "Name";

                    //------------------------
                    var fConfig = new Features();
                    fConfig.ReadOnly = true;

                    fConfig.Selectability = Selectable.Row;


                    //---------------------
                    List<Column> colsInfo = new List<Column> { 
                                new Column { Field="Name" , Title="کلید",Width="240px"} ,
                                new Column { Field="BasicKey" , Title="کلید اصلی",Width="240px"} ,
                                //new Column { Field="Repository" , Title="Repository" }  , 
                                new Column { Field="ExpireCacheSecondTime" , Title="مدت کش",Width="65px",Sortable=true }  , 
                                //new Column { Field="MethodInfo" , Title="اطلاعات متد" }  ,
                                new Column { Field="CreateDateTime" , Title="تاریخ ایجاد ",Width="155px" }  , 
                                new Column { Field="FrequencyOfUsing" , Title="تعداد دفعات استفاده ",Width="124px" }  ,                                                                                                                  
                                new Column { Field="FrequencyOfBuilding" , Title="تعداد دفعات ایجاد",Width="124px" }  ,                                                                                                                                                       
                                new Column { Field="BuildingTime" , Title="زمان ایجاد",Width="155px" }  ,
                                new Column { Field = "UsingTime", Title = "زمان استفاده",Width = "155px" ,Sortable=true},                                                                                                                                                                                                                                                          
                                new Column { Field="LastBuildDateTime" , Title="آخرین تاریخ ایجاد",Width = "200px" }  ,                                                                                                                                                                                                                                                                                           
                                new Column { Field="LastUseDateTime" , Title="اخرین تاریخ استفاده",Width = "200px" }  ,                                                                                                                                                                                                                                                                                                                           
                                new Column { Field="AverageTimeToBuild" , Title="میانگین زمان ساخت",Width = "155px" }  ,                                                                                                                                                                                                                                                                                                          
                                new Column { Field="AverageTimeToUse" , Title="میانگین زمان استفاده",Width = "155px" }  ,
                                new Column { Field="ErrorCount" , Title="تعداد خطا ",Width = "90px" }  , 
                               
            };


                    _viewInfo = new GridInfo(fConfig.CRUDOperation)
                    {
                        DataSource = dsConfig,
                        ColumnsInfo = colsInfo,
                         Features = fConfig
                    };

                }

                return _viewInfo;
            }
        }
    }
}