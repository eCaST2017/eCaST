using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Collections;
using System.Data.Entity.SqlServer;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using CTL.ViewModels;
using CTL.Models;

namespace CTL.Controllers
{

      // Custom comparer for the Product class
  

    public class AgencyContactsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private HIDSEntities context = new HIDSEntities();
        // GET: AgencyContacts
        public ActionResult Index()
        {
            return View(db.AgencyContacts.ToList());
        }


        public ActionResult _PersonnelList(int? id, int? id2, int? id3)
        {

            ViewBag.SiteID = id;
            ViewBag.ProgramID = id2;
            List<UserProfileViewModel> userlist = new List<UserProfileViewModel>();
            List<UserProfileViewModel> userlistSUB = new List<UserProfileViewModel>();

            if (id3 == 2)
            {


         

           }
          else
           {


               List<AgencyRoleViewModel> agencyRoles = new List<AgencyRoleViewModel>();
                 // get list of all sites for this agency
                    //var rolelist = (from x in db.RoleBins
                    //                where x.IsPortalUser == false
                    //              select x.RoleBinID).ToList();

                    //foreach (int val in rolelist)
                    //{

                        // && P.AgencyRoleBinID == id3 && P.AgencyRoleBinID != 2
                         var personnelcount = (from P in db.AgencyContactClinics
                                             join Y in db.AgencyContacts
                                             on P.AgencyContactID equals Y.AgencyContactID
                                             where P.ClinicID == id && P.ProgramBinID == id2 && P.AgencyRoleBinID == id3
                                             select P).Count();

                         if (personnelcount > 0)
                         {


                             var personnellist = (from P in db.AgencyContactClinics
                                                  join Y in db.AgencyContacts
                                                  on P.AgencyContactID equals Y.AgencyContactID
                                                  where P.ClinicID == id && P.ProgramBinID == id2 && P.AgencyRoleBinID != 2 && P.AgencyRoleBinID == id3
                                                  //select Y.FirstName + " " + Y.LastName + " " + "(" + "e:" + "<a style='text-decoration:none' target='_blank' href='https://mail.google.com/mail/?view=cm&fs=1&tf=1&to=" + Y.Email + "'>" + Y.Email + "</a>" + "," + "ph:" + Y.Phone + ")" + " ").ToList();
                                                  select "<div><a class='btn btn-default' style='text-decoration:none' href='#CreateAgencyContactContainerAnchor_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "' id='EditAUserInfoButton_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + "><span disabled='disabled' class='glyphicon glyphicon-pencil'></span> " + Y.FirstName + " " + Y.LastName + "</a>" + " " + "(" + "e:" + "<a style='text-decoration:none' target='_blank' href='https://mail.google.com/mail/?view=cm&fs=1&tf=1&to=" + Y.Email + "'>" + Y.Email + "</a>" + " | " + "ph:" + Y.Phone + ")" + "</div>" + "<script type='text/javascript'>$(function () {$('#EditAUserInfoButton_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + ").click(function () {" + "var listUrl = $('#listUrl').val();" + "var slash = '?id=';var adv2 = '&id2=';var adv3 = '&id3=';var adv4 = '&id4=';" + "var idW =" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + ";" + "var id2=" + SqlFunctions.StringConvert((double)id2).Trim() + ";" + "var idAC =" + SqlFunctions.StringConvert((double)id).Trim() + ";" + "var idAR =" + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + ";" + "var res=listUrl.concat(slash,idAC,adv2,id2,adv3,idW,adv4,idAR);" + "$('#ContactHolderContainer_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "'" + ").css('display','block');" + "$('#AddEditLabel_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "'" + ").html(" + "'Edit Contact');" + "$('#ContactInfoContainer_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "').load(res);" + "});});</script>").ToList();
                             //   select "<div class='btn btn-default'><span disabled='disabled' class='glyphicon glyphicon-user'></span> <a style='text-decoration:none' href='#' id='EditAUserInfoButton_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + "data-toggle='modal' data-target='#EditAUserInfoModal_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + ">" + Y.FirstName + " " + Y.LastName + "</a>" + " " + "(" + "e:" + "<a style='text-decoration:none' target='_blank' href='https://mail.google.com/mail/?view=cm&fs=1&tf=1&to=" + Y.Email + "'>" + Y.Email + "</a>" + " | " + "ph:" + Y.Phone + ")" + "</div>" + "<div class='modal fade' id='EditAUserInfoModal_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + "tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'><div class='modal-dialog'><div style='text-align:left' class='modal-content'><div class='modal-header'><h2 class='modal-title' id='myModalLabel'>Edit User Info</h2></div><div id='EditAUserInfoContainer_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + "/> </div></div></div><script type='text/javascript'>$(function () {$('#EditAUserInfoButton_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + ").click(function () {" + "var listUrl = $('#listUrl').val();" + "var slash = '/';" + "var idW =" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + ";" + "var id2=" + SqlFunctions.StringConvert((double)id).Trim() + ";" + "var res=listUrl.concat(slash,idW,slash,id2);" + "$('#EditAUserInfoContainer_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "').load(res);" + "});});</script>").ToList();

                             string[] ResultList = personnellist.ToArray();
                             string ResultString = string.Join("<span style='color:#fff'>,</span>", ResultList.ToArray());

                             var perlist = (from c in db.AgencyContactClinics
                                            where c.ClinicID == id && c.ProgramBinID == id2 && c.AgencyRoleBinID == id3
                                            select new AgencyRoleViewModel()
                                            {

                                                AgencyRoleBinID = id3,
                                                SiteID = id,
                                                ProgramID = id2,
                                                UserList = ResultString


                                            }).ToList();

                             foreach (AgencyRoleViewModel up in perlist)
                             {


                                 bool alreadyExists = agencyRoles.Any(x => x.AgencyRoleBinID == up.AgencyRoleBinID);

                                 if (alreadyExists == false)
                                 {

                                     agencyRoles.Add(up);

                                 }

                             }




                         }
                         else
                         {



                             var personnellist = (from P in db.RoleBins
                                                  where P.RoleBinID == id3
                                                  select "<div class='btn btn-default'>No Current Contacts</div>").ToList();
                             //select Y.FirstName + " " + Y.LastName + " " + "(" + "e:" + "<a style='text-decoration:none' target='_blank' href='https://mail.google.com/mail/?view=cm&fs=1&tf=1&to=" + Y.Email + "'>" + Y.Email + "</a>" + "," + "ph:" + Y.Phone + ")" + " ").ToList();
                             // select "<div class='btn btn-default'><span disabled='disabled' class='glyphicon glyphicon-pencil'></span> <a style='text-decoration:none' href='#' id='EditAUserInfoButton_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + ">" + Y.FirstName + " " + Y.LastName + "</a>" + " " + "(" + "e:" + "<a style='text-decoration:none' target='_blank' href='https://mail.google.com/mail/?view=cm&fs=1&tf=1&to=" + Y.Email + "'>" + Y.Email + "</a>" + " | " + "ph:" + Y.Phone + ")" + "</div>" + "<script type='text/javascript'>$(function () {$('#EditAUserInfoButton_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + ").click(function (e) {" + "var listUrl = $('#listUrl').val();" + "var slash = '?id=';var adv2 = '&id2=';var adv3 = '&id3=';var adv4 = '&id4=';" + "var idW =" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + ";" + "var id2=" + SqlFunctions.StringConvert((double)id2).Trim() + ";" + "var idAC =" + SqlFunctions.StringConvert((double)id).Trim() + ";" + "var idAR =" + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + ";" + "var res=listUrl.concat(slash,idAC,adv2,id2,adv3,idW,adv4,idAR);" + "$('#ContactHolderContainer_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "'" + ").css('display','block');" + "$('#AddEditLabel_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "'" + ").html(" + "'Edit Contact');" + "$('#ContactInfoContainer_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "').load(res);" + "e.preventDefault(); return false;});});</script>").ToList();
                             //   select "<div class='btn btn-default'><span disabled='disabled' class='glyphicon glyphicon-user'></span> <a style='text-decoration:none' href='#' id='EditAUserInfoButton_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + "data-toggle='modal' data-target='#EditAUserInfoModal_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + ">" + Y.FirstName + " " + Y.LastName + "</a>" + " " + "(" + "e:" + "<a style='text-decoration:none' target='_blank' href='https://mail.google.com/mail/?view=cm&fs=1&tf=1&to=" + Y.Email + "'>" + Y.Email + "</a>" + " | " + "ph:" + Y.Phone + ")" + "</div>" + "<div class='modal fade' id='EditAUserInfoModal_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + "tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'><div class='modal-dialog'><div style='text-align:left' class='modal-content'><div class='modal-header'><h2 class='modal-title' id='myModalLabel'>Edit User Info</h2></div><div id='EditAUserInfoContainer_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + "/> </div></div></div><script type='text/javascript'>$(function () {$('#EditAUserInfoButton_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "'" + ").click(function () {" + "var listUrl = $('#listUrl').val();" + "var slash = '/';" + "var idW =" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + ";" + "var id2=" + SqlFunctions.StringConvert((double)id).Trim() + ";" + "var res=listUrl.concat(slash,idW,slash,id2);" + "$('#EditAUserInfoContainer_" + SqlFunctions.StringConvert((double)Y.AgencyContactID).Trim() + SqlFunctions.StringConvert((double)P.AgencyRoleBinID).Trim() + "').load(res);" + "});});</script>").ToList();

                             string[] ResultList = personnellist.ToArray();
                             string ResultString = string.Join("<span style='color:#fff'>,</span>", ResultList.ToArray());

                             var perlist = (from c in db.RoleBins
                                            where c.RoleBinID == id3
                                            select new AgencyRoleViewModel()
                                            {

                                                AgencyRoleBinID = id3,
                                                SiteID = id,
                                                ProgramID = id2,
                                                UserList = ResultString


                                            }).ToList();

                             foreach (AgencyRoleViewModel up in perlist)
                             {


                                 bool alreadyExists = agencyRoles.Any(x => x.AgencyRoleBinID == up.AgencyRoleBinID);

                                 if (alreadyExists == false)
                                 {

                                     agencyRoles.Add(up);

                                 }

                             }







                         }








