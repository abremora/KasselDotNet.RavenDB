using Raven.Client.Indexes;
using System.Linq;

namespace KasselDotNet.RavenDB {
    public class EmployeeIndex : AbstractIndexCreationTask<Employee> {
        public class Result {
            public string Name { get; set; }
            public int Year { get; set; }
            public string Town { get; set; }
        }

        public EmployeeIndex() {
            Map = employees => from employee in employees
                select new {
                    employee.Name,
                    employee.JoinDate,
                    Year = employee.JoinDate.Year,
                    Town = employee.Adress.City,
                };            
        }
    }
}
