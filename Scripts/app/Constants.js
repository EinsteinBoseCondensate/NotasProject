Messages = Object.freeze({
    GeneralKO: function (err) { return "El servidor no responde o no tienes conexión, el error fue: " + JSON.stringify(err) },
    SaveFail: "Tu nota no ha podido editarse",
    SaveSuccess: "Tu nota se editó correctamente",
    CreateSucces: "Tu nota se creó correctamente",
    CreateFail: "Tu nota no ha podido crearse",
    EditorNotLoaded: "Lamentamos que no se pudiera cargar el editor de texto",
    SearchFail: "Algo fue mal buscando tus notas, disculpa las molestias",
    DeleteSuccess: "Tu nota se eliminó correctamente",
    DeleteFail: "Tu nota no ha podido eliminarse",
    PopUpKO: "Disculpa las molestias, algo no está en su sitio"
});