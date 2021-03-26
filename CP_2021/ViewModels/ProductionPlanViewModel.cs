using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels.Base;
using System.Windows.Input;
using System.ComponentModel;
using System.Windows;

namespace CP_2021.ViewModels
{
    internal class ProductionPlanViewModel : ViewModelBase
    {
        #region Свойства

        #region ButtonCloseMenuVisibility

        private Visibility _ButtonCloseMenuVisibility = Visibility.Collapsed;

        public Visibility ButtonCloseMenuVisibility
        {
            get => _ButtonCloseMenuVisibility;
            set
            {
                _ButtonCloseMenuVisibility = value;
                OnPropertyChanged("ButtonCloseMenuVisibility");
            }
        }

        #endregion

        #region ButtonOpenMenuVisibility

        private Visibility _ButtonOpenMenuVisibility = Visibility.Visible;

        public Visibility ButtonOpenMenuVisibility
        {
            get => _ButtonOpenMenuVisibility;
            set
            {
                _ButtonOpenMenuVisibility = value;
                OnPropertyChanged("ButtonOpenMenuVisibility");
            }
        }

        #endregion

        #region Title
        private string _Title = "Production Plan";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #endregion

        #region Команды

        #region ButtonCloseMenuCommand

        public ICommand ButtonCloseMenuCommand { get; }

        private bool CanButtonCloseMenuCommandExecute(object p) => true;

        private void OnButtonCloseMenuCommandExecuted(object p)
        {
            ButtonOpenMenuVisibility = Visibility.Visible;
            ButtonCloseMenuVisibility = Visibility.Collapsed;
        }

        #endregion

        #region ButtonOpenMenuCommand

        public ICommand ButtonOpenMenuCommand { get; }

        private bool CanButtonOpenMenuCommandExecute(object p) => true;

        private void OnButtonOpenMenuCommandExecuted(object p)
        {
            ButtonOpenMenuVisibility = Visibility.Collapsed;
            ButtonCloseMenuVisibility = Visibility.Visible;
        }

        #endregion

        #endregion

        public ProductionPlanViewModel()
        {
            #region Команды

            ButtonCloseMenuCommand = new LambdaCommand(OnButtonCloseMenuCommandExecuted, CanButtonCloseMenuCommandExecute);
            ButtonOpenMenuCommand = new LambdaCommand(OnButtonOpenMenuCommandExecuted, CanButtonOpenMenuCommandExecute);

            #endregion

        }
    }
}
