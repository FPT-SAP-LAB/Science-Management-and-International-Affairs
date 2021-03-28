//MODAL INFORMATION 
//// person infor
var person; //*
var person_name;
var person_id;
var available_person;

//// partner infor
var partner; //*
var partner_name;
var partner_id;
var person_email;
var person_profile_office_id;
var partner_country_id;
var collab_scope_id;
var available_partner;

var status_id; //*
var plan_start_date; //*
var plan_end_date; //*
var actual_start_date;
var actual_end_date;
var support;
var note;
var collab_id;

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