//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CTL.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class tblMarriageAudit
    {
        public int AID { get; set; }
        public System.Guid ID { get; set; }
        public Nullable<int> MarriageID { get; set; }
        public string RecordType { get; set; }
        public string Record { get; set; }
        public Nullable<int> RecordYear { get; set; }
        public Nullable<int> StateFileNumber { get; set; }
        public Nullable<int> RecordConstant { get; set; }
        public string County { get; set; }
        public string CountyFileNumber { get; set; }
        public string PartyOneFN { get; set; }
        public string PartyOneMN { get; set; }
        public string PartyOneLN { get; set; }
        public Nullable<System.DateTime> PartyOneDOB { get; set; }
        public Nullable<int> PartyOneDOBDay { get; set; }
        public Nullable<int> PartyOneDOBMonth { get; set; }
        public Nullable<int> PartyOneDOBYear { get; set; }
        public string PartyOneMaritalStatus { get; set; }
        public string PartyOneDissolutionDate { get; set; }
        public Nullable<int> PartyOnePriorDissolutionMonth { get; set; }
        public Nullable<int> PartyOnePriorDissolutionDay { get; set; }
        public Nullable<int> PartyOnePriorDissolutionYear { get; set; }
        public string PartyTwoFN { get; set; }
        public string PartyTwoMN { get; set; }
        public string PartyTwoLN { get; set; }
        public Nullable<System.DateTime> PartyTwoDOB { get; set; }
        public Nullable<int> PartyTwoDOBDay { get; set; }
        public Nullable<int> PartyTwoDOBMonth { get; set; }
        public Nullable<int> PartyTwoDOBYear { get; set; }
        public string PartyTwoMaritalStatus { get; set; }
        public string PartyTwoPriorDissolutionDate { get; set; }
        public Nullable<int> PartyTwoPriorDissolutionMonth { get; set; }
        public Nullable<int> PartyTwoPriorDissolutionDay { get; set; }
        public Nullable<int> PartyTwoPriorDissolutionYear { get; set; }
        public string CeremonyType { get; set; }
        public Nullable<System.DateTime> CeremonyDate { get; set; }
        public Nullable<int> CeremonyDay { get; set; }
        public Nullable<int> CeremonyMonth { get; set; }
        public Nullable<int> CeremonyYear { get; set; }
        public string PartiesRelated { get; set; }
        public string PartyOneResidenceState { get; set; }
        public string PartyOneBirthState { get; set; }
        public string PartyTwoResidenceState { get; set; }
        public string PartyTwoBirthState { get; set; }
        public string PartyOneSex { get; set; }
        public string PartyTwoSex { get; set; }
        public string VoidID { get; set; }
        public Nullable<int> VoidInd { get; set; }
        public Nullable<int> Void { get; set; }
        public string VoidDate { get; set; }
        public string CreateDt { get; set; }
        public Nullable<System.DateTime> DateAdded { get; set; }
        public Nullable<System.DateTime> LastCorrectionDate { get; set; }
        public Nullable<int> LastCorrectionYear { get; set; }
        public Nullable<int> LastCorrectionMonth { get; set; }
        public Nullable<int> LastCorrectionDay { get; set; }
        public string CorrectionNote { get; set; }
        public string FirstDateofCorrection { get; set; }
        public Nullable<int> DataEntryCorrection { get; set; }
        public Nullable<System.DateTime> DateRecordSaved { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public Nullable<byte> FlagRecord { get; set; }
        public string FlagNotes { get; set; }
    }
}
