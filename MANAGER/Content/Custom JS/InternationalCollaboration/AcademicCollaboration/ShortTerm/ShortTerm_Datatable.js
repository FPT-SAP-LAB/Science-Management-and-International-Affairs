$(document).ready(function() {
    //procedure going======================================================================= 
    var procedure_going_table = $('#procedure_going_table')
        .DataTable({
            oLanguage: {
                oPaginate: {
                    sPrevious: "Trang trước",
                    sNext: "Trang sau"
                },
                sEmptyTable: "Không có dữ liệu",
                sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
            },
            serverSide: true,
            searching: false,
            lengthChange: false,
            pageLength: 5,
            ajax: {
                url: '/AcademicCollaboration/GetProcedureList',
                type: 'POST',
                data: {
                    title: function() { return $('#search_procedure_going_title').val() },
                    direction: 1
                }
            },
            columns: [{
                    data: "no",
                    name: "no"
                },
                {
                    data: "procedure_name",
                    name: "procedure_name"
                },
                {
                    data: "full_name",
                    name: "full_name"
                },
                {
                    data: "procedure_id",
                    name: "procedure_id",
                    render: function(data) {
                        return '<a id="load_edit_procedure" class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;" data-id=' + data + '>Sửa</a> ' +
                            '<a id="delete_procedure" class="btn btn-sm btn-light-danger px-6" data-id=' + data + '>Xóa</a>'
                    }
                }
            ],
            columnDefs: [{
                    targets: -1,
                    title: 'Hành động',
                    className: 'text-center text-nowrap',
                    orderable: false,
                    width: '125px',
                },
                {
                    targets: '_all',
                    className: 'text-center'
                }
            ],
            initComplete: function() {
                $(this).parent().css('overflow-x', 'auto');
                $(this).parent().css('padding', '0');
            },
        })
    $('#procedure_going_search_btn').click(function() {
            procedure_going_table.ajax.reload();
        })
        //procedure going======================================================================= 

    //procedure coming======================================================================
    var procedure_coming_table = $('#procedure_coming_table')
        .DataTable({
            oLanguage: {
                oPaginate: {
                    sPrevious: "Trang trước",
                    sNext: "Trang sau"
                },
                sEmptyTable: "Không có dữ liệu",
                sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
            },
            serverSide: true,
            searching: false,
            pageLength: 5,
            lengthChange: false,
            ajax: {
                url: '/AcademicCollaboration/GetProcedureList',
                type: 'POST',
                data: {
                    title: function() { return $('#search_procedure_coming_title').val() },
                    direction: 2
                }
            },
            columns: [{
                    data: "no",
                    name: "no"
                },
                {
                    data: "procedure_name",
                    name: "procedure_name"
                },
                {
                    data: "full_name",
                    name: "full_name"
                },
                {
                    data: "procedure_id",
                    name: "procedure_id",
                    render: function(data) {
                        return '<a id="load_edit_procedure" class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;" data-id=' + data + '>Sửa</a> ' +
                            '<a id="delete_procedure" class="btn btn-sm btn-light-danger px-6" data-id=' + data + '>Xóa</a>'

                    }
                }
            ],
            columnDefs: [{
                    targets: -1,
                    title: 'Hành động',
                    className: 'text-center text-nowrap',
                    orderable: false,
                    width: '125px',

                },
                {
                    targets: '_all',
                    className: 'text-center'
                }
            ],
            initComplete: function() {
                $(this).parent().css('overflow-x', 'auto');
                $(this).parent().css('padding', '0');
            },
        })
    $('#procedure_coming_search_btn').click(function() {
            procedure_coming_table.ajax.reload();
        })
        //procedure coming======================================================================
    var program_going_table = $("#program_going_table").DataTable({
        oLanguage: {
            oPaginate: {
                sPrevious: "Trang trước",
                sNext: "Trang sau"
            },
            sEmptyTable: "Không có dữ liệu",
            sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
        },
        initComplete: function() {
            $(this).parent().css('overflow-x', 'auto');
            $(this).parent().css('padding', '0');
            //$(this).parent().removeClass();
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
                duration: function() { return $("#search_status_tab_2_table_1").val() },
                direction: 1,
                collab_type_id: 1
            }
        },
        columns: [{
                data: "no",
                name: "no",
                className: 'text-center'
            },
            {
                data: "partner_name",
                name: "partner_name"
            },
            { data: "program_name", name: "program_name" },
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
                render: function(data, type) {
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
                render: function(data, type, row) {
                    return '<a id="load_edit_program" data-id=' + data + ' data-direction=1 data-collab=1 class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;">Sửa</a> ' +
                        '<a id="delete_program" data-id=' + row.article_id + ' data-collab=1 data-direction=1 class="btn btn-sm btn-light-danger px-6">Xóa</a>'
                }
            },
        ],
        columnDefs: [{
            targets: -1,
            title: 'Hành động',
            orderable: false,
            width: '125px',
        }]
    });

    $('#program_going_search_btn').click(function() {
        program_going_table.ajax.reload();
    })

    var program_coming_table = $("#program_coming_table").DataTable({
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
                duration: function() { return $("#search_status_tab_2_table_2").val() },
                direction: 2,
                collab_type_id: 1
            }
        },
        columns: [{
                data: "no",
                name: "no",
                className: 'text-center'
            },
            { data: "program_name", name: "program_name" },
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
                render: function(data, type) {
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
                render: function(data, type, row) {
                    return '<a id="load_edit_program" data-id=' + data + ' data-direction=2 data-collab=1 class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;">Sửa</a> ' +
                        '<a id="delete_program" data-id=' + row.article_id + ' data-collab=1 data-direction=2 class="btn btn-sm btn-light-danger px-6">Xóa</a>'

                }
            },
        ],
        columnDefs: [{
            targets: -1,
            title: 'Hành động',
            orderable: false,
            width: '125px',
        }],
        initComplete: function() {
            $(this).parent().css('overflow-x', 'auto');
            $(this).parent().css('padding', '0');
        },
    });

    $('#program_coming_search_btn').click(function() {
        program_coming_table.ajax.reload();
    })
})