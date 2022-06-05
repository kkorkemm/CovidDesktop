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
    /// Логика взаимодействия для TimeTableDetailedPage.xaml
    /// </summary>
    public partial class TimeTableDetailedPage : Page
    {
        private TimeTable CurrentTimeTable = new TimeTable();

        public TimeTableDetailedPage(TimeTable timeTable)
        {
            InitializeComponent();

            if (timeTable != null)
            {
                CurrentTimeTable = timeTable;
            }

            DataContext = CurrentTimeTable;

            ComboDoctors.ItemsSource = AppData.GetContext().Doctor.ToList();

            UpdatePatientTable();

            if (CurrentTimeTable.Appointment.Count() == 5)
            {
                BtnAddPatient.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// Обновление таблицы пациентов
        /// </summary>
        private void UpdatePatientTable()
        {
            var patients = CurrentTimeTable.Appointment.ToList();
            GridPatients.ItemsSource = patients;
        }

        /// <summary>
        /// Отметка о получении вакцины отдельного пациента
        /// </summary>
        private void BtnReceived_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы подтверждаете получение пациентом компонента вакцины?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                var patient = (sender as Button).DataContext as Appointment;
                patient.IsReceived = true;

                try
                {
                    AppData.GetContext().SaveChanges();

                    MessageBox.Show("Информация успешно сохранена!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

            UpdatePatientTable();
        }

        /// <summary>
        /// Добавление пациента
        /// </summary>
        private void BtnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddPatientWindow addPatientWindow = new Windows.AddPatientWindow(CurrentTimeTable);
            addPatientWindow.ShowDialog();
            UpdatePatientTable();
        }

        /// <summary>
        /// Отменить запись пациента
        /// </summary>
        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Удалить запись пациента?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                var patient = (sender as Button).DataContext as Appointment;

                try
                {
                    AppData.GetContext().Appointment.Remove(patient);
                    AppData.GetContext().SaveChanges();

                    MessageBox.Show("Запись успешно удалена!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);

                    UpdatePatientTable();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// Просмотр анкеты отдельного пациента
        /// </summary>
        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = ((sender as Button).DataContext as Appointment).Patient;
            Windows.PatientDetailedWindow patientWindow = new Windows.PatientDetailedWindow(patient);
            patientWindow.ShowDialog();
        }

        /// <summary>
        /// Сохранение изменений
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppData.GetContext().SaveChanges();

                MessageBox.Show("Информация успешно сохранена!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Назад на страницу с расписанием
        /// </summary>
        private void BtnBack_Click(object sender, RoutedEventArgs e)
        {
            Navigation.SubFrame.Navigate(new TimeTablePage());
        }

        /// <summary>
        /// Закрашивание строк
        /// </summary>
        private void GridPatients_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            var appointment = e.Row.DataContext as Appointment;
            if (appointment.IsReceived == false)
            {
                e.Row.Background = Brushes.Salmon;
            }
        }
    }
}
