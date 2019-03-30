

// Re-assigns a couple of the ASP.NET validation JS functions to
// provide a more flexible approach

function UpgradeASPNETValidation() {
    // Hi-jack the ASP.NET error display only if required
    if (typeof Page_ClientValidate !== "undefined") {
        ValidatorUpdateDisplay = NicerValidatorUpdateDisplay;
        AspPage_ClientValidate = Page_ClientValidate;
        Page_ClientValidate = NicerPage_ClientValidate;
    }
}

// Extends the classic ASP.NET validation to add a class to the parent span when invalid

function NicerValidatorUpdateDisplay(val) {

    let control = val.controltovalidate;

    if (val.isvalid) {
        // do custom removing
        $("#" + control).removeClass("invalid");
        $(val).hide();

    } else {
        // do custom show
        $("#" + control).addClass("invalid");
        console.log("#" + val.id);
        $("#" + val.id).show();

    }
}


// Extends classic ASP.NET validation to include parent element styling

function NicerPage_ClientValidate(validationGroup) {
    var valid = AspPage_ClientValidate(validationGroup);

    if (!valid) {
        // do custom styling etc
    }

}