$(document).ready(function () {

    $(".DeleteUserButton").on("click", function (e) {
        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        DeleteUser(userid, DeleteUserSuccessCallback);
    });

    $(".IsSuspendedSwitch").on("click", function (e) {
        let userid = parseInt($(this).data("user-id"));
        let issuspended = $(this).prop("checked");

        UpdateSuspendedStatus(issuspended, userid, UpdateSuspendedStatusSuccessCallback);
    });

    $(".IsAdminSwitch").on("click", function (e) {
        let userid = parseInt($(this).data("user-id"));
        let isadmin = $(this).prop("checked");

        UpdateAdminStatus(isadmin, userid, UpdateAdminStatusSuccessCallback);
    });



});

//--------------------------------------------------------------------------------------

function DeleteUserSuccessCallback(data) {

}

function DeleteUser(userid, success_callback) {

    data = { "UserID": userid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/DeleteUser",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//--------------------------------------------------------------------------------------

function UpdateSuspendedStatusSuccessCallback(data) {

}

function UpdateSuspendedStatus(issuspended, userid, success_callback) {

    data = { "IsSuspended": issuspended, "UserID": userid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/UpdateSuspendedStatus",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//--------------------------------------------------------------------------------------

function UpdateAdminStatusSuccessCallback(data) {

}

function UpdateAdminStatus(isadmin, userid, success_callback) {

    data = { "IsAdmin": isadmin, "UserID": userid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/UpdateAdminStatus",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}