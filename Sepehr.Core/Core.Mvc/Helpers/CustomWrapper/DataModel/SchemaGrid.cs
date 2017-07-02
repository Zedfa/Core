using Kendo.Mvc.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Mvc.Extensions;
using System.Web.Mvc;
using System.Reflection;
using Core.Mvc.Attribute.Filter;
using Core.Cmn.Extensions;
using Core.Mvc.Extensions.FilterRelated;
using Core.Mvc.Helpers.CustomWrapper.SearchRelated;
using Core.Rep;
using Core.Cmn;


namespace Core.Mvc.Helpers.CustomWrapper.DataModel
{
    public class SchemaGrid : DataSourceSchema
    {
        public IDbContextBase IDbContextBase { get { return Service.ServiceBase.DependencyInjectionFactory.CreateInjectionInstance<IDbContextBase>(); } }
        public Type GridViewModelType { get; private set; }

        public Dictionary<string, SearchInfo> SearchInfos { get; private set; }


        public Dictionary<string, LookupConfig> LookupViewInfos { get; set; }

        public Dictionary<string, DropDownListInfo> DropDownInfos { get; set; }

        public SchemaGrid(Type modelMetaData)
        {
            GridViewModelType = modelMetaData;
            SearchInfos = new Dictionary<string, SearchInfo>();
            LookupViewInfos = new Dictionary<string, SearchRelated.LookupConfig>();
            DropDownInfos = new Dictionary<string, SearchRelated.DropDownListInfo>();

        }

        protected override void Serialize(IDictionary<string, object> json)
        {
            base.Serialize(json);

            if (GridViewModelType != null)
            {
                json["searchCustomTypes"] = BuildComprehensiveSearchObjectDictionary();
            }
        }

        private void BuildSearchLookupDictionary(Dictionary<string, object> fieldTypeInfos)
        {

            ModelFieldTypeInfo fieldTypeInfo = null;
            foreach (var sInfo in LookupViewInfos)
            {

                // type, title, viewModelName, viewModelPropertyName, lookupName, displayName, valueName, bindingName, isMultiselect, width, height
                InitialiseFieldTypeInfo(ref fieldTypeInfo);
                var SearchRelatedLookpAttr = sInfo.Value;//;(sInfo.Value as SearchLookup);
                fieldTypeInfo.CustomType = "Lookup";
                var lookupDic = new Dictionary<string, string>();
                lookupDic.Add("type", SearchRelatedLookpAttr.LookupType);
                lookupDic.Add("title", SearchRelatedLookpAttr.LookupTitle);
                lookupDic.Add("viewModelName", SearchRelatedLookpAttr.ViewModelName);
                lookupDic.Add("viewInfoName", SearchRelatedLookpAttr.ViewInfoName);
                lookupDic.Add("lookupName", SearchRelatedLookpAttr.LookupName);
                lookupDic.Add("displayName", SearchRelatedLookpAttr.NavigateViewModelDisplayName);
                lookupDic.Add("valueName", SearchRelatedLookpAttr.NavigateViewModelValueName);
                lookupDic.Add("bindingName", SearchRelatedLookpAttr.ModelBindingName);
                lookupDic.Add("isMultiselect", SearchRelatedLookpAttr.IsMultiSelect.ToString());
                lookupDic.Add("navigatePropertyUnderlayingModelName", SearchRelatedLookpAttr.NavigatePropertyUnderlayingModelName);
                lookupDic.Add("navigatePropertyUnderlayingModelIdName", SearchRelatedLookpAttr.NavigatePropertyUnderlayingModelIdName);
                fieldTypeInfo.LookupKeyValue = lookupDic;

                if (fieldTypeInfos != null)
                {
                    fieldTypeInfos.Add(sInfo.Key, fieldTypeInfo.ToJson());
                }
            }

        }

