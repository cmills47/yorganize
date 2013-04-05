$(document).ready(function () {
    // create Editor from textarea HTML element with default set of tools
    $("#editor").kendoEditor({
        encoded: true,
        tools: [
            "bold",
            "italic",
             "underline",
             "separator",
             "strikethrough",
             "fontName",
             "fontSize",
             "foreColor",
             "backColor",
             "justifyLeft",
             "justifyCenter",
             "justifyRight",
             "justifyFull",
             "insertUnorderedList",
             "insertOrderedList",
             "indent",
             "outdent",
             "formatBlock",
             "createLink",
             "unlink",
             "insertImage",
             "viewHtml"
        ]
    });
});