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

function generateArrayOfYears() {
  var max = new Date().getFullYear()
  var min = max -80
  var years = []

  for (var i = max; i >= min; i--) {
    years.push(i)
  }
  return years
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
$(".menu-link").each(function () {
    if (typeof $(this).attr('href') !== 'undefined'&&$(this).attr('href').trim() == window.location.pathname) {
        $(this).parent().addClass('menu-item-active')
        $(this).parent().parent().parent().parent().addClass('menu-item-open')
        $(this).parent().parent().parent().parent().addClass(' menu-item-here')
    }
});

$(".datetimepicker-input").keydown(function (e) {
    e.preventDefault();
})
$(function () {
    $(".required-field").append('<span style="color:red"> *</span>')
})
function validateNonEmptyField(field) {
    isValid = true;
    for (var i in field) {
        if (!checkNotNull($(field[i]).val())) {
            $(field[i]).addClass("is-invalid");
            isValid = false;
        }
    }
    return isValid;
}
$("body").on("focusout", ".is-invalid", function () {
    if (!checkNotNull($(this).val())) {
        $(this).removeClass("is-invalid")
    }
})
function checkNotNull(str) {
    if (str === null || (str != null && str.trim() == "")) {
        return false
    }
    return true
}