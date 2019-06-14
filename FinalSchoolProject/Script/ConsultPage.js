$(document).ready(function () {

    tinymce.init({
        selector: '#addPostBody',
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

    $("#addPostInConsultPageButton").on("click", function (e) {

        e.preventDefault();
        closeActiveEditors();
        $("#hidden_post_card").show(200);

    });

    $("#cancel_post_button").on("click", function (e) {

        e.preventDefault();
        resetForm();
        closeActiveEditors();

    });

    $("#confirm_post_button").on("click", function (e) {

        e.preventDefault();

        let title = $("#addPostTitle").val();
        let body = tinymce.activeEditor.getContent();
        let pageID = $(this).data("page-id");
        createPost(title, body, pageID, createPostSuccess);

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

//--------------------------------------------------------------------------------------------

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
        loadServerChanges();
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