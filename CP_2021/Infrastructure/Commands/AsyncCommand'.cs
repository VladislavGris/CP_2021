﻿using CP_2021.Infrastructure.Commands.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.Infrastructure.Commands
{
    abstract class AsyncCommand<T> : IAsyncCommand<T>
    {
        private readonly ObservableCollection<Task> _runningTasks;

        public IEnumerable<Task> RunningTasks 
        {
            get => _runningTasks;
        }

        public event EventHandler CanExecuteChanged
        {
            add => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        protected AsyncCommand()
        {
            _runningTasks = new ObservableCollection<Task>();
            _runningTasks.CollectionChanged += OnRunningTasksChanged;
        }

        bool ICommand.CanExecute(object parameter)
        {
            return CanExecute((T)parameter);
        }

        async void ICommand.Execute(object parameter)
        {
            Task runningTask = ExecuteAsync((T)parameter);

            _runningTasks.Add(runningTask);

            try
            {
                await runningTask;
            }
            finally
            {
                _runningTasks.Remove(runningTask);
            }
        }

        public abstract bool CanExecute(T parameter);
        public abstract Task ExecuteAsync(T parameter);

        private void OnRunningTasksChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CommandManager.InvalidateRequerySuggested();
        }
    }
}
