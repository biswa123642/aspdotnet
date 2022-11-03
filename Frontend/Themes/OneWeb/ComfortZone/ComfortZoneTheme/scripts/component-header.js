(function ($) {
   
    function backTotop(){
        var scrollheight = $('body').height() - $(window).height() - $('footer').height();
        scroll = $(window).scrollTop();     
        if (scroll >= scrollheight){
            $('.scroll-button-wrap .to-top').css('bottom', $(window).scrollTop() - scrollheight);           
        } 
        else {
            $('.scroll-button-wrap .to-top').css('bottom' , '12rem');
        }        
    }    
    function pageactive(activeItem){
        $('.link-list-tab-style a').each(function(){
            var currentitem = $(this).attr('href')
            if(currentitem == activeItem){
                $(this).addClass('active-link');
                
            }
        })
     }
     function pageheight(){
        var header = $('#header').height();
        var footer = $('#footer').height();
        var contentheight =  $(window).height();
        var mainHeight = contentheight - (header + footer);
        $('#content').css('min-height', mainHeight )
     }  
 
    function inpurwapper(){
        $('.product-variant select:not(:disabled)').wrap('<div class="dropdown-wrapper"></div>');
    }
    function setScroll(selection) {
        $("html, body").animate({
            scrollTop: selection.offset().top - 100
        }, 200);
    }
    function tabaccordian(){
        // accordian 
            $('.tabs-accordion .collapse:not(.show)').each(function(){
                $(this).slideUp();
            })
            $('.tabs-accordion .btn-link').on('click', function(){  
                $(this).parents('.card-header').next('.collapse').slideToggle();        
                $(this).parents('.card').siblings('.card').find('.collapse').slideUp();
                $(this).parents('.card').siblings('.card').find('.collapse').removeClass('show');
                $(this).parents('.card').siblings('.card').find('.btn-link').attr('aria-expanded','false');
            })
      }
   
    pageheight();
    $(window).scroll(backTotop);
    pageactive(window.location.pathname);
    if($('.product-variant select:not(:disabled)').length){
        inpurwapper();
    }
    

    if($('.tabs-accordion .btn-link').length){
        tabaccordian();
      }
   
    
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
   
    $('header .search-input').each(function(){
        $(this).wrap('<div class="input-wrapper"></div>');
    });
  
    $(".component.hero-carousel:not(.mulitple-bottom-links) .slides").slick('slickSetOption', {
        adaptiveHeight: true
     }, true);

     var stackedImageContainer = $(".image-animation");
       // Animate stacked images. Works best when all images are the same size.
    stackedImageContainer.each(function (i, e) {
        var animationInterval = 1000;
        $(this).find(".animated-image").each(function (i, e) {
            var image = $(e);
            var delay = i * 200;
            setTimeout(function () {
                setInterval(function () {
                    var opacity = image.css("opacity");
                    if (opacity == "0") {
                        image.css("opacity", 1);
                    } else {
                        image.css("opacity", 0);
                    }
                }, animationInterval);
            }, delay);
        });
        var staticImage = $(this).find(".static-image").first();
        $(this).css("max-width", staticImage.width());
    });
    stackedImageContainer.each(function (i, e) {
        var staticImage = $(this).find(".static-image").first();
        $(this).css("padding-bottom", staticImage.height());
    });
     // Global functions
   
      // Create smooth jumps to anchors
      $(".score-anchorpoint").each(function (i, e) {
        var anchor = $(this);
        var hash = "#" + anchor.attr("id");
        var inPageLink = $("a").filter(function (i, e) {
            return $(this).attr("href") === hash;
        });
        if (inPageLink.length) {
            inPageLink.click(function (event) {
                event.preventDefault();
                setScroll(anchor);
                if (history.pushState) {
                    history.pushState(null, null, hash);
                } else {
                    location.hash = hash;
                }
            });
        }
    })
    
})(jQuery);