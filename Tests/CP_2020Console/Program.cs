using CP_2021Console;
using System;
using System.Collections.Generic;

namespace CP_2020Console
{
    class Program
    {
        static void Main(string[] args)
        {
            Company company = new Company();
            company.Name = "CCC";
            company.Employees = new List<Employee>();
            Employee e1 = new Employee();
            e1.Name = "DDD";
            e1.Something = new List<int>();
            e1.Something.Add(1);
            e1.Something.Add(2);
            e1.Something.Add(3);
            company.Employees.Add(e1);

            Employee e2 = GetEmployee(company);
            e2.Something.Add(2);
        }

        public static Employee GetEmployee(Company company)
        {
            foreach(Employee e in company.Employees)
            {
                return e;
            }
            return new Employee();
        }
    }
}
