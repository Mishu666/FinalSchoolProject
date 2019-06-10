

$(document).ready(function () {

    $('#signupInputDOB').datepicker({
        autoclose: true,
        format: "dd/mm/yyyy"
    });

    $("#LogInSubmitButton").on("click", function (e) {
        e.preventDefault();

        let username = $("#loginInputUsername").val().trim();
        let password = $("#loginInputPassword").val().trim();

        validateAndLogin(username, password, validateAndLoginSuccess);

    });

    $("#SignUpSubmitButton").on("click", function (e) {
        e.preventDefault();

        let username = $("#signupInputUsername").val().trim();
        let password = $("#signupInputPassword").val().trim();
        let password_confirm = $("#signupInputPasswordConfirm").val().trim();
        let DOB = $("#signupInputDOB").val().trim();

        validateAndSignup(validateAndSignupSuccess);

    });

    $("#LogoutConfirmButton").on("click", function (e) {
        e.preventDefault();
        logoutUser(logoutUserSuccess);
    });

    $("#loginModal").on("hidden.bs.modal", function (e) {
        resetForm();
    });

    $("#signupModal").on("hidden.bs.modal", function (e) {
        resetForm();
    });

    $("#profile_picture").on("error", function () {
        $(this).replaceWith("<i class='fas fa-user-circle'></i>");
    });

});

//----------------------------------------------------------------------------------------------------------------------------

function validateAndLoginSuccess(data) {
    let warnings = data.d;

    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w.Text);
        $("#login_warning_space").append(warning);
        for (let wc of w.WarnControls) {
            $("#" + wc).addClass("border-danger");

        }
    }
    if (warnings.length === 0) {
        console.log("login success");
        window.location.reload();
    }
}

function validateAndLogin(username, pass,success_callback) {

    let data = { "username": username, "password": pass };

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndLogin",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });

}

//----------------------------------------------------------------------------------------------------------------------------

function validateAndSignupSuccess(data) {

    let warnings = data.d;

    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w.Text);
        $("#signup_warning_space").append(warning);
        for (let wc of w.WarnControls) {
            $("#" + wc).addClass("border-danger");

        }
    }

    if (warnings.length === 0) {
        console.log("signup success");
        window.location.reload();
    }

}

function validateAndSignup(username, password, password_confirm, DOB, success_callback) {

    let data = { "username": username, "password": password,"pass_confirm":password_confirm, "DOBstr": DOB };

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndSignup",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });

}


//----------------------------------------------------------------------------------------------------------------------------

function logoutUserSuccess(data) {
    window.location.reload();
}

function logoutUser(success_callback) {

    let data = {};

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/LogOutUser",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });
}
1
//----------------------------------------------------------------------------------------------------------------------------

function createWarning(message) {
    let warning = document.createElement("div");
    warning.className = "alert alert-danger w-75 mx-auto text-center";
    warning.setAttribute("role", "alert");
    warning.innerText = message;
    return warning;
}