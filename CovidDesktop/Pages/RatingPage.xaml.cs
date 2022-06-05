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
    /// Логика взаимодействия для RatingPage.xaml
    /// </summary>
    public partial class RatingPage : Page
    {
        public RatingPage()
        {
            InitializeComponent();

            // Список пунктов вакцинации
            var points = AppData.GetContext().VaccinationPoint.Where(p => p.UserRoleID != 1).ToList();
            ComboVacPoints.ItemsSource = points;
            ComboVacPoints.SelectedIndex = 0;

            if (AppData.CurrentUser.UserRoleID != 1)
            {
                var point = points.Where(p => p.ID == AppData.CurrentUser.ID).FirstOrDefault();
                ComboVacPoints.SelectedItem = point;
                ComboVacPoints.Visibility = Visibility.Collapsed;
            }

            UpdateInfo();
        }

        private void UpdateInfo()
        {
            var point = ComboVacPoints.SelectedItem as VaccinationPoint;

            TextReviewCount.Text = point.Review.Count().ToString();
            TextReviewAverage.Text = Math.Round(point.Review.Average(p => p.Rating), 2).ToString();

            ListReviews.ItemsSource = point.Review.ToList();
        }

        private void ComboVacPoints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateInfo();
        }
    }
}
