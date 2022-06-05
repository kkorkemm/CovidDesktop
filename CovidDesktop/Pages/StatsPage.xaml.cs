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
using System.Windows.Forms.DataVisualization.Charting;

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
            ComboPatientsTimeTypes.SelectedIndex = 0;

            Chart.ChartAreas.Add(new ChartArea("Main"));

            UpdateDiagram();
        }

        /// <summary>
        /// Обновление диаграмм при изменении параметров
        /// </summary>
        public void UpdateDiagram()
        {
            Chart.Series.Clear();

            /// ДИАГРАММА СООТНОШЕНИЕ ПОЛУЧЕННЫХ КОМПОНЕНТОВ
            if (ComboDiagramType.SelectedIndex == 0)
            {
                // настройка диаграммы
                var currentSeries = new Series("Компоненты") { IsValueShownAsLabel = true };
                Chart.Series.Add(currentSeries);
                currentSeries.Points.Clear();
                currentSeries.ChartType = SeriesChartType.Doughnut;

                // список типов компонентов (1 и 2)
                var componentTypes = AppData.GetContext().ComponentType.ToList();


                foreach(var componentType in componentTypes)
                {
                    // для конкретного пункта вакцинации
                    if (ComboVacPoints.SelectedIndex != 0)
                    {
                        var vaccinationPoint = ComboVacPoints.SelectedItem as VaccinationPoint;

                        /// если задан фильтр по датам
                        var timetables = vaccinationPoint.TimeTable.ToList();
                        if (DateFrom.SelectedDate != null)
                            timetables = timetables.Where(p => p.Date >= DateFrom.SelectedDate).ToList();
                        if (DateTo.SelectedDate != null)
                            timetables = timetables.Where(p => p.Date <= DateTo.SelectedDate).ToList().ToList();

                        int count = 0;
                        foreach (var timeTable in timetables)
                        {
                            foreach (var appointment in timeTable.Appointment)
                            {
                                if (appointment.ComponentTypeID == componentType.ID)
                                    count++;
                            }
                        }

                        currentSeries.Points.AddXY(componentType.Name,count);
                    }
                    // для всех пунктов вакцинации
                    else
                    {
                        var allPoints = AppData.GetContext().VaccinationPoint.ToList();
                        int count = 0;

                        foreach (var point in allPoints)
                        {
                            // если задан фильтр по датам
                            var timetables = point.TimeTable.ToList();
                            if (DateFrom.SelectedDate != null)
                                timetables = timetables.Where(p => p.Date >= DateFrom.SelectedDate).ToList();
                            if (DateTo.SelectedDate != null)
                                timetables = timetables.Where(p => p.Date <= DateTo.SelectedDate).ToList().ToList();

                            foreach (var timeTable in timetables)
                            {
                                foreach (var appointment in timeTable.Appointment)
                                {
                                    if (appointment.ComponentTypeID == componentType.ID)
                                        count++;
                                }
                            }
                        }

                        currentSeries.Points.AddXY(componentType.Name, count);
                    }
                }
            }
            /// ДИАГРАММА ЧАСТОТЫ ПОСЕЩЕНИЯ
            else
            {
                // настройка диаграммы
                var currentSeries = new Series("Частота записей \n на вакцинацию") { IsValueShownAsLabel = true };
                Chart.Series.Add(currentSeries);
                currentSeries.Points.Clear();
                currentSeries.ChartType = SeriesChartType.Column;

                var appoitments = AppData.GetContext().Appointment.ToList();

                /// для отдельного пункта вакцинации
                if (ComboVacPoints.SelectedIndex != 0)
                {
                    var point = ComboVacPoints.SelectedItem as VaccinationPoint;
                    appoitments = AppData.GetContext().Appointment.Where(p => p.TimeTable.VaccinationPointID == point.ID).ToList();    
                }

                // если установлены фильтры по дате
                if (ComboPatientsTimeTypes.SelectedIndex == 0)
                    appoitments = appoitments.Where(p => (p.TimeTable.Date - DateTime.Now).Days <= 7).ToList();
                if (ComboPatientsTimeTypes.SelectedIndex == 1)
                    appoitments = appoitments.Where(p => (p.TimeTable.Date - DateTime.Now).Days <= 31).ToList();
                if (ComboPatientsTimeTypes.SelectedIndex == 2)
                    appoitments = appoitments.Where(p => p.TimeTable.Date.Year - DateTime.Now.Year <= 1).ToList();

                var dates = appoitments.Select(p => p.TimeTable.Date).Distinct().ToList();


                foreach (var date in dates)
                {
                    int count = 0;

                    foreach (var appointment in appoitments)
                    {
                        if (appointment.TimeTable.Date == date)
                            count++;
                    }

                    currentSeries.Points.AddXY(date.Date, count);
                }
            }
        }

        /// <summary>
        /// При выборе пункта вакцинации (для админа)
        /// </summary>
        private void ComboVacPoints_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDiagram();
        }

        /// <summary>
        /// При выборе диапазона дат для первой диаграммы
        /// </summary>
        private void DateFrom_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDiagram();
        }

        /// <summary>
        /// При выборе варианта диапазона дат для второй диаграммы
        /// </summary>
        private void ComboPatientsTimeTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDiagram();
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

            UpdateDiagram();
        }
    }
}
