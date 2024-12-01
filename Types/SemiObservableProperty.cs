namespace deeprockitems.Types
{
    /// <summary>
    /// Defines a property that may, but is not obligated, to send a notification.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SemiObservableProperty<T>
    {
        public delegate void PropertyChanged(PropertyChangedArgs<T> args);
        public event PropertyChanged OnPropertyChanged;
        T _value;
        T _oldValue;
        public SemiObservableProperty(T value)
        {
            _value = value;
            _oldValue = value;
        }
        public T GetValue() => _value;
        public void SetValue(T value, bool shouldSendNotification = false)
        {
            _oldValue = _value;
            _value = value;
            if (!shouldSendNotification)
            {
                return;
            }
            // Send notification
            OnPropertyChanged?.Invoke(new PropertyChangedArgs<T>(_oldValue, _value));
        }
    }
}
