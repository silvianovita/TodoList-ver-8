var table;
var tableitem;
$(document).ready(function () {
    $('#SupplierData').on('keyup', function () {
        table
            .columns(4)
            .search(this.value)
            .draw();
    });
    table = $("#SupplierData").DataTable({

        serverSide: true,
        ajax:
            "/Supplier/PageData"
        ,
        "columns": [
            { "data": "id"},
            { "data": "name" },
            { "data": "joinDate" },
            {
                "render": function (data, type, row) {
                return '<button type="button" class="btn btn-warning" id="Edit" onclick="return GetById(' + row.id + ')"><i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i></button> ' +
                        '<button type = "button" class="btn btn-danger" id="Delete" onclick="return Delete(' + row.id + ')" ><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i></button'
                }
            }
        ]
    });

    $('#Itemdata').on('keyup', function () {
        tableitem
            .columns(5)
            .search(this.value)
            .draw();
    });
    tableitem = $("#Itemdata").DataTable({
        serverSide: true,
        ajax: "/Supplier/PageDataItem",
        "columns": [
            { "data": "name" },
            { "data": "stock" },
            { "data": "price" },
            { "data": "supplierId" },
            { "data": "supplierName"},
            {
                "render": function (data, type, row) {
                    return '<button type="button" class="btn btn-warning" id="Edit" onclick="return GetByIdItem(' + row.id + ')"><i class="material-icons" data-toggle="tooltip" title="Edit">&#xE254;</i></button> ' +
                        '<button type = "button" class="btn btn-danger" id="Delete" onclick="return DeleteItem(' + row.id + ')" ><i class="material-icons" data-toggle="tooltip" title="Delete">&#xE872;</i></button'
                }
            }
        ]
    });
});
function getJSessionId() {
    var jsId = document.cookie.match(/JSESSIONID=[^;]+/);
    if (jsId != null) {
        if (jsId instanceof Array)
            jsId = jsId[0].substring(11);
        else
            jsId = jsId.substring(11);
    }
    return jsId;
}

