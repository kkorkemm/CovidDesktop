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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CovidDesktop.Pages
{
    using Base;

    /// <summary>
    /// Логика взаимодействия для MainPage.xaml
    /// </summary>
    public partial class MainPage : Page
    {
        public MainPage()
        {
            InitializeComponent();
            Title = AppData.CurrentUser.Name;

            if (AppData.CurrentUser.UserRoleID == 1)
            {
                BtnAppointment.Visibility = Visibility.Collapsed;
                BtnTimeTable.Visibility = Visibility.Collapsed;
                BtnStats.IsChecked = true;
            }
        }

        /// <summary>
        /// Выход из системы
        /// </summary>
        private void BtnExit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы уверены, что хотите выйти из системы?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                AppData.CurrentUser = null;
                Navigation.MainFrame.Navigate(new LoginPage());
            }
        }
    }
}
