$(function () {
    if ($("#txtName").val() !== '') {
        $("#txtName").prop("disabled", true);
    } else {
        $("#txtName").prop("disabled", false);
    }
    if ($("#txtPhone").val() !== '') {
        $("#txtPhone").prop("disabled", true);
    } else {
        $("#txtPhone").prop("disabled", false);
    }
    if ($("#neigh").val() !== '') {
        $("#neigh").prop("disabled", true);
    } else {
        $("#neigh").prop("disabled", false);
    }
});