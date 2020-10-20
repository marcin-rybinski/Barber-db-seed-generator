using System;

namespace Barber_db_seed_generator.Models
{
    public class VisitDetail
    {
        public int VisitDetail_ID { get; set; }
        public Guid Visit_ID { get; set; }
        public int Treatment_ID { get; set; }
        public decimal TotalCost { get; set; }
    }
}