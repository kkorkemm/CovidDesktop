using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDesktop.ViewModel
{
    using MVVMCore;
    using Base;

    public class MainViewModel : INotify
    {
        private object currentView;
        public object CurrentView
        {
            get { return currentView; }
            set
            {
                currentView = value;
                OnPropertyChanged();
            }
        }

        public BaseCommand TimeTableCommand { get; set; }
        public BaseCommand StatsCommand { get; set; }
        public BaseCommand RatingCommand { get; set; }
        public BaseCommand AppointmentCommand { get; set; }

        public TimeTableViewModel TimeTableVM { get; set; }
        public StatsViewModel StatsVM { get; set; }
        public RatingViewModel RatingVM { get; set; }
        public AppointmentViewModel AppointmentVM { get; set; }
        
        public MainViewModel()
        {
            TimeTableVM = new TimeTableViewModel();
            StatsVM = new StatsViewModel();
            AppointmentVM = new AppointmentViewModel();
            RatingVM = new RatingViewModel();

            TimeTableCommand = new BaseCommand(p =>
            {
                CurrentView = TimeTableVM;
            });
            StatsCommand = new BaseCommand(p =>
            {
                CurrentView = StatsVM;
            });
            AppointmentCommand = new BaseCommand(p =>
            {
                CurrentView = AppointmentVM;
            });
            RatingCommand = new BaseCommand(p =>
            {
                CurrentView = RatingVM;
            });


            if (AppData.CurrentUser.UserRoleID == 1)
            {
                CurrentView = StatsVM;
            }
            else
            {
                CurrentView = TimeTableVM;
            }
        }
    }
}
