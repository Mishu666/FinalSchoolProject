$(document).ready(function () {

    AssignAdminSwitchLabels();
    AssignSuspendedSwitchLabels();

    $(".DeleteUserButton").on("click", function (e) {
        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        DeleteUser(userid, DeleteUserSuccessCallback);

    })

});

function AssignAdminSwitchLabels() {

    let switches = $(".IsAdminSwitch");
    let switchLabel;
    let switchContainer;

    for (let s of switches) {

        switchContainer = $(s.closest("div"));
        switchLabel = switchContainer.find("label").first();

        switchLabel.attr("for", s.id);

    }
}

function AssignSuspendedSwitchLabels() {

    let switches = $(".IsSuspendedSwitch");
    let switchLabel;
    let switchContainer;

    for (let s of switches) {

        switchContainer = $(s.closest("div"));
        switchLabel = switchContainer.find("label").first();

        switchLabel.attr("for", s.id);

    }
}

//--------------------------------------------------------------------------------------

function DeleteUserSuccessCallback(data) {

}

function DeleteUser(userid, success_callbcak) {

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