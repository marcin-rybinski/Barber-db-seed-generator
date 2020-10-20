using Barber_db_seed_generator.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Barber_db_seed_generator
{
    public class DataGeneratorService : IDataGeneratorService
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

        private readonly DateTime _visitCalendarStartDate = new DateTime(2020, 11, 1, 9, 0, 0);
        private readonly DateTime _visitCalendarEndDate = new DateTime(2020, 11, 30, 17, 0, 0);

        private readonly Random _rnd = new Random();

        private readonly List<Studio> _studios = new List<Studio>();
        private readonly List<Employee> _employees = new List<Employee>();
        private readonly List<Visit> _visits = new List<Visit>();
        private readonly List<VisitDetail> _visitsDetails = new List<VisitDetail>();
        private readonly List<Treatment> _treatments = new List<Treatment>();

        public List<Studio> GetStudiosList()
        {
            CreateStudiosList();
            return _studios;
        }
        public List<Employee> GetEmployeesList()
        {
            CreateEmployeesList();
            return _employees;
        }
        public List<Visit> GetVisitsList(int number)
        {
            CreateVisitsList(number);
            return _visits;
        }
        public List<Treatment> GetTreatmentsList()
        {
            CreateTreatmentsList();
            return _treatments;
        }
        public List<VisitDetail> GetVisitDetailsList()
        {
            return _visitsDetails;
        }
        private void CreateStudiosList()
        {
            _studios.Add(new Studio() { Studio_ID = 1, StudioName = "Hair o rama", Address = "Old Road 57", PhoneNumber = "555 13 17 16", NumberOfEmployees = 3 });
            _studios.Add(new Studio() { Studio_ID = 1, StudioName = "Hair do", Address = "Main Street 34", PhoneNumber = "555 43 23 23", NumberOfEmployees = 4 });
            _studios.Add(new Studio() { Studio_ID = 1, StudioName = "Yer hair", Address = "Old Branch 40", PhoneNumber = "555 23 65 65", NumberOfEmployees = 3 });
            _studios.Add(new Studio() { Studio_ID = 1, StudioName = "Five o hair", Address = "London Street 33", PhoneNumber = "555 32 54 73", NumberOfEmployees = 2 });
            _studios.Add(new Studio() { Studio_ID = 1, StudioName = "Hair hair hair", Address = "Village Ave 23", PhoneNumber = "555 85 32 73", NumberOfEmployees = 3 });
        }
        public void CreateEmployeesList()
        {
            
            var nameCounter = 0; 

            foreach (var s in _studios)
            {
                var occupation = "manager";
                for (var j = 0; j < s.NumberOfEmployees; j++)
                {
                    var employeeId = Guid.NewGuid();
                    _employees.Add(new Employee
                    {
                        Employee_ID = employeeId,
                        FullName = _names[nameCounter],
                        Studio_ID = s.Studio_ID,
                        Occupation = occupation,
                        Schedule = new Dictionary<DateTime, bool>()
                    });

                    for (var i = _visitCalendarStartDate; i < _visitCalendarEndDate; i = i.AddHours(1))
                    {
                        if (i.Hour < 9 || i.Hour > 16) continue;
                        _employees[nameCounter].Schedule.Add(i,true);
                    }

                    nameCounter++;
                    occupation = "regular";
                }

            }
        }

        
        public void CreateTreatmentsList()
        {
            _treatments.Add(new Treatment() {Treatment_ID = 1, TreatmentName = "Haircut", Price = 12.99M, DurationHours = 1});
            _treatments.Add(new Treatment() { Treatment_ID = 2, TreatmentName = "Beard Shave", Price = 10.99M, DurationHours = 1 });
            _treatments.Add(new Treatment() { Treatment_ID = 3, TreatmentName = "Head Shave", Price = 9.99M, DurationHours = 1 });
            _treatments.Add(new Treatment() { Treatment_ID = 4, TreatmentName = "Boy haircut", Price = 9.99M, DurationHours = 1 });
            _treatments.Add(new Treatment() { Treatment_ID = 5, TreatmentName = "Dye", Price = 15.99M, DurationHours = 1 });
        }

        public void CreateVisitsList(int number)
        {
            var range = (_visitCalendarEndDate - _visitCalendarStartDate).TotalHours;
            var visitsCounter = 0;

            do
            {
                _visits.Add(new Visit());
                var newVisitId = Guid.NewGuid();
                var studioRnd = _rnd.Next(1, 6);
                var studioEmployees = _employees.Where(e => e.Studio_ID == studioRnd).ToList();


                _visits[visitsCounter] = new Visit()
                {
                    Visit_ID = newVisitId,
                    Studio_ID = studioRnd,
                    Employee_ID = studioEmployees[_rnd.Next(1, studioEmployees.Count + 1) - 1].Employee_ID,
                    Duration = 1,
                };

                var currentEmployee =
                    _employees.FirstOrDefault(e => e.Employee_ID == _visits[visitsCounter].Employee_ID);
                var visitAppointed = false;
                var currentEmployeeVisitTryCounter = 0;

                do
                {
                    if (currentEmployeeVisitTryCounter > 1) break;
                    var visitDateTime = _visitCalendarStartDate.AddHours(_rnd.Next(0, (int) range + 1));
                    if (visitDateTime.Hour < 9 || visitDateTime.Hour > 16) continue;
                    if (currentEmployee.Schedule[visitDateTime] != true)
                    {
                        currentEmployeeVisitTryCounter++;
                        continue;
                    }

                    visitAppointed = true;
                    _visitsDetails.Add(CreateVisitDetail(_visits[visitsCounter].Visit_ID));
                    currentEmployee.Schedule[visitDateTime] = false;
                    _visits[visitsCounter].DateAndTime = visitDateTime;

                } while (visitAppointed == false);

                if (currentEmployeeVisitTryCounter > 1)
                {
                    _visits.RemoveAt(_visits.Count - 1);
                    continue;
                }

                visitsCounter++;

            } while (visitsCounter < number);
        }

        

        public VisitDetail CreateVisitDetail(Guid visitID)
        {
            var newVisitDetail = new VisitDetail
            {
                VisitDetail_ID = _visitsDetails.Count + 1,
                Treatment_ID = _treatments[_rnd.Next(1, _treatments.Count+1)-1].Treatment_ID
            };

            newVisitDetail.TotalCost += _treatments[newVisitDetail.Treatment_ID - 1].Price;
            newVisitDetail.Visit_ID = visitID;

            return newVisitDetail;
        }

        
    }

}
