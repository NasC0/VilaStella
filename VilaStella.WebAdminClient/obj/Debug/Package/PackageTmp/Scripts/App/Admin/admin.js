/*!
* Start Bootstrap - Grayscale Bootstrap Theme (http://startbootstrap.com)
* Code licensed under the Apache License v2.0.
* For details, see http://www.apache.org/licenses/LICENSE-2.0.
*/

// jQuery for page scrolling feature - requires jQuery Easing plugin
var admin = (function () {
    //$('a.page-scroll').bind('click', function (event) {
    //    var $anchor = $(this);
    //    $('html, body').stop().animate({
    //        scrollTop: $($anchor.attr('href')).offset().top
    //    }, 1500, 'easeInOutExpo');
    //    event.preventDefault();
    //});

    // Closes the Responsive Menu on Menu Item Click

    $('.logout-btn').on('click', function () {
        $('.logout-form').submit();
    });

    var getUrlParameter = function (parameterName) {
        var sPageURL = window.location.search.substring(1);
        var sURLVariables = sPageURL.split('&');
        for (var i = 0; i < sURLVariables.length; i++) {
            var sParameterName = sURLVariables[i].split('=');
            if (sParameterName[0] == parameterName) {
                return sParameterName[1];
            }
        }
    }

    return {
        getUrlParameter: getUrlParameter
    };
}());
