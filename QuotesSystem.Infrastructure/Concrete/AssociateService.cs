using Microsoft.AspNetCore.Mvc;
using QuotesSystem.Domain.databases;
using QuotesSystem.Infrastructure.Abstract;
using QuotesSystem.Repository.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Infrastructure.Concrete
{
    public class AssociateService : IAssociateService
    {
        readonly IAssociateRepository _testRepository;

        public AssociateService(IAssociateRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public LineItem AddCurrentLineItem(LineItem newItem)
        {
            return _testRepository.AddCurrentLineItem(newItem);
        }

        public bool CreateLineItems(List<LineItem> lineItems)
        {
            return _testRepository.CreateLineItems(lineItems);
        }

        public Quote CreateQuote(Quote quote)
        {
            return _testRepository.CreateQuote(quote);
        }

        public bool DeleteCurrentLineItem(LineItem deleteItem)
        {
            return _testRepository.DeleteCurrentLineItem(deleteItem);
        }

        public bool DeleteQuote(LineItem deletedItem)
        {
            return _testRepository.DeleteQuote(deletedItem) && _testRepository.DeleteLineItems(deletedItem);

        }

        public List<LineItem> ReadCurrentLineItems(LineItem lineItem)
        {
            return _testRepository.ReadCurrentLineItems(lineItem);
        }

        public List<Quote> ReadQuotes(Credentials credentials)
        {
            return _testRepository.ReadQuotes(credentials);
        }

        public bool UpdateQuote(Quote updateQuote)
        {
            return _testRepository.UpdateQuote(updateQuote);
        }

        public Credentials validateUser( Credentials credentials)
        {
            return _testRepository.validateUser(credentials);              
        }
    }
}