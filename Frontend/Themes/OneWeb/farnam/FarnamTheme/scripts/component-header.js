(function ($) {

    function autoheight() {
        $('#header .header-shell').each(function () {
            $(this).parents('#header').css("padding-top", $(this).height())
        })
    }

    function expandFooterMenu() {
        //footer submenu click event
        $(".footer-container-width .link-list h3").on("click", function () {
            var item = $(this).parents('.link-list');
            if (window.innerWidth < 768) {
                $(this).toggleClass("expanded");
                item.find("ul").slideToggle(500);
                item.parent().parent().siblings().find('.link-list').find('ul').slideUp();
                item.parent().parent().siblings().find('.link-list').find('h3').removeClass("expanded");
            }
        });
    }
    /* Method & Events calling Block */
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
    })

    $('.product-variant select:not(:disabled)').wrap('<div class="dropdown-wrapper"></div>');

    expandFooterMenu();
    autoheight()
    window.onresize = function (event) {
        autoheight()
    };

    $(".responsive-carousel .slides").slick('slickSetOption', {
        adaptiveHeight: true
     }, true);

    




})(jQuery);