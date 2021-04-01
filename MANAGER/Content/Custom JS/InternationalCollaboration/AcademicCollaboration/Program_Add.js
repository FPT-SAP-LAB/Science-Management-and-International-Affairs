//load partner into select2 in modal program going
$('#add_program_partner').select2({
    placeholder: 'Đối tác',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getPartners',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                partner_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.partner_name + '/' + obj.partner_id;
                obj.text = obj.partner_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatPartnerInfo
})
function formatPartnerInfo(partner) {
    if (partner.id) {
        let partner_name = partner.partner_name;
        if (partner_name === undefined) {
            return "Dữ liệu đơn vị đào tạo mới."
        } else {
            return partner.partner_name;
        }
    }
}


$('#add_program_language_going').select2({
    placeholder: 'Ngôn ngữ',
})
$('#add_program_language_coming').select2({
    placeholder: 'Ngôn ngữ',
})

var direction = 0
var collab_type = 0
//show going modal
$('.add-program-going').click(function () {
    $('#add_program_going').modal('show')
    direction = $(this).data('direction')
    collab_type = $(this).data('collab')
})
//show coming modal
$('.add-program-coming').click(function () {
    $('#add_program_coming').modal('show')
    direction = $(this).data('direction')
    collab_type = $(this).data('collab')
})

$('.add_program_btn').click(function () {


    var add_program_title
    var add_program_language
    var add_program_partner
    var add_program_range_date
    var note
    var content

    if (direction == 1) {
        add_program_title = $('#add_program_title_going').val()
        add_program_language = $('#add_program_language_going').val()
        add_program_partner = $('#add_program_partner').val()
        add_program_range_date = $('#add_program_range_date_going').val()
        note = $('#note_going').val()
        content = $('#summernote_going').summernote('code') + "";
    }
    if (direction == 2) {
        add_program_title = $('#add_program_title_coming').val()
        add_program_language = $('#add_program_language_coming').val()
        add_program_partner = ""
        add_program_range_date = $('#add_program_range_date_coming').val()
        note = $('#note_coming').val()
        content = $('#summernote_coming').summernote('code') + "";
    }

    if (add_program_title == "") {
        toastr.warning("Vui lòng nhập tiêu đề")
        return;
    }
    if (direction == 1 && add_program_partner == "") {
        toastr.warning("Vui lòng chọn đối tác")
        return;
    }

    var save_loader = new LoaderBtn($(".load-btn"))
    var form_data = new FormData();

    var list_image = $('.note-editor').find('img')
    if (list_image.length != 0) {
        for (i = 0; i < list_image.length; i++) {
            var temp = list_image[i];
            var temp_src = $(temp).attr('src') + "";
            content = content.replace(temp_src, 'image_' + i)
            form_data.append('image_' + i, dataURItoBlob(temp_src))
        }
    }

    form_data.append("direction", direction)
    form_data.append("collab_type", collab_type)
    form_data.append("numberOfImage", list_image.length)
    form_data.append('program_title', add_program_title)
    form_data.append('program_partner', add_program_partner)
    form_data.append('program_language', add_program_language)
    form_data.append('program_range_date', add_program_range_date)
    form_data.append('note', note)
    form_data.append('content', content)

    save_loader.startLoading();
    $.ajax({
        url: "/AcademicCollaboration/AddProgram",
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
                toastr.success('Thêm thành công');

                $('.modal-add-program').modal('hide')
                $('.modal-add-program input').val('');
                $('.modal-add-program .kt_daterangepicker_1').val(moment().format('DD/MM/yyyy') + ' - ' + moment().format('DD/MM/yyyy'));
                $('.modal-add-program select').val('').trigger('change');
                $('.modal-add-program .procedure_language').val('1').trigger('change');
                $('.modal-add-program .summernote').summernote('code', '<p><br></p>');

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