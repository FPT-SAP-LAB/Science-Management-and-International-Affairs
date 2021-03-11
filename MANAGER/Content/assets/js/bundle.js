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
