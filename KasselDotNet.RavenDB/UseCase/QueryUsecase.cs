using Raven.Client;
using System;
using System.Collections.Generic;
using System.Linq;

namespace KasselDotNet.RavenDB.UseCase {
    public class QueryUsecase {
        private IDocumentStore _store;

        public QueryUsecase(IDocumentStore store) {
            _store = store;
        }

        public IEnumerable<Employee> Query(string date) {
            var thisYear = DateTime.Parse(date);

            using (var session = _store.OpenSession()) {
                return session.Query<Employee, EmployeeIndex>()
                        .Where(x => x.JoinDate > thisYear);
            }
        }
    }
}
