notasContainerScrollTop = 0;
notaId = 0;
isEdit = false;
var repart = "#$·";//Es una solución muy de andar por casa pero la única que se me ha ocurrido en tan poco tiempo para solucionar el problema que surge al intentar meter en el argumento de una función apóstrofes, que al guardar como guardo los datos me ocurre (con una estructura más compleja en el modelo de notas se podría haber seguido al pie de la letra la guía de Quill https://quilljs.com/docs/api/#content, que ya te adelantan que te encontrarás con problemas al querer trabajar con el innerHTML)
const buttons = Object.freeze({
    create: "<button type=\"button\" class=\"btn btn-default right-mrg\" onclick=\"wipeEditor()\" style=\"\">Quemar editor <span class=\"edit-sym glyphicon glyphicon-fire\"></span></button> <button type=\"button\" onclick=\"Create()\" disabled class=\"btn btn-default savenote\">Crear</button>",
    edit: "<button type=\"button\" class=\"btn btn-default right-mrg\" onclick=\"wipeEditor()\">Quemar editor <span class=\"edit-sym glyphicon glyphicon-fire\"></span></button> <button type=\"button\"class=\"btn btn-default \" onclick=\"Save()\">Guardar <span class=\"edit-sym glyphicon\"></span></button> <button class=\"btn btn-default \" onclick=\"KillSaveState()\">No quiero editar esto <span class=\"edit-sym glyphicon\"></span></button>"
});
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
        NotaId: notaId
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
            GoWatch();
            RenderExito(Messages.SaveSuccess);
            $('html')[0].scrollTop = $(".collection-output").offset().top;
        } else {
            RenderError(Messages.SaveFail);
        }
    }).fail(function (err) { alert(Messages.GeneralKO(err)) });
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
            wipeEditorTextarea();
            refreshNotasIfNeeded();
            RenderExito(Messages.CreateSucces);
        } else {
            RenderError(Messages.CreateFail);
        }
    }).fail(function (err) { alert(Messages.GeneralKO(err)) });
}
function GoEditor(innerHTML, anchor = false, edit) {
    let URI = window.location.origin + "/Notas/Editor";
    URI += edit != undefined && edit ? "?option=Edit" : "";
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
                var inner;
                $.when( (function () { inner = decodeURIComponent(Replace(innerHTML, repart, "'")) })() ).done(function () { $(".Editor-container").find(".ql-editor")[0].innerHTML = inner });
                
            }
            $('input[id=anchor]')[0].checked = anchor;
            editor.on('text-change', function () { qlChange() });
        } else {
            RenderError(Messages.EditorNotLoaded)
        }
    });
}
function KillSaveState() {
    wipeEditorIfChallenge();
    notaId = 0;
    $('html')[0].scrollTop = $(".collection-output").offset().top;
}
function GoEditEditor(notaid, anchored, notacontent) {
    isEdit = true;
    notasContainerScrollTop = $(".main-notas-container")[0].scrollTop;
    if ($(".editor-output")[0].innerHTML == "") {
        GoEditor(Replace(notacontent,"'",repart), anchored, true);
    } else {
        $(".Editor-container").find(".ql-editor")[0].innerHTML = decodeURIComponent(notacontent);
        $('input[id=anchor]')[0].checked = anchored;
        $(".buttonscont")[0].innerHTML = buttons.edit;
    }
    notaId = notaid;
    $('html')[0].scrollTop = $(".top-main-output").offset().top;
}
function GoWatch(wipeEditor = true) {
    if (wipeEditor) {
        wipeEditorIfChallenge();
    }
    $(".collection-output").load(window.location.origin + "/Notas/MisNotas", function (res, stt, xhr) {
        if (stt !== "success") {
            RenderError(Messages.SearchFail);
        }
    });
}

function GoErase(notaid) {
    notasContainerScrollTop = $(".main-notas-container")[0].scrollTop;
    $.ajax({
        url: window.location.origin + "/Notas/Delete",
        type: 'POST',
        dataType: 'json',
        contentType: 'application/x-www-form-urlencoded; charset=utf-8',
        data: { value: notaid }
    }).done(function (res) {
        if (res.persState == 'OK') {
            RenderExito(Messages.DeleteSuccess);
            GoWatch();
            $(".main-notas-container")[0].scrollTop = notasContainerScrollTop;
        } else {
            RenderError(Messages.DeleteFail);
        }
    }).fail(function (err) { alert(Messages.GeneralKO(err)) });
}
function wipeNotas() {
    $(".collection-output")[0].innerHTML = "";
}
function wipeEditorIfChallenge() {
    if ($('input[id=keepEditor]')[0] != undefined && !$('input[id=keepEditor]')[0].checked) {
        wipeEditor();
    } else if ($('input[id=keepEditor]')[0] != undefined && $('input[id=keepEditor]')[0].checked && isEdit) {
        wipeEditorTextarea();
        isEdit = false;
        $(".buttonscont")[0].innerHTML = buttons.create;
        $('input[id=anchor]')[0].checked = false;
    }
}
function wipeEditor() {
    $(".editor-output")[0].innerHTML = "";
}
function wipeEditorTextarea() {
    $(".ql-editor")[0].innerHTML = "";
}
function refreshNotasIfNeeded() {
    if ($(".collection-output")[0].innerHTML != "") {
        GoWatch(false);
    }
}
function Replace(baseChars, chars, replacement) {
    if (!(chars == replacement)) {
        while (baseChars.indexOf(chars) !== -1) {
            baseChars = baseChars.replace(chars, replacement);
        }
    } 
    return baseChars;
}