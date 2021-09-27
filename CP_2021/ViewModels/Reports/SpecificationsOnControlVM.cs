﻿using CP_2021.Infrastructure.Commands;
using CP_2021.Infrastructure.Singletons;
using CP_2021.Infrastructure.Utils.CustomEventArgs;
using CP_2021.Models.ViewEntities;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CP_2021.ViewModels.Reports
{
    class SpecificationsOnControlVM : BaseReport<SpecificationsOnControl>
    {

        #region GenerateReportCommand

        public ICommand GenerateReportCommand { get; }

        private bool CanGenerateReportCommandExecute(object p) => true;

        private void OnGenerateReportCommandExecuted(object p)
        {
            var heads = ApplicationUnitSingleton.GetInstance().dbUnit.HeadTasks.Get().OrderBy(t => t.Task);
            HeadTasks = new ObservableCollection<string>();
            foreach (var head in heads)
            {
                HeadTasks.Add(head.Task);
            }
            FullContent = Content = new ObservableCollection<SpecificationsOnControl>(ApplicationUnitSingleton.GetInstance().dbUnit.OnControl.Get());
        }

        #endregion

        public SpecificationsOnControlVM() : base()
        {
            GenerateReportCommand = new LambdaCommand(OnGenerateReportCommandExecuted, CanGenerateReportCommandExecute);
        }
    }
}