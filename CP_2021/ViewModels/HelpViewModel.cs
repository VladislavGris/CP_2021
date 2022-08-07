using CP_2021.Infrastructure.Commands;
using CP_2021.ViewModels.Base;
using System;
using System.IO;
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

        private static readonly string _basePath = "Data/doc/";
        private readonly string _searchDescriptionPath = $"{_basePath}SearchDescriptions.xaml";
        private readonly string _actCreation = $"{_basePath}ActCreation.xaml";
        private readonly string _coopWorkReport = $"{_basePath}CoopWorkReport.xaml";
        private readonly string _documInWorkReport = $"{_basePath}DocumInWorkReport.xaml";
        private readonly string _inWorkReport = $"{_basePath}InWorkReport.xaml";
        private readonly string _noSpecReport = $"{_basePath}NoSpecReport.xaml";
        private readonly string _oECStorage = $"{_basePath}OECStorage.xaml";
        private readonly string _sKBCheck = $"{_basePath}SKBCheck.xaml";
        private readonly string _timedGivingReport = $"{_basePath}TimedGivingReport.xaml";
        private readonly string _workedDocsReport = $"{_basePath}WorkedDocsReport.xaml";
        private readonly string _vkOnStorageReport = $"{_basePath}VKOnStorage.xaml";

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
                case "ActCreation":
                    LoadDocument(_actCreation);
                    break;
                case "CoopWorkReport":
                    LoadDocument(_coopWorkReport);
                    break;
                case "DocumInWorkReport":
                    LoadDocument(_documInWorkReport);
                    break;
                case "InWorkReport":
                    LoadDocument(_inWorkReport);
                    break;
                case "NoSpecReport":
                    LoadDocument(_noSpecReport);
                    break;
                case "OECStorage":
                    LoadDocument(_oECStorage);
                    break;
                case "SKBCheck":
                    LoadDocument(_sKBCheck);
                    break;
                case "TimedGivingReport":
                    LoadDocument(_timedGivingReport);
                    break;
                case "WorkedDocsReport":
                    LoadDocument(_workedDocsReport);
                    break;
                case "VKOnStorage":
                    LoadDocument(_vkOnStorageReport);
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
            catch(FileNotFoundException /*ex*/)
            {
                MessageBox.Show("Ошибка загрузки документа");
            }
            catch(Exception ex)
            {
                MessageBox.Show($"Ошибка при открытии файла: {ex.Message}");
            }
            
        }

        #endregion

        public HelpViewModel()
        {
            #region Команды

            ChangeContentCommand = new LambdaCommand(OnChangeContentCommandExecuted, CanChangeContentCommandExecute);

            #endregion

            DocumentContent = new FlowDocumentScrollViewer();
            LoadDocument(_searchDescriptionPath);
        }
    }
}
