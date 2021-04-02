
var save_loader = new LoaderBtn($(".load-btn"))
$('#save_edit_procedure').click(function () {
    var form_data = new FormData();
    var procedure_id = procedure_id_load
    var edit_procedure_title = $('#edit_procedure_title').val()
    var edit_procedure_language = $('#edit_procedure_language').val()

    if (edit_procedure_title == "") {
        toastr.warning("Vui lòng nhập tiêu đề")
        return;
    }
    var content = $('#edit_summernote').summernote('code') + "";

    var list_image = $('.note-editor').find('img')
    var count_upload = 0;
    if (list_image.length != 0) {
        for (i = 0; i < list_image.length; i++) {
            var temp = list_image[i];
            var temp_src = $(temp).attr('src') + "";
            if (temp_src.includes('data:')) {
                count_upload++
                content = content.replace(temp_src, 'image_' + i)
                form_data.append('image_' + i, dataURItoBlob(temp_src))
            }
        }
    }

    form_data.append("numberOfImage", count_upload)
    form_data.append("procedure_id", procedure_id)
    form_data.append('procedure_name', edit_procedure_title)
    form_data.append('language_id', edit_procedure_language)
    form_data.append('content', content)

    save_loader.startLoading();
    $.ajax({
        url: "/AcademicCollaboration/SaveEdit",
        method: "post",
        dataType: "json",
        data: form_data,
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                toastr.clear()
                toastr.success('Chỉnh sửa thành công');

                $('#edit_procedure').modal('hide')
                $('#edit_procedure input').val('');
                $('#edit_summernote').summernote('reset');

                $('#procedure_going_table').DataTable().ajax.reload()
                $('#procedure_coming_table').DataTable().ajax.reload()
                save_loader.stopLoading()
            } else {
                toastr.clear()
                toastr.warning(data.content);
                save_loader.stopLoading()
            }
        },
        error: function () {
            toastr.clear();
            toastr.error('Có lỗi xảy ra khi thêm');
            save_loader.stopLoading()
        }
    });
})