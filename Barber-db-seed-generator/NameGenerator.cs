using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Text;

namespace Barber_db_seed_generator
{
    class NameGenerator
    {
        private readonly List<string> _names = new List<string>
        {
            "Cai West",
            "Caspar Peterson",
            "Tye Marriott",
            "Parker Parry",
            "Dawid Delacruz",
            "Orlando Zimmerman",
            "Antoine Flower",
            "Malachy Brock",
            "Neil Fuller",
            "Jensen Bourne",
            "Clayton Watt",
            "Krista Griffin",
            "Jess Hogan",
            "Regan Goddard",
            "Solomon Robles"
        };

        private readonly Dictionary<int, int> _studiosEmployeeNumber = new Dictionary<int, int>()
        {
            {1,3},
            {2,4},
            {3,3},
            {4,2},
            {5,3}
        };

        private readonly List<Employee> _employees = new List<Employee>();

        private readonly List<Visit> _visits = new List<Visit>();

        private readonly List<VisitDetail> _visitsDetails = new List<VisitDetail>();

        private readonly List<Treatment> _treatments = new List<Treatment>();

        public void CreateEmployeesList()
        {
            var nameCounter = 0;
            var startDate = new DateTime(2020, 11, 1, 9, 0, 0);
            var endDate = new DateTime(2020, 11, 30, 17, 0, 0);

            foreach (var (key, value) in _studiosEmployeeNumber)
            {
                var occupation = "manager";
                for (var j = 0; j < value; j++)
                {
                    var employeeId = Guid.NewGuid();
                    _employees.Add(new Employee
                    {
                        Employee_ID = employeeId,
                        FullName = _names[nameCounter],
                        Studio_ID = key,
                        Occupation = occupation,
                        Schedule = new Dictionary<DateTime, bool>()
                    });

                    for (var i = startDate; i < endDate; i = i.AddHours(1))
                    {
                        if (i.Hour < 9 || i.Hour > 16) continue;
                        _employees[nameCounter].Schedule.Add(i,true);
                    }

                    nameCounter++;
                    occupation = "regular";
                }

            }
        }


