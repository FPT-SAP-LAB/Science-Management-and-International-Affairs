/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////TABLE 2 - PROGRAM - GOING///////////////////////////////////////////////////////////
//table 3
var collab_program_going_table = $("#collab_program_going_table").DataTable({
    oLanguage: {
        oPaginate: {
            sPrevious: "Trang trước",
            sNext: "Trang sau"
        },
        sEmptyTable: "Không có dữ liệu",
        sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
    },
    searching: false,
    lengthChange: false,
    pageLength: 5,
    serverSide: true,
    ajax: {
        url: '/AcademicCollaboration/GetProgramList',
        type: 'POST',
        data: {
            title: "",
            duration: function () { return $("#search_status_tab_2_table_1").val() },
            direction: 1,
            collab_type_id: 2
        }
    },
    columns: [
        {
            data: "no",
            name: "no",
            className: 'text-center'
        },
        {
            data: "partner_name",
            name: "partner_name"
        },
        {
            data: "program_name",
            name: "program_name",
            className: 'text-center text-nowrap'
        },
        {
            data: "full_name",
            name: "full_name",
            className: 'text-center text-nowrap'
        },
        {
            data: "registration_deadline",
            name: "registration_deadline",
            className: 'text-center text-nowrap'
        },
        {
            data: "publish_time",
            name: "publish_time",
            className: 'text-center text-nowrap',
            render: function (data, type) {
                if (type === "sort" || type === "") {
                    return data;
                }
                return moment(data).format("DD/MM/YYYY HH:mm:ss")
            }
        },
        {
            data: "program_id",
            name: "program_id",
            className: 'text-center text-nowrap',
            render: function (data, type, row) {
                return '<a id="load_edit_program" data-id=' + data + ' data-collab=2 data-direction=1 class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;">Sửa</a> ' +
                    '<a id="delete_program" data-id=' + row.article_id + ' data-collab=2 data-direction=1 class="btn btn-sm btn-light-danger px-6">Xóa</a>'
            }
        },
    ],
    columnDefs: [{
        targets: -1,
        title: 'Hành động',
        orderable: false,
        width: '125px',
    }],
    initComplete: function () {
        $(this).parent().css('overflow-x', 'auto');
        $(this).parent().css('padding', '0');
    },
});

$('#collab_program_going_search_btn').click(function () {
    collab_program_going_table.ajax.reload();
})
//table 4
var collab_program_coming_table = $("#collab_program_coming_table").DataTable({
    oLanguage: {
        oPaginate: {
            sPrevious: "Trang trước",
            sNext: "Trang sau"
        },
        sEmptyTable: "Không có dữ liệu",
        sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
    },
    searching: false,
    lengthChange: false,
    pageLength: 5,
    serverSide: true,
    ajax: {
        url: '/AcademicCollaboration/GetProgramList',
        type: 'POST',
        data: {
            title: "",
            duration: function () { return $("#search_status_tab_2_table_2").val() },
            direction: 2,
            collab_type_id: 2
        }
    },
    columns: [
        {
            data: "no",
            name: "no",
            className: 'text-center'
        },
        {
            data: "program_name",
            name: "program_name"
        },
        {
            data: "full_name",
            name: "full_name",
            className: 'text-center text-nowrap'
        },
        {
            data: "registration_deadline",
            name: "registration_deadline",
            className: 'text-center text-nowrap'
        },
        {
            data: "publish_time",
            name: "publish_time",
            className: 'text-center text-nowrap',
            render: function (data, type) {
                if (type === "sort" || type === "") {
                    return data;
                }
                return moment(data).format("DD/MM/YYYY HH:mm:ss")
            }
        },
        {
            data: "program_id",
            name: "program_id",
            className: 'text-center text-nowrap',
            render: function (data, type, row) {
                return '<a id="load_edit_program" data-id=' + data + ' data-collab=2 data-direction=2 class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;">Sửa</a> ' +
                    '<a id="delete_program" data-id=' + row.article_id + ' data-collab=2 data-direction=2 class="btn btn-sm btn-light-danger px-6">Xóa</a>'
            }
        },
    ],
    columnDefs: [{
        targets: -1,
        title: 'Hành động',
        orderable: false,
        width: '125px',
    }],
    initComplete: function () {
        $(this).parent().css('overflow-x', 'auto');
        $(this).parent().css('padding', '0');
    },
});

$('#collab_program_coming_search_btn').click(function () {
    collab_program_coming_table.ajax.reload();
})