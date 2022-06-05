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
    /// Логика взаимодействия для AppointmentPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        private Patient CurrentPatient { get; set; }

        public AppointmentPage()
        {
            InitializeComponent();

            ComboComponentTypes.ItemsSource = AppData.GetContext().ComponentType.ToList();

            ComboTimeTable.ItemsSource = AppData.GetContext().TimeTable.Where(p => p.VaccinationPointID == AppData.CurrentUser.ID && p.Date > DateTime.Now && p.Appointment.Count() < 5).ToList();
        }

        /// <summary>
        /// Поиск пользователя по ИИН и телефону
        /// </summary>
        private void TextCode_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(TextCode.Text) && !string.IsNullOrWhiteSpace(TextNumber.Text))
            {
                var patient = AppData.GetContext().Patient.Where(p => p.Code == TextCode.Text && p.Telephone == TextNumber.Text).FirstOrDefault();

                if (patient != null)
                {
                    TextPatientName.Text = "Пациент: " + patient.FullName;
                    TextPatientName.FontWeight = FontWeights.Bold;
                    BtnShow.Visibility = Visibility.Visible;
                    CurrentPatient = patient;
                }
                else
                {
                    TextPatientName.Text = "Пациент: не найден";
                    BtnShow.Visibility = Visibility.Collapsed;
                    TextPatientName.FontWeight = FontWeights.Regular;
                }
            }
            else
            {
                TextPatientName.Text = "Пациент: не найден";
                BtnShow.Visibility = Visibility.Collapsed;
                TextPatientName.FontWeight = FontWeights.Regular;
            }
        }

        /// <summary>
        /// Подробнее о пациенте
        /// </summary>
        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            Windows.PatientDetailedWindow detailedWindow = new Windows.PatientDetailedWindow(CurrentPatient);
            detailedWindow.ShowDialog();
        }

        /// <summary>
        /// Сохранение записи
        /// </summary>
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();

            if (CurrentPatient == null)
                errors.AppendLine("Выберите пациента");
            if (ComboComponentTypes.SelectedItem == null)
                errors.AppendLine("Выберите тип компонента");
            if (ComboTimeTable.SelectedItem == null)
                errors.AppendLine("Выберите элемента расписания");

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var first = CurrentPatient.Appointment.Where(p => p.ComponentTypeID == 1).FirstOrDefault();
            var second = CurrentPatient.Appointment.Where(p => p.ComponentTypeID == 2).FirstOrDefault();

            if (ComboComponentTypes.SelectedIndex == 0)
            {
                if (first != null)
                    errors.AppendLine("Выбранный пациент уже получил первый компонент");
            }
            if (ComboComponentTypes.SelectedIndex == 1)
            {
                if (second != null)
                    errors.AppendLine("Выбранный пациент уже получил второй компонент");
            }

            if (errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var appointment = new Appointment()
            {
                PatientID = CurrentPatient.ID,
                TimeTableID = (ComboTimeTable.SelectedItem as TimeTable).ID,
                ComponentTypeID = (ComboComponentTypes.SelectedItem as ComponentType).ID,
                IsReceived = false
            };

            try
            {
                AppData.GetContext().Appointment.Add(appointment);
                AppData.GetContext().SaveChanges();
                MessageBox.Show("Пациент успешно записан!");

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

        private void ComboComponentTypes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var table = AppData.GetContext().TimeTable.Where(p => p.VaccinationPointID == AppData.CurrentUser.ID && p.Date > DateTime.Now && p.Appointment.Count() < 5).ToList();

            if (ComboComponentTypes.SelectedIndex == 0)
                table = table.Where(p => p.ComponentType != "II компонент").ToList();
            else
                table = table.Where(p => p.ComponentType != "I компонент").ToList();

            ComboTimeTable.ItemsSource = table;
        }
    }
}
