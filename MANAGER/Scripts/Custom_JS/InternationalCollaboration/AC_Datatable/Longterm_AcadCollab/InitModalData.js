//MODAL INFORMATION 
//// person infor
let person; //*
let person_name;
let person_id;
let available_person;

//// partner infor
let partner; //*
let partner_name;
let partner_id;
let person_email;
let person_profile_office_id;
let partner_country_id;
let collab_scope_id;
let available_partner;

let status_id; //*
let plan_start_date; //*
let plan_end_date; //*
let actual_start_date;
let actual_end_date;
let support;
let note;
let collab_id;

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
    collab_id: collab_id,
    status_id: status_id,
    plan_start_date: formatDatePicker(plan_start_date),
    plan_end_date: formatDatePicker(plan_end_date),
    actual_start_date: formatDatePicker(actual_start_date),
    actual_end_date: formatDatePicker(actual_end_date),
    support: support,
    note: note
}

let file = {
    file_id: 0,
    name: "",
    link: "",
    file_drive_id: ""
}