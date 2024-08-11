function CompareDate() {
    var date1 = document.getElementById("d1").value;
    var date2 = document.getElementById("d2").value;

    if (date1 > date2) {
        toastr.error("يجب أن يكون التاريخ أكبر من تاريخ البدايه");
        $("#search").hide();
    } else {
        $("#search").show();
    }
}