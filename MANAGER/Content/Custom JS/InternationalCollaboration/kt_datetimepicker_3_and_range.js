//datepicker
var range
$(document).ready(function () {
    $('.kt_datetimepicker_3').datetimepicker({
        format: 'DD/MM/yyyy'
    });

    $('.kt_daterangepicker_1').daterangepicker({
        buttonClasses: 'btn',
        applyClass: 'btn-primary',
        cancelClass: 'btn-secondary',
        format: 'DD/MM/yyyy'
    }, function (start, end) {
        range = start.format('DD/MM/yyyy') + ' - ' + end.format('DD/MM/yyyy')
        console.log(range)
        //$('#add_program_range_date span').html(start.format('DD/MM/yyyy') + ' - ' + end.format('DD/MM/yyyy'));
        startDate = start;
        endDate = end;
    });
    $('.kt_daterangepicker_1').val(moment().format('DD/MM/yyyy') + ' - ' + moment().format('DD/MM/yyyy'));
})
$(document).on('click', '.applyBtn', function () {
    $('.kt_daterangepicker_1').val(range);
    $('.drp-selected').text(range);
})
$(document).on('click', '.cancelBtn', function () {
    $('.kt_daterangepicker_1').val(moment().format('DD/MM/yyyy') + ' - ' + moment().format('DD/MM/yyyy'));
    $('.drp-selected').text(moment().format('DD/MM/yyyy') + ' - ' + moment().format('DD/MM/yyyy'));
})


