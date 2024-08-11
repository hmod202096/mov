
//الحصول على رقم السائق
function getValue() {

    var mytd = document.getElementById('myselect');
    var value = mytd.value;
    var text = mytd.options[mytd.selectedIndex].text;
    console.log(mytd);
    console.log(value);
    console.log(text);
}

//function CountRows() {

//    var table1 = document.getElementById('tb');

//    var count = 0;
//    var arry = [];
//    var arry_ = [];


//    for (var i = 1; i < table1.rows.length; i++) {              // Fill Array
//        if (table1.rows[i].cells[6].innerHTML != null) {
//            arry.push(table1.rows[i].cells[6].id);              // Fill Array ID Itime
//            arry_.push(table1.rows[i].cells[6].innerHTML);      // Fill Array Number Driver
//        }
//    }

//    console.log(arry);
//    console.log(arry_);

//    for (var j = 0; j < arry_.length; j++) {                    // Grob Array By Number Driver

//        count = 0;

//        for (var k = 0; k < arry_.length; k++) {
//            if (arry_[j] == arry_[k]) {
//                count++;
//            }
//        }

//        var myDiv = document.getElementById(arry[j] + '-result');       // Print Count
//        myDiv.innerHTML = count;

//        //myDiv.style.backgroundColor = "chartreuse";
//        //myDiv.style.color = "Red";
//    }
//}

//CountRows();








//var arry = [];
//var newArr = [];
//var result = null;

//$('table .code').each(function () {
//    arry.push($(this).val());
//});

//console.log(arry);

//const result = arry.filter(x => x == '2d891213-77d3-4474-b2e3-96577d00dccf');
//console.log(result.length);

//var mytd = document.getElementById(arry[i]);
//var text = mytd.options[mytd.selectedIndex].value;
//console.log(text);

//newArr = arry;

//for (var i = 0; i < arry.length; i++) {

//    var mytd = document.getElementById(arry[i]);
//    var text = mytd.options[mytd.selectedIndex].value;
//    console.log(text);

//    result = arry.filter(x => x === arry[i]);
//    console.log(result.length);

//    document.getElementById('count' + ' ' + arry[i]).innerHTML = result.length


//}



            
//            let count = 0;

//            for (var j = 0; j < arry.length; j++) {
//                if (arry[j] != "") {
//                    count = 0;
//                    for (var k = 0; k < newArr.length; k++) {
//                        if (newArr[k] === arry[j]) {
//                            count++;
//                        }
//                    };
//                    document.getElementById(arry[j]).innerHTML = count;
//                    console.log(newArr[j] + ':' + count);
//                }
//            };












//const url_table = document.getElementById("tb").rows
//                var post_urls = [];
//                for (let i = 1; i < url_table.length; i++) {
//                    post_urls.push(url_table[i].cells[5].innerText);
//                }

//                console.log(post_urls);


//                var arr = [];
//                $('table .code').each(function () {
//                    arr.push($(this).val());
//                })
//                console.log(arr);


//               var r = $("#myselect option:selected").text();
//                console.log(r);

//                var rr = $("#myselect").val();
//                console.log(rr);


//                 $('table > tbody  > tr > td > div > select').each(function (index, tr) {
//                        console.log(index);
//                        console.log(tr);



//                        let driverId = pramId;
//                        var i = 0;
//                        $("select").each(function () {
//                            if ($(this).attr("id") == driverId)
//                                i++;

//                        });
//                        console.log(i);