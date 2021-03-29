﻿
var KTBootstrapDatetimepicker = function () {
    // Private functions
    var baseDemos = function () {
        $('#kt_datetimepicker_7_1').datetimepicker({
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
    console.log(data)
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
    console.log(data)
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