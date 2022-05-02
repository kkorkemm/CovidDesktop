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
    /// Логика взаимодействия для LoginPage.xaml
    /// </summary>
    public partial class LoginPage : Page
    {
        public LoginPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Отмена (Очистка полей)
        /// </summary>
        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            LoginText.Text = "";
            PasswordText.Password = "";
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            // Проверка на заполнение полей
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(LoginText.Text))
                errors.AppendLine("Введите логин");
            if (string.IsNullOrWhiteSpace(PasswordText.Password))
                errors.AppendLine("Введите пароль");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Поиск пользователя
            var user = AppData.GetContext().VaccinationPoint.Where(p => p.Login == LoginText.Text && p.Password == PasswordText.Password).FirstOrDefault();

            // Проверка на существование пользователя
            if (user == null)
            {
                MessageBox.Show("Пользователь не найден", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            AppData.CurrentUser = user;
            Navigation.MainFrame.Navigate(new MainPage());
        }
    }
}
