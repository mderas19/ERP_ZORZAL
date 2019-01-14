﻿$("#pscat_ISV").on("keypress keyup blur", function (event) {
    //this.value = this.value.replace(/[^0-9\.]/g,'');
    $(this).val($(this).val().replace(/[^0-9\.]/g, ''));
    if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        event.preventDefault();
    }
});

//Validar Los campos numericos
function format(input) {
    var num = input.value.replace(/\,/g, '');
    if (!isNaN(num)) {
        input.value = num;
    }
    else {
        //alert('Solo se permiten numeros');
        input.value = input.value.replace(/[^\d\.]*/g, '');
    }
}
//fin
//Actualizar Municipios
//function btnActualizar(pscat_Id) {

//    console.log(pscat_Id);
//    var SubCategoria = GetSubCategoria();



//    $.ajax({
//        url: "/ProductoCategoria/UpdateSubCategoria",
//        method: "POST",
//        dataType: 'json',
//        contentType: "application/json; charset=utf-8",
//        data: JSON.stringify({ ActualizarSubCategoria: SubCategoria }),
//    }).done(function (data) {
//        if (data == '') {
//            location.reload();
//        }
//        else if (data == '-1') {
//            $('#MensajeError' + pscat_Id).text('');
//            $('#ValidationMessageFor' + pscat_Id).after('<ul id="MensajeError' + pscat_Id + '" class="validation-summary-errors text-danger">No se ha podido Actualizar el registro.</ul>');
//        }
//        else {
//            $('#MensajeError' + pscat_Id).text('');
//            $('#ValidationMessageFor' + pscat_Id).after('<ul id="MensajeError' + pscat_Id + '" class="validation-summary-errors text-danger">Campo Requerido</ul>');
//        }
//    });
//}
//function GetSubCategoria() {
//    var ActualizarSubCategoria = {
//        pscat_Descripcion: $('#pscat_Descripcion').val(),
//        pscat_ISV: $('#pscat_ISV').val(),
//        pscat_UsuarioCrea: contador,
//        pscat_Id: contador,
        
//    };
//    return ActualizarSubCategoria;
//}
//UPDATE SUBCATEGORIA
function btnActualizar(pscat_Id) {
    var tabActiva = null;
    $('a[data-toggle="tab"]').on('shown', function (e) {
        tabActiva = e.target;
    })

    console.log('tabActiva', tabActiva);

    var Descripcion = $('#DescripcionEdit_' + pscat_Id).val();
    var ISV = $('#PSVEdit_' + pscat_Id).val();


    if (Descripcion == '') {
        $('#MessageError').text('');
        $('#ErrorDescripcion').text('');
        $('#ErrorISV').text('');
        $('#ErroDescripcion_Create').after('<ul id="ErrorDescripcion" class="validation-summary-errors text-danger">Campo Descripción Requerido</ul>');

    }

    else if (ISV == '') {
        $('#MessageError').text('');
        $('#ErrorDescripcion').text('');
        $('#ErrorISV').text('');
        $('#ErrorISV_Create').after('<ul id="ErrorISV" class="validation-summary-errors text-danger">Campo ISV Requerido</ul>');
    }
    else {
        var subcategoria = {
            pscat_Id: pscat_Id,
            pscat_Descripcion: pscat_Descripcion,
            pscat_UsuarioCrea: pscat_UsuarioCrea,
            pscat_FechaCrea: pscat_FechaCrea,
            pscat_ISV: pscat_ISV
        };
        $.ajax({
            url: "/ProductoCategoria/UpdateSubCategoria",
            method: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ ActualizarSubCategoria: subcategoria }),
        }).done(function (data) {
            if (data == 'Exito') {
                location.reload();
            }
            else if (data == 'Error-01') {
                $('#MessageErrorEdit_' + pscat_Id).text('');
                $('#PSVEdit_' + pscat_Id).text('');
                $('#ErrorDescripcionEdit_' + pscat_Id).text('');
                $('#validationSummaryEdit_' + pscat_Id).after('<ul id="MessageErrorEdit_' + pscat_Id + '" class="validation-summary-errors text-danger">Campo Descripción Requerido.</ul>');
            }
            else {
                $('#MessageErrorEdit_' + pscat_Id).text('');
                $('#PSVEdit_' + pscat_Id).text('');
                $('#ErrorDescripcionEdit_' + pscat_Id).text('');
                $('#validationSummaryEdit_' + pscat_Id).after('<ul id="MessageErrorEdit_' + pscat_Id + '" class="validation-summary-errors text-danger">Se produjo un error, no se pudo actualizar el registro.</ul>');
            }
        });
    }
}


//Validacion de solo letras
function soloLetras(e) {
    key = e.keyCode || e.which;
    tecla = String.fromCharCode(key).toLowerCase();
    letras = " áéíóúabcdefghijklmnñopqrstuvwxyz";
    especiales = "8-37-39-46";

    tecla_especial = false
    for (var i in especiales) {
        if (key == especiales[i]) {
            tecla_especial = true;
            break;
        }
    }

    if (letras.indexOf(tecla) == -1 && !tecla_especial) {
        return false;
    }
}
//PARA LA VISTA PARCIAL DE LA SUBCATEGORIA
var contador = 0;