        private void BuildSearchDropDownDictionary(Dictionary<string, object> fieldTypeInfos)
        {

            ModelFieldTypeInfo fieldTypeInfo = null;
            foreach (var info in DropDownInfos)
            {
                InitialiseFieldTypeInfo(ref fieldTypeInfo);
                var dropDownAttribute = info.Value as DropDownListInfo;
                fieldTypeInfo.CustomType = "Dropdown";
                var infoDic = new Dictionary<string, string>();
                infoDic.Add("dbCategoryName", dropDownAttribute.DBCategoryName);
                infoDic.Add("url", dropDownAttribute.Url);
                infoDic.Add("propertyNameForBinding", dropDownAttribute.PropertyNameForBinding);
                infoDic.Add("valueName", dropDownAttribute.ValueName);
                infoDic.Add("displayName", dropDownAttribute.DisplayName);
                fieldTypeInfo.DropDownKeyValue = infoDic;

                fieldTypeInfos.Add(info.Key, fieldTypeInfo.ToJson());
            }

        }

        private Dictionary<string, object> BuildComprehensiveSearchObjectDictionary()
        {
            Dictionary<string, object> fieldTypeInfos = new Dictionary<string, object>();

            if(LookupViewInfos.Any()){
                BuildSearchLookupDictionary(fieldTypeInfos) ;
            }

            if (DropDownInfos.Any())
            {
               BuildSearchDropDownDictionary(fieldTypeInfos) ;
            }

            foreach (var sInfo in this.SearchInfos)
            {
                ModelFieldTypeInfo fieldTypeInfo = null;

                if (sInfo.Value is SearchConstantField)
                {
                    InitialiseFieldTypeInfo(ref fieldTypeInfo);
                    var SearchRelatedEnumAttr = (sInfo.Value as SearchConstantField);
                    fieldTypeInfo.CustomType = SearchRelatedEnumAttr.CustomType;
                    //fieldTypeInfo.ModelPropName = SearchRelatedEnumAttr.MainPropertyNameOfModel;
                    // var enumType = SearchRelatedEnumAttr.EnumType;
                    fieldTypeInfo.EnumKeyValue = GetConstantsOfSearchingField(SearchRelatedEnumAttr.ConstantsCategoryName);// enumType.GetEnumKeyValuePairEquivalents();
                }
                else if (sInfo.Value is SearchDateTimeField)
                {
                    InitialiseFieldTypeInfo(ref fieldTypeInfo);
                    var SearchRelatedEnumAttr = (sInfo.Value as SearchDateTimeField);
                    fieldTypeInfo.CustomType = SearchRelatedEnumAttr.CustomType;
                    fieldTypeInfo.ModelPropName = SearchRelatedEnumAttr.MainPropertyNameOfModel;
                }
                else if (sInfo.Value is SearchDateField)
                {
                    InitialiseFieldTypeInfo(ref fieldTypeInfo);
                    var SearchRelatedEnumAttr = (sInfo.Value as SearchDateField);
                    fieldTypeInfo.CustomType = SearchRelatedEnumAttr.CustomType;
                    fieldTypeInfo.ModelPropName = SearchRelatedEnumAttr.MainPropertyNameOfModel;
                }


                if (fieldTypeInfos != null)
                {
                    fieldTypeInfos.Add(sInfo.Key, fieldTypeInfo.ToJson());
                }

            }//end of foreach



            return fieldTypeInfos;
        }

        private Dictionary<string, string> GetConstantsOfSearchingField(string constantCategoryName)
        {
            var retDic = new Dictionary<string, string>();
            var constRep = new ConstantRepository(IDbContextBase);

            var constsList = constRep.GetConstantsOfCategory(constantCategoryName);
            constsList.ForEach(con =>
            {
                retDic.Add(con.Key, con.Value);
            });
            return retDic;
        }



