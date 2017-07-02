using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Cmn
{
    public class ViewModelBuilder //: IViewModelBuilder<T>
    {
        private static object _Object = new object();
        private static ViewModelBuilder _vmBuilder = null;

        public static ViewModelBuilder GetInstance()
        {
            lock (_Object)
            {
                if (_vmBuilder == null)
                {
                    _vmBuilder = new ViewModelBuilder();
                }
            }
            return _vmBuilder;
        }

        public ViewModel GetViewModel<ViewModel, T>(T model)
            where T : EntityBase<T>, new()
            where ViewModel : ModelBase<T>, new()
        {
            var vm = new ViewModel();
            vm.SetModel(model);
            return vm;
        }

        public IEnumerable<ViewModel> GetViewModels<ViewModel, T>(IEnumerable<T> models)
            where T : EntityBase<T>, new()
            where ViewModel : ModelBase<T>, new()
        {
            if (models != null)
                foreach (var model in models.ToList<T>())
                {
                    var vm = new ViewModel();
                    vm.SetModel(model);
                    yield return vm;
                }
        }
    }
}
