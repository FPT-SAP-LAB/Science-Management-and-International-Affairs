$(document).ready(function () {
    //$('#exchange_going_table').DataTable({
    //    oLanguage: {
    //        oPaginate: {
    //            sPrevious: "Trang trước",
    //            sNext: "Trang sau"
    //        },
    //        sEmptyTable: "Không có dữ liệu",
    //        sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
    //    },
    //    searching: false,
    //    lengthChange: false,
    //    columnDefs: [{
    //        targets: 8,
    //        width: '75px',
    //        className: 'text-center',
    //        render: function (data) {
    //            var status = {
    //                1: {
    //                    'title': 'Không hoàn thành',
    //                    'class': 'label-secondary'
    //                },
    //                2: {
    //                    'title': 'Đã hoàn thành',
    //                    'class': 'label-success'
    //                },
    //                3: {
    //                    'title': 'Đang thực hiện',
    //                    'class': 'label-danger'
    //                },
    //                4: {
    //                    'title': 'Đề xuất',
    //                    'class': 'label-warning'
    //                }
    //            };
    //            if (typeof status[data] === 'undefined') {
    //                return data;
    //            }
    //            return '<span class="label label-lg label-pill font-weight-bold ' + status[data].class + ' label-inline">' + status[data].title + '</span> ' +
    //                '<a href="#add_status" data-toggle="modal"><span class="svg-icon svg-icon-dark svg-icon-sm"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-02-01-052524/theme/html/demo1/dist/../src/media/svg/icons/Design/Edit.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">' +
    //                ' <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"> ' +
    //                '<rect x="0" y="0" width="24" height="24"/> ' +
    //                '<path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/> ' +
    //                ' <rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/> ' +
    //                '   </g> ' +
    //                '</svg><!--end::Svg Icon--></span></a>' +
    //                '<a onclick="show_status_history(13)" class="show-status-history" href="#status_history" data-toggle="modal"><span class="svg-icon svg-icon-dark svg-icon-sm"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-02-01-052524/theme/html/demo1/dist/../src/media/svg/icons/Code/Time-schedule.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"> ' +
    //                '  <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"> ' +
    //                '    <rect x="0" y="0" width="24" height="24"/> ' +
    //                '  <path d="M10.9630156,7.5 L11.0475062,7.5 C11.3043819,7.5 11.5194647,7.69464724 11.5450248,7.95024814 L12,12.5 L15.2480695,14.3560397 C15.403857,14.4450611 15.5,14.6107328 15.5,14.7901613 L15.5,15 C15.5,15.2109164 15.3290185,15.3818979 15.1181021,15.3818979 C15.0841582,15.3818979 15.0503659,15.3773725 15.0176181,15.3684413 L10.3986612,14.1087258 C10.1672824,14.0456225 10.0132986,13.8271186 10.0316926,13.5879956 L10.4644883,7.96165175 C10.4845267,7.70115317 10.7017474,7.5 10.9630156,7.5 Z" fill="#000000"/> ' +
    //                '<path d="M7.38979581,2.8349582 C8.65216735,2.29743306 10.0413491,2 11.5,2 C17.2989899,2 22,6.70101013 22,12.5 C22,18.2989899 17.2989899,23 11.5,23 C5.70101013,23 1,18.2989899 1,12.5 C1,11.5151324 1.13559454,10.5619345 1.38913364,9.65805651 L3.31481075,10.1982117 C3.10672013,10.940064 3,11.7119264 3,12.5 C3,17.1944204 6.80557963,21 11.5,21 C16.1944204,21 20,17.1944204 20,12.5 C20,7.80557963 16.1944204,4 11.5,4 C10.54876,4 9.62236069,4.15592757 8.74872191,4.45446326 L9.93948308,5.87355717 C10.0088058,5.95617272 10.0495583,6.05898805 10.05566,6.16666224 C10.0712834,6.4423623 9.86044965,6.67852665 9.5847496,6.69415008 L4.71777931,6.96995273 C4.66931162,6.97269931 4.62070229,6.96837279 4.57348157,6.95710938 C4.30487471,6.89303938 4.13906482,6.62335149 4.20313482,6.35474463 L5.33163823,1.62361064 C5.35654118,1.51920756 5.41437908,1.4255891 5.49660017,1.35659741 C5.7081375,1.17909652 6.0235153,1.2066885 6.2010162,1.41822583 L7.38979581,2.8349582 Z" fill="#000000" opacity="0.3"/> ' +
    //                ' </g> ' +
    //                '</svg><!--end::Svg Icon--></span></a> ';
    //        },
    //    },
    //    {
    //        targets: -1,
    //        title: 'Hành động',
    //        className: 'text-center text-nowrap',
    //        orderable: false,
    //        width: '125px',
    //        render: function () {
    //            return '<a class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;" data-toggle="modal" href="#edit_officer">Sửa</a><a href="#delete" onclick="parse_id(12)" class="btn btn-sm btn-light-danger px-6" data-toggle="modal">Xóa</a>'
    //        }
    //    }
    //    ],
    //    initComplete: function () {
    //        $(this).parent().css('overflow-x', 'auto');
    //        $(this).parent().removeClass();
    //        //$('.colab-table thead th').removeAttr('style');
    //    },
    //})

    //$('#exchange_coming_table').DataTable({
    //    oLanguage: {
    //        oPaginate: {
    //            sPrevious: "Trang trước",
    //            sNext: "Trang sau"
    //        },
    //        sEmptyTable: "Không có dữ liệu",
    //        sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
    //    },
    //    searching: false,
    //    lengthChange: false,
    //    columnDefs: [{
    //        targets: 8,
    //        width: '75px',
    //        className: 'text-center',
    //        render: function (data) {
    //            var status = {
    //                1: {
    //                    'title': 'Không hoàn thành',
    //                    'class': 'label-secondary'
    //                },
    //                2: {
    //                    'title': 'Đã hoàn thành',
    //                    'class': 'label-success'
    //                },
    //                3: {
    //                    'title': 'Đang thực hiện',
    //                    'class': 'label-danger'
    //                },
    //                4: {
    //                    'title': 'Đề xuất',
    //                    'class': 'label-warning'
    //                }
    //            };
    //            if (typeof status[data] === 'undefined') {
    //                return data;
    //            }
    //            return '<span class="label label-lg label-pill font-weight-bold ' + status[data].class + ' label-inline">' + status[data].title + '</span> ' +
    //                '<a href="#add_status" data-toggle="modal"><span class="svg-icon svg-icon-dark svg-icon-sm"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-02-01-052524/theme/html/demo1/dist/../src/media/svg/icons/Design/Edit.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1">' +
    //                ' <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"> ' +
    //                '<rect x="0" y="0" width="24" height="24"/> ' +
    //                '<path d="M8,17.9148182 L8,5.96685884 C8,5.56391781 8.16211443,5.17792052 8.44982609,4.89581508 L10.965708,2.42895648 C11.5426798,1.86322723 12.4640974,1.85620921 13.0496196,2.41308426 L15.5337377,4.77566479 C15.8314604,5.0588212 16,5.45170806 16,5.86258077 L16,17.9148182 C16,18.7432453 15.3284271,19.4148182 14.5,19.4148182 L9.5,19.4148182 C8.67157288,19.4148182 8,18.7432453 8,17.9148182 Z" fill="#000000" fill-rule="nonzero" transform="translate(12.000000, 10.707409) rotate(-135.000000) translate(-12.000000, -10.707409) "/> ' +
    //                ' <rect fill="#000000" opacity="0.3" x="5" y="20" width="15" height="2" rx="1"/> ' +
    //                '   </g> ' +
    //                '</svg><!--end::Svg Icon--></span></a>' +
    //                '<a onclick="show_status_history(13)" class="show-status-history" href="#status_history" data-toggle="modal"><span class="svg-icon svg-icon-dark svg-icon-sm"><!--begin::Svg Icon | path:/var/www/preview.keenthemes.com/metronic/releases/2021-02-01-052524/theme/html/demo1/dist/../src/media/svg/icons/Code/Time-schedule.svg--><svg xmlns="http://www.w3.org/2000/svg" xmlns:xlink="http://www.w3.org/1999/xlink" width="24px" height="24px" viewBox="0 0 24 24" version="1.1"> ' +
    //                '  <g stroke="none" stroke-width="1" fill="none" fill-rule="evenodd"> ' +
    //                '    <rect x="0" y="0" width="24" height="24"/> ' +
    //                '  <path d="M10.9630156,7.5 L11.0475062,7.5 C11.3043819,7.5 11.5194647,7.69464724 11.5450248,7.95024814 L12,12.5 L15.2480695,14.3560397 C15.403857,14.4450611 15.5,14.6107328 15.5,14.7901613 L15.5,15 C15.5,15.2109164 15.3290185,15.3818979 15.1181021,15.3818979 C15.0841582,15.3818979 15.0503659,15.3773725 15.0176181,15.3684413 L10.3986612,14.1087258 C10.1672824,14.0456225 10.0132986,13.8271186 10.0316926,13.5879956 L10.4644883,7.96165175 C10.4845267,7.70115317 10.7017474,7.5 10.9630156,7.5 Z" fill="#000000"/> ' +
    //                '<path d="M7.38979581,2.8349582 C8.65216735,2.29743306 10.0413491,2 11.5,2 C17.2989899,2 22,6.70101013 22,12.5 C22,18.2989899 17.2989899,23 11.5,23 C5.70101013,23 1,18.2989899 1,12.5 C1,11.5151324 1.13559454,10.5619345 1.38913364,9.65805651 L3.31481075,10.1982117 C3.10672013,10.940064 3,11.7119264 3,12.5 C3,17.1944204 6.80557963,21 11.5,21 C16.1944204,21 20,17.1944204 20,12.5 C20,7.80557963 16.1944204,4 11.5,4 C10.54876,4 9.62236069,4.15592757 8.74872191,4.45446326 L9.93948308,5.87355717 C10.0088058,5.95617272 10.0495583,6.05898805 10.05566,6.16666224 C10.0712834,6.4423623 9.86044965,6.67852665 9.5847496,6.69415008 L4.71777931,6.96995273 C4.66931162,6.97269931 4.62070229,6.96837279 4.57348157,6.95710938 C4.30487471,6.89303938 4.13906482,6.62335149 4.20313482,6.35474463 L5.33163823,1.62361064 C5.35654118,1.51920756 5.41437908,1.4255891 5.49660017,1.35659741 C5.7081375,1.17909652 6.0235153,1.2066885 6.2010162,1.41822583 L7.38979581,2.8349582 Z" fill="#000000" opacity="0.3"/> ' +
    //                ' </g> ' +
    //                '</svg><!--end::Svg Icon--></span></a> ';
    //        },
    //    },
    //    {
    //        targets: -1,
    //        title: 'Hành động',
    //        className: 'text-center text-nowrap',
    //        orderable: false,
    //        width: '125px',
    //        render: function () {
    //            return '<a class="btn btn-sm btn-light-primary px-6" style="margin-right: 10px;" data-toggle="modal" href="#edit_officer">Sửa</a><a href="#delete" onclick="parse_id(12)" class="btn btn-sm btn-light-danger px-6" data-toggle="modal">Xóa</a>'
    //        }
    //    }
    //    ],
    //    initComplete: function () {
    //        $(this).parent().css('overflow-x', 'auto');
    //        $(this).parent().removeClass();
    //        //$('.colab-table thead th').removeAttr('style');


    //    },
    //})

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
                    title: function () { return $('#search_procedure_going_title').val() },
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
                render: function (data) {
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
            initComplete: function () {
                $(this).parent().css('overflow-x', 'auto');
                $(this).parent().css('padding', '0');
            },
        })
    $('#procedure_going_search_btn').click(function () {
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
                    title: function () { return $('#search_procedure_coming_title').val() },
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
                render: function (data) {
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
            initComplete: function () {
                $(this).parent().css('overflow-x', 'auto');
                $(this).parent().css('padding', '0');
            },
        })
    $('#procedure_coming_search_btn').click(function () {
        procedure_coming_table.ajax.reload();
    })
    //procedure coming======================================================================
    $("#program_going_table").DataTable({
        oLanguage: {
            oPaginate: {
                sPrevious: "Trang trước",
                sNext: "Trang sau"
            },
            sEmptyTable: "Không có dữ liệu",
            sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
        },
        initComplete: function () {
            $(this).parent().css('overflow-x', 'auto');
            $(this).parent().css('padding', '0');
            //$(this).parent().removeClass();
        },
        searching: false,
        lengthChange: false,
        columnDefs: [{
            targets: -1,
            title: 'Hành động',
            className: 'text-center text-nowrap',
            orderable: false,
            width: '125px',
            render: function () {
                return '<a href="#edit_program" data-toggle="modal" class="btn btn-sm btn-light-primary px-6 ck-init" style="margin-right: 10px;">Sửa</a><a href="#delete" onclick="parse_id(12)" class="btn btn-sm btn-light-danger px-6" data-toggle="modal">Xóa</a>'
            }
        }]
    });

    $("#program_coming_table").DataTable({
        oLanguage: {
            oPaginate: {
                sPrevious: "Trang trước",
                sNext: "Trang sau"
            },
            sEmptyTable: "Không có dữ liệu",
            sInfo: "Đang hiển thị từ _START_ đến _END_ của _TOTAL_ bản ghi",
        },
        initComplete: function () {
            $(this).parent().css('overflow-x', 'auto');
            $(this).parent().css('padding', '0');
            //$(this).parent().removeClass();
        },
        searching: false,
        lengthChange: false,
        columnDefs: [{
            targets: -1,
            title: 'Hành động',
            className: 'text-center text-nowrap',
            orderable: false,
            width: '125px',
            render: function () {
                return '<a href="#edit_program" data-toggle="modal" class="btn btn-sm btn-light-primary px-6 ck-init" style="margin-right: 10px;">Sửa</a><a href="#delete" onclick="parse_id(12)" class="btn btn-sm btn-light-danger px-6" data-toggle="modal">Xóa</a>'
            }
        }]
    });


})