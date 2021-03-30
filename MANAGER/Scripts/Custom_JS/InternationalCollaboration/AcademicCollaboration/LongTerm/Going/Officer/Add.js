///////////////////////////////////////////TABLE 1: LONG-TERM GOING//////////////////////////////////////////
//2.ADD MODAL
var uppy1; //init for uppy.js
//init loader save button
var going_add_officer_save = new LoaderBtn($("#going_add_officer_save"));
//2.1.SHOW BUTTON
///2.1.1.THÔNG TIN CÁ NHÂN
$('#going_add_officer_name').select2({
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
    $('#going_add_officer_email').val("");
    $('#going_add_officer_facility').val(null).trigger('change');
    //enable email & office
    $('#going_add_officer_email').prop('disabled', false);
    $('#going_add_officer_facility').prop('disabled', false);
});

function formatPersonInfo(person) {
    if (person.id) {
        let mssv_msnv = person.mssv_msnv;
        let name = person.name;
        if (mssv_msnv === undefined && name === undefined) {
            return "Dữ liệu cá nhân mới."
        }
        else {
            if (mssv_msnv == "") {
                return name;
            } else {
                return mssv_msnv + " - " + name;
            }
        }
    }
}

function checkPersonAdd() {
    person = $('#going_add_officer_name').val();
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
                $('#going_add_officer_email').val("");
                $('#going_add_officer_facility').val(null).trigger('change');
                if (data.obj != null) {
                    //avaiable person data in DB
                    available_person = true; //using for Add||Edit
                    if (data.success) {
                        //auto fill data
                        let p = data.obj;
                        $('#going_add_officer_email').val(p.email);
                        $('#going_add_officer_facility').append(new Option(p.office_name, p.office_id, false, true)).trigger('change');
                        //disable email & office
                        $('#going_add_officer_email').prop('disabled', true);
                        $('#going_add_officer_facility').prop('disabled', true);
                    } else {
                        toastr.error(data.content);
                    }
                } else {
                    //new person case
                    available_person = false;
                    ///enable email and office select
                    $('#going_add_officer_email').prop('disabled', false);
                    $('#going_add_officer_facility').prop('disabled', false);
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

$('#going_add_officer_facility').select2({
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
$('#going_add_officer_traning').select2({
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
    $('#going_add_officer_nation').val(null).trigger('change');
    ///enable country
    $('#going_add_officer_nation').prop('disabled', false);
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
    partner = $('#going_add_officer_traning').val();
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
                $('#going_add_officer_nation').val(null).trigger('change');
                if (data.obj != null) {
                    //available partner data in DB
                    available_partner = true; //using for Add||Edit
                    if (data.success) {
                        //auto fill data
                        let p = data.obj;
                        $('#going_add_officer_nation').append(new Option(p.country_name, p.country_id, false, true)).trigger('change');
                        //disable country
                        $('#going_add_officer_nation').prop('disabled', true);
                    } else {
                        toastr.error(data.content);
                    }
                } else {
                    //new partner case
                    available_partner = false;
                    ///enable country
                    $('#going_add_officer_nation').prop('disabled', false);
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

$('#going_add_officer_nation').select2({
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

$('#going_add_officer_coop_scope').select2({
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
$('#going_add_officer_status').select2({
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

var available_person;
var available_partner;
//2.2. SAVE BUTTON
$('#going_add_officer_save').on('click', function () {
    //person
    let person = $('#going_add_officer_name').val();

    let person_email = $('#going_add_officer_email').val();
    let person_profile_office_id = $('#going_add_officer_facility').val();

    //partner
    let partner = $('#going_add_officer_traning').val();

    let partner_country_id = $('#going_add_officer_nation').val();

    let collab_scope_id = $('#going_add_officer_coop_scope').val();

    //detail
    let status_id = $('#going_add_officer_status').val();

    let plan_start_date = $('#going_add_officer_start_plan_date').val();
    let plan_end_date = $('#going_add_officer_end_plan_date').val();

    let actual_start_date = $('#going_add_officer_start_date').val();
    let actual_end_date = $('#going_add_officer_end_date').val();

    let evidence = uppy1.getFiles();

    let support = $('#going_add_officer_support').prop('checked');
    let note = $('#going_add_officer_note').val();

    //check empty
    if (isEmpty(person) || isEmpty(partner) || isEmpty(collab_scope_id) || isEmpty(status_id) || isEmpty(plan_start_date) || isEmpty(plan_end_date)) {
        return toastr.error("Chưa chọn đủ trường thông tin bắt buộc.");
    } else {
        let person_name = person.split('/')[0];
        let person_id = person.split('/')[1];
        let partner_name = partner.split('/')[0];
        let partner_id = partner.split('/')[1];

        let obj_person = {
            available_person: available_person,
            person_name: person_name,
            person_id: person_id,
            person_email: person_email,
            person_profile_office_id: person_profile_office_id
        }

        let obj_partner = {
            available_partner: available_partner,
            partner_name: partner_name,
            partner_id: partner_id,
            partner_country_id: partner_country_id,
            collab_scope_id: collab_scope_id
        }

        let obj_academic_collab = {
            collab_id: 0, //set 0 when add new academic collaboration
            status_id: status_id,
            plan_start_date: formatDatePicker(plan_start_date),
            plan_end_date: formatDatePicker(plan_end_date),
            actual_start_date: formatDatePicker(actual_start_date),
            actual_end_date: formatDatePicker(actual_end_date),
            support: support,
            note: note
        }

        //validate datepicker from - to
        if (!datePickerFromToValidate(plan_start_date, plan_end_date) || !datePickerFromToValidate(actual_start_date, actual_end_date)) {
            return toastr.error("`TG đi học` không được vượt quá `TG kết thúc`.")
        } else {
            //start load
            going_add_officer_save.startLoading();

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
                        going_add_officer_save.stopLoading();
                        $("#going_add_officer_close").click();
                        clearContentAddModal();
                    } else {
                        toastr.warning(data.content);
                        going_add_officer_save.stopLoading();
                    }
                },
                error: function () {
                    toastr.error("Có lỗi xảy ra.");
                    going_add_officer_save.stopLoading();
                }
            });
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
    $('#going_add_officer_email').prop('disabled', false);
    $("#going_add_officer_facility").prop('disabled', false);
    $('#going_add_officer_nation').prop('disabled', false);
    $('#going_add_officer_coop_scope').prop('disabled', false);

    //clear data
    ///THÔNG TIN CÁ NHÂN
    $('#going_add_officer_name').val(null).trigger('change');
    $('#going_add_officer_email').val('');
    $('#going_add_officer_facility').val(null).trigger('change');
    ///ĐƠN VỊ ĐÀO TẠO
    $('#going_add_officer_traning').val(null).trigger('change');
    $('#going_add_officer_nation').val(null).trigger('change');
    $('#going_add_officer_coop_scope').val(null).trigger('change');
    ///CHI TIẾT
    $('#going_add_officer_status').val(null).trigger('change');
    $('#going_add_officer_start_plan_date').val('');
    $('#going_add_officer_end_plan_date').val('');

    //clear upload file
    $('.uppy-list').html('');
    uppy1.removeFile(uppy1.getFiles().length == 0 ? '' : uppy1.getFiles()[0].id);

    $('#going_add_officer_start_date').val('');
    $('#going_add_officer_end_date').val('');
    $('#going_add_officer_support').prop('checked', false);
    $('#going_add_officer_note').val('');
}