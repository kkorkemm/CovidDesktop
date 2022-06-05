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
    /// Логика взаимодействия для PatientDetailedWindow.xaml
    /// </summary>
    public partial class PatientDetailedWindow : Window
    {
        private Patient CurrentPatient = new Patient();
        private Questionnnare CurrentQuest;

        public PatientDetailedWindow(Patient patient)
        {
            InitializeComponent();

            CurrentPatient = patient;
            DataContext = CurrentPatient;

            Questionnnare questionnnare = CurrentPatient.Questionnnare.FirstOrDefault();
            if (questionnnare == null)
            {
                questionnnare = new Questionnnare()
                {
                    PatientID = CurrentPatient.ID
                };
            }

            CurrentQuest = questionnnare;
            GridQuestionnare.DataContext = questionnnare;
        }

        private void BtnSaveQuestionnare_Click(object sender, RoutedEventArgs e)
        {
            if (CurrentQuest.ID == 0)
            {
                AppData.GetContext().Questionnnare.Add(CurrentQuest);
            }

            try
            {
                AppData.GetContext().SaveChanges();
                MessageBox.Show("Изменения в анкете пациента сохранены!", "Успех!", MessageBoxButton.OK, MessageBoxImage.Information);
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error!", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }
    }
}
