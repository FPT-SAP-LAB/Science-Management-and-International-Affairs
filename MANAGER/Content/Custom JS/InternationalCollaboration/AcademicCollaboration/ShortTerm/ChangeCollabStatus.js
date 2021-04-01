//init uppy3
var uppy3;
var collab_id;
var person_name;
var partner_name;
var change_status_save = new LoaderBtn($('#change_status_save'));

//get corresponding collab_id
$(document).on('show.bs.modal', '#change_status_modal', function (e) {
    collab_id = $(e.relatedTarget).data('id');
    person_name = $(e.relatedTarget).data('person_name');
    partner_name = $(e.relatedTarget).data('partner_name');
});

//status
$('#change_status').select2({
    placeholder: 'Trạng thái',
    allowClear: true,
    minimumResultsForSearch: -1, //hide search box
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getAcadCollabStatus',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function () {
            return {
                status_type_specific: 1 //short-term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.collab_status_id;
                obj.text = obj.collab_status_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatAcadCollabStatus
});

//Save
$("#change_status_save").on("click", function () {
    change_status_save.startLoading();

    let evidence_file = uppy3.getFiles();
    let status_id = $('#change_status').val();
    let note = $('#status_history_note').val();

    let formData = new FormData();
    formData.append("collab_id", collab_id);
    formData.append("evidence_file", evidence_file.length == 0 ? null : evidence_file[0].data);
    formData.append("folder_name", person_name + " - " + partner_name);

    formData.append("status_id", status_id);
    formData.append("note", note);

    $.ajax({
        url: "/AcademicCollaboration/changeStatus",
        type: "POST",
        data: formData,
        cache: false,
        dataType: "json",
        processData: false,
        contentType: false,
        success: function (data) {
            if (data.success) {
                toastr.success(data.content);
                exchange_coming_table.ajax.reload();
                exchange_going_table.ajax.reload();
                change_status_save.stopLoading();
                $("#change_status_close").click();
                clearContentChangeStatus();
            } else {
                toastr.warning(data.content);
                change_status_save.stopLoading();
            }
        },
        error: function () {
            toastr.error("Có lỗi xảy ra khi upload file.");
            change_status_save.stopLoading();
        }
    });
});

function clearContentChangeStatus() {
    $('#change_status').val(null).trigger('change');
    //clear upload file
    $('#change_status_upload .uppy-list').html('');
    uppy3.removeFile(uppy3.getFiles().length == 0 ? '' : uppy3.getFiles()[0].id);
    $('#status_history_note').val('');
}