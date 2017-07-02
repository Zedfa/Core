using Core.Cmn;

namespace Core.Entity
{
    public interface IViewModelBase<T> : IModelBase<T>, IViewModel where T : IEntity, new()
    {
        //IViewModelBase<T> SetViewModel(T model);
    }

}
