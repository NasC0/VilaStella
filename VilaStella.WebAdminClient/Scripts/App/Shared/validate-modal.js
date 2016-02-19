$(function() {
    $('.submit-button').on('click', function () {
        $('#modalForm').validate({
            submitHandler: function (form) {
                $('.submit-button').button('loading');
            }
        });
    });
});