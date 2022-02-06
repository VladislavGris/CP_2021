using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Units;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.ViewModels.Base;
using CP_2021.ViewModels.Reports;
using CP_2021.ViewModels.Reports.Payment;
using CP_2021.Views.UserControls.Reports;
using CP_2021.Views.UserControls.Reports.Payment;
using System;
using System.Diagnostics;
using System.Windows.Controls;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class ReportsViewModel : ViewModelBase
    {

        #region Свойства
        #region UserControls

        private CoopWorkReport _coopWork = new CoopWorkReport();
        private ActCreationReport _actCreationReport = new ActCreationReport();
        private DocumInWorkReport _documInWorkReport = new DocumInWorkReport();
        private InWorkReport _inWorkReport = new InWorkReport();
        private NoSpecReport _noSpecReport = new NoSpecReport();
        private OECStorageReport _oecStorageReport = new OECStorageReport();
        private SKBChecksReport _skbCheckReport = new SKBChecksReport();
        private TimedGivingReport _timedGivingReport = new TimedGivingReport();
        private WorkedDocsReport _workedDocsReport = new WorkedDocsReport();
        private VKOnStorageReport _vkOnStorageReport = new VKOnStorageReport();
        private PaymentReport _paymentReport = new PaymentReport();

        #endregion

        #region Content

        private ContentControl _content;

        public ContentControl Content
        {
            get => _content;
            set => Set(ref _content, value);
        }

        #endregion

        #endregion

        #region Команды

        #region Отчеты
        #region Команды отчетов




        #region ShowCoopWorkCommand

        public ICommand ShowCoopWorkCommand { get; }

        private bool CanShowCoopWorkCommandExecute(object p) => true;

        private void OnShowCoopWorkCommandExecuted(object p)
        {
            Content = _coopWork;
        }

        #endregion

        #region ShowActCreationCommand

        public ICommand ShowActCreationCommand { get; }

        private bool CanShowActCreationCommandExecute(object p) => true;

        private void OnShowActCreationCommandExecuted(object p)
        {
            Content = _actCreationReport;
        }

        #endregion

        #region ShowDocumInWorkCommand

        public ICommand ShowDocumInWorkCommand { get; }

        private bool CanShowDocumInWorkCommandExecute(object p) => true;

        private void OnShowDocumInWorkCommandExecuted(object p)
        {
            Content = _documInWorkReport;
        }

        #endregion

        #region ShowInWorkCommand

        public ICommand ShowInWorkCommand { get; }

        private bool CanShowInWorkCommandExecute(object p) => true;

        private void OnShowInWorkCommandExecuted(object p)
        {
            Content = _inWorkReport;
        }

        #endregion

        #region ShowNoSpecCommand

        public ICommand ShowNoSpecCommand { get; }

        private bool CanShowNoSpecCommandExecute(object p) => true;

        private void OnShowNoSpecCommandExecuted(object p)
        {
            Content = _noSpecReport;
        }

        #endregion

        #region ShowOECStorageCommand

        public ICommand ShowOECStorageCommand { get; }

        private bool CanShowOECStorageCommandExecute(object p) => true;

        private void OnShowOECStorageCommandExecuted(object p)
        {
            Content = _oecStorageReport;
        }

        #endregion

        #region ShowSKBCheckCommand

        public ICommand ShowSKBCheckCommand { get; }

        private bool CanShowSKBCheckCommandExecute(object p) => true;

        private void OnShowSKBCheckCommandExecuted(object p)
        {
            Content = _skbCheckReport;
        }

        #endregion

        #region ShowTimedGivingCommand

        public ICommand ShowTimedGivingCommand { get; }

        private bool CanShowTimedGivingCommandExecute(object p) => true;

        private void OnShowTimedGivingCommandExecuted(object p)
        {
            Content = _timedGivingReport;
        }

        #endregion

        #region ShowWorkedDocsCommand

        public ICommand ShowWorkedDocsCommand { get; }

        private bool CanShowWorkedDocsCommandExecute(object p) => true;

        private void OnShowWorkedDocsCommandExecuted(object p)
        {
            Content = _workedDocsReport;
        }

        #endregion

        #region ShowVKOnStorageCommand

        public ICommand ShowVKOnStorageCommand { get; }

        private bool CanShowVKOnStorageCommandExecute(object p) => true;

        private void OnShowVKOnStorageCommandExecuted(object p)
        {
            Content = _vkOnStorageReport;
        }

        #endregion

        #region ShowPaymentReportCommand

        public ICommand ShowPaymentReportCommand { get; }

        private bool CanShowPaymentReportCommandExecute(object p) => true;

        private void OnShowPaymentReportCommandExecuted(object p)
        {
            Content = _paymentReport;
        }

        #endregion

        #endregion
        #region События отчетов

        #region События

        public event EventHandler<TaskIdEventArgs> SendTaskIdToMainWindow;
        // Отправка Id на главное окно
        protected void OnSendTaskIdToMainWindow(TaskIdEventArgs e)
        {
            EventHandler<TaskIdEventArgs> handler = SendTaskIdToMainWindow;
            handler?.Invoke(this, e);
        }

        #endregion

        #region Обработчики событий
        // Получение Id от VM отчетов
        public void GetTaskIdFromReport(object sender, TaskIdEventArgs e)
        {
            Debug.WriteLine("Id:" + e.Id);
            OnSendTaskIdToMainWindow(e);
        }
        #endregion



        #endregion
        #endregion

        #endregion

        public ReportsViewModel() 
        {
            #region Команды
            ShowCoopWorkCommand = new LambdaCommand(OnShowCoopWorkCommandExecuted, CanShowCoopWorkCommandExecute);
            ShowActCreationCommand = new LambdaCommand(OnShowActCreationCommandExecuted, CanShowActCreationCommandExecute);
            ShowDocumInWorkCommand = new LambdaCommand(OnShowDocumInWorkCommandExecuted, CanShowDocumInWorkCommandExecute);
            ShowInWorkCommand = new LambdaCommand(OnShowInWorkCommandExecuted, CanShowInWorkCommandExecute);
            ShowNoSpecCommand = new LambdaCommand(OnShowNoSpecCommandExecuted, CanShowNoSpecCommandExecute);
            ShowOECStorageCommand = new LambdaCommand(OnShowOECStorageCommandExecuted, CanShowOECStorageCommandExecute);
            ShowSKBCheckCommand = new LambdaCommand(OnShowSKBCheckCommandExecuted, CanShowSKBCheckCommandExecute);
            ShowTimedGivingCommand = new LambdaCommand(OnShowTimedGivingCommandExecuted, CanShowTimedGivingCommandExecute);
            ShowWorkedDocsCommand = new LambdaCommand(OnShowWorkedDocsCommandExecuted, CanShowWorkedDocsCommandExecute);
            ShowVKOnStorageCommand = new LambdaCommand(OnShowVKOnStorageCommandExecuted, CanShowVKOnStorageCommandExecute);
            ShowPaymentReportCommand = new LambdaCommand(OnShowPaymentReportCommandExecuted, CanShowPaymentReportCommandExecute);
            #endregion

            #region Subscribe
            ((CoopWorkVM)(_coopWork.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((ActCreationReportVM)_actCreationReport.DataContext).SendTaskIdToReportVM +=GetTaskIdFromReport;
            ((DocumInWorkReportVM)_documInWorkReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((InWorkReportVM)_inWorkReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((NoSpecReportVM)_noSpecReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((OECStorageReportVM)_oecStorageReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((SKBCheckReportVM)_skbCheckReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((TimedGivingReportVM)_timedGivingReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((WorkedDocsReportVM)_workedDocsReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((VKOnStorageReportVM)_vkOnStorageReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((PaymentReportVM)_paymentReport.DataContext).SendTaskIdToReportVM += GetTaskIdFromReport;
            #endregion
        }
    }
}
