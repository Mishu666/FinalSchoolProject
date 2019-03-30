
$(document).ready(function () {

    resetLoginSignupForms();

    $("#modal_background").on("click", function (e) {
        if (e.target.id === "modal_background") {
            hideModals();
        }
    });

    $("#AccountImageButton").on("click", function (e) {
        e.preventDefault();
        showModal('login_modal');
    });

    $("#LoginSubmitButton").on("click", function (e) {

        e.preventDefault();

        var loginFormValid = Page_ClientValidate("LogInValidatoinGroup");

        if (loginFormValid) {

            var username = $("#LogInUsernameTextBox").val();
            var pass = $("#LogInPasswordTextBox").val();

            var userExists = validateLoginInfo(username, pass);

            //console.log(userExists + " -------- ");

            //if (userExists) return;

            $("#LoginWarningLabel").show();
            e.preventDefault();

        } else {
            console.log("+++++++++++++++++++++++++++++++++++");
        }
        
        
    });

});

function resetLoginSignupForms() {
    $(".login_textbox").val("");
    $(".login_textbox").removeClass("invalid");
    $(".warning_label_login").hide(); 
}

function hideModals() {
    $("#modal_background").fadeOut(200, function () {
        resetLoginSignupForms();
    });
}

function showModal(id) {
    $(".modal").hide();
    $("#" + id).show();
    $("#modal_background").fadeIn(200);
}

function switchToModal(id) {
    $(".modal").hide();
    resetLoginSignupForms();
    $("#" + id).show();
}

//validation

function validateLoginInfo(username, password) {

    var data = {};
    data = JSON.stringify(data);

    console.log(data);

    $.ajax({
        type: "POST",
        url: "UsersService.asmx/GetAll",
        dataType: "json",
        contentType: "application/json; charset=utf-8",
        data: data,
        success: function (r) {
            console.log("success");
            console.log(r.d);
        },
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        failure: function (r) {
            console.log("failure");
            console.log(r.responseText);
        }
    });

}