$('#AgregarSubCategoria').click(function () {
    var Descripcion = $('#pscat_Descripcion').val();
    var ISV = $('#pscat_ISV').val();

    if (Descripcion == '') {
        $('#MessageError').text('');
        $('#ErrorDescripcion').text('');
        $('#ErrorISV').text('');
        $('#ErroDescripcion_Create').after('<ul id="ErrorDescripcion" class="validation-summary-errors text-danger">Campo Descripción Requerido</ul>');

    }

    else if (ISV == '') {
        $('#MessageError').text('');
        $('#ErrorDescripcion').text('');
        $('#ErrorISV').text('');
        $('#ErrorISV_Create').after('<ul id="ErrorISV" class="validation-summary-errors text-danger">Campo ISV Requerido</ul>');
    }
    else {
        contador = contador + 1;
        copiar = "<tr data-id=" + contador + ">";
        copiar += "<td id = ''></td>";
        copiar += "<td id = 'Descripcion'>" + $('#pscat_Descripcion').val() + "</td>";
        copiar += "<td id = ''></td>";
        copiar += "<td id = 'ISV'>" + $('#pscat_ISV').val() + "</td>";
        copiar += "<td>" + '<button id="removerSubCategoria" class="btn btn-danger btn-xs eliminar" type="button">Quitar</button>' + "</td>";
        copiar += "</tr>";
        $('#Datatable').append(copiar);


        var SubCategoria = GetSubCategoria();

        $.ajax({
            url: "/ProductoCategoria/GuardarSubCategoria",
            method: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ tbsubcategoria: SubCategoria }),
        })
            .done(function (data) {
                $('#pscat_Descripcion').val('');
                $('#pscat_ISV').val('');

                $('#MessageError').text('');
                $('#ErrorDescripcion').text('');
                $('#ErrorISV').text('');
            });
    }
});


function GetSubCategoria() {
    var SubCategoria = {
        pscat_Descripcion: $('#pscat_Descripcion').val(),
        pscat_ISV: $('#pscat_ISV').val(),
        pscat_UsuarioCrea: contador,
        pscat_Id: contador,

    };
    return SubCategoria;
}



$(document).on("click", "#Datatable tbody tr td button#removerSubCategoria", function () {
    $(this).closest('tr').remove();
    idItem = $(this).closest('tr').data('id');
    var borrar = {
        pscat_Id: idItem,
    };
    $.ajax({
        url: "/ProductoCategoria/removeSubCategoria",
        method: "POST",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ borrado: borrar }),
    });
});

//CREATE TABLA DE SOLO DOS CAMPOS EN CREATE 
var contador = 0;

$('#CrearSub').click(function () {
    var Descripcion = $('#pscat_Descripcion').val();
    var ISV = $('#pscat_ISV').val();

    if (Descripcion == '') {
        $('#MessageError').text('');
        $('#ErrorDescripcion').text('');
        $('#ErrorISV').text('');
        $('#ErroDescripcion_Create').after('<ul id="ErrorDescripcion" class="validation-summary-errors text-danger">Campo Descripción Requerido</ul>');

    }

    else if (ISV == '') {
        $('#MessageError').text('');
        $('#ErrorDescripcion').text('');
        $('#ErrorISV').text('');
        $('#ErrorISV_Create').after('<ul id="ErrorISV" class="validation-summary-errors text-danger">Campo ISV Requerido</ul>');
    }
    else {
        contador = contador + 1;
        copiar = "<tr data-id=" + contador + ">";
        copiar += "<td id = 'Descripcion'>" + $('#pscat_Descripcion').val() + "</td>";
        copiar += "<td id = 'ISV'>" + $('#pscat_ISV').val() + "</td>";
        copiar += "<td>" + '<button id="removerSubCategoria" class="btn btn-danger btn-xs eliminar" type="button">Quitar</button>' + "</td>";
        copiar += "</tr>";
        $('#TablaSub').append(copiar);


        var SubCategoria = GetSubCategoria();

        $.ajax({
            url: "/ProductoCategoria/GuardarSubCategoria",
            method: "POST",
            dataType: 'json',
            contentType: "application/json; charset=utf-8",
            data: JSON.stringify({ tbsubcategoria: SubCategoria }),
        })
            .done(function (data) {
                $('#pscat_Descripcion').val('');
                $('#pscat_ISV').val('');

                $('#MessageError').text('');
                $('#ErrorDescripcion').text('');
                $('#ErrorISV').text('');
            });
    }
});


function GetSubCategoria() {
    var SubCategoria = {
        pscat_Descripcion: $('#pscat_Descripcion').val(),
        pscat_ISV: $('#pscat_ISV').val(),
        pscat_UsuarioCrea: contador,
        pscat_Id: contador,

    };
    return SubCategoria;
}



$(document).on("click", "#TablaSub tbody tr td button#removerSubCategoria", function () {
    $(this).closest('tr').remove();
    idItem = $(this).closest('tr').data('id');
    var borrar = {
        pscat_Id: idItem,
    };
    $.ajax({
        url: "/ProductoCategoria/removeSubCategoria",
        method: "POST",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ borrado: borrar }),
    });
});