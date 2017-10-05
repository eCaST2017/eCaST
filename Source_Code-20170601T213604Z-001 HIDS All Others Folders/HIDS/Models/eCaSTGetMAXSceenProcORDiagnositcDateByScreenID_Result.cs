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
    
    public partial class eCaSTGetMAXSceenProcORDiagnositcDateByScreenID_Result
    {
        public Nullable<System.DateTime> ProcedureDate { get; set; }
        public int ScreenProcedureID { get; set; }
        public Nullable<long> ScreeningProcGuidID { get; set; }
        public int ScreeningID { get; set; }
        public int SiteID { get; set; }
        public Nullable<long> ScreeningGuidID { get; set; }
        public Nullable<long> OfficeVisitID { get; set; }
        public int EnrollmentID { get; set; }
        public int ClinicalCycleID { get; set; }
        public int ProcedureOutcomeBinID { get; set; }
        public int ProcedureMethodBinID { get; set; }
        public string ProcedureMethodOther { get; set; }
        public int ProcedureAge { get; set; }
        public int ProcedureResultBinID { get; set; }
        public int SubContractorID { get; set; }
        public int ProviderID { get; set; }
        public int CecumReachedBinID { get; set; }
        public int QualityPrepBinID { get; set; }
        public int PatientGivenResultsBinID { get; set; }
        public int AdequateExamBinID { get; set; }
        public Nullable<bool> StateStaffOverride { get; set; }
        public Nullable<bool> Pathology { get; set; }
        public int PathPolypBinID { get; set; }
        public int PathHistBinID { get; set; }
        public int PathProviderBinID { get; set; }
        public string PathPolypsRemoved { get; set; }
        public string PathContainerCount { get; set; }
        public Nullable<bool> PathHighGradeDysplasia { get; set; }
        public string PathLargestAdenoma { get; set; }
        public Nullable<System.DateTime> SurgeryDate { get; set; }
        public Nullable<System.DateTime> DateUpdated { get; set; }
        public string UpdatedBy { get; set; }
        public Nullable<System.DateTime> DateCreated { get; set; }
        public string CreatedBy { get; set; }
        public Nullable<bool> Coverage { get; set; }
        public int ProcedureTypeBinID { get; set; }
        public int AdequacyBinID { get; set; }
        public int SpecimenTypeBinID { get; set; }
        public Nullable<System.DateTime> BillingUpdatedDate { get; set; }
        public int BillingPaidBinID { get; set; }
        public int FundingSourceBinID { get; set; }
        public Nullable<bool> AdminApproval { get; set; }
        public string OtherDxText { get; set; }
        public int IsBethesda { get; set; }
        public int SystolicBinID1 { get; set; }
        public int DiastolicBinID1 { get; set; }
        public int SystolicBinID2 { get; set; }
        public int DiastolicBinID2 { get; set; }
        public int FastingStatusBinID { get; set; }
        public int FastingStatusBinID1 { get; set; }
        public Nullable<bool> BloodSample { get; set; }
        public int TotalCholesterolBinID { get; set; }
        public int HDLCholesterolBinID { get; set; }
        public int LDLCholesterolBinID { get; set; }
        public int TriglyceridesBinID { get; set; }
        public int GlucoseBinID { get; set; }
        public int A1CPercentageBinID { get; set; }
        public int AlertBin_AlertBinID { get; set; }
        public int CholBin_TotalCholBinID { get; set; }
        public int DiastolicBin_DiastolicBinID { get; set; }
        public int SystolicBin_SystolicBinID { get; set; }
        public int TriglyceridesBin_TrigylceridesBinID { get; set; }
        public int AlertBinID { get; set; }
        public Nullable<System.DateTime> TestDate { get; set; }
        public int WorkupStatusBinID { get; set; }
        public int WorkupResultBinID { get; set; }
        public Nullable<System.DateTime> WorkupStatusDate { get; set; }
        public Nullable<bool> MedicationCounselingProvided { get; set; }
        public Nullable<bool> TranslationProvided { get; set; }
        public Nullable<bool> BPPlus { get; set; }
    }
}