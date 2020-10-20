using System;
using System.Collections.Generic;

namespace Barber_db_seed_generator.Models
{
    public class Employee
    {
        public Guid Employee_ID { get; set; }
        public string FullName { get; set; }
        public int Studio_ID { get; set; }
        public string Occupation { get; set; }

        public Dictionary<DateTime,bool> Schedule { get; set; }
    }
}