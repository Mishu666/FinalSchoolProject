var DateTime = luxon.DateTime;

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
        console.log(e.target);
        if (!$(e.target).hasClass("card-footer") && !$(e.target).hasClass("dropdown") &&
            !$(e.target).parents().hasClass("card-footer") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).parents().hasClass("edit_post_view")) {

            viewPostPage($(this).data("post-id"));

        }

    });

    $(".DeletePostButton").on("click", function (e) {

        e.preventDefault();
        let post_card = $(this).closest(".card.post");
        let post_id = post_card.data("post-id");
        let post_title = post_card.find(".post_title").first();
        let post_text = post_card.find(".post_text").first();
        let post_menu = post_card.find(".post_menu").first();
        let post_author_name = post_card.find(".post_author_name").first();

        DeletePost(post_id, DeletePostSuccessCallback);

        post_menu.remove();
        post_title.removeClass("text-gray-900");
        post_title.addClass("text-danger");
        post_title.text("[deleted]");
        post_text.addClass("text-danger");
        post_text.text("[deleted]");
        post_card.data("isdeleted", "true");
        post_author_name.replaceWith("<span class='text-danger post_author_name'>[deleted]</span>");


    });

    $(".RemovePostButton").on("click", function (e) {

        e.preventDefault();
        let post_card = $(this).closest(".card.post");
        let post_id = post_card.data("post-id");
        let post_title = post_card.find(".post_title").first();
        let post_text = post_card.find(".post_text").first();
        let post_menu = post_card.find(".post_menu").first();
        let post_author_name = post_card.find(".post_author_name").first();

        RemovePost(post_id, RemovePostSuccessCallback);

        post_menu.remove();
        post_title.removeClass("text-gray-900");
        post_title.addClass("text-danger");
        post_title.text("[removed]");
        post_text.addClass("text-danger");
        post_text.text("[removed]");
        post_card.data("isremoved", "true");
        post_author_name.replaceWith("<span class='text-danger post_author_name'>[removed]</span>");


    });

    $(".LockPostButton").on("click", function (e) {

        e.preventDefault();
        let post_card = $(this).closest(".card.post");
        let post_id = post_card.data("post-id");

        LockPost(post_id, LockPostSuccessCallback);

    });

    $(".EditPostButton").on("click", function (e) {

        e.preventDefault();
        let post_card = $(this).closest(".card.post");
        let edit_post_view = post_card.find(".edit_post_view").first();
        let default_post_view = post_card.find(".default_post_view").first();

        default_post_view.hide();
        edit_post_view.show(200);
        $("#hidden_comment_card").hide(200);
        $(".hidden_reply").hide(200);

    });

    $(".ConfirmPostEditButton").on("click", function (e) {

        let post_card = $(this).closest(".card.post");
        let post_id = post_card.data("post-id");

        EditPost(post_id, tinymce.activeEditor.getContent(), EditPostSuccessCallback);
    });

    $(".CancelPostEditButton").on("click", function (e) {

        e.preventDefault();
        let post_card = $(this).closest(".card.post");
        let edit_post_view = post_card.find(".edit_post_view").first();
        let default_post_view = post_card.find(".default_post_view").first();

        $(".main_form")[0].reset();
        edit_post_view.hide();
        default_post_view.show();

    });

    $(".ReportPostButton").on("click", function (e) {

        e.preventDefault();
        //let post_card = $(this).closest(".card.post");
        //let post_id = post_card.data("post-id");
        //DeletePost(post_id, DeletePostSuccessCallback);
        //post_card.hide(200, function () {
        //    post_card.remove();
        //});


    });

});

//----------------------------------------------------------------------------------------------------------------------------

function sort_by_rating(a, b) {

    let a_rating = parseInt($(a).data("rating"));
    let b_rating = parseInt($(b).data("rating"));

    if (a_rating > b_rating) return 1;
    if (a_rating < b_rating) return -1;
    return 0;
}

function sort_by_date(a, b) {
    let a_date = DateTime.fromFormat($(a).data("date"), "dd/MM/yyyy HH:mm:ss");
    let b_date = DateTime.fromFormat($(b).data("date"), "dd/MM/yyyy HH:mm:ss");

    if (a_date > b_date) return 1;
    if (a_date < b_date) return -1;
    return 0;
}

function sort_by_name(a, b) {
    return a.innerHTML.toLowerCase().localeCompare(b.innerHTML.toLowerCase());
}

//----------------------------------------------------------------------------------------------------------------------------

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

