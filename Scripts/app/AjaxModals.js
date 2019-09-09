function RenderExito(mensaje) {
    $(".popupContainer").load(encodeURI(window.location.origin + "/AjaxModals/ModalExitoPartial?msg=" + mensaje), function (res, stt, xhr) {
        if (stt == "success") {
            $("#Exito").modal('show');
        } else {
            alert(Messages.PopUpKO);
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
            alert(Messages.PopUpKO);
        }
    });
}
function ErrorAceptar() {
    $("#Error").modal('hide');
}