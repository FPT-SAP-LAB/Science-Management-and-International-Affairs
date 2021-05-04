// Class definition
toastr.options = {
    "closeButton": false,
    "debug": false,
    "newestOnTop": false,
    "positionClass": "toast-top-right",
    "preventDuplicates": false,
    "onclick": null,
    "timeOut": "3000",
    "showEasing": "swing",
    "hideEasing": "linear",
    "showMethod": "fadeIn",
    "hideMethod": "fadeOut"
};
var delete_avt = new LoaderBtn($("#delete_avt"))
var submit_edit_avt = new LoaderBtn($("#submit_edit_avt"))
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

                var reader = new FileReader();

                reader.onload = function (e) {
                    var image = new Image();
                    image.src = e.target.result;
                    image.onload = function () {
                        var canvas = document.createElement("canvas");
                        var ctx = canvas.getContext("2d");
                        canvas.width = image.width;
                        canvas.height = image.height;
                        ctx.drawImage(image, 0, 0);
                        var URL = canvas.toDataURL('image/jpeg');
                        var newfile = dataURItoBlob(URL);

                        var fd = new FormData()
                        fd.append('people_id', people_id)

                        if (newfile.size < imageInput["input"]["files"][0].size) {
                            fd.append('imageInput', newfile, imageInput["input"]["files"][0].name)
                        } else {
                            fd.append('imageInput', imageInput["input"]["files"][0])
                        }

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
                    };
                }

                reader.readAsDataURL(imageInput["input"]["files"][0]);
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
    KTImageInputDemo.init();
});

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