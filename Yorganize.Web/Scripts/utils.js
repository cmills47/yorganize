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

function handleAjaxError(evt, response, settings, exception) {

    var message = 'Unknown error';

    switch (response.status) {
        case 200:
            return;
        case 404:
            message = 'Resource not found.';
            break;
        default:
            message = exception;
    }

    var $html = $.parseHTML(response.responseText);
    $html.filter(function (u) {
        if (u.tagName == "TITLE") {
            message = u.text;
            return;
        }
    });

    showMessage($("div.ajax-message"), message, "alert alert-error");
    //   console.log(' details: ' + response.responseText); // TODO: REMOVE THIS FROM PRODUCTION!
}

function showMessage(selector, message, css) {
    var $div = $(selector);
    $div.hide();

    if (css) {
        $div.removeClass("alert-info alert-success alert-warning alert-error");
        $div.addClass(css);
    }

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