//Nylabone Header
(function ($) {
    $(".banner-adjustable-height .with-toggle-content .inner-container .banner-content-wrapper").on("click",function(){
        $(this).parents(".with-toggle-content").toggleClass("active");
    })
})(jQuery);  