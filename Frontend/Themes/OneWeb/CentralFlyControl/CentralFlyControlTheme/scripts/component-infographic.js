(function ($){
    $('.ipm-graphic').each(function(i,e) {
        addPadding();

        $(window).resize(function() {
            addPadding();
        });

        function addPadding() {
            if($(window).width() > 399 && $(window).width() < 1300) {
                var height = $(e).height();
                $(e).children('.wrapper').css('padding-left', (height/14.3));
            }
            else {
                $(e).children('.wrapper').css('padding-left', 0);
            }
        }
    });

})(jQuery)