///////////////////////////////////////////TABLE 1: LONG-TERM GOING//////////////////////////////////////////
// 1.SEARCH
$('#search_nation_tab_1_table_1').select2({
    placeholder: 'Quốc gia',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getCountries',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                country_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.country_name;
                obj.text = obj.country_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatCountryInfo
});

function formatCountryInfo(country) {
    return country.country_name;
}

$('#search_year_tab_1_table_1').select2({
    placeholder: 'Năm đi học',
    allowClear: true
});

//get data to year search
$.ajax({
    url: "/AcademicCollaboration/getYears",
    type: "GET",
    cache: true,
    dataType: "json",
    success: function (data) {
        if (data != null) {
            for (let year = data.obj.year_from; year <= data.obj.year_to; year++) {
                let newOption = new Option(year, year, false, false);
                $("#search_year_tab_1_table_1").val(null).trigger('change');
                $("#search_year_tab_1_table_1").append(newOption).trigger('change');
            }
        }
    },
    error: function () {
        toastr.error("Lấy dữ liệu về năm xảy ra lỗi.");
    }
});


$('#search_training_facility_tab_1_table_1').select2({
    placeholder: 'Đơn vị đào tạo',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getPartners',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                partner_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.partner_name;
                obj.text = obj.partner_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatPartnerInfo
});

function formatPartnerInfo(partner) {
    return partner.partner_name;
}

$('#search_working_facility_tab_1_table_1').select2({
    placeholder: 'Đơn vị công tác',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getOffices',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                office_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.office_name;
                obj.text = obj.office_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatOfficeInfo
});

function formatOfficeInfo(office) {
    return office.office_name;
}

//MODAL INFORMATION 
//// person infor
var person = $('#add_officer_name').val(); //*
var person_name = person.split('/')[0] === undefined ? '' : person.split('/')[0];
var person_id = person.split('/')[1] === undefined ? 0 : person.split('/')[1];
var available_person = false;

//// partner infor
var partner = $('#add_officer_traning').val(); //*
var partner_name = partner.split('/')[0] === undefined ? '' : partner.split('/')[0];
var partner_id = partner.split('/')[1] === undefined ? 0 : partner.split('/')[1];
var person_email = $('#add_officer_email').val();
var person_profile_office_id = $('#add_officer_facility').val();
var partner_country_id = $('#add_officer_nation').val();
var collab_scope_id = $('#add_officer_coop_scope').val();
var available_partner = false;

var status_id = $('#add_officer_status').val(); //*
var plan_start_date = $('#add_officer_start_plan_date').val(); //*
var plan_end_date = $('#add_officer_end_plan_date').val(); //*
var actual_start_date = $('#add_officer_start_date').val();
var actual_end_date = $('#add_officer_end_date').val();
var support = $('#add_officer_support').prop('checked');
var note = $('#add_officer_note').val();
var collab_id = 0;

var obj_person = {
    available_person: available_person,
    person_name: person_name,
    person_id: person_id,
    person_email: person_email,
    person_profile_office_id: person_profile_office_id
}

var obj_partner = {
    available_partner: available_partner,
    partner_name: partner_name,
    partner_id: partner_id,
    partner_country_id: partner_country_id,
    collab_scope_id: collab_scope_id
}

var obj_academic_collab = {
    collab_id: collab_id,
    status_id: status_id,
    plan_start_date: formatDatePicker(plan_start_date),
    plan_end_date: formatDatePicker(plan_end_date),
    actual_start_date: formatDatePicker(actual_start_date),
    actual_end_date: formatDatePicker(actual_end_date),
    support: support,
    note: note
}

var file = {
    file_id: 0,
    name: "",
    link: "",
    file_drive_id: ""
}

//2.ADD MODAL
var uppy1; //init for uppy.js
//init loader save button
var add_officer_save = new LoaderBtn($("#add_officer_save"));
//2.1.SHOW BUTTON
///2.1.1.THÔNG TIN CÁ NHÂN
$('#add_officer_name').select2({
    placeholder: 'Họ và tên',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getPeople',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                person_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.name + "/" + obj.people_id;
                obj.text = obj.name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatPersonInfo
}).on("select2:select", function () { //after select option
    checkPersonAdd();
}).on("select2:unselecting", function () {
    //after clear tag
    //clear email & office
    $('#add_officer_email').val("");
    $('#add_officer_facility').val(null).trigger('change');
    //enable email & office
    $('#add_officer_email').prop('disabled', false);
    $('#add_officer_facility').prop('disabled', false);
});

