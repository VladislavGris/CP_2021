using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class AdministrationPannelViewModel : ViewModelBase
    {

        #region Свойства

        private ApplicationUnit Unit;

        #region User

        private UserDB _user;

        public UserDB User
        {
            get => _user;
            set => Set(ref _user, value);
        }

        #endregion

        #region SelectedItem

        private UserDB _selectedItem;

        public UserDB SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        #endregion

        #region Users

        private ObservableCollection<UserDB> _users;

        public ObservableCollection<UserDB> Users
        {
            get => _users;
            set => Set(ref _users, value);
        }

        #endregion

        #endregion

        #region Команды

        #region DownPositionCommand

        public ICommand DownPositionCommand { get; }

        private bool CanDownPositionCommandExecute(object p) => SelectedItem.Position!=0 && SelectedItem.Position!=2;

        private void OnDownPositionCommandExecuted(object p)
        {
            SelectedItem.Position--;
            Unit.Commit();
            Users = new ObservableCollection<UserDB>(Unit.DBUsers.Get());
        }

        #endregion

        #region RisePositionCommand

        public ICommand RisePositionCommand { get; }

        private bool CanRisePositionCommandExecute(object p) => SelectedItem.Position != 1 && SelectedItem.Position != 2;

        private void OnRisePositionCommandExecuted(object p)
        {
            SelectedItem.Position++;
            Unit.Commit();
            Users = new ObservableCollection<UserDB>(Unit.DBUsers.Get());
        }

        #endregion

        #endregion

        public AdministrationPannelViewModel() 
        {
            #region Команды

            DownPositionCommand = new LambdaCommand(OnDownPositionCommandExecuted, CanDownPositionCommandExecute);
            RisePositionCommand = new LambdaCommand(OnRisePositionCommandExecuted, CanRisePositionCommandExecute);

            #endregion

            Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            User = UserDataSingleton.GetInstance().user;
            Users = new ObservableCollection<UserDB>(Unit.DBUsers.Get());
        }
    }
}
