using Microsoft.AspNetCore.Mvc;
using QuotesSystem.Domain.databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Infrastructure.Abstract
{
    public interface IAssociateService
    {
        Credentials validateUser(Credentials credentials);
        Quote CreateQuote(Quote quote);
        List<Quote> ReadQuotes(Credentials credentials);
        bool CreateLineItems(List<LineItem> lineItems);
        List<LineItem> ReadCurrentLineItems(LineItem lineItem);
        bool DeleteCurrentLineItem(LineItem deleteItem);
        LineItem AddCurrentLineItem(LineItem newItem);
        bool DeleteQuote(LineItem deletedItem);
        bool UpdateQuote(Quote updateQuote);
    }
}
