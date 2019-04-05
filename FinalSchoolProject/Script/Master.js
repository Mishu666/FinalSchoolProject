
//$(function () {
//    $('[data-toggle="tooltip"]').tooltip()
//})

var DateTime = luxon.DateTime;

$(document).ready(function () {

    clearLoginModal();
    
    $('.datepicker').datepicker({
        autoclose: true
    });

    $("#LogInConfirmButton").on("click", function (e) {
        clearLoginWarning();
        validateLogin(e);
    });

    $("#loginModal").on("hide.bs.modal", function (e) {
        clearLoginModal();
    });

    $("#signupModal").on("hide.bs.modal", function (e) {
        clearSignupModal();
    });

});

function validateLogin(e) {
    e.preventDefault();
    let valid = true;
    let username = $("#loginInputUsername").val();
    let password = $("#loginInputPassword").val();

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
    else {
        userExists(username, password, function (data) {

            let exists = data.d;
            console.log(exists);
            if (!exists) {
                valid = false;
                $("#loginInputUsername").addClass("border-danger");
                $("#loginInputPassword").addClass("border-danger");
                let warning = createWarning("Username and password don't match");
                $("#login_warning_space").append(warning);
            }
            else {
                console.log("valid!");
                $("#LogInConfirmButton").unbind("click").trigger("click");
            }

        });
    }
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

function clearLoginWarning() {
    $("#login_warning_space .alert").remove();
    $("#loginInputUsername").removeClass("border-danger");
    $("#loginInputPassword").removeClass("border-danger");
}

function clearLoginModal() {
    $("#loginInputUsername").val("");
    $("#loginInputPassword").val("");
    clearLoginWarning();
}

function clearSignupWarning() {
    $("#signup_warning_space .alert").remove();
    $("#signupInputUsername").removeClass("border-danger");
    $("#signupInputPassword").removeClass("border-danger");
}

function clearSignupModal() {
    $("#signupInputUsername").val("");
    $("#signupInputPassword").val("");
    clearSignupWarning();
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