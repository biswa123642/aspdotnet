(function ($) {

    window.onload = function() {
        var winHT = $('.ht').innerHeight();
    
        var  hfWinHT = winHT/2;
    
        var  trdWinHT = winHT/3;
    
        var scrollPositionC1 = $('.c1').offset().top - hfWinHT;
    
        var scrollPositionC2 = $('.c2').offset().top - hfWinHT;
    
        var scrollPositionC3 = $('.c3').offset().top - hfWinHT;
    
        var scrollPositionC4 = $('.c4').offset().top - hfWinHT;
    
        var scrollPositionC5 = $('.c5').offset().top - hfWinHT;
    
        var scrollPositionC6 = $('.c6').offset().top - hfWinHT;
    
        var scrollPositionC7 = $('.c7').offset().top - hfWinHT;
    
        var scrollPositionC8 = $('.c8').offset().top - hfWinHT;
    
        var scrollPositionC9 = $('.c9').offset().top - hfWinHT;
    
        var scrollPositionC10 = $('.c10').offset().top - hfWinHT;
    
        $(window).on('scroll', function(){
           
    
            if ($(window).scrollTop() > scrollPositionC1) {
                setTimeout( function() {
                    $('.c1 .sofa').addClass('visible');
                }, 200);
            } 
    
    
            if ($(window).scrollTop() > scrollPositionC2) {
                setTimeout( function() {
                    $('.c2 .img').addClass('visible');
                }, 200);
            } 
    
    
    
    
            if ($(window).scrollTop() > scrollPositionC3) {
                setTimeout( function() {
                    $('.c3 .img-area').addClass('visible');
                }, 200);
            } 
    
    
    
            if ($(window).scrollTop() > scrollPositionC4) {
                setTimeout( function() {
                    $('.c4 .bg').addClass('visible');
                }, 200);
            } 
    
            if ($(window).scrollTop() > scrollPositionC5) {
                setTimeout( function() {
                    $('.c5 .img-area').addClass('visible');
                }, 200);
            } 
    
    
            if ($(window).scrollTop() > scrollPositionC6) {
                setTimeout( function() {
                    $('.c6 .img-area').addClass('visible');
                }, 200);
            } 
    
    
            if ($(window).scrollTop() > scrollPositionC7) {
                setTimeout( function() {
                    $('.c7 .img-area').addClass('visible');
                }, 200);
            } 
    
    
    
            if ($(window).scrollTop() > scrollPositionC8) {
                setTimeout( function() {
                    $('.c8 .img-area').addClass('visible');
                }, 200);
            } 
    
    
            if ($(window).scrollTop() > scrollPositionC9) {
                setTimeout( function() {
                    $('.c9 .img-area').addClass('visible');
                }, 200);
            } 
    
            if ($(window).scrollTop() > scrollPositionC10) {
                setTimeout( function() {
                    $('.c10 img').addClass('visible');
                }, 200);
            } 
    
    
        });
    }
    
    })(jQuery);