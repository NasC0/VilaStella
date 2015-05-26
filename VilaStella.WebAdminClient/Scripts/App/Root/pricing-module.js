$(document).ready(function() {
	function PricingViewModel() {
		var self = this;
		self.Capparo = ko.observable(0);
		self.FullPrice = ko.observable(0);
	}

	var $fromDate = $('#From'),
		$toDate = $('#To'),
		$datepickers = $('.hasDatepicker'),
		pricingEndpoint = '/Home/GetPricing',
		viewModel = new PricingViewModel();

	function GetPricing(fromDateValue, toDateValue) {
		var data = {
				from: fromDateValue,
				to: toDateValue
			};

		$.ajax({
			type: 'POST',
			url: pricingEndpoint,
			data: data
		})
		.done(function(response) {
			viewModel.Capparo(response.Capparo);
			viewModel.FullPrice(response.FullPrice);
		})
		.fail(function(error, element) {
			console.dir(error);
			console.dir(element);
		})
	}

	$datepickers.change(function() {
		var isFilledData = true,
			currentElementValue;

		$datepickers.each(function(index, element) {
			currentElementValue = $(element).val();
			if (!(currentElementValue.length > 0)) {
				isFilledData = false;
			}
		});

		if (isFilledData) {
			GetPricing($fromDate.val(), $toDate.val());
		}
	});

	ko.applyBindings(viewModel);
});