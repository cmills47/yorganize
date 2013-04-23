
$(document).ready(function () {
    infoMessage('<i id="working" class="icon-cog icon-spin"></i> Creating SkyDrive folder...');
    $.getJSON("Account/CreateFolder", null, function(data) {
        successMessage("Successfully created the SkyDrive folder.")
    });
});

