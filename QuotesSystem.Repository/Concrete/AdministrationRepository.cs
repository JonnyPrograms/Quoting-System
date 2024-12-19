using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuotesSystem.Repository.Abstract;
using QuotesSystem.Domain.databases;
using Dapper;
using System.Collections;
using System.Net;
using MySql.Data.MySqlClient;

namespace QuotesSystem.Repository.Concrete
{
    public class AdministrationRepository: IAdministrationRepository
    {
        private IDbConnection CreateConnection()
        {
            return new SqlConnection("Server=tcp:quotesytems.database.windows.net,1433;Initial Catalog=quotesSystem;Persist Security Info=False;User ID=quoteadmin;Password=CalebandMattQuit@1;");
        }
        

        public int CreateAssociateID(Credentials newAssociate)
        {
            using var connection = CreateConnection();
            string sql = "SELECT MAX(associateID) FROM AssociateTable";
            int newAssociateID = connection.QueryFirstOrDefault<int>(sql);
            return newAssociateID + 1;
        }

        public bool CreateAssociate(Credentials newAssociate)
        {
            newAssociate.associateId = CreateAssociateID(newAssociate);
            using var connection = CreateConnection();
            string insertQuery = "INSERT INTO AssociateTable (associateId, firstName, lastName, username, password) VALUES (@AssociateId, @FirstName, @LastName, @Username, @Password)";

            var parameters = new
            {
                AssociateId = newAssociate.associateId,
                FirstName = newAssociate.firstName,
                LastName = newAssociate.lastName,
                @Username = newAssociate.username,
                @Password = newAssociate.password
            };

            int rowsAffected = connection.Execute(insertQuery, parameters);
            if (rowsAffected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Read Associates from SQL Database
        /// </summary>
        /// <returns> a List of Associates </returns>
        public List<Credentials> ReadAssociates()
        {
            using var connection = CreateConnection();
            string sql = "SELECT * FROM AssociateTable";
            List<Credentials> allAssociates = connection.QueryAsync<Credentials>(sql).Result.ToList();

           
            return allAssociates;
        }

        public bool UpdateAssociate(Credentials updateAssociate)
        {
            using var connection = CreateConnection();
            string update = "UPDATE AssociateTable SET firstName = @FirstName, lastName = @LastName, username = @Username, password = @Password WHERE associateId = @AssociateID";
            var parameters = new
            {
                AssociateID = updateAssociate.associateId,
                FirstName = updateAssociate.firstName,
                LastName = updateAssociate.lastName,
                Username = updateAssociate.username,
                Password = updateAssociate.password
            };

            int rowsAffected = connection.Execute(update, parameters);
            
            if (rowsAffected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool DeleteAssociate(Credentials deleteAssociate)
        {
            using var connection = CreateConnection();
            string delete = "DELETE FROM AssociateTable WHERE associateId = @AssociateID";
            var parameters = new
            {
                AssociateID = deleteAssociate.associateId,
            };

            int rowsAffected = connection.Execute(delete, parameters);

            if (rowsAffected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }


        /// <summary>
        /// Customer table queried to see what the Customer name is
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public async Task<string> Customerassociated(Quote quote)
        {
            var customer = "";
            string CustomersConnection = "Server=blitz.cs.niu.edu;User ID=student;Password=student;Database=csci467";
            List<Customer> Customers = new List<Customer>();
            using var connection = new MySqlConnection(CustomersConnection);

            await connection.OpenAsync();

            string query = "SELECT * FROM customers WHERE id = @customerID;";
            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@customerID", quote.customerID);
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



            if (Customers.Count == 0 || Customers.Count >= 2)
            {
                customer = "Unknown Customer";
            }
            else
            {
                customer = Customers[0].Name;
            }

            return customer;
        }

        public string Associateassociated(Quote quote)
        {

            List<Credentials> allAssociates = ReadAssociates();

            var associate = allAssociates.FirstOrDefault(n => n.associateId == quote.associateID);

            if (associate == null)
            {
                return "Unknown Associate";
            }
            else
            {
                return associate.firstName + " " + associate.lastName;
            }
        }
        /// <summary>
        /// Read In Quotes from SQL Database
        /// </summary>
        /// <returns> A List of Quotes </returns>
        public List<Quote> ReadQuotes(Filtered filterQuote)
        {
            using var connection = CreateConnection();
            string sql = "SELECT * FROM Quotes";

            
            List<Quote> quotes = connection.Query<Quote>(sql).ToList();

            for (int i = 0; i < quotes.Count; i++)
            {
                var customer = Customerassociated(quotes[i]);
                var associate = Associateassociated(quotes[i]);
                quotes[i].customer = customer.Result;
                quotes[i].associate = associate;
            }

            if (filterQuote.customerEmail != null)
            {
                quotes = quotes.Where(n => n.quoteEmail == filterQuote.customerEmail).ToList();
            }

            if (filterQuote.startDate != null)
            {
                quotes = quotes.Where(n => n.dateCreated >= filterQuote.startDate).ToList();
            }
            else if (filterQuote.endDate != null)
            {
                quotes = quotes.Where(n => n.dateCreated <= filterQuote.endDate).ToList();
            }
            else if (filterQuote.startDate != null && filterQuote.endDate != null)
            {
                quotes = quotes.Where(n => n.dateCreated <= filterQuote.endDate && n.dateCreated >= filterQuote.startDate).ToList();
            }

            if (filterQuote.status != null)
            {
                quotes = quotes.Where(n => n.workFlow == filterQuote.status).ToList();
            }
            else
            {
                quotes = quotes.Where(n => n.workFlow != "Quote Processed").ToList();
            }

            if (filterQuote.associateID != null)
            {
                quotes = quotes.Where(n => n.associateID == filterQuote.associateID).ToList();
            }

            if (filterQuote.customerID != null)
            {
                quotes = quotes.Where(n => n.customerID == filterQuote.customerID).ToList();
            }


            return quotes;
        }


        public List<Quote> ReadCompletedQuotes()
        {
            using var connection = CreateConnection();
            string sql = "SELECT * FROM Quotes";


            List<Quote> quotes = connection.Query<Quote>(sql).ToList();

            for (int i = 0; i < quotes.Count; i++)
            {
                var customer = Customerassociated(quotes[i]);
                var associate = Associateassociated(quotes[i]);
                quotes[i].customer = customer.Result;
                quotes[i].associate = associate;
            }

           
              quotes = quotes.Where(n => n.workFlow != "Quote Processed").ToList();
            quotes = quotes.Where(n => n.workFlow == "Quote Completed").ToList();


            return quotes;
        }
        public bool FinalizeQuote(Quote completeQuote)
        {
            using var connection = CreateConnection();
            string update = "UPDATE Quotes SET quoteEmail = @quoteEmail, secretNotes = @secretNotes, discount = @discount, percentage = @percentage, workflow = @workFlow, associateCommission = @associateCommission, dateFinalized = @dateCompleted WHERE quoteID = @QuoteID";


            var parameters = new
            {
                QuoteID = completeQuote.quoteID,
                quoteEmail = completeQuote.quoteEmail,
                secretNotes = completeQuote.secretNotes,
                discount = completeQuote.discount,
                percentage = completeQuote.percentage,
                associateCommission = completeQuote.associateCommission,
                workFlow = "Quote Completed",
                dateCompleted = DateTime.Now
            };

            int rowsAffected = connection.Execute(update, parameters);
            if (rowsAffected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool ProcessQuote(LineItem processQuote)
        {
            using var connection = CreateConnection();
            string update = "UPDATE Quotes SET workflow = @workFlow WHERE quoteID = @QuoteID";


            var parameters = new
            {
                QuoteID = processQuote.quoteID,
                workFlow = "Quote Processed"
            };

            int rowsAffected = connection.Execute(update, parameters);
            if (rowsAffected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public double AssociateCommission(AssociateCommission associate)
        {
            List<Credentials> allAssociates = ReadAssociates();
            allAssociates = allAssociates.Where(n => n.associateId == associate.associateId).ToList();
            var totalCommission = associate.priceAmount * (associate.commission / 100.0);
            totalCommission = Math.Round(totalCommission, 2);
            if (allAssociates[0].totalCommission == null)
            {
                return totalCommission;
            }
            else
            {
                totalCommission = (double)(allAssociates[0].totalCommission) + totalCommission;

                return totalCommission;
            }
        }

        public bool AssociateCommissionCalc(AssociateCommission associate)
        {
            using var connection = CreateConnection();
            string update = "UPDATE AssociateTable SET totalCommission = @totalCommission WHERE associateId = @associateID";

            var newCommission = AssociateCommission(associate);
            var parameters = new
            {
                totalCommission = newCommission,
                associateID = associate.associateId
            };

            int rowsAffected = connection.Execute(update, parameters);
            if (rowsAffected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
    }
}
