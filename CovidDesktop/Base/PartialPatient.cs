using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDesktop.Base
{
    public partial class Patient
    {
        public string FirstComponent
        {
            get
            {
                var components = Appointment.Where(p => p.ComponentTypeID == 1).FirstOrDefault();
                
                if (components != null)
                {
                    return components.TimeTable.Date.ToString("dd.MM.yyyy");
                }
                else
                {
                    return "-";
                }
            }
        }

        public string SecondComponent
        {
            get
            {
                var components = Appointment.Where(p => p.ComponentTypeID == 2).FirstOrDefault();

                if (components != null)
                {
                    return components.TimeTable.Date.Date.ToString("dd.MM.yyyy");
                }
                else
                {
                    return "-";
                }
            }
        }
    }
}
