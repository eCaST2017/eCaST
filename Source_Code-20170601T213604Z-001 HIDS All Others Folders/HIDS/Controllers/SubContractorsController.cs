using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Net;
using System.Web;
using System.Collections;
using System.Data.Entity.Core.Objects;
using System.Web.Security;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Web.Script.Serialization;
using System.Web.Mvc;
using CTL.ViewModels;
using CTL.Models;

namespace CTL.Controllers
{
    public class SubContractorsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private eCaSTBillingEntities dbBilling = new eCaSTBillingEntities();
        private eCaSTContextEntities edb = new eCaSTContextEntities();
        private HIDSEntities context = new HIDSEntities(); 

        // GET: SubContractors
        public ActionResult Index()
        {
            return View(db.SubContractors.ToList());
        }


        public ActionResult _FacilityList(int? id, int? id2, int? id3)
        {

            var facilitylist = (from P in db.SubContractorClinics
                                join Y in db.SubContractors
                                on P.SubContractorID equals Y.SubContractorID
                                where P.SiteID == id && P.ProgramID == id2 && P.CancerTypeBinID == id3
                                //select Y.SubContractorName).ToList();
                              //  select "<div class='btn btn-default'><span class='glyphicon glyphicon-map-marker'></span> <a disabled='disabled' style='text-decoration:none' href='#' id='EditAFacInfoButton_" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + "'" + "data-toggle='modal' data-target='#EditAFacInfoModal_" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + "'" + ">" + Y.SubContractorName + "</a></div>" + "<div class='modal fade' id='EditAFacInfoModal_" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + "'" + "tabindex='-1' role='dialog' aria-labelledby='myModalLabel' aria-hidden='true'><div class='modal-dialog'><div style='text-align:left' class='modal-content'><div class='modal-header'><h2 class='modal-title' id='myModalLabel'>Edit Facility Info</h2></div><div id='EditAFacInfoContainer_" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + "'" + "/>" + "</div></div></div>" + "<script type='text/javascript'>$(function () {$('#EditAFacInfoButton_" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + "'" + ").click(function () {" + "var listUrl = $('#listUrlF').val();" + "var slash = '/';" + "var idW =" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + ";" + "var id2=" + SqlFunctions.StringConvert((double)id).Trim() + ";" + "var id3=" + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + ";" + "var id4=" + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + ";" + "var res=listUrl.concat(slash,idW,slash,id2,slash,id3,slash,id4);" + "$('#EditAFacInfoContainer_" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + "').load(res);" + "});});</script>").ToList();
                                select "<div class='btn btn-default'><span class='glyphicon glyphicon-map-marker'></span> <a disabled='disabled' style='text-decoration:none' href='#' id='EditAFacInfoButton_" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + "'" + ">" + Y.SubContractorName + "</a></div>" + "<script type='text/javascript'>$(function () {$('#EditAFacInfoButton_" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + "'" + ").click(function (e) {" + "var listUrl = $('#listUrlF').val();" + "var slash = '?id=';var adv2 = '&id2=';var adv3 = '&id3=';var adv4 = '&id4=';" + "var idC =" + SqlFunctions.StringConvert((double)P.CancerTypeBinID).Trim() + ";" + "var idW =" + SqlFunctions.StringConvert((double)Y.SubContractorID).Trim() + ";" + "var id2=" + SqlFunctions.StringConvert((double)id2).Trim() + ";" + "var id3=" + SqlFunctions.StringConvert((double)id).Trim() + ";" + "var id4=" + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + ";" + "var res=listUrl.concat(slash,id3,adv2,id2,adv3,idC,adv4,idW);" + "$('#FacHolderContainer_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "'" + ").css('display','block');" + "$('#AddEditLabel_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)id2).Trim() + "'" + ").html(" + "'Edit Contact');" + "$('#FacInfoContainer_" + SqlFunctions.StringConvert((double)id).Trim() + SqlFunctions.StringConvert((double)P.ProgramID).Trim() + "').load(res);" + "e.preventDefault(); return false;});});</script>").ToList();
           // string[] ResultList = facilitylist;
            string ResultString = string.Join("<span style='color:#fff'>|</span>", facilitylist.ToArray());

            var faclist = (from c in db.SubContractorClinics
                           where c.SiteID == id && c.ProgramID == id2 && c.CancerTypeBinID == id3
                           select new CancerTypeViewModel()
                           {

                               FacilityList = ResultString


                           }).Distinct().ToList();



            return PartialView(faclist);

        }


        public ActionResult _FacilityEdit(int? id, int? id2, int? id3)
        {


            ViewBag.SiteID = id;
            ViewBag.ProgramID = id2;
            ViewBag.CancerTypeBinID = id3;
           

            var isclinic = (from P in db.SubContractors
                            join U in db.SubContractorClinics
                            on P.SubContractorID equals U.SubContractorID
                            where U.SiteID == id && U.ProgramID == id2 && U.CancerTypeBinID == id3 && U.IsClinic == true
                            select P.SubContractorName).SingleOrDefault();

            ViewBag.SCName = isclinic;


            //IEnumerable Option =

            //    db.SubContractors.Where(c => c.Active == true).AsEnumerable().Select(c => new SelectListItem()


            //    //(from P in context.SubContractors
            //    //          join R in context.SubContractorClinics
            //    //          on P.SubContractorID equals R.SubContractorID
            //    //          where P.Active == true && R.IsClinic == null
            //    //          select new SelectListItem()

            //    {
            //        //Text = objauth.ReplaceTagValueToTerm(c.TagValue, c.Term),
            //        Text = c.SubContractorName,
            //        Value = c.SubContractorID.ToString(),
            //        Selected = true


            //    });


            // Get Agency ID
            var agencyid = (from P in db.AgencySiteProgramSites
                            where P.SiteID == id 
                            select P.AgencySiteID).First();



            var Option = (from c in db.SubContractors
                           join x in db.SubContractorClinics
                           on c.SubContractorID equals x.SubContractorID
                           join y in db.Sites
                           on x.SiteID equals y.SiteID
                           join s in db.AgencySiteProgramSites
                           on y.SiteID equals s.SiteID
                           join k in db.AgencySites
                           on s.AgencySiteID equals k.AgencySiteID
                          // where x.SiteID == id && x.ProgramID == id2 && x.IsClinic == false
                          where s.AgencySiteID == agencyid && x.ProgramID == id2 && x.IsClinic == false
                           select new
                           {

                               Text = c.SubContractorName,
                               Value = c.SubContractorID.ToString(),
                               Selected = true

                           }).AsEnumerable().Distinct();



            var Newterms = new SelectList(Option, "Value", "Text");
            ViewBag.OptionList = Newterms;



            var medout = (from x in db.SubContractorClinics
                          where x.SiteID == id && x.ProgramID == id2 && x.CancerTypeBinID == id3 && x.IsClinic == false
                          select x.SubContractorID).Count();

            if (medout > 0)
            {


                var rows =
           (from x in db.SubContractorClinics
            where x.SiteID == id && x.ProgramID == id2 && x.CancerTypeBinID == id3 && x.IsClinic == false
            select x.SubContractorID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                ViewBag.Options = rows;

            }

            //var subcon = (from c in context.SubContractorClinics
            //                where c.ClinicID == id && c.ProgramID == id2 && c.CancerTypeBinID == id3
            //                select new CancerTypeViewModel()
            //                {

            //                 CancerTypeBinID = c.CancerTypeBinID,
            //                    ClinicID = c.ClinicID,
            //                ProgramID = c.ProgramID


            //                }).SingleOrDefault();




            return PartialView();

        }


        public ActionResult Details(int id, int? id2, int? id3, int? id4)
        {

            ViewBag.ClinicID = id2;
            ViewBag.CancerTypeBinID = id3;
            ViewBag.ProgramID = id4;

            SubContractor subcontractor = db.SubContractors.Single(x => x.SubContractorID == id);

            ViewBag.SubContractorID = subcontractor.SubContractorID;

            var CCSPcheck = (from c in db.Sites
                             join cc in db.SubContractorClinics
                             on c.SiteID equals cc.SiteID
                             where cc.SubContractorID == id && cc.ProgramID == 18
                             select c).Count();

            if (CCSPcheck > 0)
            {

                ViewBag.ProviderCheck = true;
            }
            else
            {

                ViewBag.ProviderCheck = false;

            }


            // Check for WiWo Contract

            var agency = (from Cc in db.AgencySiteProgramSites
                          where Cc.SiteID == id2
                          select Cc.AgencySiteID).SingleOrDefault();

            var contractcheck = (from Cc in dbBilling.Contracts
                                 join Dd in dbBilling.DD_ContractTypes
                                 on Cc.TypeID equals Dd.ID
                                 where Cc.AgencySiteID == agency && Cc.TypeID == 3 && Dd.Active == true
                                 select Cc.ID).Count();

            ViewBag.ContractCheck = contractcheck;

            List<SelectListItem> provrs = new List<SelectListItem>();

            var providerlist = (from c in db.Providers
                                join cc in db.ProviderProfileProviders
                                on c.ProviderID equals cc.ProviderID
                                //join dd in context.ProviderProfiles
                                //on cc.ProviderProfileID equals dd.ProviderProfileID
                                where cc.ProviderProfileID != 1
                                select c).Distinct().ToList();

            int pcount = providerlist.Count();

            foreach (Provider prov in providerlist)
            {

                //var provs =
                // (
                //     from provider in context.Providers
                //     where provider.ProviderID == prov.ProviderID
                //     select new
                //     {
                //         Id = provider.ProviderID,
                //         Name = provider.ProviderName

                //     }
                // ).Distinct();


                //if (provs.Count() > 0)
                //{
                int ProvId = prov.ProviderID;
                string PID = ProvId.ToString();
                string ProviderName = prov.ProviderName;

                bool alreadyExists = provrs.Any(x => x.Value == PID);

                if (alreadyExists == false)
                {



                    provrs.Add(new SelectListItem
                    {
                        Selected = false,
                        Text = ProviderName,
                        Value = PID.ToString()
                    });



                }
                // }



            }


            //IEnumerable Option = 

            //    context.Providers
            //    .Join(context.ProviderProfileProviders,
            //    sc => sc.ProviderID,
            //    soc => soc.ProviderID,
            //    (sc, soc) => new { 
            //        sc,soc

            //         }).Where(z => z.soc.ProviderProfileID != 1).AsEnumerable().Select(c => new SelectListItem()
            //{

            //    Text = c.sc.ProviderName,
            //    Value = c.sc.ProviderID.ToString(),
            //    Selected = true,
            //});





            //var Newterms = new SelectList(Option, "Value", "Text");
            var Newterms = new SelectList(provrs, "Value", "Text");
            ViewBag.OptionList = Newterms;


            var medout = (from x in db.ProviderSubContractors
                          join y in db.ProviderProfileProviders
                          on x.ProviderID equals y.ProviderID
                          where x.SubContractorID == id && y.ProviderProfileID != 1 && x.ProgramID == 18
                          select x.ProviderID).Count();

            if (medout > 0)
            {

                var rows =
           (from x in db.ProviderSubContractors
            join y in db.ProviderProfileProviders
            on x.ProviderID equals y.ProviderID
            where x.SubContractorID == id && y.ProviderProfileID != 1 && x.ProgramID == 18
            select x.ProviderID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                ViewBag.Options = rows;

            }

            // Pathology Providers
            IEnumerable OptionPath =

               db.Providers
               .Join(db.ProviderProfileProviders,
               sc => sc.ProviderID,
               soc => soc.ProviderID,
               (sc, soc) => new
               {
                   sc,
                   soc

               }).Where(z => z.soc.ProviderProfileID == 1).AsEnumerable().Select(c => new SelectListItem()
               {

                   Text = c.sc.ProviderName,
                   Value = c.sc.ProviderID.ToString(),
                   Selected = true,
               });


            var NewtermsPath = new SelectList(OptionPath, "Value", "Text");
            ViewBag.OptionListPATH = NewtermsPath;


            var medoutPath = (from x in db.ProviderSubContractors
                              join y in db.ProviderProfileProviders
                          on x.ProviderID equals y.ProviderID
                              where x.SubContractorID == id && y.ProviderProfileID == 1
                              select x.ProviderID).Count();

            if (medoutPath > 0)
            {

                var rows =
           (from x in db.ProviderSubContractors
            join y in db.ProviderProfileProviders
            on x.ProviderID equals y.ProviderID
            where x.SubContractorID == id && y.ProviderProfileID == 1
            select x.ProviderID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                ViewBag.OptionsPath = rows;

            }





            var medoutC = (from x in db.ProviderSubContractors
                           join y in db.ProviderProfileProviders
                           on x.ProviderID equals y.ProviderID
                           where x.SubContractorID == id && y.ProviderProfileID != 1 && x.ProgramID == 20
                           select x.ProviderID).Count();

            if (medoutC > 0)
            {

                var rows =
           (from x in db.ProviderSubContractors
            join y in db.ProviderProfileProviders
            on x.ProviderID equals y.ProviderID
            where x.SubContractorID == id && y.ProviderProfileID != 1 && x.ProgramID == 20
            select x.ProviderID).AsEnumerable().Select(i => i.ToString()).Aggregate((i, next) => i + "," + next);


                ViewBag.OptionsC = rows;

            }



            PopulateInitialCityData();
            PopulateInitialStateData();
            PopulateInitialCountyData();

            ViewBag.CityBinID = subcontractor.CityBinID;
            ViewBag.StateBinID = subcontractor.StateBinID;
            ViewBag.CountyBinID = subcontractor.CountyBinID;



            return PartialView(subcontractor);


        }


        private void PopulateInitialCityData()
        {

            var allCities = context.DDCityBins;
            var viewModel = new List<AssignedCityData>();
            foreach (var city in allCities)
            {

                viewModel.Add(new AssignedCityData
                {
                    CityBinID = city.CityBinID,
                    CityBinName = city.CityBinName,

                });

            }

            ViewBag.PossibleCityBins = viewModel;
        }


        private void PopulateInitialStateData()
        {

            var allStates = context.DDStateBins;
            var viewModel = new List<AssignedStateData>();
            foreach (var state in allStates)
            {

                viewModel.Add(new AssignedStateData
                {
                    StateBinID = state.StateBinID,
                    StateBinName = state.StateBinName,

                });

            }

            ViewBag.PossibleStateBins = viewModel;
        }



        private void PopulateInitialCountyData()
        {

            var allCounties = context.DDCountyBins;
            var viewModel = new List<AssignedCountyData>();
            foreach (var county in allCounties)
            {

                viewModel.Add(new AssignedCountyData
                {
                    CountyBinID = county.CountyBinID,
                    CountyBinName = county.CountyBinName,

                });

            }

            ViewBag.PossibleCountyBins = viewModel;
        }


        // GET: SubContractors/Create
        public ActionResult _FacilityInfo(int? id, int? id2, int? id3, int? id4)
        {

            ViewBag.SiteID = id;
            ViewBag.ProgramID = id2;
            ViewBag.CancerTypeBinID = id3;
            ViewBag.SubContractorID = id4;
            PopulateInitialCityData();
            PopulateInitialStateData();
            PopulateInitialCountyData();


            // Check for WIWO Contract
            // Get AgencyID
            var agencyid = (from P in db.AgencySiteProgramSites
                            where P.SiteID == id
                            select P.AgencySiteID).First();

            var contractcheck = (from P in dbBilling.Contracts
                                 where P.AgencySiteID == agencyid && P.TypeID == 3
                                 select P.ID).Count();

            if (contractcheck > 0)
            {

                IEnumerable OptionF = edb.DDCancerTypeBins.Where(c => c.Active == true).OrderBy(c => c.CancerTypeBinName).AsEnumerable().Select(c => new SelectListItem()

                {

                    Text = c.CancerTypeBinName,
                    Value = c.CancerTypeBinID.ToString(),
                    Selected = true,
                });


                var NewtermsF = new SelectList(OptionF, "Value", "Text");
                ViewBag.OptionListF = NewtermsF;

            }
            else
            {

                IEnumerable OptionF = edb.DDCancerTypeBins.Where(c => c.Active == true && c.CancerTypeBinID != 4).OrderBy(c => c.CancerTypeBinName).AsEnumerable().Select(c => new SelectListItem()

                {

                    Text = c.CancerTypeBinName,
                    Value = c.CancerTypeBinID.ToString(),
                    Selected = true,
                });


                var NewtermsF = new SelectList(OptionF, "Value", "Text");
                ViewBag.OptionListF = NewtermsF;


            }



            var subcontractor = (from x in db.SubContractors
                                 where x.SubContractorID == id4
                                 select new SubContractorViewModel()
                                 {
                                     SubContractorID = x.SubContractorID,
                                     SubContractorName = x.SubContractorName,
                                     Address = x.Address,
                                     CityBinID = x.CityBinID,
                                     StateBinID = x.StateBinID,
                                     CountyBinID = x.CountyBinID,
                                     ZipCode = x.ZipCode

                                 }).SingleOrDefault();





            return PartialView(subcontractor);
        }

        // GET: SubContractors/Create
        public ActionResult Create(int? id, int? id2, int? id3)
        {

            ViewBag.SiteID = id;
            ViewBag.ProgramID = id2;
            ViewBag.SubContractorID = id3;
            PopulateInitialCityData();
            PopulateInitialStateData();
            PopulateInitialCountyData();


            // Check for WIWO Contract
            // Get AgencyID
            var agencyid = (from P in db.AgencySiteProgramSites
                            where P.SiteID == id
                            select P.AgencySiteID).First();

            var contractcheck = (from P in dbBilling.Contracts
                            where P.AgencySiteID == agencyid && P.TypeID == 3
                            select P.ID).Count();

            if (contractcheck > 0)
            {

                IEnumerable OptionF = edb.DDCancerTypeBins.Where(c => c.Active == true).OrderBy(c => c.CancerTypeBinName).AsEnumerable().Select(c => new SelectListItem()

                {

                    Text = c.CancerTypeBinName,
                    Value = c.CancerTypeBinID.ToString(),
                    Selected = true,
                });


                var NewtermsF = new SelectList(OptionF, "Value", "Text");
                ViewBag.OptionListF = NewtermsF;

            }
            else
            {

                IEnumerable OptionF = edb.DDCancerTypeBins.Where(c => c.Active == true && c.CancerTypeBinID != 4).OrderBy(c => c.CancerTypeBinName).AsEnumerable().Select(c => new SelectListItem()

                {

                    Text = c.CancerTypeBinName,
                    Value = c.CancerTypeBinID.ToString(),
                    Selected = true,
                });


                var NewtermsF = new SelectList(OptionF, "Value", "Text");
                ViewBag.OptionListF = NewtermsF;


            }
           

            return PartialView();
        }



        public JsonResult _AddSubContractorF(SubContractorViewModel subcontractorviewmodel)
        {

            if (ModelState.IsValid)
            {

                // Set Record Info
                string UserNameInit = @User.Identity.Name.ToString();
                DateTime CreatedInit = DateTime.Now;

                var userid = (from x in db.AspNetUsers
                              where x.UserName == UserNameInit
                              select x).FirstOrDefault();



                if (subcontractorviewmodel.SubContractorID != null)
                {


                    // Update SubContractor


                    SubContractor subcontractor = db.SubContractors.Single(x => x.SubContractorID == subcontractorviewmodel.SubContractorID);
                    subcontractor.SubContractorName = subcontractorviewmodel.SubContractorName;
                    subcontractor.Active = subcontractorviewmodel.Active;
                    subcontractor.Address = subcontractorviewmodel.Address;
                    subcontractor.CityBinID = subcontractorviewmodel.CityBinID;
                    subcontractor.StateBinID = subcontractorviewmodel.StateBinID;
                    subcontractor.ZipCode = subcontractorviewmodel.ZipCode;
                    subcontractor.CountyBinID = subcontractorviewmodel.CountyBinID;
                    subcontractor.DateUpdated = CreatedInit;
                    subcontractor.UpdatedBy = UserNameInit;
                    db.SaveChanges();


                    return Json(new { Status = "Success", Modified = subcontractorviewmodel.SubContractorID }, JsonRequestBehavior.AllowGet);

                }
                else
                {

                    // Create SubContractor

                    SubContractor sc = new SubContractor
                    {

                        SubContractorName = subcontractorviewmodel.SubContractorName,
                        Active = subcontractorviewmodel.Active,
                        Address = subcontractorviewmodel.Address,
                        CityBinID = subcontractorviewmodel.CityBinID,
                        StateBinID = subcontractorviewmodel.StateBinID,
                        ZipCode = subcontractorviewmodel.ZipCode,
                        CountyBinID = subcontractorviewmodel.CountyBinID,
                        DateUpdated = CreatedInit,
                        UpdatedBy = UserNameInit

                    };


                    try
                    {
                        db.SubContractors.Add(sc);
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

                    // SubContractor ID
                    var SCID = sc.SubContractorID;

                  
                            if (subcontractorviewmodel.CancerTypeName != null)
                            {

                                // Cancer Types

                                var stringToSplitA = subcontractorviewmodel.CancerTypeName;

                                var queryA = from valA in stringToSplitA.Split(',')
                                             select Convert.ToInt32(valA);
                                foreach (int valA in queryA)
                                {


                                    // Add to AgencyContactClinics

                                    SubContractorClinics subcontractorclinics = new SubContractorClinics
                                    {

                                        SubContractorID = SCID,
                                        SiteID = subcontractorviewmodel.SiteID,
                                        ClinicID = Convert.ToInt32(subcontractorviewmodel.SiteID),
                                        ProgramID = Convert.ToInt32(subcontractorviewmodel.ProgramID),
                                        CancerTypeBinID = valA,
                                        IsClinic = false
                                        
                                    };

                                    db.SubContractorClinics.Add(subcontractorclinics);
                                    db.SaveChanges();


                                }


                            }



                            return Json(new { Status = "Success", Modified = subcontractorviewmodel.SubContractorID }, JsonRequestBehavior.AllowGet);

                }



            }


            return Json(new { Status = "Success" }, JsonRequestBehavior.AllowGet);


        }


        public JsonResult _UpdateSubContractorsF(CancerTypeViewModel cancertypeviewmodel)
        {

            if (ModelState.IsValid)
            {

                    // Get List of SubContractors
                    var cliniclist1 = (from y in db.SubContractorClinics
                                       where y.SiteID == cancertypeviewmodel.SiteID
                                       select y).ToList();


                    foreach (SubContractorClinics cas in cliniclist1)
                    {

                        // Remove subcontractors
                        var subcontractorlist = (from x in db.SubContractorClinics
                                                 where x.SiteID == cas.SiteID && x.ProgramID == cancertypeviewmodel.ProgramID && x.CancerTypeBinID == cancertypeviewmodel.CancerTypeBinID && x.IsClinic == false
                                                 //where x.ProgramBinID == agencyroleviewmodel.ProgramID && x.AgencyRoleBinID == agencyroleviewmodel.AgencyRoleBinID
                                                 select x).ToList();


                        foreach (SubContractorClinics acc in subcontractorlist)
                        {

                            db.SubContractorClinics.Remove(acc);
                            db.SaveChanges();

                        }


                    }

                    if (cancertypeviewmodel.FacilityList != null)
                    {

                        // Facilities
                        var stringToSplit = cancertypeviewmodel.FacilityList;

                        var query = from val in stringToSplit.Split(',')
                                    select Convert.ToInt32(val);
                        foreach (int val in query)
                        {

                            // Loop through subcontractors
                            SubContractorClinics scc = new SubContractorClinics
                            {

                                SubContractorID = val,
                                SiteID = cancertypeviewmodel.SiteID,
                                ClinicID = Convert.ToInt32(cancertypeviewmodel.SiteID),
                                ProgramID = Convert.ToInt32(cancertypeviewmodel.ProgramID),
                                CancerTypeBinID = Convert.ToInt32(cancertypeviewmodel.CancerTypeBinID),
                                IsClinic = false

                            };

                            db.SubContractorClinics.Add(scc);
                            db.SaveChanges();



                        }


                    }


                return Json(new { Status = "Success", Modified = cancertypeviewmodel.SiteID }, JsonRequestBehavior.AllowGet);

            }


            return Json(new { Status = "Success", Modified = cancertypeviewmodel.SiteID }, JsonRequestBehavior.AllowGet);

        }

       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SubContractorID,LegacySubContractorCodeCCSP,LegacySubContractorCodeWWC,LegacySubContractorCodeCRCCP,SubContractorName,Address,CityBinID,StateBinID,ZipCode,ContactName,Phone,CountyBinID,Active,DateUpdated,UpdatedBy,DigitalMamIndicator,DigitalTechCode")] SubContractor subContractor)
        {
            if (ModelState.IsValid)
            {
                db.SubContractors.Add(subContractor);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subContractor);
        }

        // GET: SubContractors/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubContractor subContractor = db.SubContractors.Find(id);
            if (subContractor == null)
            {
                return HttpNotFound();
            }
            return View(subContractor);
        }

        // POST: SubContractors/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SubContractorID,LegacySubContractorCodeCCSP,LegacySubContractorCodeWWC,LegacySubContractorCodeCRCCP,SubContractorName,Address,CityBinID,StateBinID,ZipCode,ContactName,Phone,CountyBinID,Active,DateUpdated,UpdatedBy,DigitalMamIndicator,DigitalTechCode")] SubContractor subContractor)
        {
            if (ModelState.IsValid)
            {
                db.Entry(subContractor).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(subContractor);
        }

        // GET: SubContractors/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            SubContractor subContractor = db.SubContractors.Find(id);
            if (subContractor == null)
            {
                return HttpNotFound();
            }
            return View(subContractor);
        }

        // POST: SubContractors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            SubContractor subContractor = db.SubContractors.Find(id);
            db.SubContractors.Remove(subContractor);
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
