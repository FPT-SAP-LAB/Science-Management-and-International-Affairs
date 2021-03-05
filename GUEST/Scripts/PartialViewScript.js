$("#ckfe").change(function () {
    var val = $("#ckfe").val();
    if (val == 0) {
        $(".editfe").each(function () {
            $(this).prop("disabled", true);
        });
    }
    else {
        $(".editfe").each(function () {
            $(this).prop("disabled", false);
        });
    }
});
$('#ckfe').select2({
    allowClear: true
});
$('#area').select2({
    allowClear: true
});