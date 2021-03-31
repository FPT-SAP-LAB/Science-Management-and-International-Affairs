$('#st_describe_content').on('show.bs.modal', function () {
    getLTContent();
});

function getLTContent() {
    //get content
    $.ajax({
        url: "/AcademicCollaboration/getLTContent",
        type: "GET",
        cache: false,
        data: {
            collab_type_id: 1, //short-term
            language_id: function () { return $('#st_describe_content_language').val(); }
        },
        dataType: "json",
        success: function (data) {
            if (data != null) {
                if (data.success) {
                    $('#st_describe_content_description').val(data.obj.description);
                } else {
                    toastr.error(data.content);
                }
            }
        },
        error: function () {
            toastr.error("Có lỗi xảy ra.");
        }
    });
}

$('#st_describe_content_language').select2({
    placeholder: 'Ngôn ngữ',
    allowClear: true,
    minimumResultsForSearch: -1, //hide search box
    tags: true,
    ajax: {
        url: "/AcademicActivityType/getLanguages",
        delay: 250,
        cache: true,
        dataType: 'json',
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.language_id;
                obj.text = obj.language_name;
                return data.obj;
            });
            return {
                results: data.obj
            };
        }
    },
    templateResult: formatLanguageInfor
}).on('select2:select', function () {
    getLTContent();
});

function formatLanguageInfor(language) {
    return language.language_name;
}

$('#st_describe_content_save').on('click', function () {
    $.ajax({
        url: "/AcademicCollaboration/updateLTContent",
        type: "POST",
        cache: false,
        data: {
            collab_type_id: 1, //short-term
            language_id: function () { return $('#st_describe_content_language').val(); },
            description: function () { return $('#st_describe_content_description').val(); }
        },
        dataType: "json",
        success: function (data) {
            if (data != null) {
                if (data.success) {
                    toastr.success(data.content);
                } else {
                    toastr.error(data.content);
                }
            }
        },
        error: function () {
            toastr.error("Có lỗi xảy ra.");
        }
    });
});