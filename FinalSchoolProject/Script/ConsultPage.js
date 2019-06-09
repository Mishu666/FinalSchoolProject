$(document).ready(function () {

    $(".SubscribeButton").on("click", function (e) {
        e.preventDefault();

        let page_id = parseInt($(this).data("pageid"));
        Subscibe(page_id, SubscribeSuccessCallback);

        $(this).toggleClass("btn-success");
        $(this).toggleClass("btn-primary");
        if ($(this).hasClass("btn-success")) {
            $(this).html("<i class='fas fa-check mr-2'></i>Subscribed");
        }
        if ($(this).hasClass("btn-primary")) {
            $(this).html("<i class='fas fa-plus mr-2'></i>Subscribe");
        }

    });

});

//--------------------------------------------------------------------------------------------

function SubscribeSuccessCallback(data) {

}

function Subscibe(pageid, success_callback) {

    var data = { "PageID": pageid};

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/Subscribe",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}