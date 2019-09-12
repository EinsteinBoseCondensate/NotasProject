const defaults = Object.freeze({
    exitoDefault: "Operación satisfactoria",
    errorDefault: "Operación interrumpida"
});
function RenderExito(mensaje = defaults.exitoDefault) {
    $("#MensajeExito")[0].innerHTML = mensaje;
    $("#Exito").modal('show');
}
function ExitoAceptar() {
    $("#MensajeExito")[0].innerHTML = "";
    $("#Exito").modal('hide');
}
function RenderError(mensaje = defaults.errorDefault) {
    $("#MensajeError")[0].innerHTML = mensaje;
    $("#Error").modal('show');
}
function ErrorAceptar() {
    $("#MensajeError")[0].innerHTML = "";
    $("#Error").modal('hide');
}