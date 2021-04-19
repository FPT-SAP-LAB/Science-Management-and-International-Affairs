class LoaderBtn {
    constructor(el) {
        this.el = el;
    }
    startLoading() {
        $(this.el).addClass("spinner spinner-white spinner-right")
        $(this.el).attr('disabled', '')
    }
    stopLoading() {
        $(this.el).removeClass("spinner spinner-white spinner-right")
        $(this.el).removeAttr('disabled')
    }
}
function AddComma(Num) { //function to add commas to textboxes
    Num += '';
    Num = Num.replace('.', ''); Num = Num.replace('.', ''); Num = Num.replace('.', '');
    Num = Num.replace('.', ''); Num = Num.replace('.', ''); Num = Num.replace('.', '');
    x = Num.split(',');
    x1 = x[0];
    x2 = x.length > 1 ? ',' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1))
        x1 = x1.replace(rgx, '$1' + '.' + '$2');
    return x1 + x2;
};
$(".number-comma").each(function () {
    $(this).attr('data-value', $(this).val())
    $(this).text(AddComma($(this).val()))
})
$(".datetimepicker-input").keydown(function (e) {
    e.preventDefault();
})
function validateNonEmptyField(field) {
    isValid = true;
    for (var i in field) {
        if ($(field[i]).val() === null || $(field[i]).val().trim() == "") {
            $(field[i]).addClass("is-invalid");
            isValid = false;
        }
    }
    return isValid;
}
$("body").on("focusout", ".is-invalid", function () {
    $(this).removeClass("is-invalid")
})