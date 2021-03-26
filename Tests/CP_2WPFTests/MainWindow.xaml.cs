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
            UserControl usc = null;
            GridMain.Children.Clear();

            switch (((ListViewItem)((ListView)sender).SelectedItem).Name)
            {
                //case "ItemHome":
                //    usc = new UserControlHome();
                //    GridMain.Children.Add(usc);
                //    break;
                //case "ItemCreate":
                //    usc = new UserControlCreate();
                //    GridMain.Children.Add(usc);
                //    break;
                default:
                    break;
            }
        }
    }
}

