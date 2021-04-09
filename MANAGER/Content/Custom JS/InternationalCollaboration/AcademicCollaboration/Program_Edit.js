
$('.edit_program_btn').click(function () {

    var edit_program_title
    var edit_program_language = 0;
    var edit_program_partner
    var edit_program_start_date
    var edit_program_end_date
    var note
    var content

    if (direction == 1) {
        edit_program_title = $('#edit_program_title_going').val()
        edit_program_language = $('#edit_program_language_going').val()
        edit_program_partner = $('#edit_program_partner').val()
        edit_program_start_date = $('#edit_program_start_date_going').val()
        edit_program_end_date = $('#edit_program_end_date_going').val()
        note = $('#edit_note_going').val()
        content = $('#edit_summernote_going').summernote('code') + "";
    }
    if (direction == 2) {
        edit_program_title = $('#edit_program_title_coming').val()
        edit_program_language = $('#edit_program_language_coming').val()
        edit_program_partner = ""
        edit_program_start_date = $('#edit_program_start_date_coming').val()
        edit_program_end_date = $('#edit_program_end_date_coming').val()
        note = $('#edit_note_coming').val()
        content = $('#edit_summernote_coming').summernote('code') + "";
    }

    if (edit_program_title == "") {
        toastr.warning("Vui lòng nhập tiêu đề")
        return;
    }
    if (direction == 1 && edit_program_partner == "") {
        toastr.warning("Vui lòng chọn đối tác")
        return;
    }
    if (edit_program_start_date == "" || edit_program_end_date == "") {
        toastr.warning("Vui lòng chọn thời hạn")
        return;
    }

    var save_loader = new LoaderBtn($(".load-btn"))
    var form_data = new FormData();

    var list_image = $('.modal-edit-program .note-editor').find('img')
    var count_upload = 0;
    if (list_image.length != 0) {
        for (i = 0; i < list_image.length; i++) {
            var temp = list_image[i];
            var temp_src = $(temp).attr('src') + "";
            if (temp_src.includes('data:')) {
                count_upload++
                content = content.replace(temp_src, 'image_' + i)
                console.log(temp_src)
                form_data.append('image_' + i, dataURItoBlob(temp_src))
            }
        }
    }

    form_data.append("program_id", program_id)
    form_data.append("direction", direction)
    form_data.append("numberOfImage", count_upload)
    form_data.append('program_title', edit_program_title)
    form_data.append('program_partner', edit_program_partner)
    form_data.append('program_language', edit_program_language)
    form_data.append('edit_program_start_date', edit_program_start_date)
    form_data.append('edit_program_end_date', edit_program_end_date)
    form_data.append('note', note)
    form_data.append('content', content)

    save_loader.startLoading();
    $.ajax({
        url: "/AcademicCollaboration/SaveEditProgram",
        method: "post",
        dataType: "json",
        data: form_data,
        processData: false,
        contentType: false,
        success: function (data) {
            toastr.options = {
                "closeButton": true,
                "debug": false,
                "newestOnTop": false,
                "progressBar": false,
                "positionClass": "toast-top-right",
                "preventDuplicates": false,
                "showDuration": "300",
                "hideDuration": "1000",
                "timeOut": "5000",
                "extendedTimeOut": 0,
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
            };
            if (data.success) {
                toastr.clear()
                toastr.success('Chỉnh sửa thành công');

                $('.modal-edit-program textarea').val('');
                $('.modal-edit-program input').val('');
                $('.modal-edit-program select').val('').trigger('change');
                $('.modal-edit-program .program_language').val('1').trigger('change');
                $('#edit_summernote_going').summernote('reset');
                $('#edit_summernote_coming').summernote('reset');
                $('.modal-edit-program').modal('hide')

                save_loader.stopLoading()
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
            }
            else {
                toastr.clear()
                toastr.warning(data.content);
                save_loader.stopLoading()
            }
        },
        error: function (data) {
            toastr.clear();
            toastr.error(data.content);
            save_loader.stopLoading()
        }
    });
})