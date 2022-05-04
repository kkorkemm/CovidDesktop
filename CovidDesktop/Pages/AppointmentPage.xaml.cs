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
    }
}
