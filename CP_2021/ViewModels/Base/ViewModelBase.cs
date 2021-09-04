using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace CP_2021.ViewModels.Base
{
    internal abstract class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName]string PropertyName = null)
        {
            var handlers = PropertyChanged;
            if (handlers != null)
            {
                var invocationList = handlers.GetInvocationList();
                var arg = new PropertyChangedEventArgs(PropertyName);
                foreach (var action in invocationList)
                {
                    if (action.Target is DispatcherObject dispatcher)
                    {
                        dispatcher.Dispatcher.Invoke(action, this, arg);
                    }
                    else
                    {
                        action.DynamicInvoke(this, arg);
                    }
                }
            }  
        }

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName]string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }
    }
}
