using System;

namespace KasselDotNet.RavenDB {
    public class Employee {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Salary { get; set; }
        public DateTime JoinDate { get; set; }
        public Adress Adress { get; set; }
    }

    public class EmployeeFlat {
        public string Name { get; set; }
        public int Salary { get; set; }
        public DateTime JoinDate { get; set; }
        public string City { get; set; }
    }
}
