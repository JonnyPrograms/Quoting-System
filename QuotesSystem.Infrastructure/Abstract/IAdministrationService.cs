using QuotesSystem.Domain.databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuotesSystem.Infrastructure.Abstract
{
    public interface IAdministrationService
    {
        bool CreateAssociate(Credentials newAssociate);
        List<Credentials> ReadAssociates();
        bool UpdateAssociate(Credentials updateAssociate);
        bool DeleteAssociate(Credentials deleteAssociate);
        // Quotes
        List<Quote> ReadQuotes(Filtered filterQuote);
        List<Quote> ReadCompletedQuotes();
        bool FinalizeQuote(Quote completeQuote);
        bool ProcessQuote(LineItem processQuote);
        bool AssociateCommissionCalc(AssociateCommission associate); 
    }
}
