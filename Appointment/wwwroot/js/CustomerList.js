var dataTable;
$(document).ready(function () {
    dataTable = $('#DT_Load').DataTable({
        "ajax": {
            "url": "/Statistics/Reportes/AllCustomerJson",
            "type": "GET",
            "datatype": "json"
        },
        "columns": [
            {
                "data": "id",
                "width": "10%"
            },
            {
                "data": "name",
                "width": "30%"
            },
            {
                "data": "mobily",
                "width": "30%"
            },
            {
                "data": "neighborhoods.name",
                "width": "30%",
            },
           
        ],
        "language": { "emptyTable": "Not Found Data" },
        "width": "100%"
    });
});