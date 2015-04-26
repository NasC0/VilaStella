Dropzone.options.upload = {
    init: function () {
        var $container = $('#links');

        this.on('success', function (status, response) {
            var $element = $(response);
            $element.css('display', 'none');
            $container.prepend(response);

        });
    }
}

$('.toggle-upload-container').click(function () {
    var $uploadContainer = $('.upload-container');
    if ($uploadContainer.css('display') == 'none') {
        $uploadContainer.show(2000);
    } else {
        $uploadContainer.hide(2000);
    }
});

$('#links').on('focusout', '.form-control', function (ev) {
    var $this = $(this),
        elementID = $this.parent().attr('id'),
        caption = $this.val(),
        url = 'Upload/' + elementID.toString() + '?caption=' + caption;

    $.ajax({
        url: url,
        method: "POST",
        success: function (data, status) {
            if (status !== 'notmodified') {
                toastr['success']('Успешно сменено описание!');
            }
        },
        error: function () {
            toastr['error']('Възникна проблем при смяната на описанието!')
        }
    });
});