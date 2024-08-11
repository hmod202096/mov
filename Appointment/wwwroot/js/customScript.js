function ConfirmDelete(unigueId, isDeleteClick) {
    var deleteSpan = 'deleteSpan_' + unigueId;
    var confirmDeleteSpan = 'confirmDeleteSpan_' + unigueId;

    if (isDeleteClick) {
        $('#' + deleteSpan).hide();
        $('#' + confirmDeleteSpan).show();
    } else {
        $('#' + deleteSpan).show();
        $('#' + confirmDeleteSpan).hide();
    }
}