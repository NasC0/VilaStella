$(document).ready(function () {
    var ERROR_CLASS = 'has-error';

    var $form = $('.reservation-form'),
        $submitFormButton = $('.submit-form'),
        $validator = $form.validate(),
        $datepickers = $('.hasDatepicker');

    var $paymentButtons = $('.payment-button');
    var $buttonParent = $paymentButtons.parent();
    var $paymentDropdown = $('#PaymentMethod');
    var $currentActiveButton = $($paymentButtons[0]);
    var paymentOptions = ['ByHand', 'BankTransaction', 'PayPal'];

    $paymentButtons.click(function () {
        var $self = $(this);

        if ($self.parent().hasClass(ERROR_CLASS)) {
            $buttonParent.removeClass(ERROR_CLASS);
        }

        if ($currentActiveButton !== $self) {
            $currentActiveButton.removeClass('active');
            $self.addClass('active');
            $currentActiveButton = $self;

            var paymentOptionIndex = $paymentButtons.index(this);
            $paymentDropdown.val(paymentOptions[paymentOptionIndex]);
        }
    });

    $submitFormButton.click(function(ev) {
        var $self = $(this);
        var isFormValid = $form.valid();

        if (!isFormValid) {
            ev.preventDefault();
            if ($buttonParent.hasClass(ERROR_CLASS)) {
                $buttonParent.addClass(ERROR_CLASS);
            }
        } else {
            $buttonParent.removeClass(ERROR_CLASS);
            $self.attr('disabled', 'disabled');
            $form.submit();
        }
    });

    $datepickers.change(function() {
        var $self = $(this);
        $validator.element($self);
    });
});