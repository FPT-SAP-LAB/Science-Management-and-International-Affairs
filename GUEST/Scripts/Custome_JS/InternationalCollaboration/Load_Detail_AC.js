

class Load_detail {
    load_procedure_detail(id, type_procedure) {
        var url = '/AcademicCollaboration/Procedure_Detail?id=' + id + '&type_procedure=' + type_procedure
        window.location.href = url
    }

    load_program_detail(id, type_program) {
        var url = '/AcademicCollaboration/Program_Detail?id=' + id + '&type_program=' + type_program
        window.location.href = url
    }
}
var load_detail = new Load_detail()