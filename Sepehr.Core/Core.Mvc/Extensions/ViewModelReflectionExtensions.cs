using Core.Cmn;
using Core.Mvc.Helpers.CoreKendoGrid;
using Core.Mvc.Helpers.CoreKendoGrid.Settings.ColumnConfig;
using Core.Mvc.ViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Mvc.Extensions
{
    public static class ViewModelReflectionExtensions
    {
        public static List<Column> BiuldColumnsFromViewModel(this Type viewModel)
        {
            var columns = new List<Column>();
            var propInfos = viewModel.GetProperties();
            foreach (var property in propInfos)
            {
                columns.Add(new Column { Field = property.Name  });
            }
            return columns;
        }

        public static string GetViewModelId(this IViewModel viewModel)
        {
            var IdProperty = viewModel.GetType().GetProperties().FirstOrDefault(prop => prop.Name.ToLower().Contains("id")).Name;
            
            if (IdProperty == null)
            {
                //IdProperty = viewModel.GetProperties().FirstOrDefault(prop => prop.CustomAttributes.Any(at=>at.AttributeType == Type.GetType("Key"))).Name;
                foreach (var attr in TypeDescriptor.GetAttributes(viewModel))
                {
                    //viewModel
                    if (attr.GetType().FullName.ToLower().Contains("Key".ToLower()))
                    {
                       
                    }
                }
            }
            return IdProperty;
        }
    }
}
