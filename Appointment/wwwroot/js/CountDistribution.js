function getCount() {

    // Fill Aryy
    var arry = [];
    $('table .code').each(function () {
        arry.push($(this).val());
    });


    // Write number innerHtml
    var table = document.getElementById("tb");

    for (var i = 1; i < table.rows.length; i++) {

        // Filter Arry
        var result = arry.filter(x => x === arry[i - 1]);

        table.rows[i].cells[6].innerHTML = result.length;
    };
};

getCount();