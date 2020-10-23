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
            var studios = _dataGeneratorService.GetStudiosList();
            
            at.WriteLine("SET IDENTITY_INSERT barber.Studio ON");
            at.WriteLine();

            foreach (var s in studios)
            {
                at.WriteLine(
                    "INSERT INTO barber.Studio (Studio_ID, StudioName, StudioAddress, PhoneNumber, NumberOfEmployees) " +
                    $"VALUES ({s.Studio_ID}, '{s.StudioName}', '{s.Address}', '{s.PhoneNumber}', {s.NumberOfEmployees})");
            }
            
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
                             $"VALUES ({t.Treatment_ID}, '{t.TreatmentName}', " +
                             $"{t.Price.ToString(CultureInfo.CurrentCulture).Replace(',','.')}, {t.DurationHours})");
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

                at.WriteLine("INSERT INTO barber.Visit (Visit_ID, Studio_ID, Employee_ID, [Date], [Time], " +
                             "DurationHours) " +
                             $"VALUES ('{v.Visit_ID}', {v.Studio_ID}, '{v.Employee_ID}', '{v.DateAndTime.Month}/{v.DateAndTime.Day}/{v.DateAndTime.Year}', " +
                             $"'{v.DateAndTime.Hour}:{v.DateAndTime.Minute}', {v.Duration})");
            }

            at.WriteLine();
            at.WriteLine();
        }

        public void VisitDetailSeed(StreamWriter at)
        {
            var visitDetails = _dataGeneratorService.GetVisitDetailsList();
            at.WriteLine("SET IDENTITY_INSERT barber.VisitDetail ON");
            at.WriteLine();

            foreach (var v in visitDetails)
            {
                at.WriteLine("INSERT INTO barber.VisitDetail (VisitDetail_ID, Visit_ID, Treatment_ID, TotalCost)" +
                             $"VALUES ({v.VisitDetail_ID}, '{v.Visit_ID}', {v.Treatment_ID}, {v.TotalCost.ToString(CultureInfo.CurrentCulture).Replace(',', '.')})");
            }

            at.WriteLine("SET IDENTITY_INSERT barber.VisitDetail OFF");
            at.WriteLine();
            at.WriteLine();
        }
        public void CreateFile()
        {
            const string path = @"..\..\..\..\BarberSeedData.sql";

            var at = new StreamWriter(path, true);

            at.WriteLine("USE The_Barber");
            at.WriteLine("GO");
            at.WriteLine();

            StudioSeed(at);
            EmployeeSeed(at);
            TreatmentSeed(at);
            VisitSeed(at);
            VisitDetailSeed(at);

            at.Dispose();
        }
    }
}