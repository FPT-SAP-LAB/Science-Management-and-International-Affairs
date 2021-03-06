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

$(function () {
    $(".tacgia").hide()
})
donvife = {
    1: "FPTU",
    2: "FPT Swinburne",
    3: "Fpoly",
    4: "Khác"
}
var numberOfAuthors = 0
$("#add_author_save").click(function () {
    ckfe = $("#ckfe").val()
    add_author_workplace = $("#add_author_workplace").val()
    add_author_msnv = $("#add_author_msnv").val()
    add_author_name = $("#add_author_name").val()
    add_author_title = $("#add_author_title").val()
    add_author_contractType = $("#add_author_contractType").val()
    add_author_cmnd = $("#add_author_cmnd").val()
    add_author_tax = $("#add_author_tax").val()
    add_author_bank = $("#add_author_bank").val()
    add_author_accno = $("#add_author_accno").val()
    add_author_reward = $("#add_author_reward").val()
    add_author_note = $("#add_author_note").val()
    row_id = new Date().getTime()
    $("#tacgia_body").append("\
                <tr id='"+ row_id + "'>"
        + "<td>" + donvife[ckfe] + "</td>"
        + "<td>" + add_author_msnv + "</td>"
        + "<td>" + add_author_name + "</td>"
        + "<td>" + add_author_title + "</td>"
        + "<td>" + add_author_contractType + "</td>"
        + "<td>" + add_author_cmnd + "</td>"
        + "<td>" + "" + "</td>"
        + "<td>" + add_author_tax + "</td>"
        + "<td>" + add_author_bank + "</td>"
        + "<td>" + add_author_accno + "</td>"
        + "<td>" + add_author_reward + "</td>"
        + "<td>" + "<span style='cursor:pointer' class='delete-author' data-id='" + row_id + "'><i  style='color:red' class='fas fa-times icon-md'></i></span>" + "</td>"
        + "</tr>\
                ")
    $(".tacgia").show()
    numberOfAuthors++
})
$("#tacgia_body").on('click', '.delete-author', function () {
    $("#" + $(this).data("id")).remove()
    numberOfAuthors--
    if (numberOfAuthors == 0) {
        $(".tacgia").hide()
    }
});