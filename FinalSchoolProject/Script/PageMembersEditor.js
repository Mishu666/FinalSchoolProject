$(document).ready(function () {

    $(".ConsultPagesDDL").change(function (e) {
        console.log("changing");
        let pageid = $(this).val();
        $(".ToggleSubscriptionButton").attr("disabled", true);
        window.sessionStorage.setItem("pageid", pageid);

        BindInPageGV();
        BindNotInPageGV();

    });

    $("#ConsultPageGVSection").on("click", ".InclueUserButton", function (e) {

        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        let pageid = parseInt($(".ConsultPagesDDL").val());
        $(this).attr("disabled", true);

        ToggleSubscription(userid, pageid, IncludeUserSuccessCallback);

    });

    $("#ConsultPageGVSection").on("click", ".KickUserButton", function (e) {

        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        let pageid = parseInt($(".ConsultPagesDDL").val());
        $(this).attr("disabled", true);

        ToggleSubscription(userid, pageid, KickUserSuccessCallback);

    });

    $("#SelectAllInGroupSwitch").change(function (e) {

        $(".SelectInGroupSwitch").prop("checked", $(this).prop("checked"));

    });

    $("#SelectAllNotInGroupSwitch").change(function (e) {

        $(".SelectNotInGroupSwitch").prop("checked", $(this).prop("checked"));

    });

});

$(document).ajaxStart(function () {
    $("#loading_spinner").show();
});

$(document).ajaxStop(function () {
    $("#loading_spinner").hide();
});

//--------------------------------------------------------------------------------------

function BindInPageGV() {

    let pageid = window.sessionStorage.getItem("pageid");
    $("#UsersInPageSection").load("PageMembersEditor.aspx .UsersInPageGV", { PageID: pageid });
}

function BindNotInPageGV() {

    let pageid = window.sessionStorage.getItem("pageid");
    $("#UsersNotInPageSection").load("PageMembersEditor.aspx .UsersNotInPageGV", { PageID: pageid });
}

//--------------------------------------------------------------------------------------

function KickUserSuccessCallback(data) {

    BindInPageGV();
    BindNotInPageGV();

}

function IncludeUserSuccessCallback(data) {

    BindNotInPageGV();
    BindInPageGV();

}

function ToggleSubscription(userid, pageid, success_callback) {

    data = { "UserID": userid, "PageID": pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ToggleSubscription",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}