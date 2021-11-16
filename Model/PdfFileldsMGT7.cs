using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentTextReader.Model
{
    public class PdfFileldsMGT7
    {
        public string FormLanguage { get; set; }
        public string CorporateIdentificationNumber { get; set; }
        public string GlobalLocationNumber { get; set; }
        public string PermanentAccountNumber { get; set; }
        public string NameOfTheCompany { get; set; }
        public string RegisteredOfficeAddress { get; set; }
        public string EmailIdOfTheCompany { get; set; }
        public string TelephoneNumberWithSTDCode { get; set; }
        public string Website { get; set; }
        public DateTime DateofIncorporation { get; set; }
        public string HavingShareCapital { get; set; }
        public string SharesListedOnRecognizedStockExchange { get; set; }
        public string FinancialYearFromDate { get; set; }
        public string AGMHeld { get; set; }
        public string DateOfAGM { get; set; }
        public string DueDateOfAGM { get; set; }
        public string NumberOfBusinessActivities { get; set; }
        public string MainActivityGroupCode { get; set; }
        public List<string> DescriptionOfMainActivityGroup { get; set; }
        public string BusinessActivityCode { get; set; }
        public List<string> DescriptionOfBusinessActivity { get; set; }
        public List<string> PercentOfTurnoverOfCompany { get; set; }
        public string TotalNumberOfEquitySharesAuthorised { get; set; }
        public string TotalAmountOfEquitySharesInRupeesAuthorised { get; set; }
        public string TotalNumberOfEquitySharesIssued { get; set; }
        public string TotalAmountOfEquitySharesInRupeesIssued { get; set; }
        public string TotalNumberOfEquitySharesSubscribed { get; set; }
        public string TotalAmountOfEquitySharesRupeesSubscribed { get; set; }
        public string TotalNumberOfEquitySharesPaidUp { get; set; }
        public string TotalAmountOfEquitySharesRupeesPaidUp { get; set; }
        public string TotalNumberOfPreferenceSharesAuthorised { get; set; }
        public string TotalAmountOfPreferenceSharesRupeesAuthorised { get; set; }
        public string TotalAmountOfPreferenceSharesRupeesIssued { get; set; }
        public string TotalNumberOfPreferenceSharesSubscribed { get; set; }
        public string TotalAmountOfPreferenceSharesRupeesSubscribed { get; set; }
        public string TotalAmountOfPreferenceSharesRupeesPaidUp { get; set; }
        public string NameOfDirector { get; set; }
        public string DINPANOfDirector { get; set; }
        public string DesignationOfDirectorAtTheBeginningDuringTheFinancialYear { get; set; }
        public string NumberOfEquityShareHeldByDirector { get; set; }
        public string DateOfAppointmentChangeInDesignationCessation { get; set; }
        public string NatureOfChangeAppointmentChangeInDesignationCessation { get; set; }
        public List<string> GrossSalaryofDirector { get; set; }
        public List<string> CommissionofDirector { get; set; }
        public List<string> StockOptionSweatEquityOfDirector { get; set; }
        public List<string> OtherRemmunerationOfDirector { get; set; }
        public string NameOfShareholder { get; set; }
        public string CategoryOfShareholder { get; set; }
        public string NumberOfShares { get; set; }
        public string PercentShareholding { get; set; }
    }
}
