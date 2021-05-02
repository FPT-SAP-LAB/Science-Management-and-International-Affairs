class Researcher {
    constructor(people_id, name, dob, nationality, title, position, phone, email, website, googlescholar, cv, fields) {
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
function enableEdit() {
    $("#edit-control-area").removeAttr('hidden')
    $("#edit-control-area").css('display', 'block')
    $("#edit-enable-area").hide()
    $(".researcher_infomation").attr('disabled', false)
}
$(function () {
    var save_loader = new LoaderBtn($("#save-btn"))
    $("#edit-control-area").hide()
    $("#edit-enable-btn").click(function () {
        enableEdit()
    })
    $("#save-btn").click(function () {
        if (validateNonEmptyField(["#researcher_name", "#researcher_dob"])) {
            save_loader.startLoading()
            $("#progress-bar").show()
            $(".researcher_infomation").attr('disabled', false)
            ////////////////////xử lý ở đây/////////////////////
            var url = new URL(window.location.href);
            people_id = url.searchParams.get("id");
            let name = $("#researcher_name").val()
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
        }
    })
    $("#cancel-btn").click(function () {
        Swal.fire({
            title: "Huỷ thay đổi?",
            icon: "warning",
            showCancelButton: true,
            confirmButtonText: "Đồng ý",
            cancelButtonText: "Không",
            reverseButtons: true
        }).then(function (result) {
            if (result.value) {
                window.location.reload()
            }
        });
    })
})
