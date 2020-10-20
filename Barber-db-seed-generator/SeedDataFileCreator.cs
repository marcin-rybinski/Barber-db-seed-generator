using System;
using System.Globalization;
using System.IO;

namespace Barber_db_seed_generator
{
    public class SeedDataFileCreator
    {
        private readonly IDataGeneratorService _dataGeneratorService;

        public SeedDataFileCreator(IDataGeneratorService dataGeneratorService)
        {
            _dataGeneratorService = dataGeneratorService;
        }

        public void StudioSeed(StreamWriter at)
        {
            at.WriteLine("SET IDENTITY_INSERT barber.Studio ON");
            at.WriteLine();
            at.WriteLine("INSERT INTO barber.Studio (Studio_ID, StudioName, StudioAddress, PhoneNumber, NumberOfEmployees) VALUES (1, 'Hair o rama', 'Old Road 57', '555 13 17 16', 3)");
            at.WriteLine("INSERT INTO barber.Studio (Studio_ID, StudioName, StudioAddress, PhoneNumber, NumberOfEmployees) VALUES (2, 'Hair do', 'Main Street 34', '555 14 15 16', 4)");
            at.WriteLine("INSERT INTO barber.Studio (Studio_ID, StudioName, StudioAddress, PhoneNumber, NumberOfEmployees) VALUES (3, 'Yer hair', 'Old Branch 40', '554 18 19 17', 3)");
            at.WriteLine("INSERT INTO barber.Studio (Studio_ID, StudioName, StudioAddress, PhoneNumber, NumberOfEmployees) VALUES (4, 'Five o hair', 'London Street 33', '457 89 65 85', 2)");
            at.WriteLine("INSERT INTO barber.Studio (Studio_ID, StudioName, StudioAddress, PhoneNumber, NumberOfEmployees) VALUES (5, 'Hair hair hair', 'Village Ave 23', '478 56 96 85', 3)");
            at.WriteLine("SET IDENTITY_INSERT barber.Studio OFF");
            at.WriteLine();
            at.WriteLine();
        }

        public void EmployeeSeed(StreamWriter at)
        {
            var employees = _dataGeneratorService.GetEmployeesList();

            foreach (var e in employees)
            {

                at.WriteLine("INSERT INTO barber.Employee (Employee_ID, FullName, Studio_ID," +
                             $" Occupation) VALUES ('{e.Employee_ID}', '{e.FullName}', {e.Studio_ID}, '{e.Occupation}')");
            }

            at.WriteLine();
            at.WriteLine();
        }
        
        public void TreatmentSeed(StreamWriter at)
        {
            var treatments = _dataGeneratorService.GetTreatmentsList();   
            at.WriteLine("SET IDENTITY_INSERT barber.Treatment ON");
            at.WriteLine();

            foreach (var t in treatments)
            {
                at.WriteLine("INSERT INTO barber.Treatment (Treatment_ID, TreatmentName, Price, DurationHours)" +
                             $"VALUES ({t.Treatment_ID}, '{t.TreatmentName}', {t.Price.ToString(CultureInfo.CurrentCulture).Replace(',','.')}, {t.DurationHours})");
            }
            
            at.WriteLine("SET IDENTITY_INSERT barber.Treatment OFF");
            at.WriteLine();
            at.WriteLine();
        }
        public void VisitSeed(StreamWriter at)
        {
            var visits = _dataGeneratorService.GetVisitsList(1000);

            foreach (var v in visits)
            {

                at.WriteLine("INSERT INTO barber.Visit (Visit_ID, Studio_ID, Employee_ID, DateAndTime, " +
                             "DurationHours) " +
                             $"VALUES ('{v.Visit_ID}', {v.Studio_ID}, '{v.Employee_ID}', '{v.DateAndTime.ToString(new CultureInfo("en-US", false))}', " +
                             $"{v.Duration})");
            }

            at.WriteLine();
            at.WriteLine();
        }
        public void CreateFile()
        {
            string path = @"E:\repos\stash\employeeSeed.sql";

            var at = new StreamWriter(path, true);

            at.WriteLine("USE The_Barber");
            at.WriteLine("GO");
            at.WriteLine();

            StudioSeed(at);
            EmployeeSeed(at);
            TreatmentSeed(at);
            VisitSeed(at);


            at.Dispose();
        }
    }
}