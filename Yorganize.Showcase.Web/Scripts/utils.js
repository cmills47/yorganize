function redirect(url) {
    window.location.replace(url);
}

function redirect_from_login(content) {
    if (content != null)
        if (content.redirectUrl != null && content.redirectUrl.length > 0)
            redirect(content.redirectUrl);
        else if (content.message != null)
            showMessage('.ajax-message', content.message, 'alert alert-error');
}

function getParameterByName(name) {
    name = name.replace(/[\[]/, "\\\[").replace(/[\]]/, "\\\]");
    var regexS = "[\\?&]" + name + "=([^&#]*)";
    var regex = new RegExp(regexS);
    var results = regex.exec(window.location.search);
    if (results == null)
        return "";
    else
        return decodeURIComponent(results[1].replace(/\+/g, " "));
}

function findIndex(collection, filter) {
    for (var i = 0; i < collection.length; i++) {
        if (filter(collection[i]))
            return i;
    }

    return -1;
}

$(document).ready(function () {
    $(document).ajaxError(handleAjaxError);
});

function handleAjaxError(evt, request, settings, exception) {

    var message = '';

    switch (request.status) {
        case 200:
            return;
        case 404:
            message = 'Resource not found.';
            break;
        default:
            message = exception;
    }

    var title = request.responseText.match(/<title>(.+)<\/title>/);
    if (title && title[1] != null)
        message = title[1];

    showMessage($("div.ajax-message"), message, "alert alert-error");
    // console.log(' details: ' + request.responseText); // TODO: REMOVE THIS FROM PRODUCTION!
}

function showMessage(selector, message, css) {
    var $div = $(selector);

    if (css != null)
        $div.addClass(css);

    $div.slideDown("slow");
    $div.bind("click", function () {
        $div.fadeOut("slow");
        if (css != null)
            $div.removeClass(css);
    });
    $div.html(message);
}

function infoMessage(message) {
    showMessage('.ajax-message', message, 'alert alert-info');
}

function successMessage(message) {
    showMessage('.ajax-message', message, 'alert alert-success');
}

function warningMessage(message) {
    showMessage('.ajax-message', message, 'alert alert-warning');
}

function errorMessage(message) {
    showMessage('.ajax-message', message, 'alert alert-error');
}

function validateForm($form) {
    $form.removeData("validator");
    $.validator.unobtrusive.parse($form);

    return $form;
}