        private Dictionary<string, object> GetColumnWithClrRelatedTypeForSearch()
        {
            PropertyInfo[] propInfos = GridViewModelType.GetProperties();
            var fieldTypeInfos = new Dictionary<string, object>();
            foreach (var item in propInfos)
            {
                var customAttrs = item.GetCustomAttributes(true);
                ModelFieldTypeInfo fieldTypeInfo = null;

                foreach (var attrItem in customAttrs)
                {
                    if (attrItem is SearchRelatedTypeAttribute)
                    {
                        InitialiseFieldTypeInfo(ref fieldTypeInfo);
                        var searchRelatedAttr = (attrItem as SearchRelatedTypeAttribute);
                        fieldTypeInfo.CustomType = searchRelatedAttr.CustomType;
                        fieldTypeInfo.ModelPropName = searchRelatedAttr.MainPropertyNameOfModel;
                        fieldTypeInfo.TrueEquivalent = searchRelatedAttr.TrueEquivalent;
                        fieldTypeInfo.FalseEquivalent = searchRelatedAttr.FalseEquivalent;
                        fieldTypeInfo.NavigationProperty = searchRelatedAttr.NavigationProperty;
                    }

                    else if (attrItem is SearchRelatedEnumInfoAttribute)
                    {
                        InitialiseFieldTypeInfo(ref fieldTypeInfo);
                        var SearchRelatedEnumAttr = (attrItem as SearchRelatedEnumInfoAttribute);
                        fieldTypeInfo.CustomType = SearchRelatedEnumAttr.CustomType;
                        fieldTypeInfo.ModelPropName = SearchRelatedEnumAttr.MainPropertyNameOfModel;
                        var enumType = SearchRelatedEnumAttr.EnumType;
                        //fieldTypeInfo.EnumKeyValue = enumType.GetEnumKeyValuePairEquivalents();
                    }
                    else if (attrItem is SearchLookupAttribute)
                    {
                        // type, title, viewModelName, viewModelPropertyName, lookupName, displayName, valueName, bindingName, isMultiselect, width, height
                        InitialiseFieldTypeInfo(ref fieldTypeInfo);
                        var SearchRelatedLookpAttr = (attrItem as SearchLookupAttribute);
                        fieldTypeInfo.CustomType = "Lookup";
                        var lookupDic = new Dictionary<string, string>();
                        lookupDic.Add("type", SearchRelatedLookpAttr.LookupType);
                        lookupDic.Add("title", SearchRelatedLookpAttr.LookupTitle);
                        lookupDic.Add("viewModelName", SearchRelatedLookpAttr.ViewModelName);
                        lookupDic.Add("viewModelPropertyName", SearchRelatedLookpAttr.ViewModelGridInfoName);
                        lookupDic.Add("lookupName", SearchRelatedLookpAttr.LookupName);
                        lookupDic.Add("displayName", SearchRelatedLookpAttr.NavigateViewModelDisplayName);
                        lookupDic.Add("valueName", SearchRelatedLookpAttr.NavigateViewModelValueName);
                        lookupDic.Add("bindingName", SearchRelatedLookpAttr.ModelBindingName);
                        lookupDic.Add("isMultiselect", SearchRelatedLookpAttr.IsMultiSelect.ToString());
                        lookupDic.Add("navigatePropertyUnderlayingModelName", SearchRelatedLookpAttr.NavigatePropertyUnderlayingModelName);
                        lookupDic.Add("navigatePropertyUnderlayingModelIdName", SearchRelatedLookpAttr.NavigatePropertyUnderlayingModelIdName);
                        fieldTypeInfo.LookupKeyValue = lookupDic;
                    }


                }
                if (fieldTypeInfo != null)
                {
                    fieldTypeInfos.Add(item.Name, fieldTypeInfo.ToJson());
                }
            }

            SetExcludingFields(fieldTypeInfos);
            return fieldTypeInfos;
        }

        private void SetExcludingFields(Dictionary<string, object> fieldTypeInfos)
        {
            var viewModelNonAttrs = GridViewModelType.GetCustomAttributes(false);//.FirstOrDefault(attr => attr.AttributeType.Name == "SearchExcludingFieldsAttribute");
            foreach (var attrItem in viewModelNonAttrs)
            {
                if (attrItem is SearchExcludingFieldsAttribute)
                {
                    var attr = (attrItem as SearchExcludingFieldsAttribute);
                    fieldTypeInfos.Add("excludingFields", attr.FieldsToExcludeFromSearchable);
                }
            }
        }

        private void InitialiseFieldTypeInfo(ref ModelFieldTypeInfo fieldTypeInfo)
        {
            if (fieldTypeInfo == null)
            {
                fieldTypeInfo = new ModelFieldTypeInfo();
            }
        }


    }
}