function formatPersonInfo(person) {
    if (person.id) {
        let mssv_msnv = person.mssv_msnv;
        let name = person.name;
        if (mssv_msnv === undefined && name === undefined) {
            return "Dữ liệu cá nhân mới."
        } else {
            return mssv_msnv + " - " + name;
        }
    }
}

function checkPersonAdd() {
    person = $('#add_officer_name').val();
    person_name = person.split('/')[0] === undefined ? '' : person.split('/')[0];
    person_id = person.split('/')[1] === undefined ? 0 : person.split('/')[1];

    $.ajax({
        url: "/AcademicCollaboration/checkPerson",
        type: "POST",
        data: {
            people_id: person_id,
            people_name: person_name
        },
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                //clear content
                $('#add_officer_email').val("");
                $('#add_officer_facility').val(null).trigger('change');
                if (data.obj != null) {
                    //avaiable person data in DB
                    available_person = true; //using for Add||Edit
                    if (data.success) {
                        //auto fill data
                        let p = data.obj;
                        $('#add_officer_email').val(p.email);
                        $('#add_officer_facility').append(new Option(p.office_name, p.office_id, false, true)).trigger('change');
                        //disable email & office
                        $('#add_officer_email').prop('disabled', true);
                        $('#add_officer_facility').prop('disabled', true);
                    } else {
                        toastr.error(data.content);
                    }
                } else {
                    //new person case
                    available_person = false;
                    ///enable email and office select
                    $('#add_officer_email').prop('disabled', false);
                    $('#add_officer_facility').prop('disabled', false);
                }
            } else {
                toastr.error("Kiểm tra cán bộ giảng viên có lỗi xảy ra.");
            }
        },
        error: function () {
            toastr.error("Kiểm tra cán bộ giảng viên có lỗi xảy ra.");
        }
    });
}

$('#add_officer_facility').select2({
    placeholder: 'Đơn vị - Cơ sở',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getOffices',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                office_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.office_id;
                obj.text = obj.office_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatOfficeInfo
});

///2.1.2.ĐƠN VỊ ĐÀO TẠO
$('#add_officer_traning').select2({
    placeholder: 'Đơn vị đào tạo',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getPartners',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                partner_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.partner_name + '/' + obj.partner_id;
                obj.text = obj.partner_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatPartnerInfo
}).on("select2:select", function () { //after select partner
    //check partner
    checkPartnerAdd();
}).on("select2:unselecting", function () {
    //after clear tag
    //clear country select
    $('#add_officer_nation').val(null).trigger('change');
    ///enable country
    $('#add_officer_nation').prop('disabled', false);
});

function formatPartnerInfo(partner) {
    if (partner.id) {
        let partner_name = partner.partner_name;
        if (partner_name === undefined) {
            return "Dữ liệu đơn vị đào tạo mới."
        } else {
            return partner.partner_name;
        }
    }
}

function checkPartnerAdd() {
    //process partner_name
    partner = $('#add_officer_traning').val();
    partner_name = partner.split('/')[0] === undefined ? '' : partner.split('/')[0];
    partner_id = partner.split('/')[1] === undefined ? 0 : partner.split('/')[1];

    $.ajax({
        url: "/AcademicCollaboration/checkPartner",
        type: "POST",
        data: {
            partner_name: partner_name,
            partner_id: partner_id
        },
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                //clear content
                $('#add_officer_nation').val(null).trigger('change');
                if (data.obj != null) {
                    //available partner data in DB
                    available_partner = true; //using for Add||Edit
                    if (data.success) {
                        //auto fill data
                        let p = data.obj;
                        $('#add_officer_nation').append(new Option(p.country_name, p.country_id, false, true)).trigger('change');
                        //disable country
                        $('#add_officer_nation').prop('disabled', true);
                    } else {
                        toastr.error(data.content);
                    }
                } else {
                    //new partner case
                    available_partner = false;
                    ///enable country
                    $('#add_officer_nation').prop('disabled', false);
                }
            } else {
                toastr.error("Kiểm tra đối tác có lỗi xảy ra.");
            }
        },
        error: function () {
            toastr.error("Kiểm tra đối tác có lỗi xảy ra.");
        }
    });
}

