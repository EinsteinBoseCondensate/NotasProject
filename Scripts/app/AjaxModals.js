function RenderExito(mensaje) {
    $(".popupContainer").load(encodeURI(window.location.origin + "/AjaxModals/ModalExitoPartial?msg=" + mensaje), function (res, stt, xhr) {
        if (stt == "success") {
            $("#Exito").modal('show');
        } else {
            alert("Disculpa las molestias, tu nota se ha guardado correctamente pero algo no está en su sitio!");
        }
    });
}
function ExitoAceptar() {
    $("#Exito").modal('hide');
}
function RenderError(mensaje) {
    $(".popupContainer").load(encodeURI(window.location.origin + "/AjaxModals/ModalErrorPartial?msg=" + mensaje), function (res, stt, xhr) {
        if (stt == "success") {
            $("#Error").modal();
        } else {
            alert("Disculpa las molestias,tu nota no se ha guardado y algo no está en su sitio!");
        }
    });

}
function ErrorAceptar() {
    $("#Error").modal('hide');
}
function replaceWhiteSpaces(mensaje) {
    while (mensaje.indexOf(" ") != -1) {
        mensaje.replace(" ", "$$$");
    }
    return mensaje;
}
