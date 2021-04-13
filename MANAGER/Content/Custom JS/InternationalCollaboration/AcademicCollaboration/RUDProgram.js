var direction = 0;
var collab_type = 0;
var program_id = 0;
//delete program======================================================================
$(document).on('click', '#delete_program', function () {
    let id = $(this).data('id');
    direction = $(this).data('direction');
    collab_type = $(this).data('collab');
    confirm_delete_program(id);
});

function confirm_delete_program(article_id) {
    var id = { article_id: article_id }
    Swal.fire({
        title: "Xác nhận xóa",
        text: "Có muốn xóa bản ghi này",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Xóa",
        cancelButtonText: "Hủy",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            $.ajax({
                url: '/AcademicCollaboration/DeleteProgram',
                type: "POST",
                dataType: 'json',
                data: id,
                success: function (data) {
                    if (data.success) {
                        if (direction == 1 && collab_type == 1) {
                            $('#program_going_table').DataTable().ajax.reload()
                        }
                        if (direction == 1 && collab_type == 2) {
                            $('#collab_program_going_table').DataTable().ajax.reload()
                        }
                        if (direction == 2 && collab_type == 1) {
                            $('#program_coming_table').DataTable().ajax.reload()
                        }
                        if (direction == 2 && collab_type == 2) {
                            $('#collab_program_coming_table').DataTable().ajax.reload()
                        }
                        Swal.fire("Thành công", "Xóa thành công", "success");
                    } else {
                        Swal.fire("Thất bại", "Có lỗi khi xóa", "error");
                    }
                },
                error: function () {
                    Swal.fire("Thất bại", "Có lỗi khi xóa", "error");
                },
            })
        }
    })
}
//delete program======================================================================

//load edit program===================================================================
$(document).on('click', '#load_edit_program', function () {
    program_id = $(this).data('id');
    let direction_temp = $(this).data('direction');
    if (direction_temp == 1) {
        load_program_detail_going(program_id);
    }
    if (direction_temp == 2) {
        load_program_detail_coming(program_id);
    }
    direction = $(this).data('direction');
    collab_type = $(this).data('collab');
});

function load_program_detail_going(program_id) {
    var id = { program_id: program_id }
    $.ajax({
        url: '/AcademicCollaboration/LoadEditProgram',
        type: 'post',
        dataType: 'json',
        data: id,
        success: function (data) {
            $('#edit_program_title_going').val(data.json.program_name)
            $('#edit_program_language_going').val(data.json.language_id).trigger('change')
            $('#edit_program_partner').append(new Option(data.json.partner_name, data.json.partner_name + '/' + data.json.partner_id, false, true)).trigger('change');
            $('#edit_program_start_date_going').datepicker('setDate', data.json.registration_deadline.split(' - ')[0])
            $('#edit_program_end_date_going').datepicker('setDate', data.json.registration_deadline.split(' - ')[1])
            $('#edit_note_going').val(data.json.note)
            $('#edit_summernote_going').summernote('code', data.json.content)
            $('#edit_program_going').modal('show')
        }
    })
}

function load_program_detail_coming(program_id) {
    var id = { program_id: program_id }
    $.ajax({
        url: '/AcademicCollaboration/LoadEditProgram',
        type: 'post',
        dataType: 'json',
        data: id,
        success: function (data) {
            $('#edit_program_title_coming').val(data.json.program_name)
            $('#edit_program_language_coming').val(data.json.language_id).trigger('change')
            $('#edit_program_start_date_coming').datepicker('setDate', data.json.registration_deadline.split(' - ')[0])
            $('#edit_program_end_date_coming').datepicker('setDate', data.json.registration_deadline.split(' - ')[1])
            $('#edit_note_coming').val(data.json.note)
            $('#edit_summernote_coming').summernote('code', data.json.content)
            $('#edit_program_coming').modal('show')
        }
    })
}
//load edit program===================================================================
//change select 2 load language=========================================================
$('#edit_program_language_going').select2({
    placeholder: 'Ngôn ngữ',
}).on('select2:select', function () {
    $.ajax({
        url: '/AcademicCollaboration/LoadProgramDetailLanguage',
        type: "POST",
        data: {
            "program_id": program_id,
            "language_id": $('#edit_program_language_going').val()
        },
        success: function (data) {
            $('#edit_program_title_going').val(data.articleVersion.version_title)
            $('#edit_summernote_going').summernote('code', data.articleVersion.article_content)
        },
        error: function () { }
    })
})
$('#edit_program_language_coming').select2({
    placeholder: 'Ngôn ngữ',
}).on('select2:select', function () {
    $.ajax({
        url: '/AcademicCollaboration/LoadProgramDetailLanguage',
        type: "POST",
        data: {
            "program_id": program_id,
            "language_id": $('#edit_program_language_coming').val()
        },
        success: function (data) {
            $('#edit_program_title_coming').val(data.articleVersion.version_title)
            $('#edit_summernote_coming').summernote('code', data.articleVersion.article_content)
        },
        error: function () { }
    })
})
    //change select 2 load language=========================================================