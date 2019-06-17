$(document).ready(function () {

    InitDataTables();

    $(".ConsultPagesDDL").change(function (e) {

        let pageid = $(this).val();
        $(".ToggleSubscriptionButton").attr("disabled", true);
        window.sessionStorage.setItem("pageid", pageid);

        BindGVs(BindGVsSuccessCallback);
    });

    $("#ConsultPageGVSection").on("click", ".InclueUserButton", function (e) {

        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        let pageid = parseInt($(".ConsultPagesDDL").val());
        $(this).attr("disabled", true);

        IncludeUser(userid, pageid, IncludeUserSuccessCallback);

    });

    $("#ConsultPageGVSection").on("click", ".KickUserButton", function (e) {

        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        let pageid = parseInt($(".ConsultPagesDDL").val());
        $(this).attr("disabled", true);

        KickUser(userid, pageid, KickUserSuccessCallback);

    });

    $("#ConsultPageGVSection").on("change", ".SelectAllInGroupSwitch", function (e) {

        $(".SelectInGroupSwitch").prop("checked", $(this).prop("checked"));

    });

    $("#ConsultPageGVSection").on("change", ".SelectAllNotInGroupSwitch", function (e) {

        $(".SelectNotInGroupSwitch").prop("checked", $(this).prop("checked"));

    });

    $("#ConsultPageGVSection").on("click", "#KickFromPageButton", function (e) {

        e.preventDefault();

        let pageid = parseInt($(".ConsultPagesDDL").val());
        let users_to_kick = [];

        $('input:checked.SelectInGroupSwitch').each(function () {
            users_to_kick.push($(this).data('user-id'));
        });

        if (users_to_kick.length > 0) {
            KickMultipleUsers(users_to_kick, pageid, KickMultipleUsersSuccessCallback);
        }
    });

    $("#ConsultPageGVSection").on("click", "#IncludeInPageButton", function (e) {

        e.preventDefault();

        let pageid = parseInt($(".ConsultPagesDDL").val());
        let users_to_include = [];

        $('input:checked.SelectNotInGroupSwitch').each(function () {
            users_to_include.push($(this).data('user-id'));
        });

        if (users_to_include.length > 0) {
            IncludeMultipleUsers(users_to_include, pageid, IncludeMultipleUsersSuccessCallback);
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
    let UsersInPageSec = response.find("#UsersInPageSection");
    let UsersNotInPageSec = response.find("#UsersNotInPageSection");

    $("#UsersInPageSection").replaceWith(UsersInPageSec);
    $("#UsersNotInPageSection").replaceWith(UsersNotInPageSec);

    InitDataTables();

}

function BindGVs(success_callback) {

    let pageid = window.sessionStorage.getItem("pageid");

    data = { "PageID": pageid };

    $.get("PageMembersEditor.aspx", data, success_callback);


}

//--------------------------------------------------------------------------------------

function KickUserSuccessCallback(data) {

    BindGVs(BindGVsSuccessCallback);

}

function IncludeUserSuccessCallback(data) {

    BindGVs(BindGVsSuccessCallback);

}

function IncludeUser(userid, pageid, success_callback) {

    data = { "UserID": userid, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/IncludeUser",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

function KickUser(userid, pageid, success_callback) {

    data = { "UserID": userid, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/KickUser",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//--------------------------------------------------------------------------------------

function KickMultipleUsersSuccessCallback(data) {

    BindGVs(BindGVsSuccessCallback);

}

function IncludeMultipleUsersSuccessCallback(data) {

    BindGVs(BindGVsSuccessCallback);

}

function IncludeMultipleUsers(userids, pageid, success_callback) {

    data = { "UserIDs": userids, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/IncludeMultipleUsers",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

function KickMultipleUsers(userids, pageid, success_callback) {

    data = { "UserIDs": userids, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/KickMultipleUsers",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//--------------------------------------------------------------------------------------

function InitDataTables() {

    let UsersInPage_table = $(".UsersInPageGV").DataTable({
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

    let UsersNotInPage_table = $(".UsersNotInPageGV").DataTable({
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

