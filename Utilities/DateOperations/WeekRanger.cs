using Microsoft.AspNetCore.Components.Forms;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utilities.DateOperations
{
    public class WeekRanger
    {
        private DateTime startDate;
        private DateTime endDate;

        public DateTime StartDate
        {
            get { return startDate; }
        }
        public DateTime EndDate
        {
            get { return endDate; }
        }

        public WeekRanger(DateTime currentDate,DayOfWeek weekStartingDay=DayOfWeek.Saturday) {

            // Calculate how many days to subtract to reach the start of the week
            int daysToSubtract = (int)currentDate.DayOfWeek - (int)weekStartingDay;

            // Handle the wrap-around case if inputDate.DayOfWeek is less than firstDayOfWeek (e.g., current culture is Monday, but input is Sunday)
            if (daysToSubtract < 0)
            {
                daysToSubtract += 7;
            }

            // Set the start date to the beginning of the day (midnight)
            startDate = currentDate.AddDays(-daysToSubtract).Date;

            // The end date is 6 days after the start date, set to the end of the day (11:59:59 PM)
            endDate = startDate.AddDays(6).Date.AddDays(1).AddSeconds(-1);
        }
    }
}
