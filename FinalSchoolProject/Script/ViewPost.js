$(document).ready(function () {

    tinymce.init({
        selector: '.EditableArea',
        width: '100%',
        height: 400,
        resize: false,
        branding: false,
        menubar: 'file edit insert view format table tools',
        plugins: [
            'advlist autolink lists link image charmap print preview anchor',
            'searchreplace visualblocks code fullscreen emoticons directionality',
            'insertdatetime media table paste code help wordcount'
        ],
        toolbar: 'undo redo | formatselect | link bold italic forecolor backcolor emoticons | alignleft aligncenter alignright alignjustify ltr rtl | bullist numlist outdent indent | removeformat | help'
    });


    $(".collapse_children").on("click", function (e) {

        let comment_card = $(this).closest(".card.comment");
        let child_space = comment_card.find(".child_comments_space").first();
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


    $(".DeleteCommentButton").on("click", function (e) {

        e.preventDefault();
        let comment_card = $(this).closest(".card.comment");
        let comment_id = comment_card.data("comment-id");
        let comment_text = comment_card.find(".comment_text").first();
        let comment_menu = comment_card.find(".comment_menu").first();
        let comment_author_name = comment_card.find(".comment_author_name").first();

        DeleteComment(comment_id, DeleteCommentSuccessCallback);

        comment_menu.remove();
        comment_text.addClass("text-danger");
        comment_text.text("[deleted]");
        comment_card.data("isdeleted", "true");
        comment_author_name.replaceWith("<span class='text-danger comment_author_name'>[removed]</span>");


    });

    $(".RemoveCommentButton").on("click", function (e) {

        e.preventDefault();
        let comment_card = $(this).closest(".card.comment");
        let comment_id = comment_card.data("comment-id");
        let comment_text = comment_card.find(".comment_text").first();
        let comment_menu = comment_card.find(".comment_menu").first();
        let comment_author_name = comment_card.find(".comment_author_name").first();

        RemoveComment(comment_id, RemoveCommentSuccessCallback);

        comment_menu.remove();
        comment_text.addClass("text-danger");
        comment_text.text("[removed]");
        comment_card.data("isremoved", "true");
        comment_author_name.replaceWith("<span class='text-danger comment_author_name'>[removed]</span>");


    });

    $(".EditCommentButton").on("click", function (e) {

        e.preventDefault();
        let comment_card = $(this).closest(".card.comment");
        let edit_comment_view = comment_card.find(".edit_comment_view").first();
        let default_comment_view = comment_card.find(".default_comment_view").first();

        closeActiveEditors();
        resetForm();

        edit_comment_view.show();

    });

    $(".ConfirmCommentEditButton").on("click", function (e) {

        let comment_card = $(this).closest(".card.comment");
        let comment_id = comment_card.data("comment-id");

        EditComment(comment_id, tinymce.activeEditor.getContent(), EditCommentSuccessCallback);
    });

    $(".CancelCommentEditButton").on("click", function (e) {

        e.preventDefault();
        let comment_card = $(this).closest(".card.comment");
        let edit_comment_view = comment_card.find(".edit_comment_view").first();
        let default_comment_view = comment_card.find(".default_comment_view").first();

        resetForm();
        default_comment_view.show();

    });

    $(".ReportCommentButton").on("click", function (e) {

        e.preventDefault();
        //let comment_card = $(this).closest(".card.comment");
        //let comment_id = comment_card.data("comment-id");
        //DeleteComment(comment_id, DeleteCommentSuccessCallback);
        //comment_card.hide(200, function () {
        //    comment_card.remove();
        //});


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

    $(".ReportPostButton").on("click", function (e) {

        e.preventDefault();
        //let post_card = $(this).closest(".card.post");
        //let post_id = post_card.data("post-id");
        //DeletePost(post_id, DeletePostSuccessCallback);
        //post_card.hide(200, function () {
        //    post_card.remove();
        //});


    });


    $("#confirm_comment_button").on("click", function (e) {

        e.preventDefault();
        let body = tinymce.activeEditor.getContent();
        let pageID = $(this).data("page-id");
        createNewComment(body, pageID, createNewCommentSuccess);

    });

    $("#cancel_comment_button").on("click", function (e) {

        e.preventDefault();
        resetForm();
    });

});

//----------------------------------------------------------------------------------------------------------------------------

function DeleteCommentSuccessCallback(data) {

}

function DeleteComment(comment_id, success_callback) {


    var data = { "CommentID": comment_id };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/DeleteComment",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//----------------------------------------------------------------------------------------------------------------------------

function RemoveCommentSuccessCallback(data) {

}

function RemoveComment(comment_id, success_callback) {


    var data = { "CommentID": comment_id };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/RemoveComment",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//----------------------------------------------------------------------------------------------------------------------------


function reportCommentSuccessCallback(data) {

}

function reportComment(commentID, body, success_callback) {
    var data = { "CommentID": commentID, "reportBody": body };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ReportComment",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });
}

//----------------------------------------------------------------------------------------------------------------------------

function EditCommentSuccessCallback(data) {

    let warnings = data.d;

    if (warnings.length === 0) {
        console.log("added successfully");
        window.location.reload();
    }
    else {

        $(".EditCommentWarningSpace").text(warnings[0].Text);
        $(".EditCommentWarningSpace").show();

    }

}

function EditComment(commentID, body, success_callback) {

    var data = { "Body": body, "CommentID": commentID };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndEditComment",
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