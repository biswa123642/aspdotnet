(function ($) {
    //header
    $('.header-global-menu .has-sublist').on('click', function () {
        $('.header-shell.variant-3').removeClass('active');
    });

   var mobileMediaUrl =  $('.hero.banner-with-content .banner-mobile-image img').attr('src');
    if(mobileMediaUrl){
        $('.hero.banner-with-content .inner-container').css('background-image',`url("${mobileMediaUrl}")`)
    }

})(jQuery);