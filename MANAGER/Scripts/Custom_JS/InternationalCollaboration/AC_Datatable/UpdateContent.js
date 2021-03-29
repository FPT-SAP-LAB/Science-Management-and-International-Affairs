$('#lt_describe_content').on('show.bs.modal', function () {
    //get content
    $.ajax({
        url: "/AcademicCollaboration/getLTContent",
        type: "GET",
        cache: false,
        data: {
            activity_type_id: 2, //dài hạn
            language_id: function () { return $('#lt_describe_content_language').val(); }
        },
        dataType: "json",
        success: function (data) {
            if (data != null) {
                if (data.success) {
                    $('#lt_describe_content_note').val(data.obj.descrpition);
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

$('#lt_describe_content_language').select2({
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
    dataTable.ajax.reload();
});

function formatLanguageInfor(language) {
    return language.language_name;
}