$(document).ready(function () {

    InitDatatables();

    let UsersGVPageNo = window.sessionStorage.getItem("UsersGVPageNo");
    if (UsersGVPageNo === null) {
        window.sessionStorage.setItem("UsersGVPageNo", 1);
    }

    $(".DeleteUserButton").on("click", function (e) {
        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        let row = $(this).closest("tr");
        let spinner = row.find(".loading_spinner").first();

        spinner.show();
        $(this).attr("disabled", true);

        DeleteUser(userid, DeleteUserSuccessCallback, $(this));
    });

    $(".IsSuspendedSwitch").on("click", function (e) {
        let userid = parseInt($(this).data("user-id"));
        let issuspended = $(this).prop("checked");
        let row = $(this).closest("tr");
        let spinner = row.find(".loading_spinner").first();

        spinner.show();
        $(this).attr("disabled", true);

        UpdateSuspendedStatus(issuspended, userid, UpdateSuspendedStatusSuccessCallback, $(this));
    });

    $(".IsAdminSwitch").on("click", function (e) {
        let userid = parseInt($(this).data("user-id"));
        let isadmin = $(this).prop("checked");
        let row = $(this).closest("tr");
        let spinner = row.find(".loading_spinner").first();

        spinner.show();
        $(this).attr("disabled", true);

        UpdateAdminStatus(isadmin, userid, UpdateAdminStatusSuccessCallback, $(this));
    });

});

//--------------------------------------------------------------------------------------

function DeleteUserSuccessCallback(data) {

}

function DeleteUser(userid, success_callback, sender) {

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
        success: success_callback,
        complete: function () {
            let row = sender.closest("tr");
            row.remove();
        }
    });

}

//--------------------------------------------------------------------------------------

function UpdateSuspendedStatusSuccessCallback(data) {

}

function UpdateSuspendedStatus(issuspended, userid, success_callback, sender) {

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
        success: success_callback,
        complete: function () {
            let row = sender.closest("tr");
            let spinner = row.find(".loading_spinner").first();
            spinner.hide();
            sender.attr("disabled", false);
        }

    });

}

//--------------------------------------------------------------------------------------

function UpdateAdminStatusSuccessCallback(data) {

}

function UpdateAdminStatus(isadmin, userid, success_callback, sender) {

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
        success: success_callback,
        complete: function () {
            let row = sender.closest("tr");
            let spinner = row.find(".loading_spinner").first();
            spinner.hide();
            sender.attr("disabled", false);
        }

    });

}

//--------------------------------------------------------------------------------------

function BindUsersGVSuccessCalback(data) {

    let response = $('<div />').html(data);
    let UsersGV = response.find(".UsersGV");

    $(".UsersGV").html(UsersGV.html());
    InitDatatables();
}

function BindUsersGV(success_callback) {

    let page_no = window.sessionStorage.getItem("UsersGVPageNo");

    data = { "PageNo": page_no };

    $.ajax({

        method: "GET",
        data: JSON.stringify(data),
        url: "UsersEditor.aspx",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });


}

//--------------------------------------------------------------------------------------

function InitDatatables() {

    $(".UsersGV").DataTable({
        scrollY: "70vh",
        stateSave: true,
        scrollCollapse: true,
        ordering: false,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]]
    });

}