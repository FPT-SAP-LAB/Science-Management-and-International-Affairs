$("#updateJournal").click(function () {
    $("#loading").show();
    $.ajax({
        url: "/Paper/UpdateJournal",
        type: "POST",
        cache: false,
        success: function (data) {
            $("#loading").hide();
            if (data.mess == true) {
                toastr.success(data.content);
            } else {
                toastr.error(data.content);
            }
        },
        error: function () {

        }
    });
});