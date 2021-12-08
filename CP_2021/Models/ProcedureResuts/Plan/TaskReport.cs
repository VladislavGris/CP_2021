using CP_2021.Models.Base;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;
using System.Windows.Threading;

namespace CP_2021.Models.ProcedureResuts.Plan
{
    internal class TaskReport : Entity, INotifyPropertyChanged
    {
        public Guid ToId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Header { get; set; }
        public DateTime CompleteDate { get; set; }
        public string Description {  get; set; }
        public bool Completion { get; set; }
        public string ReportDescription { get; set; }
        public string ReportToName {  get; set; }
        [NotMapped]
        private bool _reportState;
        public bool ReportState
        {
            get => _reportState;
            set => Set(ref _reportState, value); 
        }
        #region PropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string PropertyName = null)
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

        protected virtual bool Set<T>(ref T field, T value, [CallerMemberName] string PropertyName = null)
        {
            if (Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(PropertyName);
            return true;
        }

        #endregion
    }
}
