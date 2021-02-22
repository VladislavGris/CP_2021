using CP_2021.ViewModels.Base;
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
    }
}
