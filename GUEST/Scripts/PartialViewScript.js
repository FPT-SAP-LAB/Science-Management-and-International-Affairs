$("#ckfe").change(function () {
    var val = $("#ckfe").val();
    if (val == "Khác") {
        $(".input_fe").each(function () {
            $(this).prop("disabled", true);
        });
    } else {
        $(".input_fe").each(function () {
            $(this).prop("disabled", false);
        });
    }
});

$('#ckfe').select2({
    allowClear: true
});
$('#add_author_workplace').select2({
    allowClear: true,
    placeholder: "Điền mã số"
});
$('#add_author_title').select2({
    allowClear: true,
    placeholder: "Chọn chức danh"
});
$('#add_author_contractType').select2({
    allowClear: true,
    placeholder: "Chọn loại hợp đồng"
});

$(function () {
    $(".tacgia").hide()
})
donvife = {
    1: "FPTU",
    2: "FPT Swinburne",
    3: "Fpoly",
    4: "Khác"
}

//var filename = [];

class AuthorInfoView {
    constructor(add_author_workplace, add_author_msnv, add_author_name, add_author_title, add_author_contractType, add_author_cmnd, add_author_tax, add_author_bank, add_author_accno, add_author_reward, add_author_note, add_author_email, add_author_isReseacher, id, add_author_link_file) {
        if (add_author_msnv != null) this.add_author_msnv = add_author_msnv;
        else this.add_author_msnv = "";
        this.add_author_email = add_author_email;
        if (add_author_workplace != null) this.add_author_workplace = add_author_workplace;
        else this.add_author_workplace = "Khác";
        this.add_author_name = add_author_name;
        if (add_author_title != null) this.add_author_title = add_author_title;
        else this.add_author_title = "";
        if (add_author_contractType != null) this.add_author_contractType = add_author_contractType;
        else this.add_author_contractType = "";
        if (add_author_cmnd != null) this.add_author_cmnd = add_author_cmnd;
        else this.add_author_cmnd = "";
        if (add_author_tax != null) this.add_author_tax = add_author_tax;
        else this.add_author_tax = "";
        if (add_author_bank != null) this.add_author_bank = add_author_bank;
        else this.add_author_bank = "";
        if (add_author_accno != null) this.add_author_accno = add_author_accno;
        else this.add_author_accno = "";
        if (add_author_reward != null) this.add_author_reward = add_author_reward;
        else this.add_author_reward = "";
        if (add_author_note != null) this.add_author_note = add_author_note;
        else this.add_author_note = "";
        this.add_author_info_id = id;
        if (add_author_msnv != null) this.title = this.add_author_msnv + ` - ` + this.add_author_name;
        else this.title = this.add_author_name;
        if (add_author_isReseacher != null) this.add_author_isReseacher = add_author_isReseacher;
        else this.add_author_isReseacher = false;
        if (this.add_author_isReseacher == true) this.title_2 = ", Nghiên cứu viên";
        else this.title_2 = "";
        if (add_author_link_file != null) this.link_file = add_author_link_file;
        else this.link_file = "";
    }
    getHTML() {
        return `
                 <div class='col-lg-6' id='` + this.add_author_info_id + `'>
                                <!--begin::Card-->
                                <div class='card card-custom gutter-b'>
                                    <div class='card-header'>
                                        <div class='card-title'>
                                            <h3 class='card-label'>` + this.title + `</h3>
                                        </div>
                                        <div class='card-toolbar'>
                                            <a data-id='` + this.add_author_info_id + `' class='edit-author btn btn-icon btn-sm btn-hover-light-danger edit'>
                                                <i class='far fa-edit'></i>
                                            </a>
                                            <a data-id='` + this.add_author_info_id + `' class='del-author btn btn-icon btn-sm btn-hover-light-danger edit'>
                                                <i class='la la-trash'></i>
                                            </a>
                                        </div>
                                    </div>
                                    <div class='card-body author-info-card'>
                                        <div class='row'>
                                            <div class='col-lg-6 col-sm-12'>
                                                <div class='mb-7'>
                                                    <div class='d-flex justify-content-between align-items-center'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>Khu Vực:</span>
                                                        <span class='text-muted font-weight-bold'>` + this.add_author_workplace + `</span>
                                                    </div>
                                                    <div class='d-flex justify-content-between align-items-cente my-1'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>Chức danh:</span>
                                                        <a href='#' class='text-muted text-hover-primary'>` + this.add_author_title + this.title_2 + `</a>
                                                    </div>
                                                  
                                                    <div class='d-flex justify-content-between align-items-cente my-1'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>Mã số thuế: </span>
                                                        <a href='#' class='text-muted text-hover-primary'>` + this.add_author_tax + `</a>
                                                    </div>
                                                    <div class='d-flex justify-content-between align-items-cente my-1'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>CMND số:</span>
                                                        <a href='#' class='text-muted text-hover-primary'>` + this.add_author_cmnd + `</a>
                                                    </div>
                                                    <div class='d-flex justify-content-between align-items-cente my-1'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>CMND file:</span>
                                                        <a href='`+ this.link_file+`' class='text-muted text-hover-primary'>` + (this.link_file.length > 30 ? (this.link_file.substring(0, 22) + "...") : this.link_file )+ `</a>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class='col-lg-6 col-sm-12'>
                                                <div class='mb-7'>
                                                    <div class='d-flex justify-content-between align-items-center'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>Email:</span>
                                                        <span class='text-muted font-weight-bold'>` + this.add_author_email + `</span>
                                                    </div>
                                                    <div class='d-flex justify-content-between align-items-cente my-1'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>Ngân hàng: </span>
                                                        <a href='#' class='text-muted text-hover-primary'>` + this.add_author_bank + `</a>
                                                    </div>
                                                    <div class='d-flex justify-content-between align-items-cente my-1'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>Số tài khoản: </span>
                                                        <a href='#' class='text-muted text-hover-primary'>` + this.add_author_accno + `</a>
                                                    </div>
                                                    <div class='d-flex justify-content-between align-items-cente my-1'>
                                                        <span class='text-dark-75 font-weight-bolder mr-2'>Tiền thưởng: </span>
                                                        <a href='#' class='text-muted text-hover-primary'>` + this.add_author_reward + `</a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!--end::Card-->
                            </div>
                            `
    }
}
//////////////////////////////////////////////////////////////////
$("#add_author_save").click(function () {
    for (var i = 0; i < people.length; i++) {
        if (people[i].email == $("#add_author_mail").val()) {
            toastr.error("Đã có tác giả này");
            return false;
        }
    }
    ckfe = $("#ckfe").val()
    add_author_workplace = $("#ckfe").val()
    add_author_msnv = $("#add_author_msnv").val()
    add_author_name = $("#add_author_name").val()
    add_author_title = $("#add_author_title option:selected").text()
    add_author_contractType = $("#add_author_contractType option:selected").text()
    add_author_cmnd = $("#add_author_cmnd").val()
    add_author_cmnd_link = $("#add_author_cmnd_link").val()
    add_author_tax = $("#add_author_tax").val()
    add_author_bank = $("#add_author_bank").val()
    add_author_accno = $("#add_author_accno").val()
    add_author_reward = $("#add_author_reward").val()
    add_author_note = $("#add_author_note").val()
    add_author_email = $("#add_author_mail").val()
    add_author_isReseacher = $('#add_author_isReseacher').is(':checked')
    id = new Date().getTime()
    au = new AuthorInfoView(add_author_workplace, add_author_msnv, add_author_name,
        add_author_title, add_author_contractType, add_author_cmnd, add_author_tax,
        add_author_bank, add_author_accno, add_author_reward, add_author_note, add_author_email, add_author_isReseacher, id, add_author_cmnd_link)
    $("#authors-info-container").append(au.getHTML());
    var AddAuthor = {
        name: add_author_name,
        email: add_author_email,
        bank_number: add_author_accno,
        tax_code: add_author_tax,
        bank_branch: add_author_bank,
        identification_number: add_author_cmnd,
        mssv_msnv: add_author_msnv,
        office_id: $("#ckfe option:selected").attr("name"),
        contract_id: $("#add_author_contractType").val(),
        title_id: $("#add_author_title").val(),
        people_id: $("#add_author_msnv").attr("name"),
        temp_id: id,
        office_abbreviation: $("#ckfe option:selected").val(),
        is_reseacher: add_author_isReseacher,
        identification_file_link: $("#add_author_cmnd_link").val(),
    }
    people.push(AddAuthor);
    addOption();
    var inputs = $(".inputAuthor");
    for (var i = 0; i < inputs.length; i++) {
        $(inputs[i]).val("");
    }
    $('#add_author_title').val(null).trigger('change');
    $('#add_author_contractType').val(null).trigger('change');
    $('#add_author_msnv').val(null).trigger('change');
    $("#add_author_isReseacher").prop("checked", false);
});
$("#add_author_save_edit").click(function () {
    people[temp_index_edit].office_abbreviation = $("#ckfe_edit").val();
    people[temp_index_edit].office_id = $("#ckfe_edit option:selected").attr("name");
    people[temp_index_edit].mssv_msnv = $("#add_author_msnv_edit").val();
    people[temp_index_edit].name = $("#add_author_name_edit").val();
    people[temp_index_edit].title_id = $("#add_author_title_edit").val();
    people[temp_index_edit].contract_id = $("#add_author_contractType_edit").val();
    people[temp_index_edit].identification_number = $("#add_author_cmnd_edit").val();
    people[temp_index_edit].identification_file_link = $("#add_author_cmnd_link_edit").val();
    people[temp_index_edit].tax_code = $("#add_author_tax_edit").val();
    people[temp_index_edit].bank_branch = $("#add_author_bank_edit").val();
    people[temp_index_edit].bank_number = $("#add_author_accno_edit").val();
    people[temp_index_edit].email = $("#add_author_mail_edit").val();
    people[temp_index_edit].is_reseacher = $('#add_author_isReseacher_edit').is(':checked');
    people[temp_index_edit].money_string = $("#add_author_reward_edit").val();

    $("#" + people[temp_index_edit].temp_id).remove();
    var x = people[temp_index_edit].money_string;
    people[temp_index_edit].money_string = x.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");

    au = new AuthorInfoView(people[temp_index_edit].office_abbreviation, people[temp_index_edit].mssv_msnv, people[temp_index_edit].name,
        $("#add_author_title_edit option:selected").text(), $("#add_author_contractType_edit option:selected").text(),
        people[temp_index_edit].identification_number, people[temp_index_edit].tax_code,
        people[temp_index_edit].bank_branch, people[temp_index_edit].bank_number,
        people[temp_index_edit].money_string, '', people[temp_index_edit].email, people[temp_index_edit].is_reseacher, people[temp_index_edit].temp_id, people[temp_index_edit].identification_file_link);
    $("#authors-info-container").append(au.getHTML());
    addOption();
});
$("#authors-info-container").on('click', '.edit-author', function () {
    let id = $(this).data("id");
    for (var i = 0; i < people.length; i++) {
        if (people[i].temp_id == id) {
            if (people[i].office_abbreviation == null) $("#ckfe_edit").val("Khác");
            else $("#ckfe_edit").val(people[i].office_abbreviation);
            $("#ckfe_edit").trigger('change');

            if ($('#add_author_msnv_edit').find("option[value='" + people[i].mssv_msnv + "']").length) {
                $('#add_author_msnv_edit').val(people[i].mssv_msnv).trigger('change');
            } else {
                // Create a DOM Option and pre-select by default
                var newOption = new Option(people[i].mssv_msnv, people[i].mssv_msnv, true, true);
                // Append it to the select
                $('#add_author_msnv_edit').append(newOption).trigger('change');
            }

            //$("#add_author_msnv_edit").val(people[i].mssv_msnv);
            //$("#add_author_msnv_edit").trigger('change');

            $("#add_author_name_edit").val(people[i].name);
            $("#add_author_title_edit").val(people[i].title_id);
            $("#add_author_title_edit").trigger('change');
            $("#add_author_contractType_edit").val(people[i].contract_id);
            $("#add_author_contractType_edit").trigger('change');
            $("#add_author_cmnd_edit").val(people[i].identification_number);
            $("#add_author_cmnd_link_edit").val(people[i].identification_file_link);
            $("#add_author_tax_edit").val(people[i].tax_code);
            $("#add_author_bank_edit").val(people[i].bank_branch);
            $("#add_author_accno_edit").val(people[i].bank_number);
            $("#add_author_mail_edit").val(people[i].email);

            $("#add_author_reward_edit").val(people[i].money_string);

            $("#add_author_isReseacher_edit").prop("checked", people[i].is_reseacher);

            //console.log(people[i].money_string);
            if ($("#totalreward").val() != "" && people[i].money_string == "0") $("#add_author_reward_edit").prop("disabled", false);
            else $("#add_author_reward_edit").prop('disabled', true);

            var sum = 0;
            var total = $("#totalreward").val();
            if (total != null) total = total.split(",").join("");

            for (var j = 0; j < people.length; j++) {
                var data = people[j].money_string;
                if (people[j].money_string == null) data = "0";
                var temp = data.split(",").join("");
                sum = parseInt(sum) + parseInt(temp);
            }
            if (sum != parseInt(total)) $("#add_author_reward_edit").prop("disabled", false);
            else $("#add_author_reward_edit").prop('disabled', true);

            $("#edit_author_btn").click();
            temp_index_edit = i;
            break;
        }
    }
});
$("#authors-info-container").on('click', '.del-author', function () {
    let id = $(this).data("id")
    Swal.fire({
        title: "Xoá tác giả này?",
        icon: "warning",
        showCancelButton: true,
        confirmButtonText: "Xác nhận",
        cancelButtonText: "Huỷ",
        reverseButtons: true
    }).then(function (result) {
        if (result.value) {
            $("#" + id).remove();
            for (var i = 0; i < people.length; i++) {
                //console.log(i);
                if (people[i].temp_id == id) {
                    people.splice(i, 1);
                }
            }
            addOption();
        }
    });
});

function addOption() {
    $("#daidien").empty();
    for (var i = 0; i < people.length; i++) {
        if (people[i].mssv_msnv != null) $("#daidien").append(new Option(people[i].name, people[i].mssv_msnv));
    }
}