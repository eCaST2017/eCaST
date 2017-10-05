
       

   
$(function () {

    var ACUrl = $("#ACUrl").val();
    $('#AccountControlContainer').load(ACUrl);

});

//$(function () {

//    var OPUrl = $("#OPUrl").val();
//    $('#OpportunityListContainer').load(OPUrl);

//});

//$(function () {

//    var FLUrl = $("#FLUrl").val();
//    $('#FamilyLeaderListContainer').load(FLUrl);

//});

//$(function () {

//    var ORUrl = $("#ORUrl").val();
//    $('#OrganizationListContainer').load(ORUrl);

//});


$(function () {

    $("#OrgLinkInline").click(function () {


        var ACOrgUrl = $("#ACOrgUrl").val();
        $('#AccountControlContainer').load(ACOrgUrl);


    });

});



  