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
            !$(e.target).parents().hasClass("card-footer") && !$(e.target).parents().hasClass("dropdown")) {

            viewPostPage($(this).data("post-id"));

        }

    });

    $(".collapse_children").on("click", function (e) {

        let comment_card = $(this).closest(".card.comment");
        let child_space = comment_card.find(".child_comments_space");
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

    $(".DeletePostButton").on("click", function (e) {

        e.preventDefault();
        let post_card = $(this).closest(".card.post");
        let post_id = post_card.data("post-id");
        DeletePost(post_id, DeletePostSuccessCallback);
        post_card.hide(200, function() {
            post_card.remove();
        });


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
    let a_date = DateTime.fromFormat($(a).data("date"), "dd/MM/yyyy HH:mm:ss");
    let b_date = DateTime.fromFormat($(b).data("date"), "dd/MM/yyyy HH:mm:ss");

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
    if (order === "ascending") comment_list.reverse();

    for (let i = 0; i < comment_list.length; i++) {
        comment_list[i].parentNode.append(comment_list[i]);
    }
}

//---------------------------------------------------------------------------------------------------------

function userLoggedInSuccessCallback(data) {

    if (data.d === true) {

        $("#ConfirmEditButton").on("click", function (e) {
            e.preventDefault();

            clearEditUserWarnings();

            let username = $("#EditUsernameInput").val();
            let bio = $("#EditBioInput").val();
            let confirm_pass = $("#EditConfirmPasswordInput").val();
            let isprivate = $("#EditIsPrivateSwitch").prop("checked");

            updateUserInfo(username, bio, isprivate, confirm_pass, updateUserInfoSuccessCallback);

        });

        $("#CancelEditButton").on("click", function (e) {
            e.preventDefault();
            $("#edit_user_view").hide();
            $("#default_user_view").show();

            clearEditUserInputs();
            clearEditUserWarnings();

        });

        $(".EditUserButton").on("click", function (e) {
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
            let body = comment_card.find(".addReplyBody").first().val();
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
            $("#hidden_comment_card").show(200);
            $(".hidden_reply").hide(200);

        });

        $("#confirm_comment_button").on("click", function (e) {

            e.preventDefault();
            clearAddPostWarnings();
            let body = $("#addCommentBody").val();
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
            var save_btn = parent_card.find(".save_space");

            save_btn.toggleClass("active_action");

            SavePost(post_id, SavePostSuccessCallback);

        });

        $(".upvote").on("click", function (e) {
            e.preventDefault();
            var parent_card = $(this).closest(".card.post");
            var post_id = parent_card.data("post-id");
            var upvote_btn = parent_card.find(".upvote_space");
            var downvote_btn = parent_card.find(".downvote_space");
            var upvote_counter = parent_card.find(".upvote_counter");
            var downvote_counter = parent_card.find(".downvote_counter");

            if (upvote_btn.hasClass("active_action")) {
                upvote_btn.removeClass("active_action");
                upvote_counter.text(parseInt(upvote_counter.text()) - 1);
            }
            else {
                if (downvote_btn.hasClass("active_action")) {
                    downvote_btn.removeClass("active_action");
                    upvote_btn.addClass("active_action");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    downvote_counter.text(parseInt(downvote_counter.text()) - 1);

                }
                else {
                    upvote_btn.addClass("active_action");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                }
            }

            Upvote(post_id, upvoteSuccess);

        });

        $(".downvote").on("click", function (e) {
            e.preventDefault();
            var parent_card = $(this).closest(".card.post");
            var post_id = parent_card.data("post-id");
            var upvote_btn = parent_card.find(".upvote_space");
            var downvote_btn = parent_card.find(".downvote_space");
            var upvote_counter = parent_card.find(".upvote_counter");
            var downvote_counter = parent_card.find(".downvote_counter");

            if (downvote_btn.hasClass("active_action")) {
                downvote_btn.removeClass("active_action");
                downvote_counter.text(parseInt(downvote_counter.text()) - 1);
            }
            else {
                if (upvote_btn.hasClass("active_action")) {
                    upvote_btn.removeClass("active_action");
                    downvote_btn.addClass("active_action");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    upvote_counter.text(parseInt(upvote_counter.text()) - 1);

                }
                else {
                    downvote_btn.addClass("active_action");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                }
            }

            Downvote(post_id, downvoteSuccess);

        });

        //-----------------------------------------------------------------------------------------------------

        $(".comment_upvote").on("click", function (e) {

            e.preventDefault();
            var parent_card = $(this).closest(".card.comment");
            var comment_id = parent_card.data("comment-id");
            var upvote_btn = parent_card.find(".comment_upvote_space");
            var downvote_btn = parent_card.find(".comment_downvote_space");
            var upvote_counter = parent_card.find(".comment_upvote_counter");
            var downvote_counter = parent_card.find(".comment_downvote_counter");

            if (upvote_btn.hasClass("active_action")) {
                upvote_btn.removeClass("active_action");
                upvote_counter.text(parseInt(upvote_counter.text()) - 1);
            }
            else {
                if (downvote_btn.hasClass("active_action")) {
                    downvote_btn.removeClass("active_action");
                    upvote_btn.addClass("active_action");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                    downvote_counter.text(parseInt(downvote_counter.text()) - 1);

                }
                else {
                    upvote_btn.addClass("active_action");
                    upvote_counter.text(parseInt(upvote_counter.text()) + 1);
                }
            }

            CommentUpvote(comment_id, commentUpvoteSuccess);

        });

        $(".comment_downvote").on("click", function (e) {

            e.preventDefault();
            var parent_card = $(this).closest(".card.comment");
            var comment_id = parent_card.data("comment-id");
            var upvote_btn = parent_card.find(".comment_upvote_space");
            var downvote_btn = parent_card.find(".comment_downvote_space");
            var upvote_counter = parent_card.find(".comment_upvote_counter");
            var downvote_counter = parent_card.find(".comment_downvote_counter");

            if (downvote_btn.hasClass("active_action")) {
                downvote_btn.removeClass("active_action");
                downvote_counter.text(parseInt(downvote_counter.text()) - 1);
            }
            else {
                if (upvote_btn.hasClass("active_action")) {
                    upvote_btn.removeClass("active_action");
                    downvote_btn.addClass("active_action");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
                    upvote_counter.text(parseInt(upvote_counter.text()) - 1);

                }
                else {
                    downvote_btn.addClass("active_action");
                    downvote_counter.text(parseInt(downvote_counter.text()) + 1);
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

function createNewCommentSuccess(data) {
    let warnings = data.d;

    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w.Text);
        $("#add_comment_warning_space").append(warning);
        for (let wc of w.WarnControls) {
            $("#" + wc).addClass("border-danger");

        }
    }

    if (warnings.length === 0) {
        console.log("added successfully");
        window.location.reload();
    }
}

function createCommentReplySuccess(data) {
    let warnings = data.d;

    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w.Text);
        $(".add_reply_warning_space").append(warning);
        for (let wc of w.WarnControls) {
            $("." + wc).addClass("border-danger");

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

//----------------------------------------------------------------------------------------------------------------------------

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

//function reportPost(postID, success_callback) {
//    var data = { "PostID": postID };

//    $.ajax({

//        method: "POST",
//        data: JSON.stringify(data),
//        dataType: "json",
//        contentType: "application/json; charset=utf-8 ",
//        url: "UsersService.asmx/Downvote",
//        error: function (r) {
//            console.log("error");
//            console.log(r.responseText);
//        },
//        success: success_callback

//    });
//}

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

//-------------------------------------------------------------------------------------------------------

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

function updateUserInfo(username, bio, is_private, password_confirm, success_callback) {
    var data = { "Username": username, "Bio": bio, "IsPrivate": is_private, "PasswordConfirm": password_confirm };

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

//-------------------------------------------------------------------------------------------------------

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

//-------------------------------------------------------------------------------------------------------

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
    $("#EditUsernameInput").val($("#EditUsernameInput").data("default-value"));
    $("#EditBioInput").val($("#EditBioInput").data("default-value"));
    $("#EditConfirmPasswordInput").val("");
    $("#EditIsPrivateSwitch").prop("checked", $("#EditIsPrivateSwitch").data("default-value"));
}

function clearEditUserWarnings() {
    $("#edit_user_warning_space .alert").remove();
    $("#EditBioInput").removeClass("border-danger");
    $("#EditConfirmPasswordInput").removeClass("border-danger");
}