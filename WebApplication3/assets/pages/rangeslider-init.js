/**
* Theme: Codefox - Bootstrap 4 Admin Template
* Author: Coderthemes
* Component: Ion Slider
* 
*/
$(document).ready(function () {
    $("#range_01").ionRangeSlider({
        skin: "modern"
    });
    
    $("#range_02").ionRangeSlider({
        skin: "modern",
        min: 100,
        max: 1000,
        from: 550
    });
    
    $("#range_03").ionRangeSlider({
        skin: "modern",
        type: "double",
        grid: true,
        min: 0,
        max: 1000,
        from: 200,
        to: 800,
        prefix: "$"
    });
   
    $("#range_04").ionRangeSlider({
        skin: "modern",
        type: "double",
        grid: true,
        min: -1000,
        max: 1000,
        from: -500,
        to: 500
    });
    
    $("#range_05").ionRangeSlider({
        skin: "modern",
        type: "double",
        grid: true,
        min: -1000,
        max: 1000,
        from: -500,
        to: 500,
        step: 250
    });
    
    $("#range_06").ionRangeSlider({
        skin: "modern",
        grid: true,
        from: 3,
        values: ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"]
    });
    
    $("#range_07").ionRangeSlider({
        skin: "modern",
        grid: true,
        min: 1000,
        max: 1000000,
        from: 200000,
        step: 1000,
        prettify_enabled: true
    });
    
    $("#range_08").ionRangeSlider({
        skin: "modern",
        min: 100,
        max: 1000,
        from: 550,
        disable: true
    });
});