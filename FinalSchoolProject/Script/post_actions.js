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

    $("#main_content_space").on("click", ".post", function (e) {

        console.log(e.target);
        if (!$(e.target).hasClass("card-footer") && !$(e.target).hasClass("dropdown") &&
            !$(e.target).parents().hasClass("card-footer") && !$(e.target).parents().hasClass("dropdown") && !$(e.target).parents().hasClass("edit_post_view")) {

            viewPostPage($(this).data("post-id"));

        }

    });

    $("#main_content_space").on("click", ".ReportPostButton", function (e) {

        e.preventDefault();
        let post_card = $(this).closest(".card.post");
        let post_id = post_card.data("post-id");

        $("#ReportPostModal").data("post-id", post_id);
        $("#ReportPostModal").modal('show');

    });

    $("#ReportPostConfirm").on("click", function (e) {

        e.preventDefault();

        let post_id = $("#ReportPostModal").data("post-id");
        let body = $("#ReportPostTextArea").val();

        ReportPost(post_id, body, ReportPostSuccessCallback);

    });

    $(".EditPostButton").on("click", function (e) {

        e.preventDefault();
        let post_card = $(this).closest(".card.post");
        let edit_post_view = post_card.find(".edit_post_view").first();
        let default_post_view = post_card.find(".default_post_view").first();

        closeActiveEditors();
        resetForm();

        edit_post_view.show(200);


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

        resetForm();
        closeActiveEditors();
        default_post_view.show();

    });

    $("#main_content_space").on("click", ".DeletePostButton", function (e) {

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

    $("#main_content_space").on("click", ".RemovePostButton", function (e) {

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

        $(".reply_button").on("click", function (e) {
            e.preventDefault();

            let comment_card = $(this).closest(".card.comment");
            let hidden_card = comment_card.find(".card.hidden_reply").first();
            let edit_post_view = $(".edit_post_view").first();
            let default_post_view = $(".default_post_view").first();

            closeActiveEditors();
            resetForm();

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
            let comment_card = $(this).closest(".card.comment");
            let body = tinymce.activeEditor.getContent();
            let ParentCommentID = comment_card.data("comment-id");
            createCommentReply(body, ParentCommentID, createCommentReplySuccess);

        });

        $(".cancel_reply_button").on("click", function (e) {

            e.preventDefault();
            let hidden_card = $(this).closest(".card.hidden_reply");
            hidden_card.hide(200);
            resetForm();
        });

        $("#addOriginalCommentButton").on("click", function (e) {

            e.preventDefault();

            let edit_post_view = $(".edit_post_view").first();
            let default_post_view = $(".default_post_view").first();

            closeActiveEditors();
            $("#hidden_comment_card").show(200);

        });

        $("#main_content_space").on("click", ".save_post", function (e) {
            e.preventDefault();
            var parent_card = $(this).closest(".card.post");
            var post_id = parent_card.data("post-id");
            var save_btn = parent_card.find(".save_space").first();

            save_btn.toggleClass("active_action");

            SavePost(post_id, SavePostSuccessCallback);

        });

        $("#main_content_space").on("click", ".upvote", function (e) {
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

        $("#main_content_space").on("click", ".downvote", function (e) {
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

function EditPostSuccessCallback(data) {

    let warnings = data.d;

    if (warnings.length === 0) {
        console.log("added successfully");
        loadServerChanges();
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

function ReportPostSuccessCallback(data) {

    let warnings = data.d;

    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w.Text);
        $("#report_post_warning_space").append(warning);
        for (let wc of w.WarnControls) {
            $("#" + wc).addClass("border-danger");

        }
    }

    if (warnings.length === 0) {
        console.log("report success");
        $("#ReportPostModal").modal('hide');
    }

}

function ReportPost(postID, body, success_callback) {
    var data = { "PostID": postID, "reportBody": body };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndReportPost",
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

function viewPostPage(ID) {
    if (!window.location.toString().includes("ViewPost")) {
        window.location.href = "ViewPost.aspx?post-id=" + ID;
    }
}
