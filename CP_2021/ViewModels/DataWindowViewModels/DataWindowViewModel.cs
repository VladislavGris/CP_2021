using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using CP_2021.Infrastructure.Units;
using System.Linq;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using CP_2021.Data;

namespace CP_2021.ViewModels.DataWindowViewModels
{
    class DataWindowViewModel : ViewModelBase
    {
        private ProductionTaskDB _baseTask;
        #region Свойства

        #region TaskToCopy

        private ProductionTaskDB _editableTask;

        public ProductionTaskDB EditableTask
        {
            get => _editableTask;
            set => Set(ref _editableTask, value);
        }

        #endregion

        #endregion

        #region Команды

        #region SetExecutionTermCommand

        public ICommand SetExecutionTermCommand { get; }

        private bool CanSetExecutionTermCommandExecute(object p) => true;

        private void OnSetExecutionTermCommandExecuted(object p)
        {
            Debug.WriteLine("Editing");
            if((!String.IsNullOrEmpty(_editableTask.Manufacture.CalendarDays) || !String.IsNullOrEmpty(_editableTask.Manufacture.WorkingDays)) &&
                _editableTask.Payment.IsFirstPayment)
            {
                int days;
                if (!String.IsNullOrEmpty(_editableTask.Manufacture.CalendarDays)){
                    if(Int32.TryParse(_editableTask.Manufacture.CalendarDays, out days))
                    {
                        _editableTask.Manufacture.ExecutionTerm = DateTime.Now.AddDays(days);
                    }
                }
                else
                {
                    if (Int32.TryParse(_editableTask.Manufacture.WorkingDays, out days))
                    {
                        _editableTask.Manufacture.ExecutionTerm = WorkingDays.AddWorkingDays(DateTime.Now, days);
                    }
                }
            }
        }

        #endregion

        #region SetActCommand

        public ICommand SetActCommand { get; }

        private bool CanSetActCommandExecute(object p) => true;

        private void OnSetActCommandExecuted(object p)
        {
            if(!String.IsNullOrEmpty(_editableTask.Act.ActNumber) && _editableTask.Act.ActDate.HasValue)
            {
                _editableTask.Act.ByAct = true;
                _editableTask.Act.ActCreation = false;
            }
        }

        #endregion

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private bool CanSaveCommandExecute(object p) => true;

        private void OnSaveCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите сохранить изменения?", "Сохранить", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                ProductionTaskDB task = ApplicationUnitSingleton.GetInstance().dbUnit.Tasks.Get().Where(t => t.Id == _editableTask.Id).FirstOrDefault();
                if (task != null)
                {
                    task.EditingBy = "default";
                }
                ApplicationUnitSingleton.GetInstance().dbUnit.Commit();
                ((Window)p).Close();
            }
            else if(result == MessageBoxResult.No)
            {
                using(ApplicationContext context = new ApplicationContext())
                {
                    var task = context.Production_Plan.Where(p=>p.Id ==  _editableTask.Id).FirstOrDefault();
                    if (task != null)
                    {
                        task.EditingBy = "default";
                    }
                    context.SaveChanges();
                }
                ((Window)p).Close();
            }
        }

        #endregion

        #endregion

        public void SetEditableTask(ProductionTaskDB task)
        {
            _editableTask = task;
            _baseTask = task.Clone();
        }

        public DataWindowViewModel()
        {
            SaveCommand = new LambdaCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
            SetExecutionTermCommand = new LambdaCommand(OnSetExecutionTermCommandExecuted, CanSetExecutionTermCommandExecute);
            SetActCommand = new LambdaCommand(OnSetActCommandExecuted, CanSetActCommandExecute);
        }
    }
}
