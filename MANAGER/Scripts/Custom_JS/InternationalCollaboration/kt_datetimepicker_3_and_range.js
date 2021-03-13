//datepicker
var KTBootstrapDatetimepicker = function () {
    var baseDemos = function () {
        $('.kt_datetimepicker_3').datetimepicker({
            format: 'DD/MM/yyyy'
        });

    }
    return {
        // Public functions
        init: function () {
            baseDemos();
        }
    };
}();

var KTBootstrapDaterangepicker = function () {

    // Private functions
    var demos = function () {
        // minimum setup
        $('.kt_daterangepicker_1').daterangepicker({
            buttonClasses: 'btn',
            applyClass: 'btn-primary',
            cancelClass: 'btn-secondary',
            format: 'DD/MM/yyyy'
        });


    }
    return {
        // public functions
        init: function () {
            demos();
        }
    };
}();