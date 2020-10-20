using System;
using System.Collections.Generic;
using Barber_db_seed_generator.Models;

namespace Barber_db_seed_generator
{
    public interface IDataGeneratorService
    {
        List<Employee> GetEmployeesList();
        List<Visit> GetVisitsList(int number);
        List<Treatment> GetTreatmentsList();
        List<VisitDetail> GetVisitDetailsList();
    }
}