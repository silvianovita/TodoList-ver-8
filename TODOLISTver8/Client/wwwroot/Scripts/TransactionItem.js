var table;
$(document).ready(function () {
    $('#TransactionData').on('keyup', function () {
        table
            .columns(4)
            .search(this.value)
            .draw();
    });
    table = $("#TransactionData").DataTable({

        serverSide: true,
        ajax:
            "/Transaction/PageData"
        ,
        "columns": [
            { "data": "id" },
            { "data": "itemId" },
            { "data": "quantity" },
            {
                "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="Edit" onclick="return GetById(' + row.id + ')"><i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i></button> ' +
                        '<button type = "button" class="btn btn-danger" id="Delete" onclick="return Delete(' + row.id + ')" ><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i></button'
                }
            }
        ]
    });
});

function Add() {
    debugger;
    var transactionItem = new Object();
    transactionItem.Id = $('#Id').val();
    transactionItem.ItemId = $('#ItemId').val();
    transactionItem.Quantity = $('#Quantity').val();
    $.ajax({
        url: "/Transaction/Insert/",
        data: transactionItem,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            clearTextBox();
            $('#transactionItemModal').modal('hide');
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Insert Successfully',
                showConfirmButton: false,
                timer: 1500
            });
            table.ajax.reload();
        }
        else {
            Swal.fire('Error', 'Insert Failed', 'error');
        }
    });
}