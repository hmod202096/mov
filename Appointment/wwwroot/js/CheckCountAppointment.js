
function checkTheNumber() {
    let dat = $(".datepicker").val();
    let repetition = $("#txtCount").val();
    $.ajax({
        type: "GET",
        url: "/Emplooye/Reservationes/GetCountAppontment",
        data: { paramDate: dat, currentValue: repetition },
        success: function (data) {
            if (data.success) {
                toastr.success(data.message);
                $("#sav").show();
            } else {
                toastr.error(data.message);
                $("#sav").hide();
            }
        },
    });
}

checkTheNumber();