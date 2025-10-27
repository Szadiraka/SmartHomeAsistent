using Microsoft.EntityFrameworkCore;

namespace SmartHomeAsistent.Entities
{

    [Owned]
    public class RepeatSettings
    {

        public List <DayOfWeek>? DaysOfWeek { get; set; } 

        public List <DateTime>? SpecificDates { get; set; } 

        public int? RepeatCont { get; set; }

        public bool IsInfinity { get; set; }
        
        
    }
}
