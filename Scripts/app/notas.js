function AFNotaDTO() {
    return {
        "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val(),
        NoteContent: $(".ql-editor")[0].innerHTML,
        Anchor: $('input[id=anchor]')[0].checked       
    }
}
function AFNotaSaveDTO() {
    return {
        "__RequestVerificationToken": $('input[name=__RequestVerificationToken]').val(),
        NoteContent: $(".ql-editor")[0].innerHTML,
        Anchor: $('input[id=anchor]')[0].checked,
        NotaId: $("#NotaId")[0].value
    }
}
function Save() {
    $.ajax({
        url: window.location.origin + "/Notas/Save",
        type: 'POST',
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        data: AFNotaSaveDTO()
    }).done(function (res) {
        if (res.persState == 'OK') {
            $(".ql-editor")[0].innerHTML = "";
            RenderExito("Tu nota se editó correctamente");
        } else {
            RenderError("Tu nota no ha podido editarse");
        }
    }).fail(function (err) { alert("El servidor no responde o no tiene usted conexión, el error fue: " + err) });
}
function Create() {
    $.ajax({
        url: window.location.origin + "/Notas/Create", 
        type: 'POST',
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        data: AFNotaDTO()
    }).done(function(res){
        if (res.persState == 'OK') {
            $(".ql-editor")[0].innerHTML = "";
                RenderExito("Tu nota se guardó correctamente");
            } else {
                RenderError("Tu nota no ha podido guardarse");
            }
    }).fail(function (err) { alert("El servidor no responde o no tiene usted conexión, el error fue: " + err) });
}
function GoEditor(innerHTML, anchor = false, url) {
    let URI = window.location.origin + "/Notas/Editor";
    URI += url != null && url ? "?option=Edit" : "";
    $(".editor-output").load(URI, function (res, stt, xhr) {
        if (stt == "success") {

            var toolbarOptions = [[{ 'header': 1 }, { 'header': 2 }], [{ size: ['small', false, 'large', 'huge'] }, 'bold', 'italic', 'underline'],
                [{ 'list': 'ordered' }, { 'list': 'bullet' }, { 'indent': '-1' }, { 'indent': '+1' }],
                ['direction', { 'align': [] }],["clean"]];
            var editor = new Quill('#editor', {
                modules: {
                    'toolbar': toolbarOptions
                },
                placeholder:"Escribe tu nota...",
                theme: 'snow'
            });
            $(".ql-clean").on('click', function () { $(".ql-editor")[0].innerText = "" });
            function qlChange() {
                if ($(".ql-editor")[0].innerText.trim() != "") {
                    $(".savenote").attr('disabled', false);
                } else {
                    $(".savenote").attr('disabled', true);
                }
            }
            if (innerHTML != undefined) {
                $(".ql-editor")[0].innerHTML = innerHTML;

            }
            $('input[id=anchor]')[0].checked = anchor;
            editor.on('text-change', function () { qlChange() });

        } else {
            RenderError("Lamentamos que no se pudiera cargar el editor de texto")
        }
    });
}
function GoEditEditor(notaid, anchored, notacontent) {
    if ($(".editor-output")[0].innerHTML == "") {
        GoEditor(notacontent, anchored, true);
    } else {
        $(".ql-editor")[0].innerHTML = notacontent;
        $('input[id=anchor]')[0].checked = anchored;
        $(".buttonscont")[0].innerHTML = "<p class=\"text-center button-margin-top\"><a class=\"btn btn-default \" onclick=\"Save()\">Guardar <span class=\"edit-sym glyphicon\"></span></a></p>";
    }
    $("#NotaId")[0].value = notaid;
}
function GoWatch() {
    if ($('input[id=keepEditor]')[0] != undefined && !$('input[id=keepEditor]')[0].checked) {
        $(".editor-output")[0].innerHTML = "";
    }
    
    $(".collection-output").load(window.location.origin + "/Notas/MisNotas", function (res, stt, xhr) {
        if (stt == "success") {
        }
        else {
            RenderError("Algo fue mal buscando tus notas, disculpa las molestias");
        }
    });
}

function GoErase(notaid) {
    $.ajax({
        url: window.location.origin + "/Notas/Delete",
        type: 'POST',
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        data: { value: notaid }
    }).done(function (res) {
        if (res.persState == 'OK') {
            RenderExito("Tu nota se eliminó correctamente");
            GoWatch();
        } else {
            RenderError("Tu nota no ha podido eliminarse");
        }
    }).fail(function (err) { alert("El servidor no responde o no tiene usted conexión, el error fue: " + err) });
}
function wipeNotas() {
    $(".collection-output")[0].innerHTML = "";
}
function wipeEditor() {
    $(".editor-output")[0].innerHTML = "";
}