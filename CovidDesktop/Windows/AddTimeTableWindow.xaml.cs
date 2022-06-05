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

namespace CovidDesktop.Windows
{
    using Base;

    /// <summary>
    /// Логика взаимодействия для AddTimeTableWindow.xaml
    /// </summary>
    public partial class AddTimeTableWindow : Window
    {
        private TimeTable timeTable = new TimeTable();

        public AddTimeTableWindow()
        {
            InitializeComponent();

            ComboDoctor.ItemsSource = AppData.GetContext().Doctor.ToList();
            TheDatePicker.DisplayDate = DateTime.Now;

            DataContext = timeTable;
            timeTable.VaccinationPointID = AppData.CurrentUser.ID;
        }

        private void BtnAddTimeTable_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (ComboDoctor.SelectedItem == null)
                errors.AppendLine("Укажите принимающего врача");
            if (TheDatePicker.SelectedDate == null)
                errors.AppendLine("Укажите дату");
            if (TheTimePicker.SelectedTime == null)
                errors.AppendLine("Укажите время");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            timeTable.Date = (DateTime)TheDatePicker.SelectedDate;
            timeTable.Time = TheTimePicker.SelectedTime.Value.TimeOfDay;

            try
            {
                AppData.GetContext().TimeTable.Add(timeTable);
                AppData.GetContext().SaveChanges();

                MessageBox.Show("Расписание успешно обновлено!");
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