//----------------------------------------------------------------------------------------------------------------------------

function updateSort() {
    let post_list = $(".post").get();

    let option = window.sessionStorage.getItem("sort_option");
    let order = window.sessionStorage.getItem("sort_direction");

    let compareFunction = sort_by_date;
    if (option.toLowerCase() === "top") compareFunction = sort_by_rating;


    post_list.sort(compareFunction);
    if (order === "ascending") {
        post_list.reverse();
    }

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
    if (order === "ascending") {
        comment_list.reverse();
    }

    for (let i = 0; i < comment_list.length; i++) {
        comment_list[i].parentNode.append(comment_list[i]);
    }
}

//----------------------------------------------------------------------------------------------------------------------------

function userLoggedInSuccessCallback(data) {

    if (data.d === true) {

        $("#ConfirmEditButton").on("click", function (e) {
            e.preventDefault();

            clearEditUserWarnings();

            let username = $("#EditUsernameInput").val();
            let bio = tinymce.activeEditor.getContent();
            let confirm_pass = $("#EditConfirmPasswordInput").val();
            let new_password = $("#EditNewPasswordInput").val();
            let new_password_confirm = $("#EditConfirmNewPasswordInput").val();

            updateUserInfo(username, bio, confirm_pass, new_password, new_password_confirm, updateUserInfoSuccessCallback);

        });

        $("#CancelEditButton").on("click", function (e) {
            e.preventDefault();
            $("#edit_user_view").hide();
            $("#default_user_view").show();

            clearEditUserInputs();
            clearEditUserWarnings();

        });

        $("#EditUserButton").on("click", function (e) {
            e.preventDefault();
            $("#default_user_view").hide();
            $("#edit_user_view").show();
        });

        $(".reply_button").on("click", function (e) {
            e.preventDefault();
            clearAddReplyInputs();
            clearAddReplyWarnings();

            let comment_card = $(this).closest(".card.comment");
            let hidden_card = comment_card.find(".card.hidden_reply").first();
            let edit_post_view = $(".edit_post_view").first();
            let default_post_view = $(".default_post_view").first();

            default_post_view.show();
            edit_post_view.hide();
            $("#hidden_comment_card").hide(200);

            if (hidden_card.css("display") === "none") {
                $(".hidden_reply").hide(200);
                hidden_card.show(200);
            } else {
                hidden_card.hide(200);
            }

            console.log(hidden_card);

        });

        $(".confirm_reply_button").on("click", function (e) {

            e.preventDefault();
            clearAddCommentWarnings();
            let comment_card = $(this).closest(".card.comment");
            let body = tinymce.activeEditor.getContent();
            let ParentCommentID = comment_card.data("comment-id");
            createCommentReply(body, ParentCommentID, createCommentReplySuccess);

        });

        $(".cancel_reply_button").on("click", function (e) {

            e.preventDefault();
            let hidden_card = $(this).closest(".card.hidden_reply");
            hidden_card.hide(200);
            clearAddReplyInputs();
            clearAddReplyWarnings();
        });


        $("#addOriginalCommentButton").on("click", function (e) {

            e.preventDefault();

            let edit_post_view = $(".edit_post_view").first();
            let default_post_view = $(".default_post_view").first();

            default_post_view.show();
            edit_post_view.hide();
            $("#hidden_comment_card").show(200);
            $(".hidden_reply").hide(200);

        });

        $("#confirm_comment_button").on("click", function (e) {

            e.preventDefault();
            clearAddPostWarnings();
            let body = tinymce.activeEditor.getContent();
            let pageID = $(this).data("page-id");
            createNewComment(body, pageID, createNewCommentSuccess);

        });

        $("#cancel_comment_button").on("click", function (e) {

            e.preventDefault();
            $("#hidden_comment_card").hide(200);
            clearAddCommentInputs();
            clearAddCommentWarnings();
        });

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
            createPost(title, body, pageID, createPostSuccess);

        });

        $(".save_post").on("click", function (e) {
            e.preventDefault();
            var parent_card = $(this).closest(".card.post");
            var post_id = parent_card.data("post-id");
            var save_btn = parent_card.find(".save_space").first();

            save_btn.toggleClass("active_action");

            SavePost(post_id, SavePostSuccessCallback);

        });

        $(".upvote").on("click", function (e) {
            e.preventDefault();
            var parent_card = $(this).closest(".card.post");
            var post_id = parent_card.data("post-id");
            var upvote_btn = parent_card.find(".upvote_space").first();
            var downvote_btn = parent_card.find(".downvote_space").first();
            var upvote_counter = parent_card.find(".upvote_counter").first();
            var downvote_counter = parent_card.find(".downvote_counter").first();
            var current_rating = parseInt(parent_card.data("rating"));
            var isdeleted = parent_card.data("isdeleted").toLowerCase() === "true";
            var isremoved = parent_card.data("isremoved").toLowerCase() === "true";

            if (isdeleted) {
                upvote_btn.removeAttr("data-content");
                upvote_btn.popover({
                    trigger: "focus",
                    content: "cannot upvote a deleted post"
                });
                upvote_btn.popover("show");
                return;
            }

            if (isremoved) {
                upvote_btn.removeAttr("data-content");
                upvote_btn.popover({
                    trigger: "focus",
                    content: "cannot upvote a removed post"
                });
                upvote_btn.popover("show");
                return;
            }

            if (upvote_btn.hasClass("active_action")) {
                upvote_btn.removeClass("active_action");
                upvote_counter.text(parseInt(upvote_counter.text()) - 1);
                parent_card.data("rating", current_rating - 1);
            }
            else {
                if (downvote_btn.hasClass("active_action")) {
                    downvote_btn.removeClass("active_action");
                    upvote_btn.addClass("active_action");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    downvote_counter.text(parseInt(downvote_counter.text()) - 1);
                    parent_card.data("rating", current_rating + 2);

                }
                else {
                    upvote_btn.addClass("active_action");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    parent_card.data("rating", current_rating + 1);
                }
            }

            Upvote(post_id, upvoteSuccess);

        });

        $(".downvote").on("click", function (e) {
            e.preventDefault();
            var parent_card = $(this).closest(".card.post");
            var post_id = parent_card.data("post-id");
            var upvote_btn = parent_card.find(".upvote_space").first();
            var downvote_btn = parent_card.find(".downvote_space").first();
            var upvote_counter = parent_card.find(".upvote_counter").first();
            var downvote_counter = parent_card.find(".downvote_counter").first();
            var current_rating = parseInt(parent_card.data("rating"));
            var isdeleted = parent_card.data("isdeleted").toLowerCase() === "true";
            var isremoved = parent_card.data("isremoved").toLowerCase() === "true";

            if (isdeleted) {
                downvote_btn.removeAttr("data-content");
                downvote_btn.popover({
                    trigger: "focus",
                    content: "cannot downvote a deleted post"
                });
                downvote_btn.popover("show");
                return;
            }

            if (isremoved) {
                downvote_btn.removeAttr("data-content");
                downvote_btn.popover({
                    trigger: "focus",
                    content: "cannot dpwnvote a removed post"
                });
                downvote_btn.popover("show");
                return;
            }

            if (downvote_btn.hasClass("active_action")) {
                downvote_btn.removeClass("active_action");
                downvote_counter.text(parseInt(downvote_counter.text()) - 1);
                parent_card.data("rating", (current_rating + 1).toString());
            }
            else {
                if (upvote_btn.hasClass("active_action")) {
                    upvote_btn.removeClass("active_action");
                    downvote_btn.addClass("active_action");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    upvote_counter.text(parseInt(upvote_counter.text()) - 1);
                    parent_card.data("rating", (current_rating - 2).toString());

                }
                else {
                    downvote_btn.addClass("active_action");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    parent_card.data("rating", (current_rating - 1).toString());
                }
            }

            Downvote(post_id, downvoteSuccess);

        });

        //-----------------------------------------------------------------------------------------------------

        $(".comment_upvote").on("click", function (e) {

            e.preventDefault();
            var parent_card = $(this).closest(".card.comment");
            var comment_id = parent_card.data("comment-id");
            var upvote_btn = parent_card.find(".comment_upvote_space").first();
            var downvote_btn = parent_card.find(".comment_downvote_space").first();
            var upvote_counter = parent_card.find(".comment_upvote_counter").first();
            var downvote_counter = parent_card.find(".comment_downvote_counter").first();
            var current_rating = parseInt(parent_card.data("rating"));
            var isdeleted = parent_card.data("isdeleted").toLowerCase() === "true";
            var isremoved = parent_card.data("isremoved").toLowerCase() === "true";

            if (isdeleted) {
                upvote_btn.removeAttr("data-content");
                upvote_btn.popover({
                    trigger: "focus",
                    content: "cannot upvote a deleted post"
                });
                upvote_btn.popover("show");
                return;
            }

            if (isremoved) {
                upvote_btn.removeAttr("data-content");
                upvote_btn.popover({
                    trigger: "focus",
                    content: "cannot upvote a removed comment"
                });
                upvote_btn.popover("show");
                return;
            }

            if (upvote_btn.hasClass("active_action")) {
                upvote_btn.removeClass("active_action");
                upvote_counter.text(parseInt(upvote_counter.text()) - 1);
                parent_card.data("rating", (current_rating - 1).toString());
            }
            else {
                if (downvote_btn.hasClass("active_action")) {
                    downvote_btn.removeClass("active_action");
                    upvote_btn.addClass("active_action");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    downvote_counter.text(parseInt(downvote_counter.text()) - 1);
                    parent_card.data("rating", (current_rating + 2).toString());

                }
                else {
                    upvote_btn.addClass("active_action");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    parent_card.data("rating", (current_rating + 1).toString());
                }
            }

            CommentUpvote(comment_id, commentUpvoteSuccess);

        });

        $(".comment_downvote").on("click", function (e) {

            e.preventDefault();
            var parent_card = $(this).closest(".card.comment");
            var comment_id = parent_card.data("comment-id");
            var upvote_btn = parent_card.find(".comment_upvote_space").first();
            var downvote_btn = parent_card.find(".comment_downvote_space").first();
            var upvote_counter = parent_card.find(".comment_upvote_counter").first();
            var downvote_counter = parent_card.find(".comment_downvote_counter").first();
            var current_rating = parseInt(parent_card.data("rating"));
            var isdeleted = parent_card.data("isdeleted").toLowerCase() === "true";
            var isremoved = parent_card.data("isremoved").toLowerCase() === "true";

            if (isdeleted) {
                downvote_btn.removeAttr("data-content");
                downvote_btn.popover({
                    trigger: "focus",
                    content: "cannot downvote a deleted post"
                });
                downvote_btn.popover("show");
                return;
            }

            if (isremoved) {
                downvote_btn.removeAttr("data-content");
                downvote_btn.popover({
                    trigger: "focus",
                    content: "cannot downvote a removed comment"
                });
                downvote_btn.popover("show");
                return;
            }

            if (downvote_btn.hasClass("active_action")) {
                downvote_btn.removeClass("active_action");
                downvote_counter.text(parseInt(downvote_counter.text()) - 1);
                parent_card.data("rating", (current_rating + 1).toString());
            }
            else {
                if (upvote_btn.hasClass("active_action")) {
                    upvote_btn.removeClass("active_action");
                    downvote_btn.addClass("active_action");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    upvote_counter.text(parseInt(upvote_counter.text()) - 1);
                    parent_card.data("rating", (current_rating - 2).toString());

                }
                else {
                    downvote_btn.addClass("active_action");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    parent_card.data("rating", (current_rating - 1).toString());
                }
            }

            CommentDownvote(comment_id, commentDownvoteSuccess);

        });

    }
    else {

        $(".dismissable_popover").popover({
            trigger: "focus"
        });


        $(".upvote").on("click", function (e) {
            e.preventDefault();

        });

        $(".downvote").on("click", function (e) {

            e.preventDefault();
        });

        $(".comment_upvote").on("click", function (e) {

            e.preventDefault();
        });

        $(".comment_downvote").on("click", function (e) {

            e.preventDefault();
        });

    }
}

