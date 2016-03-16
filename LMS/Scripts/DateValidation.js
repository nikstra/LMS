// Include jquery in the html file.
// <script src="@Url.Content("~/Scripts/jquery-1.10.2.min.js")" type="text/javascript"></script>
$(document).ready(function () {

    function validate(id)
    {
        var dt = $(id).val();

        var d = new Date(dt);
        var firstDate = new Date('@Model.Group.StartDate.ToString()');
        var secondDate = new Date('@Model.Group.EndDate.ToString()');

        $("#submitBtn").prop('disabled', false);

        if (d.setHours(0, 0, 0, 0) < firstDate.setHours(0, 0, 0, 0)) {
            $("#submitBtn").prop('disabled', true);
            alert("Datumet får inte vara före gruppens startdatum: " + firstDate.toLocaleDateString('sv-SE'));
        }
        if (d.setHours(0, 0, 0, 0) > secondDate.setHours(0, 0, 0, 0)) {
            $("#submitBtn").prop('disabled', true);
            alert("Datumet får inte vara efter gruppens slutdatum: " + secondDate.toLocaleDateString('sv-SE'));
        }

    }

    $("#StartDate").change(function () {

        validate("#StartDate");
    });

    $("#EndDate").change(function () {

        validate("#EndDate");
    });
});
