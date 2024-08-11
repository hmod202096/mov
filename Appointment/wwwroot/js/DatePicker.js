
$(function () {
    function WireUpDatePicker() {

        const maxDate = document.getElementById("maxdate").value;

        /* const addMonths = 2;*/

        var CurrDate = new Date();

        $(".datepicker").datepicker(
            {
                dateFormat: 'yy-mm-dd',
                numberOfMonths: 1,
                changeYear: true,
                changeMonth: true,
                minDate: CurrDate,
                maxDate: maxDate

                /*maxDate: addSubtractMonths(CurrDate, addMonths)*/

            }
        );
    }

    WireUpDatePicker();
});