﻿using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Units;
using CP_2021.Models.DBModels;
using CP_2021.ViewModels.Base;
using CP_2021.Views.Windows;
using Notifications.Wpf;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.ViewModels
{
    class MyTasksViewModel : ViewModelBase
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

        #region Tasks

        private ObservableCollection<TaskDB> _tasks;

        public ObservableCollection<TaskDB> Tasks
        {
            get => _tasks;
            set => Set(ref _tasks, value);
        }

        #endregion

        #region FilterSelection

        private int _filterSelection = 0;

        public int FilterSelection
        {
            get => _filterSelection;
            set => Set(ref _filterSelection, value);
        }

        #endregion

        #endregion

        #region Команды

        #region OpenSendReportWindowCommand

        public ICommand OpenSendReportWindowCommand { get; }

        private bool CanOpenSendReportWindowCommandExecute(object p) => true;

        private void OnOpenSendReportWindowCommandExecuted(object p)
        {
            SendReportWindow window = new SendReportWindow();
            window.DataContext = new SendReportViewModel((ReportDB)p, this);
            window.Show();
        }

        #endregion

        #region OpenShowReportWindowCommand

        public ICommand OpenShowReportWindowCommand { get; }

        private bool CanOpenShowReportWindowCommandExecute(object p) => true;

        private void OnOpenShowReportWindowCommandExecuted(object p)
        {
            ViewReportWindow window = new ViewReportWindow();
            window.DataContext = new ViewReportViewModel((ReportDB)p);
            window.Show();
        }

        #endregion

        #region RefreshCommand

        public ICommand RefreshCommand { get; }

        private bool CanRefreshCommandExecute(object p) => true;

        private void OnRefreshCommandExecuted(object p)
        {
            Unit.Refresh();
            var notificationManager = new NotificationManager();
            switch (FilterSelection)
            {
                case 0:
                    if(Tasks.ToList().Count < Unit.UserTasks.Get().Where(t => t.To.Equals(User)).ToList().Count)
                    {
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Задачи",
                            Message = "У вас появились новые задачи",
                            Type = NotificationType.Information
                        });
                    }
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.To.Equals(User)));
                    break;
                case 1:
                    if (Tasks.ToList().Count < Unit.UserTasks.Get().Where(t => t.To.Equals(User) && t.Report.State == true).ToList().Count)
                    {
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Задачи",
                            Message = "У вас появились новые задачи",
                            Type = NotificationType.Information
                        });
                    }
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.To.Equals(User) && t.Report.State == true));
                    break;
                case 2:
                    if (Tasks.ToList().Count < Unit.UserTasks.Get().Where(t => t.To.Equals(User) && t.Report.State == false).ToList().Count)
                    {
                        notificationManager.Show(new NotificationContent
                        {
                            Title = "Задачи",
                            Message = "У вас появились новые задачи",
                            Type = NotificationType.Information
                        });
                    }
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.To.Equals(User) && t.Report.State == false));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region FilterSelectionChanged

        public ICommand FilterSelectionChanged { get; }

        private bool CanFilterSelectionChangedExecute(object p) => true;

        private void OnFilterSelectionChangedExecuted(object p)
        {
            Unit.Refresh();
            switch (FilterSelection)
            {
                case 0:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.To.Equals(User)));
                    break;
                case 1:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.To.Equals(User) && t.Report.State == true));
                    break;
                case 2:
                    Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(t => t.To.Equals(User) && t.Report.State == false));
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        public MyTasksViewModel() 
        {
            #region Команды

            OpenSendReportWindowCommand = new LambdaCommand(OnOpenSendReportWindowCommandExecuted, CanOpenSendReportWindowCommandExecute);
            OpenShowReportWindowCommand = new LambdaCommand(OnOpenShowReportWindowCommandExecuted, CanOpenShowReportWindowCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecuted, CanRefreshCommandExecute);
            FilterSelectionChanged = new LambdaCommand(OnFilterSelectionChangedExecuted, CanFilterSelectionChangedExecute);

            #endregion

            Unit = ApplicationUnitSingleton.GetInstance().dbUnit;
            User = UserDataSingleton.GetInstance().user;
            Tasks = new ObservableCollection<TaskDB>(Unit.UserTasks.Get().Where(u => u.To.Equals(User)));
        }

        public MyTasksViewModel(ApplicationUnit unit, UserDB user)
        {
            #region Команды

            OpenSendReportWindowCommand = new LambdaCommand(OnOpenSendReportWindowCommandExecuted, CanOpenSendReportWindowCommandExecute);
            OpenShowReportWindowCommand = new LambdaCommand(OnOpenShowReportWindowCommandExecuted, CanOpenShowReportWindowCommandExecute);
            RefreshCommand = new LambdaCommand(OnRefreshCommandExecuted, CanRefreshCommandExecute);
            FilterSelectionChanged = new LambdaCommand(OnFilterSelectionChangedExecuted, CanFilterSelectionChangedExecute);

            #endregion

            Unit = unit;
            User = user;
            Tasks = new ObservableCollection<TaskDB>(unit.UserTasks.Get().Where(u => u.To.Equals(User)));
        }
    }
}
