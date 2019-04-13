
let DateTime = luxon.DateTime;

$(document).ready(function () {

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
        console.log("login hidden hidden");
        clearLoginModal();
    });

    $("#signupModal").on("hidden.bs.modal", function (e) {
        console.log("signup hidden");
        clearSignupModal();
    });

    $("img#profile_picture").on("error", function () {
        alert("er");
        $(this).replaceWith("<i class='fas fa-user-circle'></i>");
    });

});

//----------------------------------------------------------------------------------------------------------

function validateSignup() {

    let valid = true;
    let username = $("#signupInputUsername").val().trim();

    if (username === "") {
        valid = false;
        $("#signupInputUsername").addClass("border-danger");
        let username_warning = createWarning("Username required");
        $("#signup_warning_space").append(username_warning);
    }

    if (valid) {
        usernameTaken(username, usernameTakenSuccess);
    }

}

function validateLogin() {
    let valid = true;
    let username = $("#loginInputUsername").val().trim();
    let password = $("#loginInputPassword").val().trim();

    if (username === "") {
        valid = false;
        $("#loginInputUsername").addClass("border-danger");
        let username_warning = createWarning("Username required");
        $("#login_warning_space").append(username_warning);
    }
    if (password === "") {
        valid = false;
        $("#loginInputPassword").addClass("border-danger");
        let password_warning = createWarning("Password required");
        $("#login_warning_space").append(password_warning);
    }

    if (valid) {
        userExists(username, password, userExistsSuccess);
    }

}

//----------------------------------------------------------------------------------------------------------


function userExistsSuccess(data) {

    let exists = data.d;
    let valid = true;
    let username = $("#loginInputUsername").val().trim();
    let password = $("#loginInputPassword").val().trim();

    if (!exists) {
        valid = false;
        $("#loginInputUsername").addClass("border-danger");
        $("#loginInputPassword").addClass("border-danger");
        let warning = createWarning("Username and password don't match");
        $("#login_warning_space").append(warning);
    }

    if (valid) {
        loginUser(username, password, loginUserSuccess);
    }
}

function loginUserSuccess(data) {
    window.location.reload(false);
}

function usernameTakenSuccess(data) {

    let taken = data.d;
    let valid = true;
    let username = $("#signupInputUsername").val().trim();
    let password = $("#signupInputPassword").val().trim();
    let password_confirm = $("#signupInputPasswordConfirm").val().trim();
    let DOB = $("#signupInputDOB").val().trim();

    if (taken) {
        valid = false;
        $("#signupInputUsername").addClass("border-danger");
        let warning = createWarning("Username taken");
        $("#signup_warning_space").append(warning);
    }

    if (password === "") {
        valid = false;
        $("#signupInputPassword").addClass("border-danger");
        let password_warning = createWarning("Password required");
        $("#signup_warning_space").append(password_warning);
    }
    else if (password !== password_confirm) {
        valid = false;
        $("#signupInputPassword").addClass("border-danger");
        $("#signupInputPasswordConfirm").addClass("border-danger");
        let password_warning = createWarning("Passwords don't match");
        $("#signup_warning_space").append(password_warning);
        valid = false;
    }

    if (DOB === "") {
        valid = false;
        $("#signupInputDOB").addClass("border-danger");
        let password_warning = createWarning("Date of birth required");
        $("#signup_warning_space").append(password_warning);

    } else {
        let validate_DOB = DateTime.fromFormat(DOB, "dd/MM/yyyy");
        if (!validate_DOB.isValid || validate_DOB > DateTime.local()) {
            valid = false;
            $("#signupInputDOB").addClass("border-danger");
            let password_warning = createWarning("Invalid date");
            $("#signup_warning_space").append(password_warning);
        }
    }

    if (valid) {
        signupUser(username, password, DOB, signupUserSuccess);
    }
}

function signupUserSuccess(data) {
    window.location.reload(false);
}

function logoutUserSuccess(data) {
    window.location.reload(false);
}

//----------------------------------------------------------------------------------------------------------

function usernameTaken(username, success_callback) {
    let data = { "username": username };

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

    let data = { "username": username, "password": password };

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

function loginUser(username, password, success_callback) {

    let data = { "username": username, "password": password };

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/LogInUser",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });
}

function signupUser(username, password, DOB, success_callback) {

    let data = { "username": username, "password": password, "DOB": DOB };

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/SignupUser",
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

function createLoginWarning(message) {
    let warning = createWarning(message);
}