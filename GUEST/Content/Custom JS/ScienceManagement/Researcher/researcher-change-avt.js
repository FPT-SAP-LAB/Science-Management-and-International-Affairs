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
function sleep(ms) {
    return new Promise(resolve => setTimeout(resolve, ms));
}
var KTImageInputDemo = function () {
    // Private functions
    var initDemos = function () {
        var avatar5 = new KTImageInput('kt_image_avatar');
        avatar5.on('cancel', function (imageInput) {
            
        });

        avatar5.on('change', async function (imageInput) {
            await sleep(1000);
            $("#avatar_preview").attr("style", $("#kt_image_avatar").find(".image-input-wrapper").attr("style"))
            $("#avatar_change").modal()
            $("#submit_edit_avt").click(function () {
                submit_edit_avt.startLoading()
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
                            window.location.reload()
                        }
                        else {
                            swal.fire({
                                title: 'Có lỗi xảy ra, vui lòng thử lại!',
                                type: 'error',
                                buttonsStyling: false,
                                confirmButtonText: 'OK',
                                confirmButtonClass: 'btn btn-primary font-weight-bold'
                            });
                        }
                    },
                    error: function () {
                        //alert("fail");
                    }
                });
            })
            $("#delete_avt").click(function () {
                $("#cancel_avatar").click()
            })
        });

        avatar5.on('remove', function (imageInput) {

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