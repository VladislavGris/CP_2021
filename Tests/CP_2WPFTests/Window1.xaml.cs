using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace CP_2WPFTests
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class Window1 : Window
    {
        public Window1()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            AdventureWorksLT2008Entities advenWorksEntities = new AdventureWorksLT2008Entities();

            ObjectQuery<Customer> customers = advenWorksEntities.Customers;

            var query =
            from customer in customers
            orderby customer.CompanyName
            select new
            {
                customer.LastName,
                customer.FirstName,
                customer.CompanyName,
                customer.Title,
                customer.EmailAddress,
                customer.Phone,
                customer.SalesPerson
            };

            dataGrid1.ItemsSource = query.ToList();
        }
    }
}
