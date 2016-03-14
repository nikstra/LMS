$(document).ready(function () {
    $('#role').change(function () {
        var val = $(this).val();
        if (val === "0") {
            $('.lms-hidden').css('display', 'block');
        }
        else {
            $('.lms-hidden').css('display', 'none');
        }
    });
});


