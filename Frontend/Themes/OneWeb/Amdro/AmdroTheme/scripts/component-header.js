(function ($) {
    var searchPlaceholder = $('header .search-input').attr("placeholder"); 
       

    function header_fix() {
        var header_height = $(".header-shell").height();
        if ($(window).scrollTop() > header_height) {
            $('body').addClass('header-fix');
            $('header  .search-input').attr('placeholder', 'Search')
        } else {
            $('body').removeClass('header-fix');
            $('header .search-input').attr('placeholder', searchPlaceholder)
        }
    };

    function switch_component(component) {
        if (component.next().hasClass('component-solution-finder')) {
            component.slideUp().next().slideToggle();
        }
    }

    function mobile_menu() {
        if ($(window).width() < 992) {
            $('#mobile-menu').slideToggle();
            $('body').toggleClass('mobile-open');
            $('.submenu-utility-linklist').slideUp();
        }
        if ($(window).width() < 768) {
            $('.header-shell .search-box .global-search-wrapper').slideUp();
        }
    }

    function smoothscroll(element, position) {
        $('html, body').animate({
            scrollTop: element.offset().top - position
        }, 1000);        
    }
  
    /*===============================================
            Solution finder Teaser promo
    =================================================*/
    $('.teaser-promo').each(function () {
        var componenturl = $(this).find('.field-firstcta a').attr('href');
        $(this).find('.field-teaserimage').wrapInner('<a href="' + componenturl + '"></a>');
    })

    $('.teaser-promo .field-firstcta a, .teaser-promo .field-teaserimage a').on('click', function () {
        var component = $(this).parents('.teaser-promo');
        switch_component(component)
    })

    $('#primaryvariationvalue').wrap('<div class="dropdown-wrapper"></div>');

    /*===============================================
           function call
    =================================================*/ 
   
    $(window).on('scroll', header_fix);

    /*===============================================
           Smooth scrolling
    =================================================*/

    $('a[href^="#"]').on('click', function (event) {
        event.preventDefault();
        smoothscroll($($.attr(this, 'href')), 95)
    });

    /* Method & Events calling Block */
    var $typehead = $("header .global-search .typeahead");
    if ($typehead.length && ($typehead.parents("form").eq(0).attr("data-disableautosugession") === "False")) {
        $typehead.autocomplete({
            appendTo: ".search-wrapper",
        });
    }
})(jQuery);