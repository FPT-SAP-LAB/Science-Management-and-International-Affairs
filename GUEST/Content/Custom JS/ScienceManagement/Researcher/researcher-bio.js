url = new URL(window.location.href);
people_id = url.searchParams.get("id");

var table1 = $('#acad_history_table').DataTable({
        responsive: true,
        // DOM Layout settings
        searching: false,
        bPaginate: false,
        lengthMenu: [5],
        pageLength: 10,
        bLengthChange: false,
        bInfo: false,
        language: {
            'lengthMenu': 'Display _MENU_',
        },
        // Order settings
        order: [[1, 'desc']],
        serverSide: true,
        ajax: {
            url: "/Researchers/getAcadList?id=" + people_id,
            datatype: "json",
            cache: true
         },
         columnDefs: [
             { width: "10%", targets: 0 },
             { width: "30%", targets: 1 },
             { width: "30%", targets: 2 },
             { width: "20%", targets: 3 },
             { width: "10%", targets: 4 }
         ],
        columns: [
            { data: "rownum", name: "rownum", orderable: false },
            {
                data: {}, name: "degree", render: function (data) {
                    return "<span class='acad-degree' data-id='" + data.acad_id + "'>" + data.degree + "</span>"
                }
            },
            {
                data: "place", name: "place", render: function (data) {
                    return "<span class='acad-place'>" + data + "</span>"
                }
            },
            {
                data: "time", name: "time", render: function (data) {
                    return "<span class='acad-time'>" + data + "</span>"
                }
            },
            {
                data: {}, render: function (data) {
                    return "<a data-id='" + data.people_id + "-" + data.acad_id + "' data-toggle='modal' data-target='#modal_suatieusu_hoctap' class='btn btn-sm btn-clean btn-icon edit-acad' title='Edit'> <i class='far fa-edit'></i> </a>"
                }
            },
        ],
        initComplete: function () {
            $("#loader_panel").hide()
        }
});

var table2 = $('#work_history_table').DataTable({
        responsive: true,
        // DOM Layout settings
        searching: false,
        bPaginate: false,
        lengthMenu: [5],
        pageLength: 10,
        bLengthChange: false,
        bInfo: false,
        language: {
            'lengthMenu': 'Display _MENU_',
        },
        // Order settings
        order: [[1, 'desc']],
    serverSide: true,
    columnDefs: [
        { width: "10%", targets: 0 },
        { width: "30%", targets: 1 },
        { width: "30%", targets: 2 },
        { width: "20%", targets: 3 },
        { width: "10%", targets: 4 }
    ],
        ajax: {
            url: "/Researchers/getWorkList?id=" + people_id,
            datatype: "json",
            cache: true
        },
        columns: [
            { data: "index", name: "index", orderable: false },
            {
                data: "work_unit", render: function (data) {
                    return "<span class='work_unit'>" + data + "</span>"
                }
            },
            {
                data: "title", name: "title", render: function (data) {
                    return "<span class='work_title'>" + data + "</span>"
                }
            },
            {
                data: {}, render: function (data) {
                    return "<span class='work_time'>" + data.start_year + "-" + data.end_year + "</span>"
                }
            },
            {
                data: "id", render: function (data) {
                    return "<a data-id='" + data + "' data-toggle='modal' data-target='#modal_suatieusu_work' class='btn btn-sm btn-clean btn-icon edit-work' title='Edit'> <i class='far fa-edit'></i> </a>"
                }
            },
        ],
        initComplete: function () {
            $("#loader_panel").hide()
        }
});

