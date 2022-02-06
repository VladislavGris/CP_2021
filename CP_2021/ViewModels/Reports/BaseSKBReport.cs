using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Utils.DB;
using CP_2021.Models.ViewEntities;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace CP_2021.ViewModels.Reports
{
    internal abstract class BaseSKBReport<T> : BaseReport<T> where T :BaseSKBViewEntity
    {
        #region SKBNumbers

        private ObservableCollection<string> _skbNumbers;

        public ObservableCollection<string> SKBNumbers
        {
            get => _skbNumbers;
            set => Set(ref _skbNumbers, value);
        }

        #endregion

        #region SelectedNumber

        private string _selectedNumber;

        public string SelectedNumber
        {
            get => _selectedNumber;
            set => Set(ref _selectedNumber, value);
        }

        #endregion

        protected override void OnGenerateReportCommandExecuted(object p)
        {
            base.OnGenerateReportCommandExecuted(p);
            SKBNumbers = new ObservableCollection<string>();
            var numbers = TasksOperations.GetSKBNumbers();
            foreach (var number in numbers)
            {
                SKBNumbers.Add(number.Number);
            }
        }

        #region SKBNumberChangedCommand

        public ICommand SKBNumberChangedCommand { get; }

        private bool CanSKBNumberChangedCommandExecute(object p) => true;

        private void OnSKBNumberChangedCommandExecuted(object p)
        {
            Content = new ObservableCollection<T>(FullContent.Where(t => t.SKBNumber == SelectedNumber));
            if (!String.IsNullOrEmpty(SelectedManufacture))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.Manufacturer == SelectedManufacture));
            }
            if (!String.IsNullOrEmpty(SelectedHead))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.Project == SelectedHead));
            }
        }

        #endregion

        protected override void OnManufactureChangedCommandExecuted(object p)
        {
            Content = new ObservableCollection<T>(FullContent.Where(t => t.Manufacturer == SelectedManufacture));
            if (!String.IsNullOrEmpty(SelectedHead))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.Project == SelectedHead));
            }
            if (!String.IsNullOrEmpty(SelectedNumber))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.SKBNumber == SelectedNumber));
            }
        }

        protected override void OnProjectChangedCommandExecuted(object p)
        {
            Content = new ObservableCollection<T>(FullContent.Where(t => t.Project == SelectedHead));
            if (!String.IsNullOrEmpty(SelectedManufacture))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.Manufacturer == SelectedManufacture));
            }
            if (!String.IsNullOrEmpty(SelectedNumber))
            {
                Content = new ObservableCollection<T>(Content.Where(t => t.SKBNumber == SelectedNumber));
            }
        }

        protected override void OnDropFiltersCommandExecuted(object p)
        {
            SelectedHead = null;
            SelectedManufacture = null;
            SelectedNumber = null;
            Content = FullContent;
        }

        public BaseSKBReport() : base()
        {
            SKBNumberChangedCommand = new LambdaCommand(OnSKBNumberChangedCommandExecuted, CanSKBNumberChangedCommandExecute);
        }
    }
}
