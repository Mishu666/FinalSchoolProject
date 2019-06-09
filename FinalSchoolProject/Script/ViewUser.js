$(document).ready(function () {


    tinymce.init({
        selector: '#EditBioInput',
        width: '100%',
        height: 300,
        resize: false,
        branding: false,
        menubar: false,
        plugins: [
            'advlist autolink lists link image charmap print preview anchor',
            'searchreplace visualblocks code fullscreen emoticons directionality',
            'insertdatetime media table paste code help wordcount'
        ],
        toolbar: 'link bold italic forecolor backcolor emoticons | bullist numlist | alignleft aligncenter alignright alignjustify ltr rtl'
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

});

//-------------------------------------------------------------------------------------------------

function SendMessageSuccessCallback(data) {
    console.log(data.d);

    $(".main_form")[0].reset();

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