using Dapper;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using QuotesSystem.Domain.databases;
using QuotesSystem.Repository.Abstract;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace QuotesSystem.Repository.Concrete
{
    public class AssociateRepository : IAssociateRepository
    {
        private IDbConnection CreateConnection()
        {
            return new SqlConnection("Server=tcp:quotesytems.database.windows.net,1433;Initial Catalog=quotesSystem;Persist Security Info=False;User ID=quoteadmin;Password=CalebandMattQuit@1;");
        }

        /// <summary>
        /// validate the current user 
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public Credentials validateUser(Credentials credentials)
        {
            using var connection = CreateConnection();
            string sql = "SELECT * FROM AssociateTable";
            List<Credentials> realCredentials = connection.QueryAsync<Credentials>(sql).Result.ToList();
            // to save sql server time
            //List<Credentials> realCredentials = new List<Credentials>();
            //realCredentials.Add(new Credentials { firstName = "Daniel", lastName = "Stoner", username = "dston21", password = "password1", associateId = 1 });

            var filteredCredentials = realCredentials.Where(cred =>
            (string.IsNullOrEmpty(cred.username) || cred.username == credentials.username) && (cred.password == credentials.password) ).ToList();

            if (filteredCredentials.Count != 0) {
                credentials.firstName = filteredCredentials[0].firstName;
                credentials.lastName = filteredCredentials[0].lastName;
                credentials.associateId = filteredCredentials[0].associateId;
                credentials.passed = true;
            }
            else
            {
                credentials.passed = false;
            }
            return credentials;
        }

        /// <summary>
        /// Makes the quote ID unique 
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        public string CreatequoteID(Quote quote)
        {
            var index = 0;
            var quoteId = quote.customerID.ToString() + quote.associateID.ToString();
            using var connection = CreateConnection();
            string sql = "SELECT * FROM Quotes";
            List<Quote> allQuotes = connection.QueryAsync<Quote>(sql).Result.ToList();
            
            while (true)
            {
                var filteredList = allQuotes.Where(q =>
             q.quoteID == quoteId).ToList();
                if (filteredList.Count == 0)
                {
                    return quoteId;
                }
                else
                {
                    quoteId += index.ToString();
                    index++;
                }
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

        /// <summary>
        /// Query database to create a new row in the Quotes SQL
        /// </summary>
        /// <param name="quote"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Quote CreateQuote(Quote quote)
        {
            
            quote.quoteID = CreatequoteID(quote);
            using var connection = CreateConnection();
            string insertQuery = "INSERT INTO Quotes (quoteID, associateId, customerId, quotePrice, quoteEmail, secretNotes, discount, percentage, workflow, associateCommission, dateCreated) VALUES (@QuoteID, @AssociateId, @CustomerId, @QuotePrice, @QuoteEmail, @SecretNotes, @Discount, @Percentage, @Workflow, @AssociateCommission, @DateCreated)";

            var parameters = new
            {
                QuoteID = quote.quoteID,
                AssociateId = quote.associateID,
                CustomerId = quote.customerID,
                QuotePrice = (decimal)quote.quotePrice,
                QuoteEmail = quote.quoteEmail,
                SecretNotes = quote.secretNotes,
                Discount = (decimal)quote.discount,
                Percentage = quote.percentage,
                Workflow = "Quote Created",
                AssociateCommission = (decimal)5,
                DateCreated = DateTime.Now
            };

            int rowsAffected = connection.Execute(insertQuery, parameters);
            if (rowsAffected == 0)
            {
                return quote;
            }
            else
            {
                return quote;
            }
        }


        /// <summary>
        /// reads the quotes for all the associate logged in 
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        public List<Quote> ReadQuotes(Credentials credentials)
        {
            using var connection = CreateConnection();
            string sql = "SELECT * FROM Quotes WHERE associateId = @associateId";

            // Replace with the associateId you want to filter by
            var parameters = new { associateId = credentials.associateId };

           
            List<Quote> quotes =  connection.Query<Quote>(sql, parameters).ToList();

            quotes = quotes.Where(n => n.workFlow != "Quote Processed").ToList();

            for (int i = 0; i < quotes.Count; i++)
            {
                var customer = Customerassociated(quotes[i]);
                quotes[i].customer = customer.Result;
                
            }

            return quotes;
        }

        /// <summary>
        /// creates a unique new ItemID
        /// </summary>
        /// <param name="lineItem"></param>
        /// <param name="quoteId"></param>
        /// <returns></returns>
        public string CreateItemID(LineItem lineItem, string quoteId)
        {
            string itemID = quoteId + lineItem.itemName;
            var index = 1;
            
            List<LineItem> allQuotes = ReadCurrentLineItems(lineItem);

            while (true)
            {
                var filteredList = allQuotes.Where(q =>
             q.itemId == itemID).ToList();
                if (filteredList.Count == 0)
                {
                    return itemID;
                }
                else
                {
                    itemID += index.ToString();
                    index++;
                }
            }
        }

        /// <summary>
        /// Creates a new Item inside the SQL database
        /// </summary>
        /// <param name="lineItem"></param>
        /// <param name="quoteId"></param>
        public void CreateItem(LineItem lineItem, string quoteId)
        {
            using var connection = CreateConnection();
            string insertQuery = "INSERT INTO quoteLineItems (quoteID, itemsID, quoteLineItemPrice, quoteLineItemName) VALUES (@QuoteID, @itemsID, @Price, @Name)";

            var parameters = new
            {
                QuoteID = quoteId,
                itemsID = CreateItemID(lineItem, quoteId),
                Price = lineItem.itemPrice,
                Name = lineItem.itemName,
            };

            
            int rowsAffected = connection.Execute(insertQuery, parameters);
            if (rowsAffected == 0) {
                rowsAffected = connection.Execute(insertQuery, parameters);
            }
        }

        /// <summary>
        /// Master function to create all the list of Line Items
        /// </summary>
        /// <param name="lineItems"></param>
        /// <returns></returns>
        public bool CreateLineItems(List<LineItem> lineItems)
        {
            if (lineItems[0].quoteID == null)
            {
                return false;
            }
            var quoteId = lineItems[0].quoteID;

            foreach(LineItem lineitem in lineItems)
            {
                CreateItem(lineitem, quoteId);
            }

            return true;
        }

        public List<LineItem> ReadCurrentLineItems(LineItem lineItem)
        {
            using var connection = CreateConnection();
            string sql = "SELECT itemsID AS itemId, quoteLineItemName AS itemName, quoteLineItemPrice AS itemPrice, quoteID AS quoteID FROM quoteLineItems WHERE quoteID = @quoteID";

            // Replace with the associateId you want to filter by
            var parameters = new { quoteID = lineItem.quoteID };


            List<LineItem> lineItems = connection.Query<LineItem>(sql, parameters).ToList();


            return lineItems;
        }

        public bool DeleteCurrentLineItem(LineItem deleteItem)
        {
            using var connection = CreateConnection();
            string sql = "DELETE FROM quoteLineItems WHERE itemsID = @itemsId";
            string update = "UPDATE Quotes SET quotePrice = @quotePrice WHERE quoteID = @Id";
            // Replace with the associateId you want to filter by
            var parameters = new { itemsId = deleteItem.itemId };
            var updateparms = new { quotePrice = deleteItem.itemPrice, Id = deleteItem.quoteID };
            int row = connection.Execute(update, updateparms);
            int rowsAffected = connection.Execute(sql, parameters);
            if (rowsAffected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public decimal UpdatePrice(string quoteID)
        {
            var itemPrice = new decimal(0.0);
            using var connection = CreateConnection();
            string sql = "SELECT itemsID AS itemId, quoteLineItemName AS itemName, quoteLineItemPrice AS itemPrice, quoteID AS quoteID FROM quoteLineItems WHERE quoteID = @quoteID";

            // Replace with the associateId you want to filter by
            var parameters = new { quoteID = quoteID };


            List<LineItem> lineItems = connection.Query<LineItem>(sql, parameters).ToList();
            foreach(LineItem item in lineItems)
            {
                itemPrice += item.itemPrice;
            }

            return itemPrice;
        }
        public LineItem AddCurrentLineItem(LineItem newItem)
        {
            using var connection = CreateConnection();
            string insertQuery = "INSERT INTO quoteLineItems (quoteID, itemsID, quoteLineItemPrice, quoteLineItemName) VALUES (@QuoteID, @itemsID, @Price, @Name)";
            string update = "UPDATE Quotes SET quotePrice = @quotePrice WHERE quoteID = @Id";
            
            var updateparms = new { quotePrice = UpdatePrice(newItem.quoteID) + newItem.itemPrice, Id = newItem.quoteID };
            newItem.itemId = CreateItemID(newItem, newItem.quoteID);
            var parameters = new
            {
                QuoteID = newItem.quoteID,
                itemsID = newItem.itemId,
                Price = (decimal)newItem.itemPrice,
                Name = newItem.itemName,
            };
            int row = connection.Execute(update, updateparms);
            int rowsAffected = connection.Execute(insertQuery, parameters);
            return newItem;
        }

        public bool DeleteQuote(LineItem deletedItem)
        {
            using var connection = CreateConnection();
            string sql = "DELETE FROM quoteLineItems WHERE quoteID = @quoteID";
            
            // Replace with the associateId you want to filter by
            var parameters = new { quoteID = deletedItem.quoteID };

            int rowsAffected = connection.Execute(sql, parameters);
            if (rowsAffected == 0)
            {
                return true;
            }
            else
            {
                return true;
            }
        }

        public bool DeleteLineItems(LineItem deletedItem)
        {
            using var connection = CreateConnection();
            string sql = "DELETE FROM Quotes WHERE quoteID = @quoteID";

            // Replace with the associateId you want to filter by
            var parameters = new { quoteID = deletedItem.quoteID };

            int rowsAffected = connection.Execute(sql, parameters);
            if (rowsAffected == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public bool UpdateQuote(Quote updateQuote)
        {
     
            using var connection = CreateConnection();
            string update = "UPDATE Quotes SET quoteEmail = @quoteEmail, secretNotes = @secretNotes, discount = @discount, percentage = @percentage, workflow = @workFlow, associateCommission = @associateCommission WHERE quoteID = @QuoteID";


            var parameters = new
            {
                QuoteID = updateQuote.quoteID,
                quoteEmail = updateQuote.quoteEmail,
                secretNotes = updateQuote.secretNotes,
                discount = updateQuote.discount,
                percentage = updateQuote.percentage,
                associateCommission = updateQuote.associateCommission,
                workFlow = "Quote Edited"
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
