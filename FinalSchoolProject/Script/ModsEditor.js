$(document).ready(function () {

    InitDataTables();

    $(".ConsultPagesDDL").change(function (e) {

        let pageid = $(this).val();
        $(".ToggleModButton").attr("disabled", true);
        window.sessionStorage.setItem("pageid", pageid);

        BindGVs(BindGVsSuccessCallback);
    });

    $("#GVSection").on("click", ".PromoteUserButton", function (e) {

        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        let pageid = parseInt($(".ConsultPagesDDL").val());
        $(this).attr("disabled", true);

        PromoteUser(userid, pageid, PromoteUserSuccessCallback);

    });

    $("#GVSection").on("click", ".DemoteUserButton", function (e) {

        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        let pageid = parseInt($(".ConsultPagesDDL").val());
        $(this).attr("disabled", true);

        DemoteUser(userid, pageid, DemoteUserSuccessCallback);

    });

    $("#GVSection").on("change", ".SelectAllInGroupSwitch", function (e) {

        $(".SelectInGroupSwitch").prop("checked", $(this).prop("checked"));

    });

    $("#GVSection").on("change", ".SelectAllNotInGroupSwitch", function (e) {

        $(".SelectNotInGroupSwitch").prop("checked", $(this).prop("checked"));

    });

    $("#GVSection").on("click", "#DemoteMultipleButton", function (e) {

        e.preventDefault();

        let pageid = parseInt($(".ConsultPagesDDL").val());
        let users_to_demote = [];

        $('input:checked.SelectInGroupSwitch').each(function () {
            users_to_demote.push($(this).data('user-id'));
        });

        if (users_to_demote.length > 0) {
            DemoteMultipleUsers(users_to_demote, pageid, DemoteMultipleUsersSuccessCallback);
        }
    });

    $("#GVSection").on("click", "#PromoteMultipleButton", function (e) {

        e.preventDefault();

        let pageid = parseInt($(".ConsultPagesDDL").val());
        let users_to_promote = [];

        $('input:checked.SelectNotInGroupSwitch').each(function () {
            users_to_promote.push($(this).data('user-id'));
        });

        if (users_to_promote.length > 0) {
            PromoteMultipleUsers(users_to_promote, pageid, PromoteMultipleUsersSuccessCallback);
        }
    });

});

$(document).ajaxStart(function () {
    $("#loading_spinner").show();
});

$(document).ajaxStop(function () {
    $("#loading_spinner").hide();
});

//--------------------------------------------------------------------------------------

function BindGVsSuccessCallback(data) {

    let response = $('<div />').html(data);
    let UsersInPageSec = response.find("#ModsSection");
    let UsersNotInPageSec = response.find("#NotModsSection");

    $("#ModsSection").replaceWith(UsersInPageSec);
    $("#NotModsSection").replaceWith(UsersNotInPageSec);

    InitDataTables();

}

function BindGVs(success_callback) {

    let pageid = window.sessionStorage.getItem("pageid");

    data = { "PageID": pageid };

    $.get("ModsEditor.aspx", data, success_callback);


}

//--------------------------------------------------------------------------------------

function DemoteUserSuccessCallback(data) {

    BindGVs(BindGVsSuccessCallback);

}

function PromoteUserSuccessCallback(data) {

    BindGVs(BindGVsSuccessCallback);

}

function PromoteUser(userid, pageid, success_callback) {

    data = { "UserID": userid, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/PromoteUser",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

function DemoteUser(userid, pageid, success_callback) {

    data = { "UserID": userid, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/DemoteUser",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//--------------------------------------------------------------------------------------

function DemoteMultipleUsersSuccessCallback(data) {

    BindGVs(BindGVsSuccessCallback);

}

function PromoteMultipleUsersSuccessCallback(data) {

    BindGVs(BindGVsSuccessCallback);

}

function PromoteMultipleUsers(userids, pageid, success_callback) {

    data = { "UserIDs": userids, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/PromoteMultipleUsers",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

function DemoteMultipleUsers(userids, pageid, success_callback) {

    data = { "UserIDs": userids, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/DemoteMultipleUsers",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//--------------------------------------------------------------------------------------

function InitDataTables() {

    let UsersInPage_table = $(".PageModsGV").DataTable({
        "destroy": true,
        scrollY: "60vh",
        stateSave: true,
        scrollCollapse: true,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "columnDefs": [
            { "searchable": false, "targets": [0, 2] },
            { "orderable": false, "targets": [0, 2] }
        ]
    });

    UsersInPage_table.columns.adjust().draw();

    let UsersNotInPage_table = $(".NotModsGV").DataTable({
        "destroy": true,
        scrollY: "60vh",
        stateSave: true,
        scrollCollapse: true,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "columnDefs": [
            { "searchable": false, "targets": [0, 2] },
            { "orderable": false, "targets": [0, 2] }
        ]
    });

    UsersNotInPage_table.columns.adjust().draw();

}

