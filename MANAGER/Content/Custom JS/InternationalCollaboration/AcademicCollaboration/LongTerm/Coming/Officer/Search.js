// 1.SEARCH
$('#search_nation_tab_1_table_2').select2({
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

//function formatCountryInfo(country) {
//    return country.country_name;
//}

$('#search_year_tab_1_table_2').select2({
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
                $("#search_year_tab_1_table_2").val(null).trigger('change');
                $("#search_year_tab_1_table_2").append(newOption).trigger('change');
            }
        }
    },
    error: function () {
        toastr.error("Lấy dữ liệu về năm xảy ra lỗi.");
    }
});


$('#search_training_facility_tab_1_table_2').select2({
    placeholder: 'Đơn vị đào tạo',
    allowClear: true,
    tags: true,
    ajax: {
        url: '/AcademicCollaboration/getPartnersSearching',
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

//function formatPartnerInfo(partner) {
//    return partner.partner_name;
//}

$('#search_working_facility_tab_1_table_2').select2({
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

//function formatOfficeInfo(office) {
//    return office.office_name;
//}