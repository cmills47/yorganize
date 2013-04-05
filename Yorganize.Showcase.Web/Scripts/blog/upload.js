$(document).ready(function () {
    // create upload control for post image
    $("#files").kendoUpload({
        async: {
            saveUrl: "save",
            removeUrl: "remove",
            autoUpload: true
        },
        multiple: false
    });
});