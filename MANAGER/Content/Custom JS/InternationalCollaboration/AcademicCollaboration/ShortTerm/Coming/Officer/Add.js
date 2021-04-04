///////////////////////////////////////////TABLE 2: LONG-TERM COMING//////////////////////////////////////////
//2.ADD MODAL
var uppy4; //init for uppy.js
//init loader save button
var coming_add_officer_save = new LoaderBtn($("#coming_add_officer_save"));
//2.1.SHOW BUTTON
///2.1.1.THÔNG TIN CÁ NHÂN
$('#coming_add_officer_name').select2({
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
    checkPersonComingAdd();
}).on("select2:unselecting", function () {
    //after clear tag
    //clear email & office
    $('#coming_add_officer_email').val("");
    $('#coming_add_officer_facility').val(null).trigger('change');
    //enable email & office
    $('#coming_add_officer_email').prop('disabled', false);
    $('#coming_add_officer_facility').prop('disabled', false);
});

function checkPersonComingAdd() {
    person = $('#coming_add_officer_name').val();
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
                $('#coming_add_officer_email').val("");
                $('#coming_add_officer_facility').val(null).trigger('change');
                if (data.obj != null) {
                    //avaiable person data in DB
                    available_person = true; //using for Add||Edit
                    if (data.success) {
                        //auto fill data
                        let p = data.obj;
                        $('#coming_add_officer_email').val(p.email);
                        if (!(isEmptyOrNullOrUndefined(p.office_id))) {
                            $('#coming_add_officer_facility').append(new Option(p.office_name, p.office_id, false, true)).trigger('change');
                        }
                        //disable email & office
                        $('#coming_add_officer_email').prop('disabled', true);
                        $('#coming_add_officer_facility').prop('disabled', true);
                    } else {
                        toastr.error(data.content);
                    }
                } else {
                    //new person case
                    available_person = false;
                    ///enable email and office select
                    $('#coming_add_officer_email').prop('disabled', false);
                    $('#coming_add_officer_facility').prop('disabled', false);
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

$('#coming_add_officer_facility').select2({
    placeholder: 'Đơn vị đào tạo',
    allowClear: true,
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

///2.1.2.ĐƠN VỊ CÔNG TÁC
$('#coming_add_officer_traning').select2({
    placeholder: 'Đơn vị công tác',
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
    checkPartnerComingAdd();
}).on("select2:unselecting", function () {
    //after clear tag
    //clear country select
    $('#coming_add_officer_nation').val(null).trigger('change');
    ///enable country
    $('#coming_add_officer_nation').prop('disabled', false);
});

function checkPartnerComingAdd() {
    //process partner_name
    partner = $('#coming_add_officer_traning').val();
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
                $('#coming_add_officer_nation').val(null).trigger('change');
                if (data.obj != null) {
                    //available partner data in DB
                    available_partner = true; //using for Add||Edit
                    if (data.success) {
                        //auto fill data
                        let p = data.obj;
                        $('#coming_add_officer_nation').append(new Option(p.country_name, p.country_id, false, true)).trigger('change');
                        //disable country
                        $('#coming_add_officer_nation').prop('disabled', true);
                    } else {
                        toastr.error(data.content);
                    }
                } else {
                    //new partner case
                    available_partner = false;
                    ///enable country
                    $('#coming_add_officer_nation').prop('disabled', false);
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

$('#coming_add_officer_nation').select2({
    placeholder: 'Quốc gia',
    allowClear: true,
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

$('#coming_add_officer_coop_scope').select2({
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
                collab_abbreviation_name: 'FE' //faculty exchange
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

///2.1.3.CHI TIẾT
$('#coming_add_officer_status').select2({
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

//2.2. SAVE BUTTON
$('#coming_add_officer_save').on('click', function () {
    //person
    let person = $('#coming_add_officer_name').val();

    let person_email = $('#coming_add_officer_email').val();
    let person_profile_office_id = $('#coming_add_officer_facility').val();

    //partner
    let partner = $('#coming_add_officer_traning').val();

    let partner_country_id = $('#coming_add_officer_nation').val();

    let collab_scope_id = $('#coming_add_officer_coop_scope').val();

    //detail
    let status_id = $('#coming_add_officer_status').val();

    let plan_start_date = $('#coming_add_officer_start_plan_date').val();
    let plan_end_date = $('#coming_add_officer_end_plan_date').val();

    let actual_start_date = $('#coming_add_officer_start_date').val();
    let actual_end_date = $('#coming_add_officer_end_date').val();

    let evidence = uppy4.getFiles();

    let note = $('#coming_add_officer_note').val();

    //check empty
    if (isEmptyOrNullOrUndefined(person) || isEmptyOrNullOrUndefined(person_email)
        || isEmptyOrNullOrUndefined(partner) || isEmptyOrNullOrUndefined(partner_country_id)
        || isEmptyOrNullOrUndefined(collab_scope_id) || isEmptyOrNullOrUndefined(status_id)
        || isEmptyOrNullOrUndefined(plan_start_date) || isEmptyOrNullOrUndefined(plan_end_date)) {
        return toastr.error("Chưa chọn đủ trường thông tin bắt buộc.");
    } else {
        let person_name = person.split('/')[0];
        let person_id = person.split('/')[1];
        let partner_name = partner.split('/')[0];
        let partner_id = partner.split('/')[1];

        let obj_person = objPerson(available_person, person_name, person_id, person_email, person_profile_office_id);
        console.log(obj_person);

        let obj_partner = objPartner(available_partner, partner_name, partner_id, partner_country_id, collab_scope_id);

        let obj_academic_collab = objAcadCollab(0, status_id, plan_start_date, plan_end_date, actual_start_date, actual_end_date, null, note); 

        //validate datepicker from - to
        if (!datePickerFromToValidate(plan_start_date, plan_end_date) || !datePickerFromToValidate(actual_start_date, actual_end_date)) {
            return toastr.error("`TG đi học` không được vượt quá `TG kết thúc`.")
        } else {
            //start load
            coming_add_officer_save.startLoading();

            let formData = new FormData();
            formData.append("evidence", evidence.length == 0 ? null : evidence[0].data);
            formData.append("folder_name", person_name + " - " + partner_name);

            formData.append("direction_id", 2); //coming case
            formData.append("collab_type_id", 1); //short-term

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
                        exchange_coming_table.ajax.reload();
                        coming_add_officer_save.stopLoading();
                        $("#coming_add_officer_close").click();
                        clearContentComingAddModal();
                    } else {
                        toastr.warning(data.content);
                        coming_add_officer_save.stopLoading();
                    }
                },
                error: function () {
                    toastr.error("Có lỗi xảy ra.");
                    coming_add_officer_save.stopLoading();
                }
            });
        }
    }
});

function clearContentComingAddModal() {
    //enable input and select
    $('#coming_add_officer_email').prop('disabled', false);
    $("#coming_add_officer_facility").prop('disabled', false);
    $('#coming_add_officer_nation').prop('disabled', false);
    $('#coming_add_officer_coop_scope').prop('disabled', false);

    //clear data
    ///THÔNG TIN CÁ NHÂN
    $('#coming_add_officer_name').val(null).trigger('change');
    $('#coming_add_officer_email').val('');
    $('#coming_add_officer_facility').val(null).trigger('change');
    ///ĐƠN VỊ ĐÀO TẠO
    $('#coming_add_officer_traning').val(null).trigger('change');
    $('#coming_add_officer_nation').val(null).trigger('change');
    $('#coming_add_officer_coop_scope').val(null).trigger('change');
    ///CHI TIẾT
    $('#coming_add_officer_status').val(null).trigger('change');
    $('#coming_add_officer_start_plan_date').val('');
    $('#coming_add_officer_end_plan_date').val('');

    //clear upload file
    $('.uppy-list').html('');
    uppy4.removeFile(uppy4.getFiles().length == 0 ? '' : uppy4.getFiles()[0].id);

    $('#coming_add_officer_start_date').val('');
    $('#coming_add_officer_end_date').val('');
    $('#coming_add_officer_note').val('');
}