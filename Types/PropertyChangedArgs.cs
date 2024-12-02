namespace deeprockitems.Types
{
    public class PropertyChangedArgs<T>
    {
        public T OldValue { get; init; }
        public T NewValue { get; init; }
        public PropertyChangedArgs(T oldValue, T newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
