namespace Barber_db_seed_generator.Models
{
    public class Treatment
    {
        public int Treatment_ID { get; set; }
        public string TreatmentName { get; set; }
        public decimal Price { get; set; }
        public double DurationHours { get; set; }
    }
}