        public void EmployeeSeed(StreamWriter at)
        {
            CreateEmployeesList();

            foreach (Employee e in _employees)
            {

                at.WriteLine("INSERT INTO barber.Employee (Employee_ID, FullName, Studio_ID," +
                             $" Occupation) VALUES ('{e.Employee_ID}', '{e.FullName}', {e.Studio_ID}, '{e.Occupation}')");
            }

            at.WriteLine();
            at.WriteLine();
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

       
        public void TreatmentSeed(StreamWriter at)
        {
            CreateTreatmentsList();
            
            at.WriteLine("SET IDENTITY_INSERT barber.Treatment ON");
            at.WriteLine();
            at.WriteLine("INSERT INTO barber.Treatment (Treatment_ID, TreatmentName, Price, DurationHours)" +
                         "VALUES (1, 'Haircut', 12.99, 1)");
            at.WriteLine("INSERT INTO barber.Treatment (Treatment_ID, TreatmentName, Price, DurationHours)" +
                         "VALUES (2, 'Beard Shave', 10.99, 1)");
            at.WriteLine("INSERT INTO barber.Treatment (Treatment_ID, TreatmentName, Price, DurationHours)" +
                         "VALUES (3, 'Head Shave', 9.99, 1)");
            at.WriteLine("INSERT INTO barber.Treatment (Treatment_ID, TreatmentName, Price, DurationHours)" +
                         "VALUES (4, 'Boy haircut', 9.99, 1)");
            at.WriteLine("INSERT INTO barber.Treatment (Treatment_ID, TreatmentName, Price, DurationHours)" +
                         "VALUES (5, 'Dye', 15.99, 1)");
            at.WriteLine("SET IDENTITY_INSERT barber.Treatment OFF");
            at.WriteLine();
            at.WriteLine();
        }

        public void CreateTreatmentsList()
        {
            _treatments.Add(new Treatment() {Treatment_ID = 1, TreatmentName = "Haircut", Price = 12.99M, DurationHours = 1});
            _treatments.Add(new Treatment() { Treatment_ID = 2, TreatmentName = "Beard Shave", Price = 10.99M, DurationHours = 1 });
            _treatments.Add(new Treatment() { Treatment_ID = 3, TreatmentName = "Head Shave", Price = 9.99M, DurationHours = 1 });
            _treatments.Add(new Treatment() { Treatment_ID = 4, TreatmentName = "Boy haircut", Price = 9.99M, DurationHours = 1 });
            _treatments.Add(new Treatment() { Treatment_ID = 5, TreatmentName = "Dye", Price = 15.99M, DurationHours = 1 });
        }

        public void VisitGenerator(int number)
        {
            CreateTreatmentsList();
            
            Random rnd = new Random();
            
            var startDate = new DateTime(2020,11,1,9,0,0);
            var endDate = new DateTime(2020,11,30,17,0,0);
            var range = (endDate - startDate).TotalHours;
            var visitsCounter = 0;
            

            do
            {
                _visits.Add(new Visit());
                var newVisitId = Guid.NewGuid();
                var studioRnd = rnd.Next(1, 6);
                var studioEmployees = _employees.Where(e => e.Studio_ID == studioRnd).ToList();
                

                _visits[visitsCounter] = new Visit()
                {
                    Visit_ID = newVisitId,
                    Studio_ID = studioRnd,
                    Employee_ID = studioEmployees[rnd.Next(1, studioEmployees.Count+1)-1].Employee_ID,
                    Duration = 1,
                };

                
                var currentEmployee = _employees.FirstOrDefault(e => e.Employee_ID == _visits[visitsCounter].Employee_ID);
                var visitAppointed = false;
                var currentEmployeeVisitTryCounter = 0;
                
                do
                {
                    if (currentEmployeeVisitTryCounter > 1) break; 
                    var visitDateTime = startDate.AddHours(rnd.Next(0, (int)range+1));
                    if (visitDateTime.Hour < 9 || visitDateTime.Hour > 16) continue;
                    if (currentEmployee.Schedule[visitDateTime] != true)
                    {
                        currentEmployeeVisitTryCounter++;
                        continue;
                    }
                    
                    visitAppointed = true;
                    _visitsDetails.Add(VisitDetailGenerator(_visits[visitsCounter].Visit_ID));
                    currentEmployee.Schedule[visitDateTime] = false;
                    _visits[visitsCounter].DateAndTime = visitDateTime;


                } while (visitAppointed == false);

                if (currentEmployeeVisitTryCounter > 1)
                {
                    _visits.RemoveAt(_visits.Count-1);
                    continue;
                }

                visitsCounter++;

            } while (visitsCounter < number);


        }

        public void VisitSeed(StreamWriter at)
        {
            VisitGenerator(1000);

            foreach (var v in _visits)
            {

                at.WriteLine("INSERT INTO barber.Visit (Visit_ID, Studio_ID, Employee_ID, DateAndTime, " +
                             "DurationHours) " +
                             $"VALUES ('{v.Visit_ID}', {v.Studio_ID}, '{v.Employee_ID}', '{v.DateAndTime.ToString(new CultureInfo("en-US", false))}', " +
                             $"{v.Duration})");
            }

            at.WriteLine();
            at.WriteLine();
        }

        public VisitDetail VisitDetailGenerator(Guid visitID)
        {
            Random rnd = new Random();

            var newVisitDetail = new VisitDetail
            {
                VisitDetail_ID = _visitsDetails.Count + 1,
                Treatment_ID = _treatments[rnd.Next(1, _treatments.Count+1)-1].Treatment_ID
            };

            newVisitDetail.TotalCost += _treatments[newVisitDetail.Treatment_ID - 1].Price;
            newVisitDetail.Visit_ID = visitID;

            return newVisitDetail;
        }

        public void GenerateFile()
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

    class Employee
    {
        public Guid Employee_ID { get; set; }
        public string FullName { get; set; }
        public int Studio_ID { get; set; }
        public string Occupation { get; set; }

        public Dictionary<DateTime,bool> Schedule { get; set; }
    }

    class Visit
    {
        public Guid Visit_ID { get; set; }
        public int Studio_ID { get; set; }
        public Guid Employee_ID { get; set; }
        public DateTime DateAndTime { get; set; }
        public double Duration { get; set; }
    }

    class Treatment
    {
        public int Treatment_ID { get; set; }
        public string TreatmentName { get; set; }
        public decimal Price { get; set; }
        public double DurationHours { get; set; }
    }

    class VisitDetail
    {
        public int VisitDetail_ID { get; set; }
        public Guid Visit_ID { get; set; }
        public int Treatment_ID { get; set; }
        public decimal TotalCost { get; set; }
    }
    

}
