using Raven.Client;
using System;

namespace KasselDotNet.RavenDB.UseCase {
    public class StoreUsecase {
        private IDocumentStore _store;

        public StoreUsecase(IDocumentStore store) {
            _store = store;
        }
        public Employee Store(string name, string city) {
            using (var session = _store.OpenSession()) {
                var employee = new Employee {
                    Name = name,
                    JoinDate = DateTime.UtcNow,
                    Adress = new Adress {
                        City = city
                    }
                };
                session.Store(employee);
                session.SaveChanges();
                return employee;
            }
        }
    }
}
