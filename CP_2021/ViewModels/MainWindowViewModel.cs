using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels.Base;
using System.Windows;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        #region Title
        private string _Title = "Main Window";

        /// <summary>Заголовок окна</summary>
        public string Title
        {
            get => _Title;
            set => Set(ref _Title, value);
        }
        #endregion

        #region Команды

        #region CloseApplicationCommand

        public ICommand CloseApplicationCommand { get; }

        private void OnCloseApplicationCommandExecuted(object p)
        {
            Application.Current.Shutdown();
        }

        private bool CanCloseApplicationCommandExecute(object p) => true;

        #endregion

        #endregion

        public MainWindowViewModel()
        {

            #region Команды

            CloseApplicationCommand = new LambdaCommand(OnCloseApplicationCommandExecuted, CanCloseApplicationCommandExecute);

            #endregion

        }
    }
}
