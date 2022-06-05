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
    /// Логика взаимодействия для TimeTablePage.xaml
    /// </summary>
    public partial class TimeTablePage : Page
    {
        public TimeTablePage()
        {
            InitializeComponent();

            // Типы компонентов
            var componentTypes = AppData.GetContext().ComponentType.ToList();
            componentTypes.Insert(0, new ComponentType { Name = "Все" });
            ComboComponentTypes.ItemsSource = componentTypes;

            UpdateTimeTable();
        }

        /// <summary>
        /// Вывод расписания
        /// </summary>
        private void UpdateTimeTable()
        {
            // Расписание
            var timetable = AppData.GetContext().TimeTable.Where(p => p.VaccinationPointID == AppData.CurrentUser.ID).OrderBy(p => p.Date).ThenBy(p => p.Time).ToList();

            // при использовании фильтров
            if (ComboComponentTypes.SelectedIndex != 0)
                timetable = timetable.Where(p => p.ComponentType == (ComboComponentTypes.SelectedItem as ComponentType).Name).ToList();
            if (DateFrom.SelectedDate != null && DateTo.SelectedDate != null)
                timetable = timetable.Where(p => p.Date >= DateFrom.SelectedDate && p.Date <= DateTo.SelectedDate).ToList();
            if (DateFrom.SelectedDate != null)
                timetable = timetable.Where(p => p.Date >= DateFrom.SelectedDate).ToList();
            if (DateTo.SelectedDate != null)
                timetable = timetable.Where(p => p.Date <= DateTo.SelectedDate).ToList();
            if (DateFrom.SelectedDate == null && DateTo.SelectedDate == null)
                timetable = timetable.Where(p => p.Date >= DateTime.Now).ToList();

            ListTimeTable.ItemsSource = timetable;

            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(ListTimeTable.ItemsSource);
            PropertyGroupDescription groupDescription = new PropertyGroupDescription("Date");
            view.GroupDescriptions.Add(groupDescription);
        }

        /// <summary>
        /// Использование фильтров
        /// </summary>
        private void DateTo_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateTimeTable();
        }

        /// <summary>
        /// Добавления элемента расписания
        /// </summary>
        private void BtnAddTimeTable_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddTimeTableWindow addWindow = new Windows.AddTimeTableWindow();
            addWindow.ShowDialog();

            UpdateTimeTable();
        }

        private void ListTimeTable_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var item = ItemsControl.ContainerFromElement(ListTimeTable, e.OriginalSource as DependencyObject) as ListViewItem;

            if (item != null)
            {
                Navigation.SubFrame.Navigate(new TimeTableDetailedPage(item.DataContext as TimeTable));
            }
        }
    }
}
