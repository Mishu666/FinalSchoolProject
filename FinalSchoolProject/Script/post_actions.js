$(document).ready(function () {

    userLoggedIn(userLoggedInSuccessCallback);

    let sort_dir = window.sessionStorage.getItem("sort_direction");
    if (sort_dir === null) {
        sort_dir = "ascending";
    }

    let sort_option = window.sessionStorage.getItem("sort_option");
    if (sort_option === null) {
        sort_option = "new";
    }

    setSortDirection(sort_dir);

    setSortOption(sort_option);


    updateSort();

    updateCommentSort();


    $("#sortDirectionDropdown").on("click", function (e) {
        let button = $("#sortDirectionDropdown > i");
        if (button.hasClass("fa-arrow-up")) {
            setSortDirection("ascending");
        }
        else if (button.hasClass("fa-arrow-down")) {
            setSortDirection("descending");
        }
        updateSort();
        updateCommentSort();
    });

    $("#sortByNew").on("click", function (e) {

        setSortOption("new");
        updateSort();
        updateCommentSort();

    });

    $("#sortByRating").on("click", function (e) {

        setSortOption("top");
        updateSort();
        updateCommentSort();

    });

    $(".post").on("click", function (e) {
        if (!$(e.target).hasClass("upvote") && !$(e.target).hasClass("downvote")) {

            viewPostPage($(this).data("id"));

        }

    });

    $(".collapse_children").on("click", function (e) {

        let comment_id = $(this).parent().data("comment-id");
        let child_space = $(".comment[data-comment-id='" + comment_id + "'] > #comment_footer > .child_comments_space");
        if ($(this).hasClass("fa-chevron-up")) {
            $(this).removeClass("fa-chevron-up");
            $(this).addClass("fa-chevron-down");
            child_space.hide(200);
        }
        else if ($(this).hasClass("fa-chevron-down")) {
            $(this).removeClass("fa-chevron-down");
            $(this).addClass("fa-chevron-up");
            child_space.show(200);
        }
    });


});

//-----------------------------------------------------------------------------------------------------------------

function sort_by_rating(a, b) {

    let a_rating = parseInt($(a).data("rating"));
    let b_rating = parseInt($(b).data("rating"));

    if (a_rating > b_rating) return 1;
    if (a_rating < b_rating) return -1;
    return 0;
}

function sort_by_date(a, b) {
    let a_date = Date.parse($(a).data("date"));
    let b_date = Date.parse($(b).data("date"));

    if (a_date > b_date) return 1;
    if (a_date < b_date) return -1;
    return 0;
}

function sort_by_name(a, b) {
    return a.innerHTML.toLowerCase().localeCompare(b.innerHTML.toLowerCase());
}

function setSortOption(new_option) {

    window.sessionStorage.setItem("sort_option", new_option.toLowerCase());
    $("#sort_option_text").text(new_option.toLowerCase());

}

function setSortDirection(new_dir) {

    window.sessionStorage.setItem("sort_direction", new_dir);

    if (new_dir === "descending") {
        $("#sortDirectionDropdown > i").addClass("fa-arrow-up");
        $("#sortDirectionDropdown > i").removeClass("fa-arrow-down");
    }
    if (new_dir === "ascending") {

        $("#sortDirectionDropdown > i").addClass("fa-arrow-down");
        $("#sortDirectionDropdown > i").removeClass("fa-arrow-up");
    }


}

function updateSort() {
    let post_list = $(".post").get();

    let option = window.sessionStorage.getItem("sort_option");
    let order = window.sessionStorage.getItem("sort_direction");

    let compareFunction = sort_by_date;
    if (option.toLowerCase() === "top") compareFunction = sort_by_rating;


    post_list.sort(compareFunction);
    if (order === "ascending") post_list.reverse();

    console.log(post_list);

    for (let i = 0; i < post_list.length; i++) {
        post_list[i].parentNode.append(post_list[i]);
    }
}

function updateCommentSort() {
    let comment_list = $(".original_comments > .comment").get();

    let option = window.sessionStorage.getItem("sort_option");
    let order = window.sessionStorage.getItem("sort_direction");

    let compareFunction = sort_by_date;
    if (option.toLowerCase() === "top") compareFunction = sort_by_rating;


    comment_list.sort(compareFunction);
    if (order === "ascending") comment_list.reverse();

    for (let i = 0; i < comment_list.length; i++) {
        comment_list[i].parentNode.append(comment_list[i]);
    }
}

//---------------------------------------------------------------------------------------------------------

