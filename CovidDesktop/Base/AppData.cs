using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CovidDesktop.Base
{
    public class AppData
    {
        private static CovidSystemDBEntities context;
        public static CovidSystemDBEntities GetContext()
        {
            if (context == null)
                context = new CovidSystemDBEntities();
            return context;
        }

        /// <summary>
        /// Текущий пользователь системы (пункт вакцинации или админ)
        /// </summary>
        public static VaccinationPoint CurrentUser { get; set; }
    }
}