$('#add_officer_nation').select2({
    placeholder: 'Quốc gia',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getCountries',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                country_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.country_id;
                obj.text = obj.country_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatCountryInfo
});

$('#add_officer_coop_scope').select2({
    placeholder: 'Phạm vi hợp tác',
    allowClear: true,
    minimumResultsForSearch: -1, //hide search box
    ajax: {
        url: '/AcademicCollaboration/getCollabScopes',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function () {
            return {
                collab_abbreviation_name: 'JTP' //join training program
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.scope_id;
                obj.text = obj.scope_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatCollabScope
});

function formatCollabScope(scope) {
    return scope.scope_abbreviation + " - " + scope.scope_name;
}

///2.1.3.CHI TIẾT
$('#add_officer_status').select2({
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
                status_type_specific: 2 //long-term
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

function formatAcadCollabStatus(acs) {
    return acs.collab_status_name;
}

//2.2. SAVE BUTTON
$('#add_officer_save').on('click', function () {
    //person
    person = $('#add_officer_name').val();
    person_name = person.split('/')[0];
    person_id = person.split('/')[1]

    person_email = $('#add_officer_email').val();
    person_profile_office_id = $('#add_officer_facility').val();

    //partner
    partner = $('#add_officer_traning').val();
    partner_name = partner.split('/')[0];
    partner_id = partner.split('/')[1];

    partner_country_id = $('#add_officer_nation').val();

    collab_scope_id = $('#add_officer_coop_scope').val();

    //detail
    status_id = $('#add_officer_status').val();

    plan_start_date = $('#add_officer_start_plan_date').val();
    plan_end_date = $('#add_officer_end_plan_date').val();

    actual_start_date = $('#add_officer_start_date').val();
    actual_end_date = $('#add_officer_end_date').val();

    evidence = uppy1.getFiles();

    support = $('#add_officer_support').prop('checked');
    note = $('#add_officer_note').val();

    obj_person = {
        available_person: available_person,
        person_name: person_name,
        person_id: person_id,
        person_email: person_email,
        person_profile_office_id: person_profile_office_id
    }

    obj_partner = {
        available_partner: available_partner,
        partner_name: partner_name,
        partner_id: partner_id,
        partner_country_id: partner_country_id,
        collab_scope_id: collab_scope_id
    }

    obj_academic_collab = {
        collab_id: 0, //set 0 when add new academic collaboration
        status_id: status_id,
        plan_start_date: formatDatePicker(plan_start_date),
        plan_end_date: formatDatePicker(plan_end_date),
        actual_start_date: formatDatePicker(actual_start_date),
        actual_end_date: formatDatePicker(actual_end_date),
        support: support,
        note: note
    }

    //check empty
    if (isEmpty(person) || isEmpty(partner) || isEmpty(collab_scope_id) || isEmpty(status_id) || isEmpty(plan_start_date) || isEmpty(plan_end_date)) {
        return toastr.error("Chưa chọn đủ trường thông tin bắt buộc.");
    } else {
        //validate datepicker from - to
        if (!datePickerFromToValidate(plan_start_date, plan_end_date) || !datePickerFromToValidate(actual_start_date, actual_end_date)) {
            return toastr.error("`TG đi học` không được vượt quá `TG kết thúc`.")
        } else {
            //start load
            add_officer_save.startLoading();

            let formData = new FormData();
            formData.append("evidence", evidence.length == 0 ? null : evidence[0].data);
            formData.append("folder_name", person_name + " - " + partner_name);

            formData.append("direction_id", 1); //going case
            formData.append("collab_type_id", 2); //long-term

            let obj_person_stringify = JSON.stringify(obj_person);
            let obj_partner_stringify = JSON.stringify(obj_partner);
            let obj_academic_collab_stringify = JSON.stringify(obj_academic_collab);

            formData.append("obj_person_stringify", obj_person_stringify);
            formData.append("obj_partner_stringify", obj_partner_stringify);
            formData.append("obj_academic_collab_stringify", obj_academic_collab_stringify);

            //Save actually
            $.ajax({
                url: "/AcademicCollaboration/saveAcademicCollaboration",
                type: "POST",
                data: formData,
                cache: false,
                dataType: "json",
                processData: false,
                contentType: false,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.content);
                        collab_going_table.ajax.reload();
                        add_officer_save.stopLoading();
                        $("#add_officer_close").click();
                        clearContentAddModal();
                    } else {
                        toastr.warning(data.content);
                        add_officer_save.stopLoading();
                    }
                },
                error: function () {
                    toastr.error("Có lỗi xảy ra khi upload file.");
                    add_officer_save.stopLoading();
                }
            });
            for (var pair of formData.entries()) {
                console.log(pair[0] + ', ' + pair[1]);
            }
        }
    }
});

function isEmpty(value) {
    return value === "" ? true : false;
}

function datePickerFromToValidate(from, to) {
    if (!(from === "" || to === "")) {
        let date_from = Date.parse(formatDatePicker(from));
        let date_to = Date.parse(formatDatePicker(to));
        if (date_from > date_to) {
            return false;
        }
        return true;
    }
    return true;
}

function formatDatePicker(date) {
    if (date != '') {
        return date.split('/')[2] + '/' + date.split('/')[1] + '/' + date.split('/')[0];
    }
}

function clearContentAddModal() {
    //enable input and select
    $('#add_officer_email').prop('disabled', false);
    $("#add_officer_facility").prop('disabled', false);
    $('#add_officer_nation').prop('disabled', false);
    $('#add_officer_coop_scope').prop('disabled', false);

    //clear data
    ///THÔNG TIN CÁ NHÂN
    $('#add_officer_name').val(null).trigger('change');
    $('#add_officer_email').val('');
    $('#add_officer_facility').val(null).trigger('change');
    ///ĐƠN VỊ ĐÀO TẠO
    $('#add_officer_traning').val(null).trigger('change');
    $('#add_officer_nation').val(null).trigger('change');
    $('#add_officer_coop_scope').val(null).trigger('change');
    ///CHI TIẾT
    $('#add_officer_status').val(null).trigger('change');
    $('#add_officer_start_plan_date').val('');
    $('#add_officer_end_plan_date').val('');

    //clear upload file
    $('.uppy-list').html('');
    uppy1.removeFile(uppy1.getFiles().length == 0 ? '' : uppy1.getFiles()[0].id);

    $('#add_officer_start_date').val('');
    $('#add_officer_end_date').val('');
    $('#add_officer_support').prop('checked', false);
    $('#add_officer_note').val('');
}

//3.EDIT MODAL
var uppy2; //init uppy2

//3.1.Init select2 for select element in edit modal
///3.2.1.THÔNG TIN CÁ NHÂN
$('#edit_officer_name').select2({
    placeholder: 'Họ và tên',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getPeople',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                person_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.name + "/" + obj.people_id;
                obj.text = obj.name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatPersonInfo
}).on("select2:select", function () { //after select option
    checkPersonEdit();
}).on("select2:unselecting", function () {
    //after clear tag
    //clear email & office
    $('#edit_officer_email').val("");
    $('#edit_officer_facility').val(null).trigger('change');
    //enable email & office
    $('#edit_officer_email').prop('disabled', false);
    $('#edit_officer_facility').prop('disabled', false);
});

function checkPersonEdit() {
    person = $('#edit_officer_name').val();
    person_name = person.split('/')[0] === undefined ? '' : person.split('/')[0];
    person_id = person.split('/')[1] === undefined ? 0 : person.split('/')[1];

    $.ajax({
        url: "/AcademicCollaboration/checkPerson",
        type: "POST",
        data: {
            people_id: person_id,
            people_name: person_name
        },
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                //clear content
                $('#edit_officer_email').val("");
                $('#edit_officer_facility').val(null).trigger('change');
                if (data.obj != null) {
                    //avaiable person data in DB
                    available_person = true; //using for Add||Edit
                    if (data.success) {
                        //auto fill data
                        let p = data.obj;
                        $('#edit_officer_email').val(p.email);
                        $('#edit_officer_facility').append(new Option(p.office_name, p.office_id, false, true)).trigger('change');
                        //disable email & office
                        $('#edit_officer_email').prop('disabled', true);
                        $('#edit_officer_facility').prop('disabled', true);
                    } else {
                        toastr.error(data.content);
                    }
                } else {
                    //new person case
                    available_person = false;
                    ///enable email and office select
                    $('#edit_officer_email').prop('disabled', false);
                    $('#edit_officer_facility').prop('disabled', false);
                }
            } else {
                toastr.error("Kiểm tra cán bộ giảng viên có lỗi xảy ra.");
            }
        },
        error: function () {
            toastr.error("Kiểm tra cán bộ giảng viên có lỗi xảy ra.");
        }
    });
}

