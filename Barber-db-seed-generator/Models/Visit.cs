using System;

namespace Barber_db_seed_generator.Models
{
    public class Visit
    {
        public Guid Visit_ID { get; set; }
        public int Studio_ID { get; set; }
        public Guid Employee_ID { get; set; }
        public DateTime DateAndTime { get; set; }
        public double Duration { get; set; }
    }
}