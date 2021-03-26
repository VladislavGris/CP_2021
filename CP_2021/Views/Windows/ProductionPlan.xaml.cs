using System.Windows;
using System.Windows.Controls;

namespace CP_2WPFTests
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            ButtonOpenMenu.Visibility = Visibility.Visible;
        }

        private void ListViewMenu_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                //case "ItemHome":
                //    this.MainGrid.Content = new UserControlHome();
                //    break;
                //case "ItemCreate":
                //    this.MainGrid.Content = new UserControlCreate();
                //    break;
                default:
                    break;
            }
        }
    }
}

