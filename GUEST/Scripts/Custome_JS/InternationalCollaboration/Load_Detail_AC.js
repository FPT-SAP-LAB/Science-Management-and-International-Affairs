

class Load_detail {
    load_somthing() { }

    load_procedure_detail(id) {
        var url = '/AcademicCollaboration/Procedure_Detail?id=' + id
        window.location.href = url
    }

    load_program_detail(id) {
        var url = '/AcademicCollaboration/Program_Detail?id=' + id
        window.location.href = url
    }
}
var load_detail = new Load_detail()
load_detail.load_somthing() 