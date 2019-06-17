

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

        validateAndSignup(username, password, password_confirm, DOB, validateAndSignupSuccess);

    });

    $("#LogoutConfirmButton").on("click", function (e) {
        e.preventDefault();
        logoutUser(logoutUserSuccess);
    });

    $(".modal").on("hidden.bs.modal", function (e) {
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
        loadServerChanges();
        $("#loginModal").modal('hide');
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
        loadServerChanges();
        $("#signupModal").modal('hide');
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
    loadServerChanges();
    $("#logoutModal").modal('hide');
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

//----------------------------------------------------------------------------------------------------------------------------

function createWarning(message) {
    let warning = document.createElement("div");
    warning.className = "alert alert-danger w-75 mx-auto text-center";
    warning.setAttribute("role", "alert");
    warning.innerText = message;
    return warning;
}

//----------------------------------------------------------------------------------------------------------------------------

function loadServerChanges() {
    console.log(window.location.pathname);
    window.location.reload();
}

//----------------------------------------------------------------------------------------------------------------------------

function closeActiveEditors() {

    $("#hidden_post_card").hide();
    $(".edit_post_view").hide();
    $(".default_post_view").show();
    $("#hidden_comment_card").hide();
    $(".edit_comment_view").hide();
    $(".default_comment_view").show();
}

function resetForm() {
    $(".main_form")[0].reset();
    removeAlerts();
}

function removeAlerts() {
    $(".alert-danger").remove();
    $(".border-danger").removeClass("border-danger");
}