
let DateTime = luxon.DateTime;

$(document).ready(function () {

    clearLoginModal();
    clearSignupModal();

    $('#signupInputDOB').datepicker({
        autoclose: true,
        format: "dd/mm/yyyy"
    });

    $("#sortDirectionDropdown").on("click", function (e) {
        $(this).children(["i"]).toggleClass("fa-arrow-up");
        $(this).children(["i"]).toggleClass("fa-arrow-down");
    });

    $("#LogInSubmitButton").on("click", function (e) {
        e.preventDefault();
        clearLoginWarnings();
        validateAndLogin(validateAndLoginSuccess);
    });

    $("#SignUpSubmitButton").on("click", function (e) {
        e.preventDefault();
        clearSignupWarnings();
        validateAndSignup(validateAndSignupSuccess);
    });

    $("#LogoutConfirmButton").on("click", function (e) {
        e.preventDefault();
        logoutUser(logoutUserSuccess);
    });

    $("#loginModal").on("hidden.bs.modal", function (e) {
        console.log("login hidden hidden");
        clearLoginModal();
    });

    $("#signupModal").on("hidden.bs.modal", function (e) {
        console.log("signup hidden");
        clearSignupModal();
    });

    $("img#profile_picture").on("error", function () {
        $(this).replaceWith("<i class='fas fa-user-circle'></i>");
    });

});

//----------------------------------------------------------------------------------------------------------

function loginUserSuccess(data) {
    console.log("login success");
    //window.location.reload();
}

function signupUserSuccess(data) {
    window.location.reload();
}

function logoutUserSuccess(data) {
    window.location.reload();
}

function validateAndLoginSuccess(data) {
    let warnings = data.d;

    for (let w of warnings) {
        let warning = createWarning(w);
        $("#login_warning_space").append(warning);
        if (w === "username cannot be empty") {
            $("#loginInputUsername").addClass("border-danger");
        }
        if (w === "password cannot be empty") {
            $("#loginInputPassword").addClass("border-danger");
        }
        if (w === "username and password do not match") {
            $("#loginInputUsername").addClass("border-danger");
            $("#loginInputPassword").addClass("border-danger");
        }
    }

    if (warnings.length === 0) {
        window.location.reload();
    }
}

function validateAndSignupSuccess(data) {

    let warnings = data.d;

    for (let w of warnings) {
        let warning = createWarning(w);
        $("#signup_warning_space").append(warning);
        if (w === "username cannot be empty") {
            $("#signupInputUsername").addClass("border-danger");
        }
        if (w === "password cannot be empty") {
            $("#signupInputPassword").addClass("border-danger");
        }
        if (w === "birth date cannot be empty") {
            $("#signupInputDOB").addClass("border-danger");
        }
        if (w === "invalid date of birth") {
            $("#signupInputDOB").addClass("border-danger");
        }
    }}

//----------------------------------------------------------------------------------------------------------

function validateAndSignup() {

    let username = $("#signupInputUsername").val().trim();
    let password = $("#signupInputPassword").val().trim();
    let DOB = $("#signupInputDOB").val().trim();

    let data = { "username": username, "password": password, "DOB": DOB };

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

function validateAndLogin(success_callback) {
    let username = $("#loginInputUsername").val().trim();
    let password = $("#loginInputPassword").val().trim();

    let data = { "username": username, "password": password };

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

function userLoggedIn(success_callback) {

    let data = {};

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/UserLoggedIn",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });
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

//----------------------------------------------------------------------------------------------------------

function clearLoginWarnings() {
    $("#login_warning_space .alert").remove();
    $(".login_textbox").removeClass("border-danger");
}

function clearLoginModal() {
    $(".login_textbox").val("");
    clearLoginWarnings();
}

function clearSignupWarnings() {
    $("#signup_warning_space .alert").remove();
    $(".signup_textbox").removeClass("border-danger");
}

function clearSignupModal() {
    $(".signup_textbox").val("");
    clearSignupWarnings();
}

function createWarning(message) {
    let warning = document.createElement("div");
    warning.className = "alert alert-danger w-75 mx-auto";
    warning.setAttribute("role", "alert");
    warning.innerText = message;
    return warning;
}