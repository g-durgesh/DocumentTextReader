using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DocumentTextReader.Model
{
    public class PdfFieldsAOC4Json
    {
        public string AuthorizedCapitalOfTheCompany { get; set; }
        public string NumberOfMembersOfTheCompany { get; set; }
        public string CIN { get; set; }
        public string GLN { get; set; }
        public string NameOfTheCompany { get; set; }
        public string AddressOfRegisteredOffice { get; set; }
        public string EmailOfTheCompany { get; set; }
        public List<DateTime> FinancialYear { get; set; }
        public string NatureOfFinancialStatements { get; set; }
        public string ProvisionalFinancialStatementsFiledEarlier { get; set; }
        public string AdoptedInAdjournedAGM { get; set; }
        public List<string> DINOrIncometaxPAN { get; set; }
        public List<string> Names { get; set; }
        public string Designation { get; set; }
        public List<DateTime> DateOfSigningOfFinancialStatements { get; set; }
        public string DateOfSigningOfBoardsReport { get; set; }
        public string DateOfSigningOfReportsOnTheFinancialStatementsByTheAuditors { get; set; }
        public string AnnualGeneralMeetingAGMHeld { get; set; }
        public string DateOfAGM { get; set; }
        public string DueDateOfAGM { get; set; }
        public string extensionForFinancialYearOrAGMGranted { get; set; }
    }
}
