function dataURItoBlob(dataURI) {
    var blobBin = atob(dataURI.split(',')[1]);
    var array = [];
    for (var i = 0; i < blobBin.length; i++) {
        array.push(blobBin.charCodeAt(i));
    }
    var mimeString = dataURI.split(',')[0].split(':')[1].split(';')[0];
    var file = new Blob([new Uint8Array(array)], { type: mimeString });
    return file;
}

var save_loader = new LoaderBtn($(".load-btn"))
var direction = 1;
$("#add_procedure_coming").click(function() {
    direction = 2;
})

$("#add_procedure_going").click(function() {
    direction = 1;
})

$('#save_procedure').click(function() {
    var form_data = new FormData();

    var proceduce_title = $('#add_procedure_title').val()
    var partner_language_type = $('#add_procedure_language').val()

    if (proceduce_title == "") {
        toastr.warning("Vui lòng nhập tiêu đề")
        return;
    }
    var content = $('#add_summernote').summernote('code') + "";

    var list_image = $('.note-editor').find('img')
    if (list_image.length != 0) {
        for (i = 0; i < list_image.length; i++) {
            var temp = list_image[i];
            var temp_src = $(temp).attr('src') + "";
            content = content.replace(temp_src, 'image_' + i)
            form_data.append('image_' + i, dataURItoBlob(temp_src))
        }
    }
    console.log(content)
    form_data.append("direction", direction)
    form_data.append("numberOfImage", list_image.length)
    form_data.append('procedure_title', proceduce_title)
    form_data.append('partner_language_type', partner_language_type)
    form_data.append('content', content)

    for (var p of form_data) {
        let name = p[0];
        let value = p[1];

        console.log(name, value)
    }
    save_loader.startLoading();
    $.ajax({
        url: "/AcademicCollaboration/AddProcedure",
        method: "post",
        dataType: "json",
        data: form_data,
        processData: false,
        contentType: false,
        success: function(data) {
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

                $('#add_procedure').modal('hide')
                $('#add_procedure input').val('');
                $('#add_summernote').summernote('reset');

                save_loader.stopLoading()

                $('#procedure_going_table').DataTable().ajax.reload()
                $('#procedure_coming_table').DataTable().ajax.reload()
            } else {
                toastr.clear()
                toastr.warning(data.content);
                save_loader.stopLoading()
            }
        },
        error: function(data) {
            toastr.clear();
            toastr.error(data.content);
            save_loader.stopLoading()
        }
    });
})