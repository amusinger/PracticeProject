﻿
jQuery(document).ready(function () {

    /*
        Fullscreen background
    */
    $.backstretch([
                    "/Content/img/1.jpg",
                    "/Content/img/2.jpg",
                    "/Content/img/3.jpg"
                  //  "2.jpg"
	              //, "3.jpg"
	              //, "1.jpg"
    ], { duration: 3000, fade: 750 });

    /*
        Form validation
    */
    $('.login-form input[type="text"], .login-form input[type="password"], .login-form textarea').on('focus', function () {
        $(this).removeClass('input-error');
    });

    $('.login-form').on('submit', function (e) {

        $(this).find('input[type="text"], input[type="password"], textarea').each(function () {
            if ($(this).val() == "") {
                e.preventDefault();
                $(this).addClass('input-error');
            }
            else {
                $(this).removeClass('input-error');
            }
        });

    });


});
