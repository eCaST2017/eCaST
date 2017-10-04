


// Slide controls for Clinical Measurements

$(function () {

  
    $('#CholMedYesDays').slider()
.on('slide', function (ev) {
    $('#CholMedYesValueIDHIDDEN').val(ev.value);
});

    $('#HyperTensionMedYesDays').slider()
.on('slide', function (ev) {
    $('#HyperTensionMedYesValueIDHIDDEN').val(ev.value);
});

    $('#DiabetesMedYesDays').slider()
.on('slide', function (ev) {
    $('#DiabetesMedYesValueIDHIDDEN').val(ev.value);
});

    $('#FruitCups').slider()
.on('slide', function (ev) {
    $('#FruitValueIDHIDDEN').val(ev.value);
});

    $('#VegCups').slider()
.on('slide', function (ev) {
    $('#VegValueIDHIDDEN').val(ev.value);
});

    $('#ModActivity').slider()
.on('slide', function (ev) {
    $('#ModerateActivityValueIDHIDDEN').val(ev.value);
});

    $('#VigActivity').slider()
.on('slide', function (ev) {
    $('#VigorousActivityValueIDHIDDEN').val(ev.value);
});

//    $('#SecondHand').slider()
//.on('slide', function (ev) {
//    $('#SmokingSecondHandValueIDHIDDEN').val(ev.value);
    //});


    // To Account for Non Answer
    if ($("#checkboxSMOKINGSECONDHANDCHECK").is(':checked')) {


        var physval = $('#SmokingSecondHandValueIDNS').val();
        $('#SmokingSecondHandValueIDHIDDEN').val(physval);



    }
    else {

        $('#SecondHand').slider()
.on('slide', function (ev) {
    $('#SmokingSecondHandValueIDHIDDEN').val(ev.value);
});


    }

//    $('#systolic1').slider()
// .on('slide', function (ev) {
//     $('#SystolicReading').val(ev.value);
// });


//    $('#diastolic1').slider()
//.on('slide', function (ev) {
//    $('#DiastolicReading').val(ev.value);
//});


//    $('#PhysicalHealth').slider()
//.on('slide', function (ev) {
//    $('#PhysicalHealthValueIDHIDDEN').val(ev.value);
//});


    // To Account for Non Answer
    if ($("#checkboxPHYSICALHEALTHCHECK").is(':checked')) {


        var physval = $('#PhysicalHealthValueIDNS').val();
        $('#PhysicalHealthValueIDHIDDEN').val(physval);



    }
    else {

        $('#PhysicalHealth').slider()
.on('slide', function (ev) {
    $('#PhysicalHealthValueIDHIDDEN').val(ev.value);
});


    }



    // To Account for Non Answer
    if ($("#checkboxMENTALHEALTHCHECK").is(':checked')) {


        var mentalval = $('#MentalHealthValueIDNS').val();
        $('#MentalHealthValueIDHIDDEN').val(mentalval);



    }
    else {

        $('#MentalHealth').slider()
    .on('slide', function (ev) {
        $('#MentalHealthValueIDHIDDEN').val(ev.value);
    });


    }


    // To Account for Non Answer
    if ($("#checkboxMENTALPHYSICALHEALTHCHECK").is(':checked')) {


        var mentalphysval = $('#MentalPhysicalHealthValueIDNS').val();
        $('#MentalPhysicalHealthValueIDHIDDEN').val(mentalphysval);



    }
    else {

        $('#MentalPhysicalHealth').slider()
    .on('slide', function (ev) {
        $('#MentalPhysicalHealthValueIDHIDDEN').val(ev.value);
    });


    }


    // Edit Screen
    $('#heightE').slider()
  .on('slide', function (ev) {
      $('#DDHeightBinIDHIDDENE').val(ev.value);
  });


    $('#weightE').slider()
.on('slide', function (ev) {
    $('#DDWeightBinIDHIDDENE').val(ev.value);
});


    $('#waistE').slider()
.on('slide', function (ev) {
    $('#DDWaistBinIDHIDDENE').val(ev.value);
});


    $('#hipE').slider()
.on('slide', function (ev) {
    $('#DDHipBinIDHIDDENE').val(ev.value);
});


    $('#CholMedYesDaysE').slider()
.on('slide', function (ev) {
    $('#CholMedYesValueIDHIDDENE').val(ev.value);
});

    $('#HyperTensionMedYesDaysE').slider()
.on('slide', function (ev) {
    $('#HyperTensionMedYesValueIDHIDDENE').val(ev.value);
});

    $('#DiabetesMedYesDaysE').slider()
.on('slide', function (ev) {
    $('#DiabetesMedYesValueIDHIDDENE').val(ev.value);
});

    $('#FruitCupsE').slider()
.on('slide', function (ev) {
    $('#FruitValueIDHIDDENE').val(ev.value);
});

    $('#VegCupsE').slider()
.on('slide', function (ev) {
    $('#VegValueIDHIDDENE').val(ev.value);
});

    $('#ModActivityE').slider()
.on('slide', function (ev) {
    $('#ModerateActivityValueIDHIDDENE').val(ev.value);
});

    $('#VigActivityE').slider()
.on('slide', function (ev) {
    $('#VigorousActivityValueIDHIDDENE').val(ev.value);
});

//    $('#SecondHandE').slider()
//.on('slide', function (ev) {
//    $('#SmokingSecondHandValueIDHIDDENE').val(ev.value);
//});



    // To Account for Non Answer
    if ($("#checkboxSMOKINGSECONDHANDCHECK").is(':checked')) {


        var physval = $('#SmokingSecondHandValueIDNS').val();
        $('#SmokingSecondHandValueIDHIDDENE').val(physval);



    }
    else {

        $('#SecondHandE').slider()
.on('slide', function (ev) {
    $('#SmokingSecondHandValueIDHIDDENE').val(ev.value);
});


    }



    // To Account for Non Answer
    if ($("#checkboxPHYSICALHEALTHCHECK").is(':checked')) {

    
        var physval = $('#PhysicalHealthValueIDNS').val();
        $('#PhysicalHealthValueIDHIDDENE').val(physval);



    }
    else {

        $('#PhysicalHealthE').slider()
.on('slide', function (ev) {
    $('#PhysicalHealthValueIDHIDDENE').val(ev.value);
});


    }


    // To Account for Non Answer
    if ($("#checkboxMENTALHEALTHCHECK").is(':checked')) {


        var physval = $('#MENTALHealthValueIDNS').val();
        $('#MentalHealthValueIDHIDDENE').val(physval);



    }
    else {


        $('#MentalHealthE').slider()
    .on('slide', function (ev) {
        $('#MentalHealthValueIDHIDDENE').val(ev.value);
    });


    }


    // To Account for Non Answer
    if ($("#checkboxMENTALPHYSICALHEALTHCHECK").is(':checked')) {


        var physval = $('#MENTALPHYSICALHealthValueIDNS').val();
        $('#MentalPhysicalHealthValueIDHIDDENE').val(physval);



    }
    else {

        $('#MentalPhysicalHealthE').slider()
    .on('slide', function (ev) {
        $('#MentalPhysicalHealthValueIDHIDDENE').val(ev.value);
    });


    }

    // Add Testing Screen

    $('#systolic1').slider()
.on('slide', function (ev) {
    $('#SystolicBinID1HIDDEN').val(ev.value);
});


    $('#diastolic1').slider()
   .on('slide', function (ev) {
       $('#DiastolicBinID1HIDDEN').val(ev.value);
   });


  
    $('#systolic2').slider()
  .on('slide', function (ev) {
      $('#SystolicBinID2HIDDEN').val(ev.value);
  });


    $('#diastolic2').slider()
   .on('slide', function (ev) {
       $('#DiastolicBinID2HIDDEN').val(ev.value);
   });

    // Cholesterol
    $('#totalchol').slider()
   .on('slide', function (ev) {
       $('#TotalCholesterolBinIDHIDDEN').val(ev.value);
   });


    $('#hdlchol').slider()
   .on('slide', function (ev) {
       $('#HDLCholesterolBinIDHIDDEN').val(ev.value);
   });


    $('#ldlchol').slider()
  .on('slide', function (ev) {
      $('#LDLCholesterolBinIDHIDDEN').val(ev.value);
  });


    $('#trichol').slider()
   .on('slide', function (ev) {
       $('#TriglyceridesBinIDHIDDEN').val(ev.value);
   });


    // Glucose
    $('#glucose').slider()
   .on('slide', function (ev) {
       $('#GlucoseBinIDHIDDEN').val(ev.value);
   });


    $('#a1c').slider()
   .on('slide', function (ev) {
       $('#A1CPercentageBinIDHIDDEN').val(ev.value);
   });


    // Support Sessions

    // Session length
    $('#length').slider()
    .on('slide', function (ev) {
        $('#HCHBSessionLengthBinIDHIDDEN').val(ev.value);
    });


    // Patient Confidence
    $('#confidence').slider()
    .on('slide', function (ev) {
        $('#HCPatientConfidenceHIDDEN').val(ev.value);
    });

    // Patient Importance
    $('#importance').slider()
    .on('slide', function (ev) {
        $('#HCPatientImportanceHIDDEN').val(ev.value);
    });
  
});



