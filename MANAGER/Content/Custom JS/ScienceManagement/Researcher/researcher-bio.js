
var KTBootstrapDatetimepicker = function () {
    // Private functions
    var baseDemos = function () {
        $('#kt_datetimepicker_7_1').datetimepicker({
            maxDate: new Date(),
            format: 'YYYY'
        });
        $('#kt_datetimepicker_7_2').datetimepicker({
            format: 'YYYY',
            useCurrent: true //cái này có tác dụng làm cho 2 mốc ngày có thể trùng nhau được
        });

        $('#kt_datetimepicker_7_1').on('change.datetimepicker', function (e) {
            $('#kt_datetimepicker_7_2').datetimepicker('minDate', e.date);
        });
        $('#kt_datetimepicker_7_2').on('change.datetimepicker', function (e) {
            $('#kt_datetimepicker_7_1').datetimepicker('maxDate', e.date);
        });

        /////////////////////////////////////////////////////////////////////////////////////////////////
        $('#kt_datetimepicker_7_3').datetimepicker({
            maxDate: new Date(),
            format: 'YYYY'
        });
        $('#kt_datetimepicker_7_4').datetimepicker({
            format: 'YYYY',
            useCurrent: true //cái này có tác dụng làm cho 2 mốc ngày có thể trùng nhau được
        });

        $('#kt_datetimepicker_7_3').on('change.datetimepicker', function (e) {
            $('#kt_datetimepicker_7_4').datetimepicker('minDate', e.date);
        });
        $('#kt_datetimepicker_7_4').on('change.datetimepicker', function (e) {
            $('#kt_datetimepicker_7_3').datetimepicker('maxDate', e.date);
        });
        /////////////////////////////////////////////////////////////////////////////////////////////////
        $('#kt_datetimepicker_7_5').datetimepicker({
            maxDate: new Date(),
            format: 'YYYY'
        });
        $('#kt_datetimepicker_7_6').datetimepicker({
            format: 'YYYY',
            useCurrent: true //cái này có tác dụng làm cho 2 mốc ngày có thể trùng nhau được
        });

        $('#kt_datetimepicker_7_5').on('change.datetimepicker', function (e) {
            $('#kt_datetimepicker_7_6').datetimepicker('minDate', e.date);
        });
        $('#kt_datetimepicker_7_6').on('change.datetimepicker', function (e) {
            $('#kt_datetimepicker_7_5').datetimepicker('maxDate', e.date);
        });
        /////////////////////////////////////////////////////////////////////////////////////////////////
        $('#kt_datetimepicker_7_7').datetimepicker({
            maxDate: new Date(),
            format: 'YYYY'
        });
        $('#kt_datetimepicker_7_8').datetimepicker({
            format: 'YYYY',
            useCurrent: true //cái này có tác dụng làm cho 2 mốc ngày có thể trùng nhau được
        });

        $('#kt_datetimepicker_7_7').on('change.datetimepicker', function (e) {
            $('#kt_datetimepicker_7_8').datetimepicker('minDate', e.date);
        });
        $('#kt_datetimepicker_7_8').on('change.datetimepicker', function (e) {
            $('#kt_datetimepicker_7_7').datetimepicker('maxDate', e.date);
        });

    }
    return {
        // Public functions
        init: function () {
            baseDemos();
        }
    };
}();

jQuery(document).ready(function () {
    KTBootstrapDatetimepicker.init();
});
var submit_new_acad = new LoaderBtn($("#submit_new_acad"))
class AcadEvent {
    constructor(people_id, degree, location, start, end) {
        this.people_id = people_id;
        this.degree = degree;
        this.location = location;
        this.start = start;
        this.end = end;
    }
}
$("#submit_new_acad").click(function () {
    submit_new_acad.startLoading();
    var url = new URL(window.location.href);
    people_id = url.searchParams.get("id");
    degree = $("#acad_hocvi").val();
    acad_location = $("#acad_location").val();
    start = $("#add_acad_start").val();
    end = $("#add_acad_end").val();
    let data = new AcadEvent(people_id, degree, acad_location, start, end);
    var fd = new FormData();
    fd.append('data', JSON.stringify({ data: data }));
    $.ajax({
        url: "/Biography/AddNewAcadEvent",
        type: "POST",
        data: fd,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.mess == "ss") {
                window.location.reload()
            }
            else window.location.reload()
        },
        error: function () {
            //alert("fail");
        }
    });
})

