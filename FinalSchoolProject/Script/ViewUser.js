$(document).ready(function () {


    tinymce.init({
        selector: '#EditBioInput',
        resize: false,
        branding: false,
        menubar: false,
        width: '100%',
        height: 200,
        body_class: "form-control",
        plugins: [
            'advlist autolink lists link image charmap print preview anchor',
            'searchreplace visualblocks code fullscreen emoticons directionality',
            'insertdatetime media table paste code help wordcount'
        ],
        toolbar: 'link bold italic forecolor backcolor emoticons | alignleft alignright'
    });

    tinymce.init({
        selector: '#NewMessageInput',
        width: '100%',
        height: '100%',
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

    $("#SendMessageButton").on("click", function (e) {
        e.preventDefault();

        let msg = tinymce.activeEditor.getContent();
        let recipient_id = $(this).data("recipient-id");
        SendMessage(recipient_id, msg, SendMessageSuccessCallback);

    });

    $("#DeleteUserButton").on("click", function (e) {
        e.preventDefault();
        let userid = parseInt($(this).data("user-id"));

        DeleteUser(userid, DeleteUserSuccessCallback);

    });

    $("#ConfirmEditButton").on("click", function (e) {
        e.preventDefault();
        removeAlerts();


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
        resetForm();

    });

    $("#EditUserButton").on("click", function (e) {
        e.preventDefault();
        $("#default_user_view").hide();
        $("#edit_user_view").show();
    });

});

//-------------------------------------------------------------------------------------------------

function SendMessageSuccessCallback(data) {
    console.log(data.d);
    resetForm();

    if (data.d === "empty message") {
        $("#NewMessageLabel").removeClass("text-success");
        $("#NewMessageLabel").addClass("text-danger");
        $("#NewMessageLabel").text("Cannot send an empty message");
    }
    else {
        $("#NewMessageLabel").removeClass("text-danger");
        $("#NewMessageLabel").addClass("text-success");
        $("#NewMessageLabel").text("Message Sent!");
    }

}

function SendMessage(RecipientID, msg, success_callback) {

    var data = { "RecipientID": RecipientID, "Message": msg };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/SendMessage",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback

    });

}

//-------------------------------------------------------------------------------------------------

function DeleteUserSuccessCallback(data) {
    window.location = "All.aspx";
}

function DeleteUser(userid, success_callback) {

    data = { "UserID": userid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/DeleteUser",
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
        loadServerChanges();
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