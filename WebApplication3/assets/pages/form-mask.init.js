

/**
* Theme: Codefox - Bootstrap 4 Admin Template
* Author: Coderthemes
* form mask init
*/

jQuery(function($) {
    $('.autonumber').autoNumeric('init');
});

jQuery.browser = {};
(function () {
    jQuery.browser.msie = false;
    jQuery.browser.version = 0;
    if (navigator.userAgent.match(/MSIE ([0-9]+)\./)) {
        jQuery.browser.msie = true;
        jQuery.browser.version = RegExp.$1;
    }
})();