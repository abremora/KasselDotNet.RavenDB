using Raven.Client;

namespace KasselDotNet.RavenDB.UseCase {

    public class LoadUsecase {
        private IDocumentStore _store;

        public LoadUsecase(IDocumentStore store) {
            _store = store;
        }

        public Employee Load(string id) {
            using (var session = _store.OpenSession()) {
                return session.Load<Employee>(id);
            }
        }
    }
}
