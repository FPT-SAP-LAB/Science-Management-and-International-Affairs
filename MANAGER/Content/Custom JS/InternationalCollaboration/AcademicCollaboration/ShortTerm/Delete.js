$(document).on('click', '#delete_officer', function () {
    let acad_collab_id = $(this).data('id');
    console.log(acad_collab_id);
    Swal.fire({
        title: "Xác nhận xóa",
        text: "Dữ liệu hoàn toàn sau khi xóa. Bạn có muốn xóa dữ liệu bản ghi?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Xóa",
        cancelButtonText: "Hủy",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            $.ajax({
                url: '/AcademicCollaboration/deleteAcademicCollaboration',
                type: "POST",
                data: { acad_collab_id: acad_collab_id },
                success: function (data) {
                    if (data.success) {
                        exchange_coming_table.ajax.reload();
                        exchange_going_table.ajax.reload();
                        Swal.fire(data.title, data.content, "success");
                    } else {
                        Swal.fire(data.title, data.content, "error");
                    }
                },
                error: function () {
                    Swal.fire("Thất bại", "Có lỗi khi xóa", "error");
                },
            })
        }
    });
});