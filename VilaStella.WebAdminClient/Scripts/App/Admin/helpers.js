function successfullStatusChange() {
    makeToast('success', 'Статусът на резервацията беше променен успешно!');
}

function failedStatusChange() {
    makeToast('error', 'Възникна грешка при промяната на статуса на резервацията!');
}

function SuccessfullyDeletedEntry(data) {
    if (data.Result) {
        makeToast('success', 'Успешно изтрита резервация!');
        $('.to-delete').hide(1000);
    } else if (!data.Result) {
        makeToast('error', 'Възникна грешка при изтриването на резервацията!');
    }
}

function FailedDeleteEntry() {
    makeToast('error', 'Възникна грешка при изтриването на резервацията!');
}

function statusChanged(ev) {
    var $this = $(ev);
    $this.parent().parent().submit();
}

function makeToast(mode, text) {
    toastr[mode](text);
}

$('.delete-link').on('click', function (ev) {
    var confirmationResult = confirm('Сигурен ли си че искаш да изтриеш този запис?');
    var $this = $(this);

    if (confirmationResult) {
        $this.parent().parent().addClass('to-delete');
    } else {
        return false;
    }
})