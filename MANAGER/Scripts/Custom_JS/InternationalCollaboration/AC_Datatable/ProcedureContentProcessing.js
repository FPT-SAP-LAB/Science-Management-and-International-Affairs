
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

$('#save_procedure').click(function () {
    var form_data = new FormData();

    var proceduce_title = $('#add_procedure_title').val()
    var partner_language_type = $('#add_procedure_language').val()
  
    if (proceduce_title == "") {
        toastr.warning("Vui lòng nhập tiêu đề")
        return;
    }
    var content = $('.summernote').summernote('code') + "";

    var list_image = $('.note-editor').find('img')
    if (list_image.length != 0) {
        for (i = 0; i < list_image.length; i++) {
            var temp = list_image[i];
            var temp_src = $(temp).attr('src') + "";
            content = content.replace(temp_src, 'image_' + i)
            form_data.append('image_' + i, dataURItoBlob(temp_src))
        }
    }
    form_data.append("numberOfImage", list_image.length)
    console.log(content)
    form_data.append('proceduce_title', proceduce_title)
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
        error: function () {
            toastr.error("Có lỗi xảy ra");
        },
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
                "timeOut": 0,
                "extendedTimeOut": 0,
                "showEasing": "swing",
                "hideEasing": "linear",
                "showMethod": "fadeIn",
                "hideMethod": "fadeOut",
            };
            if (data.success) {
                toastr.clear()
                toastr.success('Thêm thành công<br />Ấn để quay lại danh sách<br />');
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