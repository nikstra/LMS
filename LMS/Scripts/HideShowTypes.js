$(document).ready(function () {

    // Configure jQuery.
    $(function () {
        $.support.transition = false;
        $('.lms-hidden').collapse({
            toggle: false
        });
    });

    // Add collapse to all tags hidden and shown by select #move.
    $('.lms-hidden').addClass('collapse');

    // On change, hide all divs and show only the selected option.
    $('#move').change(function () {
        var val = $(this).val();
        var selector = '.lms-' + val;

        $('.lms-hidden').collapse('hide');

        if (val !== "0")
            $(selector).collapse('show');
    });
});
