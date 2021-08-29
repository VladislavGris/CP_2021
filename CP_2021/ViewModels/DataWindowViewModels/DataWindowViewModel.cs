using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;

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

        #region SaveCommand

        public ICommand SavePaymentCommand { get; }

        private bool CanSavePaymentCommandExecute(object p) => true;

        private void OnSavePaymentCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите сохранить изменения?", "Сохранить", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                ApplicationUnitSingleton.GetInstance().dbUnit.Commit();
                ((Window)p).Close();
            }else if(result == MessageBoxResult.No)
            {
                _editableTask.Payment = _baseTask.Payment.Clone();
                ((Window)p).Close();
            }
            
        }

        #endregion

        #region SaveLaborCostsCommand

        public ICommand SaveLaborCostsCommand { get; }

        private bool CanSaveLaborCostsCommandExecute(object p) => true;

        private void OnSaveLaborCostsCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите сохранить изменения?", "Сохранить", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                ApplicationUnitSingleton.GetInstance().dbUnit.Commit();
                ((Window)p).Close();
            }
            else if (result == MessageBoxResult.No)
            {
                _editableTask.LaborCosts = _baseTask.LaborCosts.Clone();
                ((Window)p).Close();
            }

        }

        #endregion

        #region SetExecutionTermCommand

        public ICommand SetExecutionTermCommand { get; }

        private bool CanSetExecutionTermCommandExecute(object p) => true;

        private void OnSetExecutionTermCommandExecuted(object p)
        {
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

        #region SaveCommand

        public ICommand SaveCommand { get; }

        private bool CanSaveCommandExecute(object p) => true;

        private void OnSaveCommandExecuted(object p)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите сохранить изменения?", "Сохранить", MessageBoxButton.YesNoCancel);
            if (result == MessageBoxResult.Yes)
            {
                ApplicationUnitSingleton.GetInstance().dbUnit.Commit();   
            }
            ((Window)p).Close();
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
            SavePaymentCommand = new LambdaCommand(OnSavePaymentCommandExecuted, CanSavePaymentCommandExecute);
            SaveLaborCostsCommand = new LambdaCommand(OnSaveLaborCostsCommandExecuted, CanSaveLaborCostsCommandExecute);
            SaveCommand = new LambdaCommand(OnSaveCommandExecuted, CanSaveCommandExecute);
            SetExecutionTermCommand = new LambdaCommand(OnSetExecutionTermCommandExecuted, CanSetExecutionTermCommandExecute);
        }
    }
}
