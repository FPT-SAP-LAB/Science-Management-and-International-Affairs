    // Class definition
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
        console.log(imageInput)
                swal.fire({
        title: 'Image successfully changed !',
                    type: 'success',
                    buttonsStyling: false,
                    confirmButtonText: 'Awesome!',
                    confirmButtonClass: 'btn btn-primary font-weight-bold'
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
        KTImageInputDemo.init();
    });
class Researcher {
    constructor(people_id, name, dob, nationality, title,position, phone,email,website,googlescholar,cv,fields) {
        this.people_id = people_id,
        this.name = name,
        this.nationality = nationality,
        this.position = position,
        this.title = title,
        this.dob = dob,
        this.phone = phone,
        this.email = email,
        this.website = website,
        this.googlescholar = googlescholar,
        this.cv = cv,
        this.fields = fields
    }
}
$("#save-btn").click(function () {
    var url = new URL(window.location.href);
    people_id = url.searchParams.get("id");
    name = $("#researcher_name").val()
    dob = $("#researcher_dob").val()
    nationality = $("#country_select").select2('data');
    title = $("#title_select").select2('data');
    position = $("#position_select").select2('data');
    phone = $("#researcher_phone").val()
    email = $("#researcher_email").val()
    website = $("#researcher_website").val()
    googlescholar = $("#researcher_gscholar").val()
    cv = $("#researcher_cv").val()
    fields = $("#interested_fields_select").select2('data');
    let info = new Researcher(people_id, name, dob, nationality, title, position, phone, email, website, googlescholar, cv, fields)
    var fd = new FormData();
    console.log(info)
    fd.append('info', JSON.stringify({ info: info }));
    $.ajax({
        url: "/Researchers/EditResearcher",
        type: "POST",
        data: fd,
        processData: false,
        contentType: false,
        success: function (response) {
            if (response.mess == "ss") {
                //editRequest();
                window.location.reload()
            }
            else window.location.reload()
        },
        error: function () {
            //alert("fail");
        }
    });
})
