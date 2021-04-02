
var procedure_id_load;

//delete procedure======================================================================
$(document).on('click', '#delete_procedure', function () {
    let id = $(this).data('id');
    confirm_delete_procedure(id);
});

function confirm_delete_procedure(procedure_id) {
    var id = { procedure_id: procedure_id }
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
                url: '/AcademicCollaboration/DeleteProcedure',
                type: "POST",
                dataType: 'json',
                data: id,
                success: function (data) {
                    if (data.success) {
                        $('#procedure_going_table').DataTable().ajax.reload()
                        $('#procedure_coming_table').DataTable().ajax.reload()
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
//delete procedure======================================================================

//load edit procedure===================================================================
$(document).on('click', '#load_edit_procedure', function () {
    let id = $(this).data('id');
    load_procedure_detail(id);
});

function load_procedure_detail(procedure_id) {
    procedure_id_load = procedure_id
    var id = { procedure_id: procedure_id }
    $.ajax({
        url: '/AcademicCollaboration/LoadEditProcedure',
        type: 'post',
        dataType: 'json',
        data: id,
        success: function (data) {
            $('#edit_procedure_title').val(data.json.procedure_name)
            $('#edit_summernote').summernote('code', data.json.content)
            $('#edit_procedure_language').val(data.json.language_id).trigger('change')
            $('#edit_procedure').modal('show')
        }
    })
}
//load edit procedure===================================================================
//change select 2 load language=========================================================
$('#edit_procedure_language').select2({
    placeholder: 'Ngôn ngữ',
}).on('select2:select', function () {
    $.ajax({
        url: '/AcademicCollaboration/LoadContentDetailLanguage',
        type: "POST",
        data: {
            "procedure_id": procedure_id_load,
            "language_id": $('#edit_procedure_language').val()
        },
        success: function (data) {
            $('#edit_procedure_title').val(data.articleVersion.version_title)
            $('#edit_summernote').summernote('code', data.articleVersion.article_content)
        },
        error: function () {
        }
    })
})
//change select 2 load language=========================================================

