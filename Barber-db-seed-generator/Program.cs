using System;

namespace Barber_db_seed_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            NameGenerator ng = new NameGenerator();
            ng.GenerateFile();
        }
    }
}
