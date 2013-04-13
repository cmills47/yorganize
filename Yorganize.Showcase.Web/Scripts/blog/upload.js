
function onUpload(e) {
    e.data = { 'postID': $('#ID').val() };
    $('#spinner-upload').removeClass("hidden");
    $('#preview-image').addClass("hidden");
    e.sender.disable();
    console.log("uploading ", e);
}

function onUploadError(e) {
    console.log("error ", e);
}

function onUploadSuccess(e) {
    var url = e.response + "?" + new Date().getTime();
    console.log(url);
    $('#preview-image').attr("src", url);
    $('#featured-image').val(e.response);
}

function onUploadComplete(e) {
    e.sender.enable();
    $('#spinner-upload').addClass("hidden");
    $('#preview-image').removeClass("hidden");
}

function onFeaturedImageRemove() {
    if (confirm("Are you sure you want to remove the featured image?")) {
        $('#featured-image').val("");
        $('#featured-image-preview').remove();
        $('#remove-featured-image').remove();
    }

    return false;
}