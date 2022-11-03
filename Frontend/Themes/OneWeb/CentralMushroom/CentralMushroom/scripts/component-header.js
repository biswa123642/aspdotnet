(function ($) {
    
    
    var $typehead = $("header .global-search .typeahead");
    if ($typehead.length && ($typehead.parents("form").eq(0).attr("data-disableautosugession") === "False")) {
        $typehead.autocomplete({
            appendTo: ".search-wrapper",
        });
    }

    $('.product-variant select').on('click', function (e) {
        e.stopPropagation();
        $(this).parents('.dropdown-wrapper').toggleClass('open')
    })
    $('body').on('click', function () {
        $('.product-variant select').parents('.dropdown-wrapper').removeClass('open');
        $('.header-global-menu ul.items li.item ').removeClass('open');
    });    

    $('.hamburger-box').on('click',function(){
        $('#header  .bottom-section').slideToggle();
    })
    
    $('.mobile-search-toggle').on('click',function(){
        $('#header  .bottom-section').slideUp();
    })
   
    
  
    $(".component.hero-carousel:not(.mulitple-bottom-links) .slides").slick('slickSetOption', {
        adaptiveHeight: true
     }, true);

    
    
})(jQuery);