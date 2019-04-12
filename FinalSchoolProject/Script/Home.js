﻿$(document).ready(function () {

    userLoggedIn(userLoggedInSuccessCallback);

    function userLoggedInSuccessCallback(data) {
        console.log(data.d);
        if (data.d === true) {

            $(".upvote").on("click", function (e) {

                e.preventDefault();
                var post_id = $(this).data("post-id");
                var upvote_btn = $(this);
                var downvote_btn = $(".downvote[data-post-id='" + upvote_btn.data("post-id") + "']");
                var vote_counter = $("#vote_counter[data-post-id='" + post_id + "']");

                if (upvote_btn.hasClass("active")) {
                    upvote_btn.removeClass("active");
                    vote_counter.text(parseInt(vote_counter.text()) - 1);
                }
                else {
                    if (downvote_btn.hasClass("active")) {
                        downvote_btn.removeClass("active");
                        upvote_btn.addClass("active");
                        vote_counter.text(parseInt(vote_counter.text()) + 2);

                    }
                    else {
                        upvote_btn.addClass("active");
                        vote_counter.text(parseInt(vote_counter.text()) + 1);
                    }
                }

                function upvoteSuccess(data) {
                }

                Upvote(post_id, upvoteSuccess);

            });

            $(".downvote").on("click", function (e) {

                e.preventDefault();
                var post_id = $(this).data("post-id");
                var downvote_btn = $(this);
                var upvote_btn = $(".upvote[data-post-id='" + downvote_btn.data("post-id") + "']");
                var vote_counter = $("#vote_counter[data-post-id='" + post_id + "']");

                if (downvote_btn.hasClass("active")) {
                    downvote_btn.removeClass("active");
                    vote_counter.text(parseInt(vote_counter.text()) + 1);
                }
                else {
                    if (upvote_btn.hasClass("active")) {
                        upvote_btn.removeClass("active");
                        downvote_btn.addClass("active");
                        vote_counter.text(parseInt(vote_counter.text()) - 2);

                    }
                    else {
                        downvote_btn.addClass("active");
                        vote_counter.text(parseInt(vote_counter.text()) - 1);
                    }
                }

                function downvoteSuccess(data) {
                }

                Downvote(post_id, downvoteSuccess);

            });
        }
        else {

            $('[data-toggle="popover"]').popover();

            $(".upvote").on("click", function (e) {
                console.log("oops");
                e.preventDefault();
                $(this).popover();

            });

            $(".downvote").on("click", function (e) {

                e.preventDefault();
                $(this).popover();

            });

        }
    }

});

function Upvote(postID, success_callback) {

    var data = { "PostID": postID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/Upvote",
        error: function (r) {
            console.log("error");
            console.log(r.status);
        },
        success: success_callback

    });

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