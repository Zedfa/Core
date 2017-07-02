namespace Core.Serialization
{
    public interface IPropertyAccessor
    {
        object Get(object target);
        void Set(object target, object value);
    }
}