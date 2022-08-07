using System.Windows;

namespace CP_2021.Views.Windows
{
    public partial class SearchWindow : Window
    {
        public SearchWindow()
        {
            InitializeComponent();
            SearchTextBox.Focus();
        }
    }
}