function userLoggedInSuccessCallback(data) {

    if (data.d === true) {

        $("#addPostInConsultPageButton").on("click", function (e) {

            e.preventDefault();
            $("#hidden_post_card").show(200);

        });

        $("#cancel_post_button").on("click", function (e) {

            e.preventDefault();
            $("#hidden_post_card").hide(200);
            clearAddPostInputs();
            clearAddPostWarnings();
        });

        $("#confirm_post_button").on("click", function (e) {

            e.preventDefault();
            clearAddPostWarnings();
            let title = $("#addPostTitle").val();
            let body = $("#addPostBody").val();
            let pageID = $(this).data("page-id");
            createPost(title, body, pageID, createPostSuccessCallback);

        });

        $(".upvote").on("click", function (e) {

            e.preventDefault();
            var post_id = $(this).parent().data("post-id");
            var upvote_btn = $(this).parent();
            var downvote_btn = $(".downvote_space[data-post-id='" + post_id + "']");
            var upvote_counter = $(".upvote_counter[data-post-id='" + post_id + "']");
            var downvote_counter = $(".downvote_counter[data-post-id='" + post_id + "']");

            if (upvote_btn.hasClass("active")) {
                upvote_btn.removeClass("active");
                upvote_counter.text(parseInt(upvote_counter.text()) - 1);
            }
            else {
                if (downvote_btn.hasClass("active")) {
                    downvote_btn.removeClass("active");
                    upvote_btn.addClass("active");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    downvote_counter.text(parseInt(downvote_counter.text()) - 1);

                }
                else {
                    upvote_btn.addClass("active");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                }
            }

            Upvote(post_id, upvoteSuccess);

        });

        $(".downvote").on("click", function (e) {
            e.preventDefault();
            var post_id = $(this).parent().data("post-id");
            var downvote_btn = $(this).parent();
            var upvote_btn = $(".upvote_space[data-post-id='" + post_id + "']");
            var downvote_counter = $(".downvote_counter[data-post-id='" + post_id + "']");
            var upvote_counter = $(".upvote_counter[data-post-id='" + post_id + "']");

            if (downvote_btn.hasClass("active")) {
                downvote_btn.removeClass("active");
                downvote_counter.text(parseInt(downvote_counter.text()) - 1);
            }
            else {
                if (upvote_btn.hasClass("active")) {
                    upvote_btn.removeClass("active");
                    downvote_btn.addClass("active");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    upvote_counter.text(parseInt(upvote_counter.text()) - 1);

                }
                else {
                    downvote_btn.addClass("active");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                }
            }

            Downvote(post_id, downvoteSuccess);

        });

        //-----------------------------------------------------------------------------------------------------

        $(".comment_upvote").on("click", function (e) {

            e.preventDefault();
            var comment_id = $(this).parent().data("comment-id");
            var upvote_btn = $(this).parent();
            var downvote_btn = $(".comment_downvote_space[data-comment-id='" + comment_id + "']");
            var upvote_counter = $(".comment_upvote_counter[data-comment-id='" + comment_id + "']");
            var downvote_counter = $(".comment_downvote_counter[data-comment-id='" + comment_id + "']");

            if (upvote_btn.hasClass("active")) {
                upvote_btn.removeClass("active");
                upvote_counter.text(parseInt(upvote_counter.text()) - 1);
            }
            else {
                if (downvote_btn.hasClass("active")) {
                    downvote_btn.removeClass("active");
                    upvote_btn.addClass("active");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    downvote_counter.text(parseInt(downvote_counter.text()) - 1);

                }
                else {
                    upvote_btn.addClass("active");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                }
            }

            CommentUpvote(comment_id, commentUpvoteSuccess);

        });

        $(".comment_downvote").on("click", function (e) {

            e.preventDefault();
            var comment_id = $(this).parent().data("comment-id");
            var downvote_btn = $(this).parent();
            var upvote_btn = $(".comment_upvote_space[data-comment-id='" + comment_id + "']");
            var downvote_counter = $(".comment_downvote_counter[data-comment-id='" + comment_id + "']");
            var upvote_counter = $(".comment_upvote_counter[data-comment-id='" + comment_id + "']");

            if (downvote_btn.hasClass("active")) {
                downvote_btn.removeClass("active");
                downvote_counter.text(parseInt(downvote_counter.text()) - 1);
            }
            else {
                if (upvote_btn.hasClass("active")) {
                    upvote_btn.removeClass("active");
                    downvote_btn.addClass("active");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    upvote_counter.text(parseInt(upvote_counter.text()) - 1);

                }
                else {
                    downvote_btn.addClass("active");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                }
            }

            CommentDownvote(comment_id, commentDownvoteSuccess);

        });

    }
    else {

        $("#addPostInConsultPageButton").popover({
            trigger: "focus"
        });

        $(".dismissable_popover").popover({
            trigger: "focus"
        });


        $(".upvote_space").on("click", function (e) {
            console.log("dooot");
            e.preventDefault();

        });

        $(".downvote_space").on("click", function (e) {

            e.preventDefault();
        });

        $(".comment_upvote_space").on("click", function (e) {

            e.preventDefault();
        });

        $(".comment_downvote_space").on("click", function (e) {

            e.preventDefault();
        });

    }
}

//--------------------------------------------------------------------------------------------

function downvoteSuccess(data) {
}

function upvoteSuccess(data) {

}

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

//-----------------------------------------------------------------------------------------

function commentDownvoteSuccess(data) {
    console.log(data.d);
}

function commentUpvoteSuccess(data) {
    console.log(data.d);
}

function createPostSuccessCallback(data) {
    let warnings = data.d;
    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w);
        $("#add_post_warning_space").append(warning);
        if (w === "Title cannot be empty") {
            $("#addPostTitle").addClass("border-danger");
        }
        if (w === "Post cannot be empty") {
            $("#addPostBody").addClass("border-danger");
        }
    }

    if (warnings.length === 0) {
        console.log("added successfully");
        window.location.reload();
    }
}

function CommentUpvote(commentID, success_callback) {

    var data = { "CommentID": commentID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/CommentUpvote",
        error: function (r) {
            console.log("error");
            console.log(r.status);
        },
        success: success_callback

    });

}

function CommentDownvote(commentID, success_callback) {

    var data = { "CommentID": commentID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/CommentDownvote",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

function createPost(title, body, pageID, success_callback) {
    var data = { "title": title, "body": body, "PageID": pageID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndCreatePost",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });
}

function reportPost(postID, success_callback) {
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

//-------------------------------------------------------------------------------------------------------

function viewPostPage(ID) {
    if (!window.location.toString().includes("ViewPost")) {
        window.location.href = "ViewPost.aspx?post-id=" + ID;
    }
}

function clearAddPostWarnings() {
    $("#add_post_warning_space .alert").remove();
    $("#addPostTitle").removeClass("border-danger");
    $("#addPostBody").removeClass("border-danger");
}

function clearAddPostInputs() {

    $("#addPostTitle").val("");
    $("#addPostBody").val("");
}