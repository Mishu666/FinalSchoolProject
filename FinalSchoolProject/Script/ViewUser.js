$(document).ready(function () {


    tinymce.init({
        selector: '.EditableArea',
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

    $("#SendMessageButton").on("click", function (e) {
        e.preventDefault();

        let msg = $("#NewMessageInput").val();
        let recipient_id = $(this).data("recipient-id");
        SendMessage(recipient_id, msg, SendMessageSuccessCallback);

    });

});

//-------------------------------------------------------------------------------------------------

function SendMessageSuccessCallback(data) {
    console.log(data.d);

    $("#NewMessageInput").val("");
    $("#NewMessageLabel").text("Message Sent!");

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