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

        default_comment_view.hide();
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

        $(".main_form")[0].reset();
        edit_comment_view.hide();
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

        $(".EditCommentWarningSpace").text("Comment cannot be empty");
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
