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
    public class AdministrationService: IAdministrationService
    {
        readonly IAdministrationRepository _testRepository;

        public AdministrationService(IAdministrationRepository testRepository)
        {
            _testRepository = testRepository;
        }

        public bool AssociateCommissionCalc(AssociateCommission associate)
        {
            return _testRepository.AssociateCommissionCalc(associate);
        }

        public bool CreateAssociate(Credentials newAssociate)
        {
            return _testRepository.CreateAssociate(newAssociate);
        }

        public bool DeleteAssociate(Credentials deleteAssociate)
        {
            return _testRepository.DeleteAssociate(deleteAssociate);
        }

        public bool FinalizeQuote(Quote completeQuote)
        {
            return _testRepository.FinalizeQuote(completeQuote);
        }

        public bool ProcessQuote(LineItem processQuote)
        {
            return _testRepository.ProcessQuote(processQuote);
        }

        /// <summary>
        /// Converts the first and last name of an associate to a full name from Repository
        /// </summary>
        /// <returns> All Associates from repository </returns>
        public List<Credentials> ReadAssociates()
        {
            List<Credentials> allAssociates = _testRepository.ReadAssociates();

            foreach(Credentials associate in allAssociates)
            {
                associate.fullName = associate.firstName + " " + associate.lastName;

                if (associate.totalCommission == null)
                {
                    associate.totalCommission = 0.00;
                }
            }

            return allAssociates;
        }

        public List<Quote> ReadCompletedQuotes()
        {
            return _testRepository.ReadCompletedQuotes();
        }

        public List<Quote> ReadQuotes(Filtered filterQuote)
        {
            return _testRepository.ReadQuotes(filterQuote);
        }

        public bool UpdateAssociate(Credentials updateAssociate)
        {
            return _testRepository.UpdateAssociate(updateAssociate);
        }
    }
}
