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
    /// Логика взаимодействия для StatsPage.xaml
    /// </summary>
    public partial class StatsPage : Page
    {
        public StatsPage()
        {
            InitializeComponent();

            var points = AppData.GetContext().VaccinationPoint.Where(p => p.UserRoleID != 1).ToList();
            points.Insert(0, new VaccinationPoint { Name = "Все" });
            ComboVacPoints.ItemsSource = points;
            ComboVacPoints.SelectedIndex = 0;

            if (AppData.CurrentUser.UserRoleID != 1)
            {
                var currentPoint = points.Where(p => p.ID == AppData.CurrentUser.ID).FirstOrDefault();
                ComboVacPoints.SelectedItem = currentPoint;
                ComboVacPoints.Visibility = Visibility.Collapsed;
            }

            ComboDiagramType.SelectedIndex = 0;
        }

        public void UpdateDiagram()
        {

        }

        /// <summary>
        /// При выборе пункта вакцинации (для админа)
        /// </summary>
        private void ComboVacPoints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        /// <summary>
        /// При выборе типа диаграммы
        /// </summary>
        private void ComboDiagramType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ComboDiagramType.SelectedIndex == 0)
            {
                DateFrom.Visibility = Visibility.Visible;
                DateTo.Visibility = Visibility.Visible;
                ComboPatientsTimeTypes.Visibility = Visibility.Collapsed;
            }
            else
            {
                DateFrom.Visibility = Visibility.Collapsed;
                DateTo.Visibility = Visibility.Collapsed;
                ComboPatientsTimeTypes.Visibility = Visibility.Visible;
            }
        }
    }
}
