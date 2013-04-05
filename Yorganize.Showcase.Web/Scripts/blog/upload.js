
function onUpload(e) {
    e.data = { 'postID': $('#ID').val() };
    $('#spinner').removeClass("hidden");
    $('#preview-image').addClass("hidden");
    e.sender.disable();
    console.log("uploading ", e);
}

function onUploadError(e) {
    console.log("error ", e);
}

function onUploadSuccess(e) {
    $('#preview-image').attr("src", e.response);
    $('#featured-image').val(e.response);
    
    console.log("success ", e);
}

function onUploadComplete(e) {
    e.sender.enable();
    $('#spinner').addClass("hidden");
    $('#preview-image').removeClass("hidden");
    console.log("complete ", e);
}