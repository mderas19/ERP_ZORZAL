﻿    $(document).ready(function () {
        var Anulada = $('#solef_EsAnulada').val();
        if (Anulada == 1) {
            $('#bottonAnular').hide();
            document.getElementById("solef_EsAnulada").disabled = true;
        }
    });

    function AnularSolictud() {
    var solefId = $('#solef_Id').val();
    var Anulada = 1
    $.ajax({
        url: "/SolicitudEfectivo/AnularSolcitudEfectivo",
        method: "POST",
        dataType: 'json',
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify({ solefId: solefId, Anulada: Anulada }),

    })
    .done(function (data) {
        if (data.length > 0) {
            var url = $("#RedirectTo").val();
            location.href = url;
        }
        else {
            alert("Registro No Actualizado");
        }
    });
}
