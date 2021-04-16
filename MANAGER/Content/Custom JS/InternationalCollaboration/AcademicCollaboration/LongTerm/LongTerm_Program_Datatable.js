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
        sInfoEmpty: "",
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
            name: "partner_name",
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
                $(td).css({ 'min-width': '90px', 'max-width': '300px' });
            },
            className: 'text-center',
        },
        {
            data: "program_name",
            name: "program_name",
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
                $(td).css({ 'min-width': '90px', 'max-width': '300px' });
            },
            className: 'text-center',
        },
        {
            data: "full_name",
            name: "full_name",
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
            },
            className: 'text-center',
        },
        {
            data: "registration_deadline",
            name: "registration_deadline",
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
            },
            className: 'text-center',
        },
        {
            data: "publish_time",
            name: "publish_time",
            render: function (data, type) {
                if (type === "sort" || type === "") {
                    return data;
                }
                return moment(data).format("DD/MM/YYYY HH:mm:ss")
            },
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
            },
            className: 'text-center',
        },
        {
            data: "program_id",
            name: "program_id",
            render: function (data, type, row) {
                return '<a id="load_edit_program" data-id=' + data + ' data-collab=2 data-direction=1 class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;">Sửa</a> ' +
                    '<a id="delete_program" data-id=' + row.article_id + ' data-collab=2 data-direction=1 class="btn btn-sm btn-light-danger px-6">Xóa</a>'
            },
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
            },
            className: 'text-center text-nowrap',
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
        $(this).parent().css('width', '100%');
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
        sInfoEmpty: "",
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
            name: "program_name",
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
                $(td).css({ 'min-width': '90px', 'max-width': '300px' });
            },
            className: 'text-center',
        },
        {
            data: "full_name",
            name: "full_name",
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
            },
            className: 'text-center',
        },
        {
            data: "registration_deadline",
            name: "registration_deadline",
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
            },
            className: 'text-center',
        },
        {
            data: "publish_time",
            name: "publish_time",
            render: function (data, type) {
                if (type === "sort" || type === "") {
                    return data;
                }
                return moment(data).format("DD/MM/YYYY HH:mm:ss")
            },
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
            },
            className: 'text-center',
        },
        {
            data: "program_id",
            name: "program_id",
            render: function (data, type, row) {
                return '<a id="load_edit_program" data-id=' + data + ' data-collab=2 data-direction=1 class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;">Sửa</a> ' +
                    '<a id="delete_program" data-id=' + row.article_id + ' data-collab=2 data-direction=1 class="btn btn-sm btn-light-danger px-6">Xóa</a>'
            },
            createdCell: function (td) {
                $(td).css('padding', '0 5px')
            },
            className: 'text-center text-nowrap',
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
        $(this).parent().css('width', '100%');
    },
});

$('#collab_program_coming_search_btn').click(function () {
    collab_program_coming_table.ajax.reload();
})