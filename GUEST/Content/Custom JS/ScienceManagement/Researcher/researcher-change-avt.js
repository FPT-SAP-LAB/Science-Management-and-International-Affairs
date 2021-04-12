// Class definition
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "timeOut": "2000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};
var KTImageInputDemo = function () {
    // Private functions
    var initDemos = function () {
        var avatar5 = new KTImageInput('kt_image_avatar');
        avatar5.on('cancel', function (imageInput) {
            swal.fire({
                title: 'Image successfully changed !',
                type: 'success',
                buttonsStyling: false,
                confirmButtonText: 'Awesome!',
                confirmButtonClass: 'btn btn-primary font-weight-bold'
            });
        });
        
        avatar5.on('change', function (imageInput) {
            $("#progress-bar").show()
            //toastr.success("Đang thay đổi ảnh hồ sơ....");
            url = new URL(window.location.href);
            people_id = url.searchParams.get("id");
            var fd = new FormData()
            fd.append('imageInput', imageInput["input"]["files"][0])
            fd.append('people_id', people_id)

            $.ajax({
                url: "/Researchers/EditProfilePhoto",
                type: "POST",
                data: fd,
                processData: false,
                contentType: false,
                success: function (response) {
                    if (response.res == 1) {
                        swal.fire({
                            title: 'Cập nhật ảnh hồ sơ thành công!',
                            type: 'success',
                            buttonsStyling: false,
                            confirmButtonText: 'OK',
                            confirmButtonClass: 'btn btn-primary font-weight-bold'
                        });
                        window.location.reload()
                    }
                    else window.location.reload()
                },
                error: function () {
                    //alert("fail");
                }
            });
        });

        avatar5.on('remove', function (imageInput) {
            swal.fire({
                title: 'Image successfully removed !',
                type: 'error',
                buttonsStyling: false,
                confirmButtonText: 'Got it!',
                confirmButtonClass: 'btn btn-primary font-weight-bold'
            });
        });
    }

    return {
        // public functions
        init: function () {
            initDemos();
        }
    };
}();

KTUtil.ready(function () {
    $("#progress-bar").hide()
    KTImageInputDemo.init();
});