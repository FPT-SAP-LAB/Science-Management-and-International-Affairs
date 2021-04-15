var ConfigDatepicker = function () {

    var arrows;
    if (KTUtil.isRTL()) {
        arrows = {
            leftArrow: '<i class="la la-angle-right"></i>',
            rightArrow: '<i class="la la-angle-left"></i>'
        }
    } else {
        arrows = {
            leftArrow: '<i class="la la-angle-left"></i>',
            rightArrow: '<i class="la la-angle-right"></i>'
        }
    }

    var today = new Date();
    var dd = String(today.getDate()).padStart(2, '0');
    var mm = String(today.getMonth() + 1).padStart(2, '0'); //January is 0!
    var yyyy = today.getFullYear();

    today = dd + '/' + mm + '/' + yyyy;

    // Private functions
    var demos = function () {
        $('.custom-pickyear').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            todayHighlight: true,
            templates: arrows,
            startView: "years",
            format: "yyyy",
            minViewMode: "years"
        });

        $('.custom-pickdate').datepicker({
            rtl: KTUtil.isRTL(),
            orientation: "bottom left",
            todayHighlight: true,
            templates: arrows,
            format: "dd/mm/yyyy",
            endDate: today
            //maxDate: new Date().getDate()
        });
    }

    return {
        // public functions
        init: function () {
            demos();
        }
    };
}();

jQuery(document).ready(function () {
    ConfigDatepicker.init();
});