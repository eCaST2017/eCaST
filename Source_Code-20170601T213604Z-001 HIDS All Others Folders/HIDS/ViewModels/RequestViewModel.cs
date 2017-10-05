using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using System.Data.Entity;
using System.Globalization;
using System.Web.Security;

namespace CTL.ViewModels
{
    public class RequestViewModel
    {

        public string RT = DateTime.Now.ToString("yyyy-MM-dd");

        public string RequestDate
        {
            get
            {
                return RT;
            }
            set
            {
                RT = value;
            }
        }
        public DateTime _RequestDateActual = DateTime.Now;

        public DateTime RequestDateActual
        {

            get
            {
                return _RequestDateActual;

            }
            set
            {

                _RequestDateActual = value;

            }


        }

        public int RequestID { get; set; }
        public string RequestTrackerTypeName { get; set; }
        public string RequestTrackerComment { get; set; }
        public int? DaysOpen { get; set; }
        public int? ScreeningID { get; set; }
        public int? ClientID { get; set; }
        public int? RequestTrackerID { get; set; }
        //public int UserID { get; set; }
        public int? RequestModeID { get; set; }
        public int? ApplicationID { get; set; }
        public int? ProjectID { get; set; }
        public string ProjectName { get; set; }
        public int? AssetCount { get; set; }
        public bool? Urgent { get; set; }
        public string ApplicationName { get; set; }
       // string _UserID = Membership.GetUser().UserName.ToString();//set Default Value here 
        //public string RequestorID
        //{
        //    get
        //    {
        //        return _UserID;
        //    }
        //    set
        //    {
        //        _UserID = value;
        //    }
        //}

        public string RequestorID { get; set; }
        //public string UserID { get; set; }
        [Required]
        public string RequestTitle { get; set; }
        public string RequestProgramName { get; set; }
        public string RequestTypeName { get; set; }
        public string AgencyName { get; set; }
        public string RequestActionName { get; set; }
        public string PriorityName { get; set; }
        public string StateName { get; set; }
        public string AssigneeName { get; set; }
        public string RequestorName { get; set; }
        [Required]
        public int DivisionID { get; set; }
        [Required]
        public int RequestProgramID { get; set; }
        [Required]
        public int RequestTypeID { get; set; }
        [Required]
        public int PriorityID { get; set; }

        int _StateID = 1;//set Default Value here 
        public int StateID
        {
            get
            {
                return _StateID;
            }
            set
            {
                _StateID = value;
            }
        }


        //public int StatusID { get; set; }
        [Required]
        //[UIHint("MultilineText")]
        [AllowHtml]
        public string RequestDesc { get; set; }

        //[UIHint("tinymce_jquery_full"), AllowHtml]
        [AllowHtml]
        public string RequestorNotes { get; set; }
        [Required]
        public string UserID { get; set; }
        public string UserName { get; set; }
        //Developer Fields
        //[UIHint("MultilineText")] 
        //[UIHint("tinymce_jquery_full"), AllowHtml]
        public string RequestedByID { get; set; }

        [AllowHtml]
        public string DeveloperNotes { get; set; }
        //[UIHint("MultilineText")] 
        //[UIHint("tinymce_jquery_full"), AllowHtml]
        [AllowHtml]
        public string TestLog { get; set; }
        public decimal? TimeToComplete { get; set; }
        public DateTime? ReleaseDate { get; set; }
        public DateTime? DueDate { get; set; }
        public DateTime? BroadcastDate { get; set; }
        public bool TrainingIssue { get; set; }
        public int? Requestor { get; set; }
        public bool? Overdue { get; set; }
        public bool? OverdueWarning { get; set; }
        public bool? RequestorViewed { get; set; }
        public bool? AssigneeViewed { get; set; }
        public bool? TestingMode { get; set; }
        public string Participants { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? DateCreated { get; set; }





    }
}