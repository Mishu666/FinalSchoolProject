$(document).ready(function () {

    $(".upvote").on("click", function (e) {

        e.preventDefault();
        var post_id = $(this).data("id");
        Upvote(post_id);

    });

    $(".downvote").on("click", function (e) {

        e.preventDefault();
        var post_id = $(this).data("id");
        Downvote(post_id);

    });

});

function Upvote(postID, success_callback) {

    var data = { "PostID": postID };
    alert(this);
    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/Upvote",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

function colorVoteButton(buttonID, voteType) {
    if (voteType == "upvote") {
        $("#" + buttonID).addClass(""); //CONTINUE HERE
    }
}

function Downvote(postID, success_callback) {

    var data = { "PostID": postID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/Downvote",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}