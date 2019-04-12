
//$(function () {
//    $('[data-toggle="tooltip"]').tooltip()
//})

var DateTime = luxon.DateTime;

$(document).ready(function () {

    clearLoginModal();

    $('#signupInputDOB').datepicker({
        autoclose: true,
        format: "dd/mm/yyyy"
    });

    $("#LogInSubmitButton").on("click", function (e) {
        e.preventDefault();
        clearLoginWarnings();
        validateLogin();
    });

    $("#SignUpSubmitButton").on("click", function (e) {
        e.preventDefault();
        clearSignupWarnings();
        validateSignup();
    });

    $("#loginModal").on("hidden.bs.modal", function (e) {
        clearLoginModal();
    });

    $("#signupModal").on("hidden.bs.modal", function (e) {
        clearSignupModal();
    });

});

function validateSignup() {

    let valid = true;
    let username = $("#signupInputUsername").val();
    let password = $("#signupInputPassword").val();
    let password_confirm = $("#signupInputPasswordConfirm").val();
    let DOB = $("#signupInputDOB").val();

    function usernameTakenSuccess(data) {

        let taken = data.d;
        if (taken) {
            $("#signupInputUsername").addClass("border-danger");
            let warning = createWarning("Username taken");
            $("#signup_warning_space").append(warning);
            valid = false;
        }
        if (valid) {
            unbindAndTriggerButton("SignUpSubmitButton");
        }
    }

    if (username === "") {
        $("#signupInputUsername").addClass("border-danger");
        let username_warning = createWarning("Username required");
        $("#signup_warning_space").append(username_warning);
        valid = false;
    }
    else {
        usernameTaken(username, usernameTakenSuccess);
    }

    if (password === "") {
        $("#signupInputPassword").addClass("border-danger");
        let password_warning = createWarning("Password required");
        $("#signup_warning_space").append(password_warning);
        valid = false;
    }
    else if (password !== password_confirm) {
        $("#signupInputPassword").addClass("border-danger");
        $("#signupInputPasswordConfirm").addClass("border-danger");
        let password_warning = createWarning("Passwords don't match");
        $("#signup_warning_space").append(password_warning);
        valid = false;
    }

    if (DOB === "") {
        $("#signupInputDOB").addClass("border-danger");
        let password_warning = createWarning("Date of birth required");
        $("#signup_warning_space").append(password_warning);
        valid = false;
    } else {
        var validate_DOB = DateTime.fromFormat(DOB, "dd/MM/yyyy");
        if (!validate_DOB.isValid || validate_DOB > DateTime.local()) {
            $("#signupInputDOB").addClass("border-danger");
            let password_warning = createWarning("Invalid date");
            $("#signup_warning_space").append(password_warning);
            valid = false;
        }
    }

}

function validateLogin() {
    let valid = true;
    let username = $("#loginInputUsername").val();
    let password = $("#loginInputPassword").val();

    function userExistsSuccess(data) {

        let exists = data.d;
        console.log(exists);
        if (!exists) {
            valid = false;
            $("#loginInputUsername").addClass("border-danger");
            $("#loginInputPassword").addClass("border-danger");
            let warning = createWarning("Username and password don't match");
            $("#login_warning_space").append(warning);
        }

        if (valid) {
            unbindAndTriggerButton("LogInSubmitButton");
        }
    }

    if (username === "") {
        $("#loginInputUsername").addClass("border-danger");
        let username_warning = createWarning("Username required");
        $("#login_warning_space").append(username_warning);
        valid = false;
    }
    if (password === "") {
        $("#loginInputPassword").addClass("border-danger");
        let password_warning = createWarning("Password required");
        $("#login_warning_space").append(password_warning);
        valid = false;
    }

    if (valid) {
        userExists(username, password, userExistsSuccess);
    }

}

function unbindAndTriggerButton(id) {
    $("#" + id).unbind("click").trigger("click");
}

function usernameTaken(username, success_callback) {
    var data = { "username": username };

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/UsernameTaken",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });
}

function userExists(username, password, success_callback) {

    var data = { "username": username, "password": password };

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/UserExists",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });

}

function userLoggedIn(success_callback) {

    data = {};

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

function createLoginWarning(message) {
    let warning = createWarning(message);
}