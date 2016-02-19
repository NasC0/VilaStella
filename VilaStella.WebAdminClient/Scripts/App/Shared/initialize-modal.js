$(function () {
    $('#modalWindow').modal('show');

    $('#modalWindow').on('hide.bs.modal', function (e) {
        window.location.href = "/";
        preventDefault();
    });
});