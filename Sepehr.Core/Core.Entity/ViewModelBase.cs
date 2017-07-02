using Core.Cmn;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;


namespace Core.Entity
{
    [Serializable]
    [DataContract(Name = "ViewModelBase")]
    public abstract class ViewModelBase<T> : ModelBase<T>, IViewModelBase<T>, IViewModel where T : IEntity, new()
    {

        public ViewModelBase()
            : base()
        {
           // SearchConfigurations = new List<ModelFieldTypeInfo>();
            SetModel(new T());
        }

        public ViewModelBase(T model)
            : base(model)
        {
           // SearchConfigurations = new List<ModelFieldTypeInfo>();
            SetModel(model);
        }

        public static ViewModel GetViewModel<ViewModel>(T model)
            //where T : EntityBase<T>, new()
            where ViewModel : ViewModelBase<T>, new()
        {
            var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            var vm = new ViewModel()
            {
                Culture = culture.EnglishName
            };
            vm.SetModel(model);
            return vm;
        }

        public static ViewModel GetViewModel<ViewModel>(T model, CultureInfo cultureInfo)
            //where T : EntityBase<T>, new()
      where ViewModel : ViewModelBase<T>, new()
        {
            var vm = new ViewModel()
            {
                Culture = cultureInfo.EnglishName
            };
            vm.SetModel(model);
            return vm;
        }

        public static IEnumerable<ViewModel> GetViewModels<ViewModel>(IEnumerable<T> models)
            //where T : EntityBase<T>, new()

            where ViewModel : ViewModelBase<T>, new()
        {
            if (models != null)
            {
                var culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
                foreach (var model in models.ToList<T>())
                {
                    var vm = new ViewModel()
                    {
                        Culture = culture.Name
                    };
                    vm.SetModel(model);
                    yield return vm;
                }
            }

        }


        public static IEnumerable<ViewModel> GetViewModels<ViewModel>(IEnumerable<T> models, CultureInfo cultureInfo)
            //where T : EntityBase<T>, new()
            where ViewModel : ViewModelBase<T>, new()
        {
            if (models != null)
            {
                foreach (var model in models.ToList<T>())
                {
                    var vm = new ViewModel()
                    {
                        Culture = cultureInfo.Name
                    };
                    vm.SetModel(model);
                    yield return vm;
                }

            }

        }

        //public List<ModelFieldTypeInfo> SearchConfigurations { get; private set; }
        [IgnoreDataMember]
        [JsonIgnore]
        public string Culture { get; private set; }
        protected virtual void AppendSearchConfiguration()
        {

        }



    }
}
