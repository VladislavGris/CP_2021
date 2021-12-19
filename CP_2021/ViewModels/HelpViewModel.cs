using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;

namespace CP_2021.ViewModels
{
    internal class HelpViewModel : ViewModelBase
    {
        #region Свойства

        private static string _basePath = "Data/doc/";
        private string _noSpecNumberPath =  $"{_basePath}NoSpecificationNumber.xaml";
        private string _searchDescriptionPath = $"{_basePath}SearchDescriptions.xaml";
        private string _nalichieNaSkladePath = $"{_basePath}NalichieNaSklade.xaml";

        #region ContentContainerContent

        private FlowDocumentScrollViewer _documentContent;

        public FlowDocumentScrollViewer DocumentContent
        {
            get => _documentContent;
            set => Set(ref _documentContent, value);
        }

        #endregion

        #endregion

        #region Команды

        #region ChangeContentCommand

        public ICommand ChangeContentCommand { get; }

        private bool CanChangeContentCommandExecute(object p) => true;

        private void OnChangeContentCommandExecuted(object p)
        {
            var list = (ListViewItem)p;
            switch (list.Name)
            {
                case "SearchDescriptions":
                    LoadDocument(_searchDescriptionPath);
                    break;
                case "NoSpecNum":
                    LoadDocument(_noSpecNumberPath);
                    break;
                case "NalichieNaSklade":
                    LoadDocument(_nalichieNaSkladePath);
                    break;
                case "PredostavlenieDavalchOtch":
                    break;
                case "SpecifNaControle":
                    break;
                case "SpecifKVipisk":
                    break;
                case "VRabote":
                    break;
                case "OtrabotkDoc":
                    break;
                case "FormirovanieActov":
                    break;
                case "ProverkaSKB":
                    break;
                case "SkladO":
                    break;
                case "VRabotePoIsp":
                    break;
                case "GotovPoIsp":
                    break;
                case "PlanProizv":
                    break;
                case "IzdZaPeriod":
                    break;
                default:
                    break;
            }
        }

        #endregion

        #endregion

        #region Методы

        private void LoadDocument(string path)
        {
            try
            {
                using (FileStream fs = File.Open(path, FileMode.Open))
                {
                    FlowDocument doc = XamlReader.Load(fs) as FlowDocument;
                    if (doc != null)
                    {
                        DocumentContent.Document = doc;
                    }
                }
            }
            catch(FileNotFoundException ex)
            {
                MessageBox.Show("Ошибка загрузки документа");
            }
            
        }

        #endregion

        public HelpViewModel()
        {
            #region Команды

            ChangeContentCommand = new LambdaCommand(OnChangeContentCommandExecuted, CanChangeContentCommandExecute);

            #endregion

            DocumentContent = new FlowDocumentScrollViewer();
            using(FileStream fs = File.Open(_noSpecNumberPath, FileMode.Open))
            {
                FlowDocument doc = XamlReader.Load(fs) as FlowDocument;
                if(doc != null)
                {
                    DocumentContent.Document = doc;
                }
            }
        }
    }
}
