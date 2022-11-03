(function ($) {
   //for hiding remaining accordion
    $('.accordion.variant-1 .items li').on('click', function(){
        var accordion= $(".accordion.variant-1 .items li");
        accordion.not($(this)).removeClass('active');
        accordion.not($(this)).find(".toggle-content").hide();
    })
   
})(jQuery);