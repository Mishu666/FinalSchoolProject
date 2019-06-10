$(document).ready(function () {

    $(".DeleteUserButton").on("click", function (e) {
        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));
        DeleteUser(userid, DeleteUserSuccessCallback);

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