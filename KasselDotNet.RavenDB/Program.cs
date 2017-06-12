using KasselDotNet.RavenDB.UseCase;
using Newtonsoft.Json;
using Raven.Client;
using Raven.Client.Document;
using System;
using System.Collections.Generic;
using System.IO;

namespace KasselDotNet.RavenDB {
    class Program {
        static void Main(string[] args) {
            var store = GetStore();
            RegisterIndex(store);

            bool exit = false;
            while (!exit) {
                try {
                    Console.WriteLine("----------------------");
                    Console.WriteLine("[0]: Bulk dummy data");
                    Console.WriteLine("[1]: Store");
                    Console.WriteLine("[2]: Load");
                    Console.WriteLine("[3]: Query");
                    Console.WriteLine("[4]: Update by Id");
                    Console.WriteLine("[5]: Update by Query");
                    Console.WriteLine("[x]: Exit");
                    var key = Console.ReadKey();
                    Console.WriteLine();
                    switch (char.ToLower(key.KeyChar)) {
                        case '0':
                            Bulk(store);
                            continue;
                        case '1':
                            Store(store);
                            continue;
                        case '2':
                            Load(store);
                            continue;
                        case '3':
                            Query(store);
                            continue;
                        case '4':
                            UpdateById(store);
                            continue;
                        case '5':
                            UpdateByQuery(store);
                            continue;
                        case 'x':
                            exit = true;
                            break;
                    }
                }
                catch(Exception e) {
                    Console.WriteLine("Fehler: " + e.Message);
                }
            }
        }

        private static void UpdateByQuery(IDocumentStore store) {
            Console.WriteLine("Update for town?");
            var city = Console.ReadLine();
            Console.WriteLine("Update with bonus?");
            var bonus = Console.ReadLine();
            var employees = new UpdateUsecase(store).UpdateByCity(city, bonus);

            Print(employees);
        }

        private static void Print<T>(IEnumerable<T> employees) {
            var counter = 1;
            foreach (var employee in employees) {
                var serializedEmployee = JsonConvert.SerializeObject(employee);
                Console.WriteLine(counter);
                Console.WriteLine(serializedEmployee);
                counter++;
            }
        }

        private static void UpdateById(IDocumentStore store) {
            Console.WriteLine("Update for Id?");
            var id = Console.ReadLine();
            Console.WriteLine("Update with bonus?");
            var bonus = Console.ReadLine();
            var employee = new UpdateUsecase(store).UpdateById(id, bonus);

            Print(new List<Employee> { employee });
        }

        private static void Bulk(IDocumentStore store) {
            var dummy = File.ReadAllText(@".\DummyData\DummyData.json");
            var employees = JsonConvert.DeserializeObject<List<EmployeeFlat>>(dummy);
            using (var bulk = store.BulkInsert()) {
                employees.ForEach(x => {
                    var employee = new Employee {
                        Name = x.Name,
                        JoinDate = x.JoinDate,
                        Salary = x.Salary,
                        Adress = new Adress {
                            City = x.City
                        }
                    };
                    bulk.Store(employee);
                });
            }
            Console.WriteLine("Import completed.");
        }

        private static void Query(IDocumentStore store) {
            Console.WriteLine("Query for > joining date?");
            var join = Console.ReadLine();
            var employees = new QueryUsecase(store).Query(join);

            Print(employees);
        }

        private static void Load(IDocumentStore store) {
            Console.WriteLine("Load by ID?");
            var id = Console.ReadLine();
            var employee = new LoadUsecase(store).Load(id);
            var serializedEmployee = JsonConvert.SerializeObject(employee);
            Console.WriteLine(serializedEmployee);
            Console.WriteLine("Press key to continue");
            Console.ReadKey();
        }

        private static void Store(IDocumentStore store) {
            Console.WriteLine("Employee name?");
            var name = Console.ReadLine();
            Console.WriteLine("City?");
            var city = Console.ReadLine();
            var employee = new StoreUsecase(store).Store(name, city);
            Console.WriteLine("Id: " + employee.Id);
        }

        private static void RegisterIndex(IDocumentStore store) {
            new EmployeeIndex().Execute(store);
        }

        private static IDocumentStore GetStore() {
            var store = new DocumentStore {
                ConnectionStringName = "RavenDb",
                Conventions = new DocumentConvention() {
                    FindTypeTagName = (type) => type.Name
                }
            };
            store.Initialize();
            return store;
        }
    }
}
