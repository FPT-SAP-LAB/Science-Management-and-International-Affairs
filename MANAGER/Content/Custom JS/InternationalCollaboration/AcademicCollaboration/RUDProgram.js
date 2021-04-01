
var program_id_load;

//delete program======================================================================
$(document).on('click', '#delete_program', function () {
    let id = $(this).data('id');
    confirm_delete_program(id);
});

function confirm_delete_program(program_id) {
    var id = { program_id: program_id }
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
                url: '/AcademicCollaboration/Deleteprogram',
                type: "POST",
                dataType: 'json',
                data: id,
                success: function (data) {
                    if (data.success) {
                        $('#program_going_table').DataTable().ajax.reload()
                        $('#program_coming_table').DataTable().ajax.reload()
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
    let id = $(this).data('id');
    load_program_detail(id);
});

function load_program_detail(program_id) {
    program_id_load = program_id
    var id = { program_id: program_id }
    $.ajax({
        url: '/AcademicCollaboration/LoadEditProgram',
        type: 'post',
        dataType: 'json',
        data: id,
        success: function (data) {
            $('#edit_program_title_going').val(data.json.program_name)
            $('#edit_program_language_going').val(data.json.language_id).trigger('change')
            $('#edit_program_partner').val(data.json.partner_id).trigger('change')
            $('#edit_program_range_date_going').val(data.json.registration_deadline)
            $('#edit_note_going').val(data.json.note)
            $('#edit_summernote_going').summernote('code', data.json.content)
            $('#edit_program_going').modal('show')
        }
    })
}
//load edit program===================================================================
//change select 2 load language=========================================================
$('#edit_program_language').select2({
    placeholder: 'Ngôn ngữ',
}).on('select2:select', function () {
    $.ajax({
        url: '/AcademicCollaboration/LoadContentDetailLanguage',
        type: "POST",
        data: {
            "program_id": program_id_load,
            "language_id": $('#edit_program_language').val()
        },
        success: function (data) {
            $('#edit_summernote').summernote('code', data.content)
        },
        error: function () {
        }
    })
})
//change select 2 load language=========================================================

