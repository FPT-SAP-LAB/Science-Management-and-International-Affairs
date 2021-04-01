/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
/////////////////////////////////////////////////////TABLE 2 - PROGRAM - GOING///////////////////////////////////////////////////////////
//table 3
var program_going_table = $("#collab_program_going_table").DataTable({
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
            render: function () {
                return '<a href="#edit_program" data-toggle="modal" class="btn btn-sm btn-light-primary px-6 ck-init" style="margin-right: 10px;">Sửa</a><a href="#delete" onclick="parse_id(12)" class="btn btn-sm btn-light-danger px-6" data-toggle="modal">Xóa</a>'
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

//table 4
$("#collab_program_coming_table").DataTable({
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
            render: function () {
                return '<a href="#edit_program" data-toggle="modal" class="btn btn-sm btn-light-primary px-6 ck-init" style="margin-right: 10px;">Sửa</a><a href="#delete" onclick="parse_id(12)" class="btn btn-sm btn-light-danger px-6" data-toggle="modal">Xóa</a>'
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