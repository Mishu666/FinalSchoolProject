$(document).ready(function () {

    clearLoginModal();

    $("#LogInConfirmButton").on("click", function (e) {

        loginSubmit(e);

    });

    $("#loginModal").on("hide.bs.modal", function (e) {
        clearLoginModal();
    });

    $("#signupModal").on("hide.bs.modal", function (e) {
        clearSignupModal();
    });

});


$(function () {
    $('[data-toggle="tooltip"]').tooltip()
})

function loginSubmit(e) {
    clearLoginWarning();
    var valid = true;
    var username = $("#loginInputUsername").val();
    var password = $("#loginInputPassword").val();

    if (username === "") {
        e.preventDefault();
        $("#loginInputUsername").addClass("border-danger");
        let username_warning = createWarning("Username required");
        $("#login_warning_space").append(username_warning);
        valid = false;
    }
    if (password === "") {
        e.preventDefault();
        $("#loginInputPassword").addClass("border-danger");
        let password_warning = createWarning("Password required");
        $("#login_warning_space").append(password_warning);
        valid = false;
    }

    if (valid) {
        let exists = userExists(username, password);
        if (!exists) {
            e.preventDefault();
            $("#loginInputUsername").addClass("border-danger");
            $("#loginInputPassword").addClass("border-danger");
            let warning = createWarning("Username and password don't match");
            $("#login_warning_space").append(warning);
        }
    }

}

function userExists(username, password) {

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
        success: function (r) {
            console.log("success");
            console.log(r.d);
            return r.d
        }
    })

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