var table3 = $('#award_history_table').DataTable({
    responsive: true,
    // DOM Layout settings
    searching: false,
    bPaginate: false,
    lengthMenu: [5],
    pageLength: 10,
    bLengthChange: false,
    bInfo: false,
    language: {
        'lengthMenu': 'Display _MENU_',
    },
    columnDefs: [
        { width: "10%", targets: 0 },
        { width: "30%", targets: 1 },
        { width: "30%", targets: 2 },
        { width: "20%", targets: 3 },
        { width: "10%", targets: 4 }
    ],
    // Order settings
    order: [[1, 'desc']],
    serverSide: true,
    ajax: {
        url: "/Researchers/GetAwards?id=" + people_id,
        datatype: "json",
        cache: true
    },
    columns: [
        { data: "index", name: "index", orderable: false },
        {
            data: "competion_name", render: function (data) {
                return "<span class='competion_name'>" + data + "</span>"
            }
        },
        {
            data: "rank", name: "rank", render: function (data) {
                return "<span class='rank'>" + data + "</span>"
            }
        },
        {
            data: "award_time", render: function (data) {
                return "<span class='award_time'>" + data + "</span>"
            }
        },
        {
            data: "id", render: function (data) {
                return "<a data-id='" + data + "' data-toggle='modal' data-target='#modal_edit_award' class='btn btn-sm btn-clean btn-icon edit-award' title='Edit'> <i class='far fa-edit'></i> </a>"
            }
        },
    ],
    initComplete: function () {
        $("#loader_panel").hide()
    }
});

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

        $('#date_award').datetimepicker({
            maxDate: new Date(),
            format: 'DD/MM/YYYY'
        });
        $('#edit_date_award').datetimepicker({
            maxDate: new Date(),
            format: 'DD/MM/YYYY'
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


var submit_new_work = new LoaderBtn($("#submit_new_work"))
var submit_new_acad = new LoaderBtn($("#submit_new_acad"))
var submit_edit_acad = new LoaderBtn($("#submit_edit_acad"))
var submit_edit_work = new LoaderBtn($("#submit_edit_work"))
var submit_new_award = new LoaderBtn($("#submit_new_award"))
var delete_acad = new LoaderBtn($("#delete_acad"))
var delete_work = new LoaderBtn($("#delete_work"))
var submit_edit_award = new LoaderBtn($("#submit_edit_award"))
var delete_award = new LoaderBtn($("#delete_award"))
var delete_avt = new LoaderBtn($("#delete_avt"))
var submit_edit_avt = new LoaderBtn($("#submit_edit_avt"))
class WorkEvent {
    constructor(people_id, title, location, start, end) {
        this.people_id = people_id;
        this.title = title;
        this.location = location;
        this.start = start;
        this.end = end;
    }
}
function resetAndCloseModals(success) {
    $(".modal").find('input').val('')
    $('.modal').modal('hide');
    submit_new_work.stopLoading()
    submit_new_acad.stopLoading()
    submit_new_award.stopLoading()
    submit_edit_acad.stopLoading()
    submit_edit_work.stopLoading()
    delete_acad.stopLoading()
    delete_work.stopLoading()
    delete_avt.stopLoading()
    submit_edit_avt.stopLoading()
    if (success) {
        toastr.success("Cập nhật thành công!");
    } else {
        toastr.error("Đã xảy ra lỗi!");
    }
}
$(function () {
    $("#submit_new_acad").click(function () {
        if (validateNonEmptyField(["#acad_hocvi", "#acad_location", "#add_acad_start", "#add_acad_end"])) {
            submit_new_acad.startLoading();
            var url = new URL(window.location.href);
            people_id = url.searchParams.get("id");
            degree = $("#acad_hocvi").val();
            acad_location = $("#acad_location").val();
            let start = $("#add_acad_start").val();
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
                    if (response.success == true) {
                        table1.ajax.reload()
                        resetAndCloseModals(response.success);
                    }
                    else {
                        table1.ajax.reload()
                        resetAndCloseModals(response.success)
                    }
                },
                error: function () {
                    //alert("fail");
                }
            });
        }
    })
    $("#submit_new_work").click(function () {
        if (validateNonEmptyField(["#work_location", "#work_title", "#add_work_start", "#add_work_end"])) {
            submit_new_work.startLoading();
            var url = new URL(window.location.href);
            people_id = url.searchParams.get("id");
            title = $("#work_title").val();
            work_location = $("#work_location").val();
            let start = $("#add_work_start").val();
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
                    if (response.success == true) {
                        table2.ajax.reload()
                        resetAndCloseModals(response.success);
                    }
                    else {
                        table2.ajax.reload()
                        resetAndCloseModals(response.success)
                    }
                },
                error: function () {
                    //alert("fail");
                }
            });
        }
    })
    $("#submit_new_award").click(function () {
        if (validateNonEmptyField(["#add_award_name", "#add_award_rank", "#add_award_date"])) {
            submit_new_award.startLoading();
            var url = new URL(window.location.href);
            data = new FormData()
            people_id = url.searchParams.get("id");
            add_award_name = $("#add_award_name").val();
            add_award_rank = $("#add_award_rank").val();
            add_award_date = $("#add_award_date").val();
            data.append("people_id", people_id)
            data.append("add_award_name", add_award_name)
            data.append("add_award_rank", add_award_rank)
            data.append("add_award_date", add_award_date)
            $.ajax({
                url: "/Biography/AddAward",
                type: "POST",
                data: data,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.success == true) {
                        table3.ajax.reload();
                        resetAndCloseModals(response.success);
                    }
                    else {
                        table3.ajax.reload();
                        resetAndCloseModals(response.success)
                    }
                },
                error: function () {
                    //alert("fail");
                }
            });
        }
    })
    $("#acad_history_table").on("click", ".edit-acad", (function () {
        id = $(this).data('id');
        degree = $(this).parent().parent().find(".acad-degree").data('id')
        place = $(this).parent().parent().find(".acad-place").text()
        time = $(this).parent().parent().find(".acad-time").text()
        console.log(degree)
        $("#acad_suahocvi").val(degree);
        $("#edit_location").val(place)
        $("#edit-acad-start").val(time.split('-')[0].trim())
        $("#edit-acad-end").val(time.split('-')[1].trim())
        $("#delete_acad").attr("data-id", $(this).data('id'));
    }));
    $("#submit_edit_acad").click(function () {
        if (validateNonEmptyField(["#acad_suahocvi", "#edit_location", "#edit-acad-start", "#edit-acad-end"])) {
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
                    if (response.success == true) {
                        table1.ajax.reload()
                        resetAndCloseModals(response.success);
                    }
                    else {
                        table1.ajax.reload()
                        resetAndCloseModals(response.success);
                    }
                },
                error: function () {
                    //alert("fail");
                }
            });
        }
    });
    $("#work_history_table").on("click", ".edit-work", function () {
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
        submit_edit_work.startLoading()
        var url = new URL(window.location.href);
        let data = {
            id: id,
            people_id: url.searchParams.get("id"),
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
                if (response.success == true) {
                    table2.ajax.reload();
                    resetAndCloseModals(response.success);
                }
                else {
                    table2.ajax.reload();
                    resetAndCloseModals(response.success);
                }
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
                        if (response.success == true) {
                            table2.ajax.reload();
                            resetAndCloseModals(response.success);
                        }
                        else {
                            table2.ajax.reload();
                            resetAndCloseModals(response.success);
                        }
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
                        if (response.success == true) {
                            table1.ajax.reload();
                            resetAndCloseModals(response.success);
                        }
                        else {
                            table1.ajax.reload();
                            resetAndCloseModals(response.success);
                        }
                    },
                    error: function () {
                        //alert("fail");
                    }
                });
            }
        });
    })

    $("#award_history_table").on("click", ".edit-award", (function () {
        id = $(this).data('id');
        competion_name = $(this).parent().parent().find(".competion_name").text()
        award_time = $(this).parent().parent().find(".award_time").text()
        rank = $(this).parent().parent().find(".rank").text()
        $("#edit_award_name").val(competion_name);
        $("#edit_award_rank").val(rank)
        $("#sua_award_date").val(award_time)
        $("#delete_award").attr("data-id", $(this).data('id'));
    }));

    $("#submit_edit_award").click(function () {
        if (validateNonEmptyField(["#edit_award_name", "#edit_award_rank", "#sua_award_date"])) {
            submit_edit_award.startLoading()
            var url = new URL(window.location.href);
            people_id = url.searchParams.get("id");
            var fd = new FormData();
            fd.append('id', id);
            fd.append('competion_name', $("#edit_award_name").val());
            fd.append('award_time', $("#sua_award_date").val());
            fd.append('rank', $("#edit_award_rank").val());
            $.ajax({
                url: "/Biography/EditAward",
                type: "POST",
                data: fd,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.success == true) {
                        table3.ajax.reload();
                        resetAndCloseModals(response.success);
                    }
                    else {
                        table3.ajax.reload();
                        resetAndCloseModals(response.success);
                    }
                    submit_edit_award.stopLoading()
                },
                error: function () {
                    //alert("fail");
                }
            });
        }
    })

    $("#delete_award").click(function () {
        Swal.fire({
            title: "Xoá giải thưởng này?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Xác nhận",
            cancelButtonText: "Huỷ",
            reverseButtons: true
        }).then(function (result) {
            delete_award.startLoading()
            if (result.value) {
                var fd = new FormData();
                fd.append('id', $("#delete_award").data('id'));
                $.ajax({
                    url: "/Biography/DeleteAward",
                    type: "POST",
                    data: fd,
                    processData: false,
                    contentType: false,
                    success: function (response) {
                        if (response.success == true) {
                            table3.ajax.reload();
                            resetAndCloseModals(response.success);
                        }
                        else {
                            table3.ajax.reload();
                            resetAndCloseModals(response.success);
                        }
                        delete_award.stopLoading();
                    },
                    error: function () {
                        //alert("fail");
                    }
                });
            }
        });
    })
})