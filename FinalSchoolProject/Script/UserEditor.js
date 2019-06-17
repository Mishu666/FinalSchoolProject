$(document).ready(function () {

    InitDatatables();

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

    $(".UsersGV").replaceWith(UsersGV);
    InitDatatables();
}

function BindUsersGV(success_callback) {

    $.get("UsersEditor.aspx", success_callback);
}

//--------------------------------------------------------------------------------------

function InitDatatables() {

    let UsersTable = $(".UsersGV").DataTable({
        "destroy": true,
        scrollY: "60vh",
        stateSave: true,
        scrollCollapse: true,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "columnDefs": [
            { "searchable": false, "targets": [1, 2, 3, 4, 5, 6] },
            { "orderable": false, "targets": [1, 4, 5, 6] }
        ]
    });

    UsersTable.columns.adjust().draw();

}