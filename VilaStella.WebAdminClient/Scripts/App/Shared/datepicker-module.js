var dates = [];
var toDates = [];
var isFromDateSelected = false;
var dateFormat = 'dd.m.yy';
var currentSelection = '';
var protocol = window.location.protocol;
var host = window.location.host;
var url = protocol + "//" + host + "/Home/GetOverlapDates"

$.ajax({
    url: url,
    method: "GET",
    success: function (data, status) {
        dates = data.Dates;
        $('#To, #From').datepicker('refresh');
    },
    error: function (response) {
        console.log(response);
    }
});

$('#From').datepicker({
    minDate: +1,
    onClose: function (selectedDate) {
        $('#To').datepicker('option', 'minDate', selectedDate);
    },
    onSelect: function (dateText, instance) {
        if (currentSelection !== undefined) {
            var index = dates.indexOf(currentSelection);
            dates.splice(index, 1);
        }

        isFromDateSelected = true;
        toDates = buildValidToDatesArray(dateText);
        currentSelection = dateText;
        dates.push(dateText);
    },
    beforeShowDay: beforeShowDate,
    prevText: '<i class="icon-arrow-left8"></i>',
    nextText: '<i class="icon-arrow-right8"></i>',
    dateFormat: dateFormat
});

$('#To').datepicker({
    minDate: +1,
    onClose: function (selectedDate) {
        $('#From').datepicker('option', 'maxDate', selectedDate);
    },
    beforeShowDay: beforeShowValidDates,
    prevText: '<i class="icon-arrow-left8"></i>',
    nextText: '<i class="icon-arrow-right8"></i>',
    dateFormat: dateFormat
});

function beforeShowDate(date) {
    var string = jQuery.datepicker.formatDate(dateFormat, date);
    return [dates.indexOf(string) === -1];
}

function beforeShowValidDates(date) {
    var string = jQuery.datepicker.formatDate(dateFormat, date);
    if (isFromDateSelected) {
        return [toDates.indexOf(string) !== -1];
    } else {
        return [dates.indexOf(string) === -1];
    }
}

function buildValidToDatesArray(startDate) {
    var startDateObject = getDateFromString(startDate);
    var nextStartDateString = jQuery.datepicker.formatDate(dateFormat, startDateObject);
    var startDateIndex = dates.indexOf(nextStartDateString);
    var validDates = [];
    var nextDisabledDate;

    if (startDateObject > getDateFromString(dates[dates.length - 1]))
    {
    	isFromDateSelected = false;
    	return;
    }

    for (var i = 0; i < dates.length; i++) 
    {
    	var currentDateString = dates[i];
    	var currentDisabledDate = getDateFromString(currentDateString);
    	if (currentDisabledDate > startDateObject) {
    		nextDisabledDate = currentDisabledDate;
    		break;
    	}
    }

    startDateObject = startDateObject.addDays(1);
    nextStartDateString = jQuery.datepicker.formatDate(dateFormat, startDateObject);

    while (startDateObject < nextDisabledDate)
    {
    	validDates.push(nextStartDateString);
    	startDateObject = startDateObject.addDays(1);
    	nextStartDateString = jQuery.datepicker.formatDate(dateFormat, startDateObject);
    }

    return validDates;
}

function getDateFromString(dateString) {
    var splitDate = dateString.split('.');
    var day = parseInt(splitDate[0]),
        month = parseInt(splitDate[1]) - 1,
        year = parseInt(splitDate[2]);
    var resultDate = new Date(year, month, day);

    return resultDate;
}

Date.prototype.addDays = function(days) {
	var newDate = new Date(this.valueOf());
	newDate.setDate(newDate.getDate() + days);
	return newDate;
}

jQuery(function ($) {
    $.validator.addMethod('date',
    function (value, element) {
        if (this.optional(element)) {
            return true;
        }

        var ok = true;
        try {
            $.datepicker.parseDate(dateFormat, value);
        }
        catch (err) {
            ok = false;
        }
        return ok;
    });
});