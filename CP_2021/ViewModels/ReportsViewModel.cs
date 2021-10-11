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

        private ApplicationUnit Unit;
        #region UserControls

        private NoSpecificationReport _noSpec = new NoSpecificationReport();
        private SpecificationsOnControlReport _onControl = new SpecificationsOnControlReport();
        private SpecInVipiskReport _inVipisk = new SpecInVipiskReport();
        private CoopWorkReport _coopWork = new CoopWorkReport();
        private InProgressReport _inProgress = new InProgressReport();
        private DocumentationReport _doc = new DocumentationReport();
        private SKBCheckReport _skbCheck = new SKBCheckReport();
        private OETSStoreageReport _oetsStorage = new OETSStoreageReport();
        private GivingStorageReport _givingStorage = new GivingStorageReport();
        private GivingReportsReport _givingReports = new GivingReportsReport();
        private ActFormReport _actFormReport = new ActFormReport();
        private ByActReport _byActReport = new ByActReport();
        private FirstPaymentReport _firstPaymentReport = new FirstPaymentReport();
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
        #region ShowNoSpecCommand

        public ICommand ShowNoSpecCommand { get; }

        private bool CanShowNoSpecCommandExecute(object p) => true;

        private void OnShowNoSpecCommandExecuted(object p)
        {
            Content = _noSpec;
            
        }

        #endregion

        #region ShowSpecOnControlCommand

        public ICommand ShowSpecOnControlCommand { get; }

        private bool CanShowSpecOnControlCommandExecute(object p) => true;

        private void OnShowSpecOnControlCommandExecuted(object p)
        {
            Content = _onControl;
            
        }

        #endregion
        #region ShowSpecOnVipiskCommand

        public ICommand ShowSpecOnVipiskCommand { get; }

        private bool CanShowSpecOnVipiskCommandExecute(object p) => true;

        private void OnShowSpecOnVipiskCommandExecuted(object p)
        {
            Content = _inVipisk;
        }

        #endregion
        #region ShowCoopWorkCommand

        public ICommand ShowCoopWorkCommand { get; }

        private bool CanShowCoopWorkCommandExecute(object p) => true;

        private void OnShowCoopWorkCommandExecuted(object p)
        {
            Content = _coopWork;
        }

        #endregion

        #region ShowInProgressCommand

        public ICommand ShowInProgressCommand { get; }

        private bool CanShowInProgressCommandExecute(object p) => true;

        private void OnShowInProgressCommandExecuted(object p)
        {
            Content = _inProgress;
        }

        #endregion
        #region ShowDocumentationCommand

        public ICommand ShowDocumentationCommand { get; }

        private bool CanShowDocumentationCommandExecute(object p) => true;

        private void OnShowDocumentationCommandExecuted(object p)
        {
            Content = _doc;
        }

        #endregion

        #region ShowSKBCheckCommand

        public ICommand ShowSKBCheckCommand { get; }

        private bool CanShowSKBCheckCommandExecute(object p) => true;

        private void OnShowSKBCheckCommandExecuted(object p)
        {
            Content = _skbCheck;
        }

        #endregion

        #region ShowOETSStorageCommand

        public ICommand ShowOETSStorageCommand { get; }

        private bool CanShowOETSStorageCommandExecute(object p) => true;

        private void OnShowOETSStorageCommandExecuted(object p)
        {
            Content = _oetsStorage;
        }

        #endregion

        #region ShowGivingStorageCommand

        public ICommand ShowGivingStorageCommand { get; }

        private bool CanShowGivingStorageCommandExecute(object p) => true;

        private void OnShowGivingStorageCommandExecuted(object p)
        {
            Content = _givingStorage;
        }

        #endregion

        #region ShowGivingReportsCommand

        public ICommand ShowGivingReportsCommand { get; }

        private bool CanShowGivingReportsCommandExecute(object p) => true;

        private void OnShowGivingReportsCommandExecuted(object p)
        {
            Content = _givingReports;
        }

        #endregion

        #region ShowActFormCommand

        public ICommand ShowActFormCommand { get; }

        private bool CanShowActFormCommandExecute(object p) => true;

        private void OnShowActFormCommandExecuted(object p)
        {
            Content = _actFormReport;
        }

        #endregion

        #region ShowByActCommand

        public ICommand ShowByActCommand { get; }

        private bool CanShowByActCommandExecute(object p) => true;

        private void OnShowByActCommandExecuted(object p)
        {
            Content = _byActReport;
        }

        #endregion

        #region ShowFirstPaymentCommand

        public ICommand ShowFirstPaymentCommand { get; }

        private bool CanShowFirstPaymentCommandExecute(object p) => true;

        private void OnShowFirstPaymentCommandExecuted(object p)
        {
            Content = _firstPaymentReport;
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
            ShowNoSpecCommand = new LambdaCommand(OnShowNoSpecCommandExecuted, CanShowNoSpecCommandExecute);
            ShowSpecOnControlCommand = new LambdaCommand(OnShowSpecOnControlCommandExecuted, CanShowSpecOnControlCommandExecute);
            ShowSpecOnVipiskCommand = new LambdaCommand(OnShowSpecOnVipiskCommandExecuted, CanShowSpecOnVipiskCommandExecute);
            ShowCoopWorkCommand = new LambdaCommand(OnShowCoopWorkCommandExecuted, CanShowCoopWorkCommandExecute);
            ShowInProgressCommand = new LambdaCommand(OnShowInProgressCommandExecuted, CanShowInProgressCommandExecute);
            ShowDocumentationCommand = new LambdaCommand(OnShowDocumentationCommandExecuted, CanShowDocumentationCommandExecute);
            ShowSKBCheckCommand = new LambdaCommand(OnShowSKBCheckCommandExecuted, CanShowSKBCheckCommandExecute);
            ShowOETSStorageCommand = new LambdaCommand(OnShowOETSStorageCommandExecuted, CanShowOETSStorageCommandExecute);
            ShowGivingStorageCommand = new LambdaCommand(OnShowGivingStorageCommandExecuted, CanShowGivingStorageCommandExecute);
            ShowGivingReportsCommand = new LambdaCommand(OnShowGivingReportsCommandExecuted, CanShowGivingReportsCommandExecute);
            ShowActFormCommand = new LambdaCommand(OnShowActFormCommandExecuted, CanShowActFormCommandExecute);
            ShowByActCommand = new LambdaCommand(OnShowByActCommandExecuted, CanShowByActCommandExecute);
            ShowFirstPaymentCommand = new LambdaCommand(OnShowFirstPaymentCommandExecuted, CanShowFirstPaymentCommandExecute);
            #endregion

            #region Subscribe
            ((NoSpecVM)(_noSpec.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((SpecificationsOnControlVM)(_onControl.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((SpecificationsInVipiskVM)(_inVipisk.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((CoopWorkVM)(_coopWork.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((InProgressVM)(_inProgress.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((DocumentationVM)(_doc.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((SKBCkeckVM)(_skbCheck.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((OETSStorageVM)(_oetsStorage.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((GivingStorageVM)(_givingStorage.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((GivingReportsVM)(_givingReports.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((ActFormVM)(_actFormReport.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((ByActVM)(_byActReport.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            ((FirstPaymentVM)(_firstPaymentReport.DataContext)).SendTaskIdToReportVM += GetTaskIdFromReport;
            #endregion
        }
    }
}