$('#edit_officer_facility').select2({
    placeholder: 'Đơn vị - Cơ sở',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getOffices',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                office_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.office_id;
                obj.text = obj.office_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatOfficeInfo
});

///3.2.2.ĐƠN VỊ ĐÀO TẠO
$('#edit_officer_traning').select2({
    placeholder: 'Đơn vị đào tạo',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getPartners',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                partner_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.partner_name + '/' + obj.partner_id;
                obj.text = obj.partner_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatPartnerInfo
}).on("select2:select", function () { //after select partner
    //check partner
    checkPartnerEdit();
}).on("select2:unselecting", function () {
    //after clear tag
    //clear country select
    $('#edit_officer_nation').val(null).trigger('change');
    ///enable country
    $('#edit_officer_nation').prop('disabled', false);
});

function checkPartnerEdit() {
    //process partner_name
    partner = $('#edit_officer_traning').val();
    partner_name = partner.split('/')[0] === undefined ? '' : partner.split('/')[0];
    partner_id = partner.split('/')[1] === undefined ? 0 : partner.split('/')[1];

    $.ajax({
        url: "/AcademicCollaboration/checkPartner",
        type: "POST",
        data: {
            partner_name: partner_name,
            partner_id: partner_id
        },
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                //clear content
                $('#edit_officer_nation').val(null).trigger('change');
                if (data.obj != null) {
                    //available partner data in DB
                    available_partner = true; //using for Add||Edit
                    if (data.success) {
                        //auto fill data
                        let p = data.obj;
                        $('#edit_officer_nation').append(new Option(p.country_name, p.country_id, false, true)).trigger('change');
                        //disable country
                        $('#edit_officer_nation').prop('disabled', true);
                    } else {
                        toastr.error(data.content);
                    }
                } else {
                    //new partner case
                    available_partner = false;
                    ///enable country
                    $('#edit_officer_nation').prop('disabled', false);
                }
            } else {
                toastr.error("Kiểm tra đối tác có lỗi xảy ra.");
            }
        },
        error: function () {
            toastr.error("Kiểm tra đối tác có lỗi xảy ra.");
        }
    });
}

