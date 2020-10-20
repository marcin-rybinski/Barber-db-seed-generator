namespace Barber_db_seed_generator
{
    class Program
    {
        static void Main(string[] args)
        {
           var seedData = new SeedDataFileCreator(new DataGeneratorService());
           seedData.CreateFile();
        }
    }
}
