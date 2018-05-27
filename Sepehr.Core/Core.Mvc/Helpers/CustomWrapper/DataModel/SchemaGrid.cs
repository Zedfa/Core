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

        public Dictionary<string, SearchInfo> SearchSchemaList { get; private set; }


        public Dictionary<string, LookupConfig> LookupViewSchema { get; set; }

        public Dictionary<string, DropDownListInfo> DropDownSchema { get; set; }

        public Dictionary<string, AutoCompleteInfo> AutoCompleteSchema { get; set; }


        public SchemaGrid(Type modelMetaData)
        {
            GridViewModelType = modelMetaData;
            SearchSchemaList = new Dictionary<string, SearchInfo>();
            LookupViewSchema = new Dictionary<string, LookupConfig>();
            DropDownSchema = new Dictionary<string, DropDownListInfo>();
            AutoCompleteSchema = new Dictionary<string, AutoCompleteInfo>();
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
            foreach (var sInfo in LookupViewSchema)
            {

                // type, title, viewModelName, viewModelPropertyName, lookupName, displayName, valueName, bindingName, isMultiselect, width, height
                InitializeFieldTypeInfo(ref fieldTypeInfo);
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
            foreach (var info in DropDownSchema)
            {
                InitializeFieldTypeInfo(ref fieldTypeInfo);
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
        private void BuildSearchAutoCompleteDictionary(Dictionary<string, object> fieldTypeInfos)
        {

            ModelFieldTypeInfo fieldTypeInfo = null;
            foreach (var info in AutoCompleteSchema)
            {
                InitializeFieldTypeInfo(ref fieldTypeInfo);
                var autoCompleteAttribute = info.Value as AutoCompleteInfo;
                fieldTypeInfo.CustomType = "AutoComplete";
                var infoDic = new Dictionary<string, string>();
                infoDic.Add("url", autoCompleteAttribute.Url);
                infoDic.Add("propertyNameForBinding", autoCompleteAttribute.PropertyNameForBinding);
                infoDic.Add("valueName", autoCompleteAttribute.ValueName);
                infoDic.Add("displayName", autoCompleteAttribute.DisplayName);
                infoDic.Add("searchProperty", autoCompleteAttribute.SearchProperty);
                infoDic.Add("watermark", autoCompleteAttribute.Watermark);

                fieldTypeInfo.AutoCompleteKeyValue = infoDic;

                fieldTypeInfos.Add(info.Key, fieldTypeInfo.ToJson());
            }

        }

        private Dictionary<string, object> BuildComprehensiveSearchObjectDictionary()
        {
            Dictionary<string, object> schemaMetaData = new Dictionary<string, object>();

            if(LookupViewSchema.Any()){
                BuildSearchLookupDictionary(schemaMetaData) ;
            }

            if (DropDownSchema.Any())
            {
               BuildSearchDropDownDictionary(schemaMetaData) ;
            }

            if (AutoCompleteSchema.Any())
            {
                BuildSearchAutoCompleteDictionary(schemaMetaData);
            }

            foreach (var sInfo in SearchSchemaList)
            {
                ModelFieldTypeInfo fieldTypeInfo = null;

                if (sInfo.Value is SearchConstantField)
                {
                    InitializeFieldTypeInfo(ref fieldTypeInfo);
                    var SearchRelatedEnumAttr = (sInfo.Value as SearchConstantField);
                    fieldTypeInfo.CustomType = SearchRelatedEnumAttr.CustomType;
                   
                    fieldTypeInfo.EnumKeyValue = GetConstantsOfSearchingField(SearchRelatedEnumAttr.ConstantsCategoryName);
                }
                else if (sInfo.Value is SearchDateTimeField)
                {
                    InitializeFieldTypeInfo(ref fieldTypeInfo);
                    var SearchRelatedEnumAttr = (sInfo.Value as SearchDateTimeField);
                    fieldTypeInfo.CustomType = SearchRelatedEnumAttr.CustomType;
                    fieldTypeInfo.ModelPropName = SearchRelatedEnumAttr.MainPropertyNameOfModel;
                }
                else if (sInfo.Value is SearchDateField)
                {
                    InitializeFieldTypeInfo(ref fieldTypeInfo);
                    var SearchRelatedEnumAttr = (sInfo.Value as SearchDateField);
                    fieldTypeInfo.CustomType = SearchRelatedEnumAttr.CustomType;
                    fieldTypeInfo.ModelPropName = SearchRelatedEnumAttr.MainPropertyNameOfModel;
                }


                if (schemaMetaData != null)
                {
                    schemaMetaData.Add(sInfo.Key, fieldTypeInfo.ToJson());
                }

            }



            return schemaMetaData;
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

           
       
        private void InitializeFieldTypeInfo(ref ModelFieldTypeInfo fieldTypeInfo)
        {
            if (fieldTypeInfo == null)
            {
                fieldTypeInfo = new ModelFieldTypeInfo();
            }
        }


    }
}
