$(document).ready(function () {

    InitDatatables();

    $(".DeletePageButton").on("click", function (e) {
        e.preventDefault();
        let pageid = parseInt($(this).data("page-id"));
        let row = $(this).closest("tr");
        let spinner = row.find(".loading_spinner").first();

        spinner.show();
        $(this).attr("disabled", true);

        DeletePage(pageid, DeletePageSuccessCallback, $(this));
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

function BindPagesGVSuccessCalback(data) {

    let response = $('<div />').html(data);
    let PagesGV = response.find(".ConsultPagesGV");

    $(".ConsultPagesGV").replaceWith(PagesGV);
    InitDatatables();
}

function BindPagesGV(success_callback) {

    $.get("PagesEditor.aspx", success_callback);
}

//--------------------------------------------------------------------------------------

function InitDatatables() {

    let PagesTable = $(".ConsultPagesGV").DataTable({
        "destroy": true,
        scrollY: "60vh",
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