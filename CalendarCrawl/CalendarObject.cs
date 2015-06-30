using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarCrawl
{
    class AbsenceObject
    {
        public string EventDescription { get; set; }
        public DateTime EventDate { get; set; }
        public DateTime EventStartTime { get; set; }
        public DateTime EventEndTime { get; set; }
        public TimeSpan EventSpan { get; set; }
        public List<string> EventInvites { get; set; }
        public string EventCreator { get; set; }
        public string EventLocation { get; set; }
    }

    class Scheduler
    {
        public Scheduler(List<AbsenceObject> absenceList)
        {
            Console.WriteLine("changing the schedule based on absence objects");
            foreach (AbsenceObject absence in absenceList)
            {
                  
            }

        }
    }
}