$('#edit_officer_nation').select2({
    placeholder: 'Quốc gia',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getCountries',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function (params) {
            return {
                country_name: params.term
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.country_id;
                obj.text = obj.country_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatCountryInfo
});

$('#edit_officer_coop_scope').select2({
    placeholder: 'Phạm vi hợp tác',
    allowClear: true,
    minimumResultsForSearch: -1, //hide search box
    ajax: {
        url: '/AcademicCollaboration/getCollabScopes',
        delay: 250,
        cache: true,
        dataType: 'json',
        data: function () {
            return {
                collab_abbreviation_name: 'JTP' //join training program
            };
        },
        processResults: function (data) {
            data.obj.map(function (obj) {
                obj.id = obj.scope_id;
                obj.text = obj.scope_name;
                return data.obj;
            });

            return {
                results: data.obj
            };
        }
    },
    templateResult: formatCollabScope
});

function formatCollabScope(scope) {
    return scope.scope_abbreviation + " - " + scope.scope_name;
}

///3.2.3.CHI TIẾT
$('#edit_officer_status').select2({
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
                status_type_specific: 2 //long-term
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

//3.2.GET CORESSPONDING DATA
$('#edit_officer').on('show.bs.modal', function (e) {
    //init save button
    var acad_collab_id = $(e.relatedTarget).data('id');
    $.ajax({
        url: "/AcademicCollaboration/getAcademicCollaboration",
        type: "GET",
        data: {
            direction: 1, /*going*/
            collab_type_id: 2 /*long-term*/,
            acad_collab_id: acad_collab_id
        },
        cache: false,
        dataType: "json",
        success: function (data) {
            if (data != null) {
                if (data.success) {
                    let acadCollab = data.obj;
                    //fill data to correspoding select, input
                    available_person = true; //person had data in db
                    $("#edit_officer_name").append(new Option(acadCollab.people_name, acadCollab.people_name + '/' + acadCollab.people_id, false, true)).trigger('change');
                    $("#edit_officer_email").val(acadCollab.email);
                    $("#edit_officer_email").prop("disabled", true);
                    $("#edit_officer_facility").append(new Option(acadCollab.office_name, acadCollab.office_id, false, true)).trigger('change');
                    $("#edit_officer_facility").prop("disabled", true);

                    available_partner = true; //partner had data in db
                    $("#edit_officer_traning").append(new Option(acadCollab.partner_name, acadCollab.partner_name + '/' +acadCollab.partner_id, false, true)).trigger('change');
                    $("#edit_officer_nation").append(new Option(acadCollab.country_name, acadCollab.country_id, false, true)).trigger('change');
                    $("#edit_officer_nation").prop("disabled", true);
                    $("#edit_officer_coop_scope").append(new Option(acadCollab.scope_name, acadCollab.scope_id, false, true)).trigger('change');

                    $("#edit_officer_status").append(new Option(acadCollab.collab_status_name, acadCollab.collab_status_id, false, true)).trigger('change');
                    $("#edit_officer_start_plan_date").val(moment(acadCollab.plan_study_start_date).format("DD/MM/YYYY"));
                    $("#edit_officer_end_plan_date").val(moment(acadCollab.plan_study_start_date).format("DD/MM/YYYY"));

                    $("#edit_officer_start_date").val(acadCollab.actual_study_start_date == null ? "" : moment(acadCollab.actual_study_start_date).format("DD/MM/YYYY"));
                    $("#edit_officer_end_date").val(acadCollab.actual_study_end_date == null ? "" : moment(acadCollab.actual_study_end_date).format("DD/MM/YYYY"));

                    console.log(acadCollab.file_name);
                    console.log(acadCollab.file_link);
                    console.log(acadCollab.file_drive_id);

                    file.file_id = acadCollab.file_id;
                    file.name = acadCollab.file_name;
                    file.link = acadCollab.file_link;
                    file.file_drive_id = acadCollab.file_drive_id;

                    collab_id = acadCollab.collab_id;

                    $("#edit_officer_support").prop("checked", acadCollab.is_supported);
                    $("#edit_officer_note").val(acadCollab.note);
                } else {
                    toastr.error(data.content);
                }
            }
        },
        error: function () {
            toastr.error("Có lỗi xảy ra.");
        }
    })
});

//3.3.EDIT SAVE BUTTON
var edit_officer_save = new LoaderBtn($('#edit_officer_save'));
var old_evidence = "";
$('#edit_officer_save').on('click', function () {
    //person
    person = $('#edit_officer_name').val();
    person_name = person.split('/')[0];
    person_id = person.split('/')[1]

    person_email = $('#edit_officer_email').val();
    person_profile_office_id = $('#edit_officer_facility').val();

    //partner
    partner = $('#edit_officer_traning').val();
    partner_name = partner.split('/')[0];
    partner_id = partner.split('/')[1];

    partner_country_id = $('#edit_officer_nation').val();

    collab_scope_id = $('#edit_officer_coop_scope').val();

    //detail
    status_id = $('#edit_officer_status').val();

    plan_start_date = $('#edit_officer_start_plan_date').val();
    plan_end_date = $('#edit_officer_end_plan_date').val();

    actual_start_date = $('#edit_officer_start_date').val();
    actual_end_date = $('#edit_officer_end_date').val();

    evidence = uppy2.getFiles();

    support = $('#edit_officer_support').prop('checked');
    note = $('#edit_officer_note').val();

    obj_person = {
        available_person: available_person,
        person_name: person_name,
        person_id: person_id,
        person_email: person_email,
        person_profile_office_id: person_profile_office_id
    }

    obj_partner = {
        available_partner: available_partner,
        partner_name: partner_name,
        partner_id: partner_id,
        partner_country_id: partner_country_id,
        collab_scope_id: collab_scope_id
    }

    obj_academic_collab = {
        collab_id: collab_id, //set corresponding collab_id when update an academic collaboration
        status_id: status_id,
        plan_start_date: formatDatePicker(plan_start_date),
        plan_end_date: formatDatePicker(plan_end_date),
        actual_start_date: formatDatePicker(actual_start_date),
        actual_end_date: formatDatePicker(actual_end_date),
        support: support,
        note: note
    }

    //check empty
    if (isEmpty(person) || isEmpty(partner) || isEmpty(collab_scope_id) || isEmpty(status_id) || isEmpty(plan_start_date) || isEmpty(plan_end_date)) {
        return toastr.error("Chưa chọn đủ trường thông tin bắt buộc.");
    } else {
        //validate datepicker from - to
        if (!datePickerFromToValidate(plan_start_date, plan_end_date) || !datePickerFromToValidate(actual_start_date, actual_end_date)) {
            return toastr.error("`TG đi học` không được vượt quá `TG kết thúc`.")
        } else {
            //start load
            edit_officer_save.startLoading();

            let formData = new FormData();
            let file_stringify = JSON.stringify(file);
            formData.append("old_evidence_stringify", file_stringify);
            formData.append("new_evidence", evidence.length == 0 ? null : evidence[0].data);
            formData.append("folder_name", person_name + " - " + partner_name);

            formData.append("direction_id", 1); //going case
            formData.append("collab_type_id", 2); //long-term

            let obj_person_stringify = JSON.stringify(obj_person);
            let obj_partner_stringify = JSON.stringify(obj_partner);
            let obj_academic_collab_stringify = JSON.stringify(obj_academic_collab);

            formData.append("obj_person_stringify", obj_person_stringify);
            formData.append("obj_partner_stringify", obj_partner_stringify);
            formData.append("obj_academic_collab_stringify", obj_academic_collab_stringify);

            //Save actually
            $.ajax({
                url: "/AcademicCollaboration/updateAcademicCollaboration",
                type: "POST",
                data: formData,
                cache: false,
                dataType: "json",
                processData: false,
                contentType: false,
                success: function (data) {
                    if (data.success) {
                        toastr.success(data.content);
                        collab_going_table.ajax.reload();
                        edit_officer_save.stopLoading();
                        $("#edit_officer_close").click();
                        clearContentEditModal();
                    } else {
                        toastr.warning(data.content);
                        edit_officer_save.stopLoading();
                    }
                },
                error: function () {
                    toastr.error("Có lỗi xảy ra khi upload file.");
                    edit_officer_save.stopLoading();
                }
            });
            for (var pair of formData.entries()) {
                console.log(pair[0] + ', ' + pair[1]);
            }
        }
    }
});

function clearContentEditModal() {
    //enable input and select
    $('#add_officer_email').prop('disabled', false);
    $("#add_officer_facility").prop('disabled', false);
    $('#add_officer_nation').prop('disabled', false);
    $('#add_officer_coop_scope').prop('disabled', false);

    //clear data
    ///THÔNG TIN CÁ NHÂN
    $('#add_officer_name').val(null).trigger('change');
    $('#add_officer_email').val('');
    $('#add_officer_facility').val(null).trigger('change');
    ///ĐƠN VỊ ĐÀO TẠO
    $('#add_officer_traning').val(null).trigger('change');
    $('#add_officer_nation').val(null).trigger('change');
    $('#add_officer_coop_scope').val(null).trigger('change');
    ///CHI TIẾT
    $('#add_officer_status').val(null).trigger('change');
    $('#add_officer_start_plan_date').val('');
    $('#add_officer_end_plan_date').val('');

    //clear upload file
    $('.uppy-list').html('');
    uppy2.removeFile(uppy2.getFiles().length == 0 ? '' : uppy2.getFiles()[0].id);

    $('#add_officer_start_date').val('');
    $('#add_officer_end_date').val('');
    $('#add_officer_support').prop('checked', false);
    $('#add_officer_note').val('');
}

