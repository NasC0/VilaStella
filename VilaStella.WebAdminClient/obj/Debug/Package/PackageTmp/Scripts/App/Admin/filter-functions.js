function makeRequired($element) {
    $element.find('input').prop('required', true);
}

$(function () {
    var $elements = $('.filter-elements').children();
    var $filterBy = $('.filter-change');
    var currentSelectedIndex = $filterBy.find(':selected').index();
    var $currentSelectedFilterOption = $($elements[currentSelectedIndex]);

    $currentSelectedFilterOption.show();
    makeRequired($currentSelectedFilterOption);

    $filterBy.on('change', function () {
        var $this = $(this);
        currentSelectedIndex = $this.find(':selected').index();
        $currentSelectedFilterOption = $($elements[currentSelectedIndex]);

        $elements.hide();
        $currentSelectedFilterOption.show();
        makeRequired($currentSelectedFilterOption);
    });
});