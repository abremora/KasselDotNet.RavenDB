using Raven.Abstractions.Extensions;
using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KasselDotNet.RavenDB.UseCase {
    public class UpdateUsecase {
        private IDocumentStore _store;

        public UpdateUsecase(IDocumentStore store) {
            _store = store;
        }
        public Employee UpdateById(string id, string bonus) {
            var plusSalary = Convert.ToInt32(bonus);
            using (var session = _store.OpenSession()) {
                var employee = session.Load<Employee>(id);
                employee.Salary += plusSalary;
                session.SaveChanges();
                return employee;
            }
        }

        public IEnumerable<Employee> UpdateByCity(string town, string bonus) {
            var plusSalary = Convert.ToInt32(bonus);
            using (var session = _store.OpenSession()) {
                var employees = session.Query<EmployeeIndex.Result, EmployeeIndex>()
                      .Where(x => x.Town == town)
                      .OfType<Employee>();

                employees.ForEach(x => x.Salary += plusSalary);
                session.SaveChanges();
                return employees;
            }
        }
    }
}