function userLoggedIn(success_callback) {

    let data = {};

    $.ajax({
        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/UserLoggedIn",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback
    });
}

//----------------------------------------------------------------------------------------------------------------------------

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

//----------------------------------------------------------------------------------------------------------------------------

function downvoteSuccess(data) {
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

//----------------------------------------------------------------------------------------------------------------------------

function commentUpvoteSuccess(data) {
    console.log(data.d);
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

//----------------------------------------------------------------------------------------------------------------------------

function commentDownvoteSuccess(data) {
    console.log(data.d);
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

//----------------------------------------------------------------------------------------------------------------------------

function createPostSuccess(data) {
    let warnings = data.d;

    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w.Text);
        $("#add_post_warning_space").append(warning);
        for (let wc of w.WarnControls) {
            $("#" + wc).addClass("border-danger");

        }
    }

    if (warnings.length === 0) {
        console.log("added successfully");
        window.location.reload();
    }
}

function createPost(title, body, pageID, success_callback) {
    var data = { "title": title, "Body": body, "PageID": pageID };

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

//----------------------------------------------------------------------------------------------------------------------------

function reportPostSuccessCallback(data) {

}

function reportPost(postID, body, success_callback) {
    var data = { "PostID": postID, "reportBody": body };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ReportPost",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });
}

//----------------------------------------------------------------------------------------------------------------------------

function EditPostSuccessCallback(data) {

    let warnings = data.d;

    if (warnings.length === 0) {
        console.log("added successfully");
        window.location.reload();
    }
    else {

        $(".EditPostWarningSpace").text("Post cannot be empty");
        $(".EditPostWarningSpace").show();

    }

}

function EditPost(postID, body, success_callback) {

    var data = { "Body": body, "PostID": postID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndEditPost",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//----------------------------------------------------------------------------------------------------------------------------

function createNewCommentSuccess(data) {
    let warnings = data.d;

    if (warnings.length === 0) {
        console.log("added successfully");
        window.location.reload();
    }
    else {
        $("#add_comment_warning_space").text(warnings[0].Text);
        $("#add_comment_warning_space").show();
    }
}

function createNewComment(body, post_id, success_callback) {
    var data = { "Body": body, "ParentPostID": post_id };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndCreateComment",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });
}

//----------------------------------------------------------------------------------------------------------------------------

function createCommentReplySuccess(data) {
    let warnings = data.d;

    if (warnings.length === 0) {
        console.log("added successfully");
        window.location.reload();
    }
    else {
        $(".add_reply_warning_space").text(warnings[0].Text);
        $(".add_reply_warning_space").show();
    }
}

function createCommentReply(body, parent_comment_id, success_callback) {
    var data = { "Body": body, "ParentCommentID": parent_comment_id };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndCreateCommentReply",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });
}

//----------------------------------------------------------------------------------------------------------------------------

function updateUserInfoSuccessCallback(data) {

    let warnings = data.d;

    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w.Text);
        $("#edit_user_warning_space").append(warning);
        for (let wc of w.WarnControls) {
            $("." + wc).addClass("border-danger");

        }
    }

    if (warnings.length === 0) {
        console.log("added successfully");
        window.location.reload();
    }

}

function updateUserInfo(username, bio, password_confirm, new_pass, new_pass_confirm, success_callback) {
    var data = {
        "Username": username,
        "Bio": bio,
        "PasswordConfirm": password_confirm,
        "NewPassword": new_pass,
        "NewPasswordConfirm": new_pass_confirm
    };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndUpdateUserInfo",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });
}

//----------------------------------------------------------------------------------------------------------------------------

function SavePostSuccessCallback(data) {

}

function SavePost(post_id, success_callback) {

    var data = { "PostID": post_id };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/SavePost",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });
}

//----------------------------------------------------------------------------------------------------------------------------

function DeletePostSuccessCallback(data) {

}

function DeletePost(post_id, success_callback) {


    var data = { "PostID": post_id };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/DeletePost",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//----------------------------------------------------------------------------------------------------------------------------

function RemovePostSuccessCallback(data) {

}

function RemovePost(post_id, success_callback) {


    var data = { "PostID": post_id };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/RemovePost",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//----------------------------------------------------------------------------------------------------------------------------

function LockPostSuccessCallback(data) {

}

function LockPost(post_id, success_callback) {


    var data = { "PostID": post_id };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/LockPost",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//----------------------------------------------------------------------------------------------------------------------------

function viewPostPage(ID) {
    if (!window.location.toString().includes("ViewPost")) {
        window.location.href = "ViewPost.aspx?post-id=" + ID;
    }
}

//----------------------------------------------------------------------------------------------------------------------------

function clearAddPostWarnings() {
    $("#add_post_warning_space .alert").remove();
    $("#addPostTitle").removeClass("border-danger");
    $("#addPostBody").removeClass("border-danger");
}

function clearAddPostInputs() {

    $("#addPostTitle").val("");
    $("#addPostBody").val("");
}

function clearAddCommentWarnings() {
    $("#add_comment_warning_space .alert").remove();
    $("#addCommentBody").removeClass("border-danger");
}

function clearAddCommentInputs() {

    $("#addCommentBody").val("");
}

function clearAddReplyWarnings() {
    $(".add_reply_warning_space .alert").remove();
    $(".addReplyBody").removeClass("border-danger");
}

function clearAddReplyInputs() {

    $(".addReplyBody").val("");
}

function clearEditUserInputs() {
    $(".main_form")[0].reset();
}

function clearEditUserWarnings() {
    $("#edit_user_warning_space .alert").remove();
    $("#EditBioInput").removeClass("border-danger");
    $("#EditConfirmPasswordInput").removeClass("border-danger");
}