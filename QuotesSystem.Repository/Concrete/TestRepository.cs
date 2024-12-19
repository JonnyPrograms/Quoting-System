using MySql.Data.MySqlClient;
using QuotesSystem.Domain.databases;
using QuotesSystem.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Intrinsics.X86;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuotesSystem.Repository.Concrete
{
    public class TestRepository : ITestRepository
    {
        public async Task<List<Customer>> GetNamesAsync()
        {
            string CustomersConnection = "Server=blitz.cs.niu.edu;User ID=student;Password=student;Database=csci467";
            List<Customer> Customers = new List<Customer>();
            using var connection = new MySqlConnection(CustomersConnection);

            await connection.OpenAsync();

            using var command = new MySqlCommand("SELECT * FROM customers;", connection);
            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                var id = reader.GetValue(0);
                var name = reader.GetValue(1);
                var city = reader.GetValue(2);
                var street = reader.GetValue(3);
                var contact = reader.GetValue(4);
                Customers.Add(new Customer { Id = Convert.ToInt32(id), Name = Convert.ToString(name) ?? "", City = Convert.ToString(city) ?? "", Street = Convert.ToString(street) ?? "", Contact = Convert.ToString(contact) ?? "" });
            }


            //server
            // premade data

            return Customers;
        }
    }
}
