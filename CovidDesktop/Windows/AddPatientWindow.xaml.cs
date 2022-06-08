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
    /// Логика взаимодействия для AddPatientWindow.xaml
    /// </summary>
    public partial class AddPatientWindow : Window
    {
        private Patient CurrentPatient;
        private TimeTable CurrentTimetable;

        public AddPatientWindow(TimeTable timeTable)
        {
            InitializeComponent();

            CurrentTimetable = timeTable;

            ComboComponentTypes.ItemsSource = AppData.GetContext().ComponentType.ToList();

            if (CurrentTimetable.ComponentType != "Нет данных")
            {
                ComboComponentTypes.Visibility = Visibility.Collapsed;

                if (CurrentTimetable.ComponentType == "I компонент")
                {
                    ComboComponentTypes.SelectedIndex = 0;
                }
                if (CurrentTimetable.ComponentType == "II компонент")
                {
                    ComboComponentTypes.SelectedIndex = 1;
                }
            }
            else
            {
                
            }
        }

        /// <summary>
        /// Добавление пациента (осторожно, говнокод)
        /// </summary>
        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder error = new StringBuilder();
            if (CurrentPatient == null)
                error.AppendLine("Выберите пациента");
            if (ComboComponentTypes.SelectedItem == null)
                error.AppendLine("Выберите компонент");

            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var first = CurrentPatient.Appointment.Where(p => p.ComponentTypeID == 1).FirstOrDefault();
            var second = CurrentPatient.Appointment.Where(p => p.ComponentTypeID == 2).FirstOrDefault();

            if (ComboComponentTypes.SelectedIndex == 0)
            {
                if (first != null)
                    error.AppendLine("Выбранный пациент уже получил первый компонент");
            }
            if (ComboComponentTypes.SelectedIndex == 1)
            {
                if (second != null)
                    error.AppendLine("Выбранный пациент уже получил второй компонент");
            }
            
            if (error.Length > 0)
            {
                MessageBox.Show(error.ToString(), "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            Appointment appointment = new Appointment()
            {
                TimeTableID = CurrentTimetable.ID,
                ComponentTypeID = ComboComponentTypes.SelectedIndex + 1,
                PatientID = CurrentPatient.ID,
                IsReceived = false
            };

            try
            {
                AppData.GetContext().Appointment.Add(appointment);
                AppData.GetContext().SaveChanges();

                MessageBox.Show("Пациент успешно добавлен!", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Information);

                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        /// <summary>
        /// Поиск пациента
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
                    CurrentPatient = patient;
                }
                else
                {
                    TextPatientName.Text = "Пациент: не найден";
                    TextPatientName.FontWeight = FontWeights.Regular;
                }
            }
            else
            {
                TextPatientName.Text = "Пациент: не найден";
                TextPatientName.FontWeight = FontWeights.Regular;
            }
        }

        /// <summary>
        /// Открытие окна с информацией о пациенте
        /// </summary>
        private void BtnShow_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentPatient != null)
            {
                PatientDetailedWindow patientDetailed = new PatientDetailedWindow(CurrentPatient);
                patientDetailed.ShowDialog();
            }
            else
            {
                MessageBox.Show("Выберите пациента", "Внимание!", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
        }
    }
}