var submit_new_work = new LoaderBtn($("#submit_new_work"))
var submit_new_acad = new LoaderBtn($("#submit_new_acad"))
var submit_edit_acad = new LoaderBtn($("#submit_edit_acad"))
var submit_edit_work = new LoaderBtn($("#submit_edit_work"))
var delete_acad = new LoaderBtn($("#delete_acad"))
var delete_work = new LoaderBtn($("#delete_work"))
class WorkEvent {
    constructor(people_id, title, location, start, end) {
        this.people_id = people_id;
        this.title = title;
        this.location = location;
        this.start = start;
        this.end = end;
    }
}
$("#submit_new_work").click(function () {
    submit_new_work.startLoading();
    var url = new URL(window.location.href);
    people_id = url.searchParams.get("id");
    title = $("#work_title").val();
    work_location = $("#work_location").val();
    start = $("#add_work_start").val();
    end = $("#add_work_end").val();
    let data = new WorkEvent(people_id, title, work_location, start, end);
    var fd = new FormData();
    fd.append('data', JSON.stringify({ data: data }));
    $.ajax({
        url: "/Biography/AddWorkEvent",
        type: "POST",
        data: fd,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.mess == "ss") {
                window.location.reload()
            }
            else window.location.reload()
        },
        error: function () {
            //alert("fail");
        }
    });
})

$(".edit-acad").click(function () {
    id = $(this).data('id');
    degree = $(this).parent().parent().find(".acad-degree").data('id')
    place = $(this).parent().parent().find(".acad-place").text()
    time = $(this).parent().parent().find(".acad-time").text()
    $("#acad_suahocvi").val(degree);
    $("#edit_location").val(place)
    $("#edit-acad-start").val(time.split('-')[0].trim())
    $("#edit-acad-end").val(time.split('-')[1].trim())
    $("#delete_acad").attr("data-id", $(this).data('id'));
})
$("#submit_edit_acad").click(function () {
    submit_edit_acad.startLoading()
    let data = {
        people_id: id.split('-')[0],
        acad_id: id.split('-')[1],
        degree: $("#acad_suahocvi").val(),
        location: $("#edit_location").val(),
        start: $("#edit-acad-start").val(),
        end: $("#edit-acad-end").val()
    }
    var fd = new FormData();
    fd.append('data', JSON.stringify({ data: data }));
    $.ajax({
        url: "/Biography/EditAcadEvent",
        type: "POST",
        data: fd,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.mess == "ss") {
                window.location.reload()
            }
            else window.location.reload()
        },
        error: function () {
            //alert("fail");
        }
    });
});
$(".edit-work").click(function () {
    id = $(this).data('id');
    place = $(this).parent().parent().find(".work_unit").text()
    work_title = $(this).parent().parent().find(".work_title").text()
    time = $(this).parent().parent().find(".work_time").text()
    $("#edit_work_title").val(work_title);
    $("#edit_work_location").val(place)
    $("#edit-work-start").val(time.split('-')[0].trim())
    $("#edit-work-end").val(time.split('-')[1].trim())
    $("#delete_work").attr("data-id", $(this).data('id'));
})
$("#submit_edit_work").click(function () {
    submit_edit_acad.startLoading()
    var url = new URL(window.location.href);
    people_id = url.searchParams.get("id");
    let data = {
        id: id,
        people_id: people_id,
        place: $("#edit_work_location").val(),
        work_title: $("#edit_work_title").val(),
        start: $("#edit-work-start").val(),
        end: $("#edit-work-end").val()
    }
    var fd = new FormData();
    fd.append('data', JSON.stringify({ data: data }));
    $.ajax({
        url: "/Biography/EditWorkEvent",
        type: "POST",
        data: fd,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.mess == "ss") {
                window.location.reload()
            }
            else window.location.reload()
        },
        error: function () {
            //alert("fail");
        }
    });
});
$("#delete_work").click(function () {
    Swal.fire({
        title: "Xoá dòng tiểu sử này?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Xác nhận",
        cancelButtonText: "Huỷ",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            delete_work.startLoading()
            data = {
                id: $("#delete_work").data('id'),
            }
            var fd = new FormData();
            fd.append('data', JSON.stringify({ data: data }));
            $.ajax({
                url: "/Biography/DeleteWorkEvent",
                type: "POST",
                data: fd,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.mess == "ss") {
                        window.location.reload()
                    }
                    else window.location.reload()
                },
                error: function () {
                    //alert("fail");
                }
            });
        }
    });
})
$("#delete_acad").click(function () {
     Swal.fire({
        title: "Xoá dòng tiểu sử này?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Xác nhận",
        cancelButtonText: "Huỷ",
        reverseButtons: true
     }).then(function (result) {
         delete_acad.startLoading()
        if (result.value) {
            data = {
                acad_id: $("#delete_acad").data('id').split('-')[1],
                people_id: $("#delete_acad").data('id').split('-')[0]
            }
            var fd = new FormData();
            fd.append('data', JSON.stringify({ data: data }));
            $.ajax({
                url: "/Biography/DeleteAcadEvent",
                type: "POST",
                data: fd,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.mess == "ss") {
                        window.location.reload()
                    }
                    else window.location.reload()
                },
                error: function () {
                    //alert("fail");
                }
            });
        } 
    });
})