                  //  }

                    return PartialView(agencyRoles);


            }

            return PartialView();

        }


     
        public ActionResult _PersonnelEdit(int? id, int? id2, int? id3)
        {

            ViewBag.SiteID = id;
            ViewBag.ProgramID = id2;
            ViewBag.AgencyRoleBinID = id3;


            if (id3 == 7)
            {


                //IEnumerable Option = dbAdmin.UserProfiles.Where(c => c.Active == true).AsEnumerable().Select(c => new SelectListItem()

                //{
                //    //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                //    Text = c.FirstName + " " + c.LastName,
                //    Value = c.UserID.ToString(),
                //    Selected = true,
                //});


                //var Newterms = new SelectList(Option, "Value", "Text");
                //ViewBag.OptionList = Newterms;


            

            }
            else
            {

              //  List<AgencyContact> Option0 = new List<AgencyContact>();
             //   IEnumerable<AgencyContact> Option0 = new IEnumerable<AgencyContact>;
              // List Option0 = new List();
                //IEnumerable Option = db.AgencyContacts.Join(db.AgencyContactClinics,
                //          p => p.AgencyContactID,
                //          e => e.AgencyContactID,
                //          (p, e) => new { p, e }).Where(c => c.p.Active == true && c.e.ClinicID == id && c.e.ProgramBinID == id2).AsEnumerable().Select(c => new SelectListItem()

                //{
                //    //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
                //    Text = c.p.FirstName + " " + c.p.LastName,
                //    Value = c.p.AgencyContactID.ToString(),
                //    Selected = true,
                //});

                var options = (from c in db.AgencyContacts
                               join x in db.AgencyContactClinics
                               on c.AgencyContactID equals x.AgencyContactID
                               where x.ClinicID == id && x.ProgramBinID == id2
                               select new {

                                   Text = c.FirstName + " " + c.LastName,
                                   Value = c.AgencyContactID.ToString(),
                                   Selected = true
                               
                               }).AsEnumerable().Distinct();

                //foreach (SelectListItem up in Option)
                //{

                //    bool alreadyExists = Option0.Any(x => x.AgencyContactID == Convert.ToInt32(up.Value));

                //    if (alreadyExists == false)
                //    {
                //        var newItem = new SelectListItem { Text = "INFORMATIONAL", Value = "INFORMATIONAL" };
                //        Option0.(newItem);

                //    }

                //}


             
                var Newterms = new SelectList(options, "Value", "Text");
                ViewBag.OptionList = Newterms;


                var medout = (from x in db.AgencyContactClinics
                              where x.ClinicID == id && x.ProgramBinID == id2 && x.AgencyRoleBinID == id3
                              select x.AgencyContactID).Count();

                if (medout > 0)
                {


                    var rows =
               (from x in db.AgencyContactClinics
                where x.ClinicID == id && x.ProgramBinID == id2 && x.AgencyRoleBinID == id3
                select x.AgencyContactID).AsEnumerable().Distinct().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                    ViewBag.Options = rows;

                }

            }

            return PartialView();

        }


        // GET: AgencyContacts/Create
        public ActionResult _ContactInfo(int? id, int? id2, int? id3, int? id4)
        {

            ViewBag.SiteID = id;
            ViewBag.ProgramID = id2;
            ViewBag.AgencyContactID = id3;
            ViewBag.AgencyRoleID = id4;
            ViewBag.PossibleProgramBins = db.Programs.Where(x => x.Active == true);
            ViewBag.PossibleAgencyContacts = db.AgencyContacts.Where(x => x.Active == true && x.AgencyContactID != 2);
            ViewBag.PossibleAgencyRoleTypeBins = context.DDAgencyRoleTypeBins.Where(x => x.Active == true);

            var approleprograms = db.ApplicationRolePrograms.ToList();
            var agencyroles = db.RoleBins.ToList();


            // Get AgencyID
            //var possibleagencyrolebins = (from P in approleprograms
            //                              join u in agencyroles
            //                              on P.RoleID equals u.RoleBinID
            //                              where P.ProgramID == id2 && u.Active == true && u.RoleBinID != 2
            //                              select u).Distinct().ToList();

            var possibleagencyrolebins = (from P in db.RoleBins
                                          where P.Active == true && P.IsPortalUser == false
                                          select P).Distinct().ToList();

            ViewBag.PossibleAgencyRoleBins = possibleagencyrolebins;



            // Get AgencyID
            var agencyid = (from P in db.AgencySiteProgramSites
                            where P.SiteID == id
                            select P.AgencySiteID).First();


            List<SelectListItem> OptionFS = new List<SelectListItem>();

            var sitelist = (from x in db.Sites
                            //join y in context.ProgramSites
                            //on x.SiteID equals y.SiteID
                            join z in db.AgencySiteProgramSites
                            on x.SiteID equals z.SiteID
                            where x.Active == true && z.ProgramID == id2 && z.AgencySiteID == agencyid
                            select x).ToList();


            foreach (Site s in sitelist)
            {


                var site = (from x in db.Sites
                            where x.SiteID == s.SiteID
                            select x).SingleOrDefault();


                bool alreadyExists = OptionFS.Any(x => x.Value == Convert.ToString(site.SiteID));

                if (alreadyExists == false)
                {

                    OptionFS.Add(new SelectListItem { Text = site.SiteName, Value = Convert.ToString(site.SiteID) });

                }


            }


           var NewtermsFS = new SelectList(OptionFS, "Value", "Text");
           ViewBag.OptionListFS = NewtermsFS;


           // var medout = (from x in db.AgencySiteProgramSites
           //               where x.SiteID == id && x.ProgramID == id2 && x.AgencySiteID == agencyid
           //               select x.SiteID).Count();

           // if (medout > 0)
           // {


           //     var rows =
           //(from x in db.AgencySiteProgramSites
           // where x.SiteID == id && x.ProgramID == id2 && x.AgencySiteID == agencyid
           // select x.SiteID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


           //     ViewBag.Options = rows;

           // }


            List<SelectListItem> OptionFSA = new List<SelectListItem>();

            var rolelist = (from x in db.RoleBins
                            where x.Active == true && x.IsPortalUser == false && x.AgencyTypeRoleBinID == 5
                            select x).ToList();


            foreach (RoleBin r in rolelist)
            {


                var role = (from x in db.RoleBins
                            where x.RoleBinID == r.RoleBinID
                            select x).SingleOrDefault();


                bool alreadyExists = OptionFSA.Any(x => x.Value == Convert.ToString(role.RoleBinID));

                if (alreadyExists == false)
                {

                    OptionFSA.Add(new SelectListItem { Text = role.RoleBinName, Value = Convert.ToString(role.RoleBinID) });

                }


            }


            var NewtermsFSA = new SelectList(OptionFSA, "Value", "Text");
            ViewBag.OptionListFSA = NewtermsFSA;

            //if (id3 != null)
            //{


            //// Get Role
            //var agencyrolebin = (from P in db.AgencyContactClinics
            //                              where P.AgencyContactID == id3
            //                              select P.AgencyRoleBinID).Distinct().ToList();


            var agencycontacts = db.AgencyContacts.ToList();
            var agencycontactclinics = db.AgencyContactClinics.ToList();
           // var agencyroles = context.DDAgencyRoleBins.ToList();
            var agencyroletypes = context.DDAgencyRoleTypeBins.ToList();

            var agencycontact = (from x in agencycontacts
                                 join y in agencycontactclinics
                                 on x.AgencyContactID equals y.AgencyContactID
                                 join z in agencyroles
                                 on y.AgencyRoleBinID equals z.RoleBinID
                                 join k in agencyroletypes
                                 on z.AgencyTypeRoleBinID equals k.AgencyRoleTypeBinID
                                 where x.AgencyContactID == id3 && y.ClinicID == id && y.ProgramBinID == id2 && z.RoleBinID == id4 
                                 select new AgencyContactViewModel()
                                 {
                                     AgencyContactID = x.AgencyContactID,
                                     AgencyRoleBinID = y.AgencyRoleBinID,
                                     AgencyRoleTypeBinID = k.AgencyRoleTypeBinID,
                                     FirstName = x.FirstName,
                                     LastName = x.LastName,
                                     Title = x.Title,
                                     Phone = x.Phone,
                                     Fax = x.Fax,
                                     Email = x.Email,
                                     MITrainingDate = x.MITrainingDate,
                                     UpdatedBy = x.UpdatedBy,
                                     DateCreated = x.DateCreated,
                                     CreatedBy = x.CreatedBy

                                 }).SingleOrDefault();
        

              return PartialView(agencycontact);


            //}


            //return PartialView();


        }

        public ActionResult Details(int id, int? id2, int? id3)
        {

            ViewBag.SiteID = id;
            ViewBag.ProgramID = id2;
            ViewBag.AgencyContactID = id3;


            var agencycontacts = db.AgencyContacts.ToList();
            var agencycontactclinics = db.AgencyContactClinics.ToList();
            var agencyroles = db.RoleBins.ToList();
            var agencyroletypes = context.DDAgencyRoleTypeBins.ToList();

            var agencycontact = (from x in agencycontacts
                                 join y in agencycontactclinics
                                 on x.AgencyContactID equals y.AgencyContactID
                                 join z in agencyroles
                                 on y.AgencyRoleBinID equals z.RoleBinID
                                 join k in agencyroletypes
                                 on z.AgencyTypeRoleBinID equals k.AgencyRoleTypeBinID
                                 where x.AgencyContactID == id3
                                 select new AgencyContactViewModel()
                                 {
                                     AgencyContactID = x.AgencyContactID,
                                     AgencyRoleBinID = y.AgencyRoleBinID,
                                     AgencyRoleTypeBinID = k.AgencyRoleTypeBinID,
                                     FirstName = x.FirstName,
                                     LastName = x.LastName,
                                     Title = x.Title,
                                     Phone = x.Phone,
                                     Fax = x.Fax,
                                     Email = x.Email,
                                     MITrainingDate = x.MITrainingDate,
                                     UpdatedBy = x.UpdatedBy,
                                     DateCreated = x.DateCreated,
                                     CreatedBy = x.CreatedBy

                                 }).SingleOrDefault();

           
            return PartialView(agencycontact);


        }

        // GET: AgencyContacts/Create
        public ActionResult Create(int? id, int? id2, int? id3)
        {

            ViewBag.SiteID = id;
            ViewBag.ProgramID = id2;
            ViewBag.AgencyContactID = id3;

            ViewBag.PossibleProgramBins = db.Programs.Where(x => x.Active == true);
            ViewBag.PossibleAgencyContacts = db.AgencyContacts.Where(x => x.Active == true && x.AgencyContactID != 2);
            ViewBag.PossibleAgencyRoleTypeBins = context.DDAgencyRoleTypeBins.Where(x => x.Active == true);

            var approleprograms = db.ApplicationRolePrograms.ToList();
            var agencyroles = db.RoleBins.ToList();


            // Get AgencyID
            //var possibleagencyrolebins = (from P in approleprograms
            //                              join u in agencyroles
            //                              on P.RoleID equals u.RoleBinID
            //                              where P.ProgramID == id2 && u.Active == true && u.RoleBinID != 2
            //                              select u).Distinct().ToList();

            var possibleagencyrolebins = (from P in db.RoleBins
                                           where P.Active == true && P.IsPortalUser == false 
                                          select P).Distinct().ToList();

            ViewBag.PossibleAgencyRoleBins = possibleagencyrolebins;



            // Get AgencyID
            var agencyid = (from P in db.AgencySiteProgramSites
                            where P.SiteID == id
                            select P.AgencySiteID).First();


            List<SelectListItem> OptionFS = new List<SelectListItem>();

            var sitelist = (from x in db.Sites
                            //join y in context.ProgramSites
                            //on x.SiteID equals y.SiteID
                            join z in db.AgencySiteProgramSites
                            on x.SiteID equals z.SiteID
                            where x.Active == true && z.ProgramID == id2 && z.AgencySiteID == agencyid
                            select x).ToList();


            foreach (Site s in sitelist)
            {


                var site = (from x in db.Sites
                            where x.SiteID == s.SiteID
                            select x).SingleOrDefault();


                bool alreadyExists = OptionFS.Any(x => x.Value == Convert.ToString(site.SiteID));

                if (alreadyExists == false)
                {

                    OptionFS.Add(new SelectListItem { Text = site.SiteName, Value = Convert.ToString(site.SiteID) });

                }


            }


            var NewtermsFS = new SelectList(OptionFS, "Value", "Text");
            ViewBag.OptionListFS = NewtermsFS;



          
            return PartialView();


        }


        public JsonResult _AddAgencyContactF(AgencyContactViewModel agencycontactviewmodel)
        {

            if (ModelState.IsValid)
            {

                // Set Record Info
                string UserNameInit = @User.Identity.Name.ToString();
                DateTime CreatedInit = DateTime.Now;

                 var userid = (from x in db.AspNetUsers
                          where x.UserName == UserNameInit
                          select x).FirstOrDefault();



                 if (agencycontactviewmodel.AgencyContactID != null)
                 {


                     // Update Contact
                     AgencyContact agcontact = db.AgencyContacts.Single(x => x.AgencyContactID == agencycontactviewmodel.AgencyContactID);
                     agcontact.FirstName = agencycontactviewmodel.FirstName;
                     agcontact.LastName = agencycontactviewmodel.LastName;
                     agcontact.Title = agencycontactviewmodel.Title;
                     agcontact.Phone = agencycontactviewmodel.Phone;
                     agcontact.Fax = agencycontactviewmodel.Fax;
                     agcontact.Email = agencycontactviewmodel.Email;
                     agcontact.MITrainingDate = agencycontactviewmodel.MITrainingDate;
                     agcontact.DateUpdated = CreatedInit;
                     agcontact.UpdatedBy = UserNameInit;
                     db.SaveChanges();


                     return Json(new { Status = "Success", Modified = agencycontactviewmodel.AgencyContactID }, JsonRequestBehavior.AllowGet);

                 }
                 else
                 {

                     // Create Agency Contact

                     AgencyContact ac = new AgencyContact
                     {

                         FirstName = agencycontactviewmodel.FirstName,
                         LastName = agencycontactviewmodel.LastName,
                         Title = agencycontactviewmodel.Title,
                         Phone = agencycontactviewmodel.Phone,
                         Fax = agencycontactviewmodel.Fax,
                         Email = agencycontactviewmodel.Email,
                         Active = true,
                         MITrainingDate = agencycontactviewmodel.MITrainingDate,
                         DateCreated = CreatedInit,
                         CreatedBy = UserNameInit,
                         DateUpdated = CreatedInit,
                         UpdatedBy = UserNameInit

                     };


                     try
                     {
                         db.AgencyContacts.Add(ac);
                         db.SaveChanges();
                     }
                     catch (DbEntityValidationException dbEx)
                     {
                         foreach (var validationErrors in dbEx.EntityValidationErrors)
                         {
                             foreach (var validationError in validationErrors.ValidationErrors)
                             {
                                 Debug.WriteLine("Property: {0} Error: {1}",
                            validationError.PropertyName, validationError.ErrorMessage);
                             }
                         }
                     }


                     // Agency Contact ID
                     var ACID = ac.AgencyContactID;

                     // If Agency Wide Role, Get list of all sites for agency and add to them

                     if (agencycontactviewmodel.AgencyRoleTypeBinID == 5)
                     {

                         // get agency id
                         var agencyid = (from x in db.AgencySiteProgramSites
                                         where x.SiteID == agencycontactviewmodel.SiteID
                                         select x.AgencySiteID).First();


                        // get list of all sites for this agency
                        //var sitelist = (from x in db.AgencySiteProgramSites
                        //                where x.ProgramID == agencycontactviewmodel.ProgramBinID && x.AgencySiteID == agencyid
                        //                select x.SiteID).ToList();

                        var sitelist = (from x in db.Sites
                                        join z in db.AgencySiteProgramSites
                                        on x.SiteID equals z.SiteID
                                        where x.Active == true && z.ProgramID == agencycontactviewmodel.ProgramBinID && z.AgencySiteID == agencyid
                                        select x.SiteID).ToList();

                        foreach (int val in sitelist)
                         {

                             if (agencycontactviewmodel.AgencyRoleName != null)
                             {

                                 // Roles

                                 var stringToSplitA = agencycontactviewmodel.AgencyRoleName;

                                 var queryA = from valA in stringToSplitA.Split(',')
                                              select Convert.ToInt32(valA);
                                 foreach (int valA in queryA)
                                 {


                                     // Add to AgencyContactClinics

                                     AgencyContactClinics agencycontactclinics = new AgencyContactClinics
                                     {

                                         AgencyContactID = ACID,
                                         ClinicID = val,
                                         //  AgencyRoleBinID = Convert.ToInt32(agencycontactviewmodel.AgencyRoleBinID),
                                         AgencyRoleBinID = valA,
                                         ProgramBinID = Convert.ToInt32(agencycontactviewmodel.ProgramBinID)


                                     };

                                     db.AgencyContactClinics.Add(agencycontactclinics);
                                     db.SaveChanges();


                                 }


                             }

                         }


                     }
                     else
                     {

                         if (agencycontactviewmodel.SiteName != null)
                         {

                             // Sites

                             var stringToSplit = agencycontactviewmodel.SiteName;

                             var query = from val in stringToSplit.Split(',')
                                         select Convert.ToInt32(val);
                             foreach (int val in query)
                             {


                                 // Add to AgencyContactClinics

                                 AgencyContactClinics agencycontactclinics = new AgencyContactClinics
                                 {

                                     AgencyContactID = ACID,
                                     ClinicID = val,
                                     AgencyRoleBinID = Convert.ToInt32(agencycontactviewmodel.AgencyRoleBinID),
                                     ProgramBinID = Convert.ToInt32(agencycontactviewmodel.ProgramBinID)


                                 };

                                 db.AgencyContactClinics.Add(agencycontactclinics);
                                 db.SaveChanges();


                             }


                         }

                     }

                     return Json(new { Status = "Success", Modified = ACID }, JsonRequestBehavior.AllowGet);

                 }

               

            }


            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);


        }

        public JsonResult _UpdateAgencyRolesF(AgencyRoleViewModel agencyroleviewmodel)
        {

            if (ModelState.IsValid)
            {


                // Need to remove agency level contacts from all clinics in agency - then add them back later

                // Check for Agency Level Role Type
                var rolelist = (from B in db.RoleBins select B).ToList();
                var roletypelist = (from B in context.DDAgencyRoleTypeBins select B).ToList();

                var rolecheck1 = (from x in rolelist
                                  join y in roletypelist
                                  on x.AgencyTypeRoleBinID equals y.AgencyRoleTypeBinID
                                  where x.RoleBinID == agencyroleviewmodel.AgencyRoleBinID
                                  select y.AgencyRoleTypeBinID).SingleOrDefault();

                if (rolecheck1 == 5)
                {

                    // Get Agency
                    var agencyid1 = (from y in db.AgencySiteProgramSites
                                     where y.SiteID == agencyroleviewmodel.SiteID
                                     select y.AgencySiteID).First();


                    // Get List of Clinics
                    //var cliniclist1 = (from y in db.AgencySiteProgramSites
                    //                   where y.AgencySiteID == agencyid1
                    //                   select y).ToList();
                    var cliniclist1 = (from x in db.Sites
                                    join z in db.AgencySiteProgramSites
                                    on x.SiteID equals z.SiteID
                                    where x.Active == true && z.ProgramID == agencyroleviewmodel.ProgramID && z.AgencySiteID == agencyid1
                                    select z).ToList();


                    foreach (AgencySiteProgramSites cas in cliniclist1)
                    {

                        // Remove all personnel
                        var agencycontactlist = (from x in db.AgencyContactClinics
                                                 where x.ClinicID == cas.SiteID && x.ProgramBinID == agencyroleviewmodel.ProgramID && x.AgencyRoleBinID == agencyroleviewmodel.AgencyRoleBinID
                                                 //where x.ProgramBinID == agencyroleviewmodel.ProgramID && x.AgencyRoleBinID == agencyroleviewmodel.AgencyRoleBinID
                                                 select x).ToList();


                        foreach (AgencyContactClinics acc in agencycontactlist)
                        {

                            db.AgencyContactClinics.Remove(acc);
                            db.SaveChanges();

                        }


                    }


                }
                else
                {


                    // Remove all personnel
                    var agencycontactlist = (from x in db.AgencyContactClinics
                                             where x.ClinicID == agencyroleviewmodel.SiteID && x.ProgramBinID == agencyroleviewmodel.ProgramID && x.AgencyRoleBinID == agencyroleviewmodel.AgencyRoleBinID
                                             //where x.ProgramBinID == agencyroleviewmodel.ProgramID && x.AgencyRoleBinID == agencyroleviewmodel.AgencyRoleBinID
                                             select x).ToList();


                    foreach (AgencyContactClinics acc in agencycontactlist)
                    {

                        db.AgencyContactClinics.Remove(acc);
                        db.SaveChanges();

                    }


                }


                if (agencyroleviewmodel.UserList != null)
                {

                    // Agency Contacts

                    var stringToSplit = agencyroleviewmodel.UserList;

                    var query = from val in stringToSplit.Split(',')
                                select Convert.ToInt32(val);
                    foreach (int val in query)
                    {


                        var rolecheck = (from x in rolelist
                                          join y in roletypelist
                                          on x.AgencyTypeRoleBinID equals y.AgencyRoleTypeBinID
                                          where x.RoleBinID == agencyroleviewmodel.AgencyRoleBinID
                                          select y.AgencyRoleTypeBinID).SingleOrDefault();

                        if (rolecheck == 5)
                        {


                            // Loop through clinics and add agency level contacts to all clinics


                            // Get Agency
                            var agencyid = (from y in db.AgencySiteProgramSites
                                            where y.SiteID == agencyroleviewmodel.SiteID
                                            select y.AgencySiteID).First();


                            // Get List of Clinics
                            //var cliniclist = (from y in db.AgencySiteProgramSites
                            //                  where y.AgencySiteID == agencyid
                            //                  select y).ToList();
                            var cliniclist = (from x in db.Sites
                                               join z in db.AgencySiteProgramSites
                                               on x.SiteID equals z.SiteID
                                               where x.Active == true && z.ProgramID == agencyroleviewmodel.ProgramID && z.AgencySiteID == agencyid
                                               select z).ToList();


                            foreach (AgencySiteProgramSites cas in cliniclist)
                            {



                                var arout = (from x in db.AgencyContacts
                                             where x.AgencyContactID == val
                                             select x.AgencyContactID).FirstOrDefault();


                                AgencyContactClinics accl = new AgencyContactClinics
                                {

                                    AgencyContactID = arout,
                                    ClinicID = cas.SiteID,
                                    ProgramBinID = Convert.ToInt32(agencyroleviewmodel.ProgramID),
                                    AgencyRoleBinID = Convert.ToInt32(agencyroleviewmodel.AgencyRoleBinID)


                                };

                                db.AgencyContactClinics.Add(accl);
                                db.SaveChanges();



                            }


                        }
                        else
                        {

                            var arout = (from x in db.AgencyContacts
                                         where x.AgencyContactID == val
                                         select x.AgencyContactID).FirstOrDefault();


                            AgencyContactClinics accl = new AgencyContactClinics
                            {

                                AgencyContactID = arout,
                                ClinicID = Convert.ToInt32(agencyroleviewmodel.SiteID),
                                ProgramBinID = Convert.ToInt32(agencyroleviewmodel.ProgramID),
                                AgencyRoleBinID = Convert.ToInt32(agencyroleviewmodel.AgencyRoleBinID)


                            };

                            db.AgencyContactClinics.Add(accl);
                            db.SaveChanges();


                        }


                    }



                }



                return Json(new { Status = "Success", Modified = agencyroleviewmodel.SiteID }, JsonRequestBehavior.AllowGet);



            }


            return Json(new { Status = "Success", Modified = agencyroleviewmodel.SiteID }, JsonRequestBehavior.AllowGet);

        }
      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "AgencyContactID,UserId,FirstName,LastName,Title,Phone,Fax,Email,MITrainingDate,DateUpdated,UpdatedBy,DateCreated,CreatedBy,Active")] AgencyContact agencyContact)
        {
            if (ModelState.IsValid)
            {
                db.AgencyContacts.Add(agencyContact);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(agencyContact);
        }

        // GET: AgencyContacts/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgencyContact agencyContact = db.AgencyContacts.Find(id);
            if (agencyContact == null)
            {
                return HttpNotFound();
            }
            return View(agencyContact);
        }

        // POST: AgencyContacts/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "AgencyContactID,UserId,FirstName,LastName,Title,Phone,Fax,Email,MITrainingDate,DateUpdated,UpdatedBy,DateCreated,CreatedBy,Active")] AgencyContact agencyContact)
        {
            if (ModelState.IsValid)
            {
                db.Entry(agencyContact).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(agencyContact);
        }

        // GET: AgencyContacts/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            AgencyContact agencyContact = db.AgencyContacts.Find(id);
            if (agencyContact == null)
            {
                return HttpNotFound();
            }
            return View(agencyContact);
        }

        // POST: AgencyContacts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            AgencyContact agencyContact = db.AgencyContacts.Find(id);
            db.AgencyContacts.Remove(agencyContact);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