function Add() {
    debugger;
    var res = validate();
    if (res == false) {
        return false;
    }
    var supplier = new Object();
    supplier.Id = $('#Id').val();
    supplier.Name = $('#Name').val();
    //supplier.JoinDate = $('#JoinDate').val();
    supplier.JoinDate = moment($('#JoinDate').val()).format(); 
    $.ajax({
        url: "/Supplier/Insert/",
        data: supplier,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            clearTextBox();
            $('#supplierModal').modal('hide');
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

function GetById(Id) {
    debugger;
    $.ajax({
        url: "/Supplier/GetById/",
        type: "GET",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        data: { id: Id },
        success: function (result) {
            debugger;
            //const obj = JSON.parse(result);
            //$('#Id').val(obj.Id);
            $('#Id').val(result[0]['id']);
            $('#Name').val(result[0]['name']);
            //$('#JoinDate').val(result[0]['joinDate']);
            $('#JoinDate').val(result[0]['joinDate'].substring(0, 10));
            $('#supplierModal').modal('show');
            $('#Update').show();
            $('#Add').hide();

        }
    })
}

//function for updating supplier's record
function Update() {
    debugger;
    var res = validate();
    if (res == false) {
        return false;
    }
    var supplier = new Object();
    supplier.Id = $('#Id').val();
    supplier.Name = $('#Name').val();
    supplier.JoinDate = moment($('#JoinDate')).format();
    $.ajax({
        url: "/Supplier/Update/",
        data: supplier,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            debugger;
            $('#supplierModal').modal('hide');
            clearTextBox();
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            table.ajax.reload();
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
        }
    })
}
function Delete(id) {
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Supplier/Delete/",
                type: "DELETE",
                data: { id: id },
                success: function (result) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully'
                    });
                    table.ajax.reload();
                },
                error: function (result) {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            });
        }
    })
}
//Function for clearing the textboxes
function clearTextBox() {
    $('#Id').val("");
    $('#Name').val("");
    $('#JoinDate').val("");
    $('#Update').hide();
    $('#Add').show();
    $('#Name').css('border-color', 'lightgrey');
}
function GetByIdItem(Id) {
    debugger;
    $.ajax({
        url: "/Item/GetByID/",
        type: "GET",
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        data: { id: Id },
        success: function (result) {
            debugger;
            $('#Id').val(result[0]['id']);
            $('#ItemName').val(result[0]['name']);
            $('#Stock').val(result[0]['stock']);
            $('#Price').val(result[0]['price']);
            $('#SupplierId').val(result[0]['supplierId']);
            $('#itemModal').modal('show');
            $('#UpdateItem').show();
            $('#AddItem').hide();

        }
    })
}
function clearTextBoxItem() {
    $('#Id').val("");
    $('#ItemName').val("");
    $('#Stock').val("");
    $('#Price').val("");
    $('#SupplierId').val("");
    $('#UpdateItem').hide();
    $('#AddItem').show();
    $('#Name').css('border-color', 'lightgrey');
}
function AddItem() {
    debugger;
    var item = new Object();
    item.Id = $('#Id').val();
    item.Name = $('#ItemName').val();
    item.Stock = $('#Stock').val();
    item.Price = $('#Price').val();
    item.SupplierId = $('#SupplierId').val();
    $.ajax({
        url: "/Item/Insert/",   
        data: item,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            debugger;
            clearTextBoxItem();
            $('#ItemModal').modal('hide');
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Insert Successfully',
                showConfirmButton: false,
                timer: 1500
            });
            tableitem.ajax.reload();
        }
        else {
            Swal.fire('Error', 'Insert Failed', 'error');
        }
    });
}
function UpdateItem() {
    debugger;
    var item = new Object();
    item.Id = $('#Id').val();
    item.Name = $('#ItemName').val();
    item.stock = $('#Stock').val();
    item.Price = $('#Price').val();
    item.SupplierId = $('#SupplierId').val();
    $.ajax({
        url: "/Item/Update/",
        data: item,
        type: "POST"
    }).then((result) => {
        if (result.statusCode == 200) {
            debugger;
            $('#itemModal').modal('hide');
            clearTextBoxItem();
            Swal.fire({
                position: 'center',
                type: 'success',
                title: 'Update Successfully',
                showConfirmButton: false,
                timer: 6000
            });
            tableitem.ajax.reload();
        }
        else {
            Swal.fire('Error!', 'Update Failed.', 'error');
        }
    })
}
function DeleteItem(id) {
    debugger;
    Swal.fire({
        title: 'Are you sure?',
        text: "You won't be able to revert this!",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#3085d6',
        cancelButtonColor: '#d33',
        confirmButtonText: 'Yes, delete it!'
    }).then((result) => {
        if (result.value) {
            $.ajax({
                url: "/Item/Delete/",
                type: "DELETE",
                data: { id: id },
                success: function (result) {
                    Swal.fire({
                        position: 'center',
                        type: 'success',
                        title: 'Delete Successfully'
                    });
                    tableitem.ajax.reload();
                },
                error: function (result) {
                    Swal.fire('Error', 'Failed to Delete', 'error');
                    ClearScreen();
                }
            });
        }
    })
}
//Valdidation using jquery
function validate() {
    var isValid = true;
    if ($('#Name').val().trim() == "") {
        $('#Name').css('border-color', 'Red');
        $('#Name').focus();
        isValid = false;
    }
    else {
        $('#Name').css('border-color', 'lightgrey');
    }

    return isValid;
}
$(function () {
    $("[id*=btnSweetAlert]").on("click", function () {
        var id = $(this).closest('tr').find('[id*=id]').val();
        swal({
            title: 'Are you sure?' + ids,
            text: "You won't be able to revert this!" + id,
            type: 'warning',
            showCancelButton: true,
            confirmButtonColor: '#3085d6',
            cancelButtonColor: '#d33',
            confirmButtonText: 'Yes',
            cancelButtonText: 'No',
            confirmButtonClass: 'btn btn-success',
            cancelButtonClass: 'btn btn-danger',
            buttonsStyling: false
        }).then(function (result) {
            if (result) {
                $.ajax({
                    type: "POST",
                    url: "Default.aspx/DeleteClick",
                    data: "{id:" + id + "}",
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (r) {
                        if (r.d == "Deleted") {
                            location.reload();
                        }
                        else {
                            swal("Data Not Deleted", r.d, "success");
                        }
                    }
                });
            }
        },
            function (dismiss) {
                if (dismiss == 'cancel') {
                    swal('Cancelled', 'No record Deleted', 'error');
                }
            });
        return false;
    });
});