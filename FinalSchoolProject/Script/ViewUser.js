$(document).ready(function () {



});

function getUserPostsSuccess(data) {

    let posts = data.d;

    for (let post of posts) {
        console.log(post);
    }

}

function getUserPosts(success_callback) {
    let data = {};

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/GetUserPosts",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });

}