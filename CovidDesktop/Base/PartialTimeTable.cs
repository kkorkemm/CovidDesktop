using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDesktop.Base
{
    public partial class TimeTable
    {
        public string PatientCount
        {
            get
            {
                if (Appointment.Count() > 0)
                    return $"{Appointment.Count()} человек";
                else
                    return "0 человек";
            }
        }

        public string ComponentType
        {
            get
            {
                if (Appointment.Count() > 0)
                    return Appointment.FirstOrDefault().ComponentType.Name;
                else
                    return "Нет данных";
            }
        }

        public string Color
        {
            get
            {
                if (Appointment.Count() == 5)
                {
                        return "#68D172";
                }
                else
                {
                    return "#FFA384";
                }
            }
        }
    }
}