$(function () {

    // Cholesterol load
    if ($("#HighCholValueID").val() == 1) {

        $("#CholMedSection").css("display", "block"),
        $("#CholMedYesSection").css("display", "none"),
        $("#CholMedNoSection").css("display", "none");


    }
    else if ($("#HighCholValueID").val() == 2) {

        $("#CholMedSection").css("display", "none"),
        $("#CholMedYesSection").css("display", "none"),
        $("#CholMedNoSection").css("display", "none");

    }

    else {

        $("#CholMedSection").css("display", "none"),
        $("#CholMedYesSection").css("display", "none"),
       $("#CholMedNoSection").css("display", "none");


    }


    // Cholesterol Button Click
    $("#HighCholValueID").change(function () {


        if ($("#HighCholValueID").val() == 1) {

            $("#CholMedSection").css("display", "block"),
            $("#CholMedYesSection").css("display", "none"),
            $("#CholMedNoSection").css("display", "none");


        }
        else if ($("#HighCholValueID").val() == 2) {

            $("#CholMedSection").css("display", "none"),
            $("#CholMedYesSection").css("display", "none"),
            $("#CholMedNoSection").css("display", "none");

        }

        else {

            $("#CholMedSection").css("display", "none"),
            $("#CholMedYesSection").css("display", "none"),
           $("#CholMedNoSection").css("display", "none");


        }


    });



    // Cholesterol Med load
    if ($("#CholMedValueID").val() == 1) {

        $("#CholMedSection").css("display", "block"),
        $("#CholMedYesSection").css("display", "block"),
        $("#CholMedNoSection").css("display", "none");


    }
    else if ($("#CholMedValueID").val() == 2) {

        $("#CholMedSection").css("display", "block"),
        $("#CholMedYesSection").css("display", "none"),
        $("#CholMedNoSection").css("display", "none");

    }
    else if ($("#CholMedValueID").val() == 3) {

        $("#CholMedSection").css("display", "block"),
        $("#CholMedYesSection").css("display", "none"),
        $("#CholMedNoSection").css("display", "none");

    }
    else if ($("#CholMedValueID").val() == 5) {

        $("#CholMedSection").css("display", "block"),
        $("#CholMedYesSection").css("display", "none"),
        $("#CholMedNoSection").css("display", "none");

    }
    else if ($("#CholMedValueID").val() == 7) {

        $("#CholMedSection").css("display", "block"),
        $("#CholMedYesSection").css("display", "none"),
        $("#CholMedNoSection").css("display", "none");

    }
    else if ($("#CholMedValueID").val() == 8) {

        $("#CholMedSection").css("display", "block"),
        $("#CholMedYesSection").css("display", "none"),
        $("#CholMedNoSection").css("display", "none");

    }
    else if ($("#CholMedValueID").val() == 9) {

        $("#CholMedSection").css("display", "block"),
        $("#CholMedYesSection").css("display", "none"),
        $("#CholMedNoSection").css("display", "none");

    }

    else {

        $("#CholMedSection").css("display", "none"),
        $("#CholMedYesSection").css("display", "none"),
       $("#CholMedNoSection").css("display", "none");


    }



    // Cholesterol Med Button Click
    $("#CholMedValueID").change(function () {

       
        if ($("#CholMedValueID").val() == 1) {

            $("#CholMedSection").css("display", "block"),
            $("#CholMedYesSection").css("display", "block"),
            $("#CholMedNoSection").css("display", "none");


            }
        else if ($("#CholMedValueID").val() == 2) {

            $("#CholMedSection").css("display", "block"),
            $("#CholMedYesSection").css("display", "none"),
            $("#CholMedNoSection").css("display", "none");

        }

        else{

            $("#CholMedSection").css("display", "block"),
            $("#CholMedYesSection").css("display", "none"),
           $("#CholMedNoSection").css("display", "none");


        }


    });

    // Hypertension Load
    if ($("#HyperTensionValueID").val() == 1) {

        $("#HyperTensionMedSection").css("display", "block"),
        $("#HyperTensionMeasureSection").css("display", "block"),
        $("#HyperTensionMedYesSection").css("display", "none");



    }
    else if ($("#HyperTensionValueID").val() == 2) {

        $("#HyperTensionMedSection").css("display", "none"),
        $("#HyperTensionMeasureSection").css("display", "none"),
        $("#HyperTensionMedYesSection").css("display", "none");


    }

    else {

        $("#HyperTensionMedSection").css("display", "none"),
        $("#HyperTensionMeasureSection").css("display", "none"),
        $("#HyperTensionMedYesSection").css("display", "none");



    }


    // HyperTension Button Click
    $("#HyperTensionValueID").change(function () {


        if ($("#HyperTensionValueID").val() == 1) {

            $("#HyperTensionMedSection").css("display", "block"),
            $("#HyperTensionMeasureSection").css("display", "block"),
            $("#HyperTensionMedYesSection").css("display", "none");



        }
        else if ($("#HyperTensionValueID").val() == 2) {

            $("#HyperTensionMedSection").css("display", "none"),
            $("#HyperTensionMeasureSection").css("display", "none"),
            $("#HyperTensionMedYesSection").css("display", "none");


        }

        else {

            $("#HyperTensionMedSection").css("display", "none"),
            $("#HyperTensionMeasureSection").css("display", "none"),
            $("#HyperTensionMedYesSection").css("display", "none");



        }


    });

    // Hypertension Med Load
    if ($("#HyperTensionMedValueID").val() == 1) {

        $("#HyperTensionMedYesSection").css("display", "block");



    }
    else if ($("#HyperTensionMedValueID").val() == 2) {

        $("#HyperTensionMedYesSection").css("display", "none");


    }

    else {

        $("#HyperTensionMedYesSection").css("display", "none");



    }


    // HyperTension Medicine Button Click
    $("#HyperTensionMedValueID").change(function () {

       
        if ($("#HyperTensionMedValueID").val() == 1) {

            $("#HyperTensionMedYesSection").css("display", "block");
           


        }
        else if ($("#HyperTensionMedValueID").val() == 2) {

            $("#HyperTensionMedYesSection").css("display", "none");
            

        }

        else {

            $("#HyperTensionMedYesSection").css("display", "none");
           


        }


    });



    // Page Load
    if ($("#HyperTensionMeasureValueID").val() == 1) {

        $("#HyperTensionMeasureYesSection").css("display", "block");



    }
    else if ($("#HyperTensionMeasureValueID").val() == 2) {

        $("#HyperTensionMeasureYesSection").css("display", "none");


    }

    else {

        $("#HyperTensionMeasureYesSection").css("display", "none");



    }


    // HyperTension Measure Button Click
    $("#HyperTensionMeasureValueID").change(function () {


        if ($("#HyperTensionMeasureValueID").val() == 1) {

            $("#HyperTensionMeasureYesSection").css("display", "block");



        }
        else if ($("#HyperTensionMeasureValueID").val() == 2) {

            $("#HyperTensionMeasureYesSection").css("display", "none");


        }

        else {

            $("#HyperTensionMeasureYesSection").css("display", "none");



        }


    });

    if ($("#DiabetesValueID").val() == 1) {

        $("#DiabetesMedSection").css("display", "block"),
        $("#DiabetesMedYesSection").css("display", "none");



    }
    else if ($("#DiabetesValueID").val() == 2) {

        $("#DiabetesMedSection").css("display", "none"),
        $("#DiabetesMedYesSection").css("display", "none");


    }

    else {

        $("#DiabetesMedSection").css("display", "none"),
        $("#DiabetesMedYesSection").css("display", "none");



    }
  

    // Diabetes Button Click
    $("#DiabetesValueID").change(function () {


        if ($("#DiabetesValueID").val() == 1) {

            $("#DiabetesMedSection").css("display", "block"),
            $("#DiabetesMedYesSection").css("display", "none");



        }
        else if ($("#DiabetesValueID").val() == 2) {

            $("#DiabetesMedSection").css("display", "none"),
            $("#DiabetesMedYesSection").css("display", "none");


        }

        else {

            $("#DiabetesMedSection").css("display", "none"),
            $("#DiabetesMedYesSection").css("display", "none");



        }


    });


    if ($("#DiabetesMedValueID").val() == 1) {

        $("#DiabetesMedYesSection").css("display", "block");



    }
    else if ($("#DiabetesMedValueID").val() == 2) {

        $("#DiabetesMedYesSection").css("display", "none");


    }

    else {

        $("#DiabetesMedYesSection").css("display", "none");



    }

    // Diabetes Med Button Click
    $("#DiabetesMedValueID").change(function () {


        if ($("#DiabetesMedValueID").val() == 1) {

            $("#DiabetesMedYesSection").css("display", "block");



        }
        else if ($("#DiabetesMedValueID").val() == 2) {

            $("#DiabetesMedYesSection").css("display", "none");


        }

        else {

            $("#DiabetesMedYesSection").css("display", "none");



        }


    });

    // Barrier Page Load
    if ($("#HBSessionCompletionTypeBinID").val() == 5) {

        $("#BarrierSection").css("display", "block");



    }
   
    else {

        $("#BarrierSection").css("display", "none");



    }

    // HB Completion Button Click
    $("#HBSessionCompletionTypeBinID").change(function () {


        if ($("#HBSessionCompletionTypeBinID").val() == 5) {

            
            $("#BarrierSection").css("display", "block");

            //var theText = 'Choose...';
            //$("#HBSessionBarrierTypeBinID option").each(function () {
            //    if ($(this).text() == theText) {
            //        $(this).attr('selected', 'selected');
            //    }
            //});
            


        }

        else {


            

            $("#BarrierSection").css("display", "none");
           
           

        }


    });


    



    $("#SaveExitButton").click(function () {

       
        document.getElementById("Saveid").value = "1";

        //Create Screen
        var hvH = $("#DDHeightBinIDHIDDEN").val();
        document.getElementById("DDHeightBinID").value = hvH;

        var hvW = $("#DDWeightBinIDHIDDEN").val();
        document.getElementById("DDWeightBinID").value = hvW;

        var hvWA = $("#DDWaistBinIDHIDDEN").val();
        document.getElementById("DDWaistBinID").value = hvWA;

        var hvHIP = $("#DDHipBinIDHIDDEN").val();
        document.getElementById("DDHipBinID").value = hvHIP;

        var hvCMD = $("#CholMedYesValueIDHIDDEN").val();
        document.getElementById("CholMedYesValueID").value = hvCMD;

        var hvHTM = $("#HyperTensionMedYesValueIDHIDDEN").val();
        document.getElementById("HyperTensionMedYesValueID").value = hvHTM;

        var hvDMV = $("#DiabetesMedYesValueIDHIDDEN").val();
        document.getElementById("DiabetesMedYesValueID").value = hvDMV;

        var hvFC = $("#FruitValueIDHIDDEN").val();
        document.getElementById("FruitValueID").value = hvFC;

        var hvVEG = $("#VegValueIDHIDDEN").val();
        document.getElementById("VegValueID").value = hvVEG;

        var hvMOD = $("#ModerateActivityValueIDHIDDEN").val();
        document.getElementById("ModerateActivityValueID").value = hvMOD;

        var hvVIG = $("#VigorousActivityValueIDHIDDEN").val();
        document.getElementById("VigorousActivityValueID").value = hvVIG;

        var hvSEC = $("#SmokingSecondHandValueIDHIDDEN").val();
        document.getElementById("SmokingSecondHandValueID").value = hvSEC;

        var hvPHY = $("#PhysicalHealthValueIDHIDDEN").val();
        document.getElementById("PhysicalHealthValueID").value = hvPHY;

        var hvMH = $("#MentalHealthValueIDHIDDEN").val();
        document.getElementById("MentalHealthValueID").value = hvMH;

        var hvMHPHY = $("#MentalPhysicalHealthValueIDHIDDEN").val();
        document.getElementById("MentalPhysicalHealthValueID").value = hvMHPHY;

       

    });

    // Get Edit Values on Load
    $(function () {

        // Edit Screen
        //var hvHE = $("#DDHeightBinIDHIDDENE").val();
        //document.getElementById("DDHeightBinID").value = hvHE;

        //var hvWE = $("#DDWeightBinIDHIDDENE").val();
        //document.getElementById("DDWeightBinID").value = hvWE;

        //var hvWAE = $("#DDWaistBinIDHIDDENE").val();
        //document.getElementById("DDWaistBinID").value = hvWAE;

        //var hvHIPE = $("#DDHipBinIDHIDDENE").val();
        //document.getElementById("DDHipBinID").value = hvHIPE;

        //var hvCMDE = $("#CholMedYesValueIDHIDDENE").val();
        //document.getElementById("CholMedYesValueID").value = hvCMDE;

        //var hvHTME = $("#HyperTensionMedYesValueIDHIDDENE").val();
        //document.getElementById("HyperTensionMedYesValueID").value = hvHTME;

        //var hvDMVE = $("#DiabetesMedYesValueIDHIDDENE").val();
        //document.getElementById("DiabetesMedYesValueID").value = hvDMVE;

        //var hvFCE = $("#FruitValueIDHIDDENE").val();
        //document.getElementById("FruitValueID").value = hvFCE;

        //var hvVEGE = $("#VegValueIDHIDDENE").val();
        //document.getElementById("VegValueID").value = hvVEGE;

        //var hvMODE = $("#ModerateActivityValueIDHIDDENE").val();
        //document.getElementById("ModerateActivityValueID").value = hvMODE;

        //var hvVIGE = $("#VigorousActivityValueIDHIDDENE").val();
        //document.getElementById("VigorousActivityValueID").value = hvVIGE;

        //var hvSECE = $("#SmokingSecondHandValueIDHIDDENE").val();
        //document.getElementById("SmokingSecondHandValueID").value = hvSECE;

        //var hvPHYE = $("#PhysicalHealthValueIDHIDDENE").val();
        //document.getElementById("PhysicalHealthValueID").value = hvPHYE;

        //var hvMHE = $("#MentalHealthValueIDHIDDENE").val();
        //document.getElementById("MentalHealthValueID").value = hvMHE;

        //var hvMHPHYE = $("#MentalPhysicalHealthValueIDHIDDENE").val();
        //document.getElementById("MentalPhysicalHealthValueID").value = hvMHPHYE;



    });


    $("#DashboardButton").click(function () {


        //document.getElementById("Saveid").value = "1";

       

        // Edit Screen
        var hvHE = $("#DDHeightBinIDHIDDENE").val();
        document.getElementById("DDHeightBinID").value = hvHE;

        var hvWE = $("#DDWeightBinIDHIDDENE").val();
        document.getElementById("DDWeightBinID").value = hvWE;

        var hvWAE = $("#DDWaistBinIDHIDDENE").val();
        document.getElementById("DDWaistBinID").value = hvWAE;

        var hvHIPE = $("#DDHipBinIDHIDDENE").val();
        document.getElementById("DDHipBinID").value = hvHIPE;

        var hvCMDE = $("#CholMedYesValueIDHIDDENE").val();
        document.getElementById("CholMedYesValueID").value = hvCMDE;

        var hvHTME = $("#HyperTensionMedYesValueIDHIDDENE").val();
        document.getElementById("HyperTensionMedYesValueID").value = hvHTME;

        var hvDMVE = $("#DiabetesMedYesValueIDHIDDENE").val();
        document.getElementById("DiabetesMedYesValueID").value = hvDMVE;

        var hvFCE = $("#FruitValueIDHIDDENE").val();
        document.getElementById("FruitValueID").value = hvFCE;

        var hvVEGE = $("#VegValueIDHIDDENE").val();
        document.getElementById("VegValueID").value = hvVEGE;

        var hvMODE = $("#ModerateActivityValueIDHIDDENE").val();
        document.getElementById("ModerateActivityValueID").value = hvMODE;

        var hvVIGE = $("#VigorousActivityValueIDHIDDENE").val();
        document.getElementById("VigorousActivityValueID").value = hvVIGE;

        var hvSECE = $("#SmokingSecondHandValueIDHIDDENE").val();
        document.getElementById("SmokingSecondHandValueID").value = hvSECE;

        var hvPHYE = $("#PhysicalHealthValueIDHIDDENE").val();
        document.getElementById("PhysicalHealthValueID").value = hvPHYE;

        var hvMHE = $("#MentalHealthValueIDHIDDENE").val();
        document.getElementById("MentalHealthValueID").value = hvMHE;

        var hvMHPHYE = $("#MentalPhysicalHealthValueIDHIDDENE").val();
        document.getElementById("MentalPhysicalHealthValueID").value = hvMHPHYE;


        //Add Assessment Date
        var assDate = $("#AssessmentDateHIDDEN").val();
        document.getElementById("AssessmentDate").value = assDate;


    });

    $("#AddTestingButton").click(function () {


        if ($("#TestDD").val() == 59) {


            // Cholesterol
            var hvSC = $("#TotalCholesterolBinIDHIDDEN").val();
            document.getElementById("TotalCholesterolBinID").value = hvSC;

            var hvDC = $("#HDLCholesterolBinIDHIDDEN").val();
            document.getElementById("HDLCholesterolBinID").value = hvDC;

            var hvS2C = $("#LDLCholesterolBinIDHIDDEN").val();
            document.getElementById("LDLCholesterolBinID").value = hvS2C;

            var hvD2C = $("#TriglyceridesBinIDHIDDEN").val();
            document.getElementById("TriglyceridesBinID").value = hvD2C;

            //Add Test Screen
            var hvCLDate = $("#CLTestDateHIDDEN").val();
            document.getElementById("CLTestDate").value = hvCLDate;


        }

        else if ($("#TestDD").val() == 58) {

            // Blood Pressure
            var hvS = $("#SystolicBinID1HIDDEN").val();
            document.getElementById("SystolicBinID1").value = hvS;

            var hvD = $("#DiastolicBinID1HIDDEN").val();
            document.getElementById("DiastolicBinID1").value = hvD;

            var hvS2 = $("#SystolicBinID2HIDDEN").val();
            document.getElementById("SystolicBinID2").value = hvS2;

            var hvD2 = $("#DiastolicBinID2HIDDEN").val();
            document.getElementById("DiastolicBinID2").value = hvD2;

            //Add Test Screen
            var hvBPDate = $("#BPTestDateHIDDEN").val();
            document.getElementById("BPTestDate").value = hvBPDate;

        }

        else {

            // Glucose
            var hvG = $("#GlucoseBinIDHIDDEN").val();
            document.getElementById("GlucoseBinID").value = hvG;

            //var hvA = $("#A1CPercentageBinIDHIDDEN").val();
            //document.getElementById("A1CPercentageBinID").value = hvA;

            //Add Test Screen
            var hvGLUDate = $("#GLUTestDateHIDDEN").val();
            document.getElementById("GLUTestDate").value = hvGLUDate;


        }

    });


  

    $("#AddSessionButton").click(function () {


        if ($("#SessionDD").val() == 1) {


            ////Add Date
            //var ssdDate = $("#HBSupportSessionDateHIDDEN").val();
            //document.getElementById("HBSupportSessionDate").value = ssdDate;


        }
        else if ($("#SessionDD").val() == 2) {


            ////Add Date
            //var ssdDate = $("#HCHBSupportSessionDateHIDDEN").val();
            //document.getElementById("HBSupportSessionDate").value = ssdDate;


            ////Add Length of Session
            //var slength = $("#HCHBSessionLengthBinIDHIDDEN").val();
            //document.getElementById("HBSessionLengthBinID").value = slength;

            ////Add Patient Confidence
            //var pcon = $("#HCPatientConfidenceHIDDEN").val();
            //document.getElementById("PatientConfidence").value = pcon;


            ////Add Patient Importance
            //var pimp = $("#HCPatientImportanceHIDDEN").val();
            //document.getElementById("PatientImportance").value = pimp;



        }
        else if ($("#SessionDD").val() == 3) {


            ////Add Date
            //var ssdDate = $("#CMSupportSessionDateHIDDEN").val();
            //document.getElementById("HBSupportSessionDate").value = ssdDate;


        }
        else {

            // Do Nothing

        }

    });



    $("#BPWorkupTestingButton").click(function () {


     
            //Add Date
        var bpwuDate = $("#BPWorkupStatusDateHIDDEN").val();
        document.getElementById("WorkupStatusDate").value = bpwuDate;




    });


    $("#WorkupTestingButton").click(function () {


        //Add Date
        var wuDate = $("#WorkupStatusDateHIDDEN").val();
        document.getElementById("WorkupStatusDate").value = wuDate;




    });



  
});