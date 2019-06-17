$(document).ready(function () {

    InitDatatables();

    $("#GVSpace",).on("click",".DeletePageButton", function (e) {
        e.preventDefault();
        let pageid = parseInt($(this).data("page-id"));
        let row = $(this).closest("tr");
        let spinner = row.find(".loading_spinner").first();

        spinner.show();
        $(this).attr("disabled", true);

        DeletePage(pageid, DeletePageSuccessCallback, $(this));
    });

    $("#NewConsultPageInsertButton").on("click", function(e){
        
        e.preventDefault();

        let pagename = $("#NewConsultPageNameInput").val();
        CreatePage(pagename, CreatePageSuccessCallback);

    });

});

//--------------------------------------------------------------------------------------

function DeletePageSuccessCallback(data) {

}

function DeletePage(Pageid, success_callback, sender) {

    data = { "PageID": Pageid };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/DeletePage",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback,
        complete: function () {
            let row = sender.closest("tr");
            row.remove();
        }
    });

}
//--------------------------------------------------------------------------------------

function CreatePageSuccessCallback(data)
{

    let warnings = data.d;

    for (let w of warnings) {
        console.log(w);
        let warning = createWarning(w.Text);
        $("#AddPageWarningSpace").append(warning);
        for (let wc of w.WarnControls) {
            $("#" + wc).addClass("border-danger");

        }
    }
    if (warnings.length === 0) {
        console.log("add success");
        BindPagesGV(BindPagesGVSuccessCalback);
        resetForm();
    }

}

function CreatePage(pagename, success_callback)
{

    data = { "PageName": pagename };

    $.ajax({

        method: "POST",
        data: JSON.stringify(data),
        dataType: "json",
        contentType: "application/json; charset=utf-8 ",
        url: "UsersService.asmx/ValidateAndCreatePage",
        error: function (r) {
            console.log("error");
            console.log(r.responseText);
        },
        success: success_callback,
        complete: function () {
            let row = sender.closest("tr");
            row.remove();
        }
    });

}

//--------------------------------------------------------------------------------------

function BindPagesGVSuccessCalback(data) {

    let response = $('<div />').html(data);
    let PagesGV = response.find("#GVSpace");

    $("#GVSpace").replaceWith(PagesGV);
    InitDatatables();
}

function BindPagesGV(success_callback) {

    $.get("ConsultPageEditor.aspx", success_callback);
}

//--------------------------------------------------------------------------------------

function InitDatatables() {

    let PagesTable = $(".ConsultPagesGV").DataTable({
        "destroy": true,
        scrollY: "40vh",
        stateSave: true,
        scrollCollapse: true,
        "lengthMenu": [[10, 25, 50, -1], [10, 25, 50, "All"]],
        "columnDefs": [
            { "searchable": false, "targets": [1, 2, 3] },
            { "orderable": false, "targets": [2, 3] }
        ]
    });

    PagesTable.columns.adjust().draw();

}