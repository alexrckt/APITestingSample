using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace APITestingSample.Tests
{
    internal class DBTest : BaseTest
    {
        DBHelper helper;

        [SetUp]
        public void Setup()
        {
            helper = new DBHelper();
        }

        [Test]
        public void InnerJoinTest()
        {
            var query = 
                "SELECT employees.first_name, employees.last_name, jobs.job_title, departments.department_name " +
                "FROM employees " +
                "INNER JOIN jobs ON jobs.job_id = employees.job_id " +
                "INNER JOIN departments ON departments.department_id = employees.department_id " +
                "WHERE departments.department_name = 'IT'";

            var result = helper.GetQueryResult(query);
            Assert.That(result.Rows.Count > 0, "Couldn't get query results.");

            foreach (DataRow row in result.Rows)
            {
                Assert.That(row.Field<string>("job_title"), Is.EqualTo("Programmer"));
            }
        }

        [Test]
        public void LeftJoinTest()
        {
            var query =
                "SELECT employees.first_name, employees.last_name, dependents.relationship, dependents.first_name as dependents_first_name, dependents.last_name as dependents_last_name " +
                "FROM employees " +
                "LEFT JOIN dependents ON employees.employee_id = dependents.employee_id";

            var result = helper.GetQueryResult(query);
            Assert.That(result.Rows.Count > 0, "Couldn't get query results.");

            foreach (DataRow row in result.Rows)
            {
                if (row.Field<string>("last_name").Equals("Weiss"))
                {
                    Assert.That(row.Field<string>("dependents_last_name"), Is.Null);
                    break;
                }
            }
        }

        [Test]
        public void SelfJoinTest()
        {
            var query =
                "SELECT employees.first_name, employees.last_name, managers.first_name as manager_first_name, managers.last_name as manager_last_name " +
                "FROM employees " +
                "LEFT JOIN employees managers ON employees.manager_id = managers.employee_id";

            var result = helper.GetQueryResult(query);
            Assert.That(result.Rows.Count > 0, "Couldn't get query results.");

            foreach (DataRow row in result.Rows)
            {
                if(row.Field<string>("last_name").Equals("Kochhar"))
                {
                    Assert.That(row.Field<string>("manager_last_name"), Is.EqualTo("King"));
                    break;
                }
            }
        }
    }
}
