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
     // page active
     function pageactive(activeItem){
        $('.page-list .items li a').each(function(){
            var currentitem = $(this).attr('href')
            if(currentitem == activeItem){
                $(this).addClass('active-link');
                $(this).parents('.has-submenu-list').addClass('active-link');
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
     
     function initBorders(selector, numberOfBorders) {
		$(selector).each(function (i, e) {
			var $borders = $('<div/>').addClass('borders');
			for (var i = 0; i < numberOfBorders; i++) {
				$borders.append($('<div/>'));
			}
			$(e).append($borders);
			setTimeout(function () {
				$('.borders', e).addClass('init');
			}, 0);
		});
	}
    function banneradjustable(){
        $('.banner-adjustable-height').each(function(){
            var bg = $(this).find('.banner-content-wrapper').attr('style');
            $(".hero-banner-title").attr("style", bg);       
            if ($('.hero-banner-description').is(':empty')){
                $('.hero-banner-description').hide();
            }
            
        });
    }
    function inpurwapper(){
        $('.product-variant select:not(:disabled)').wrap('<div class="dropdown-wrapper"></div>');
    }
    function updateTableHeaders(){
        var sticky = $('.heading-fixed'),
        scroll = $(window).scrollTop();     
        if (scroll >= stickyOffset){
            sticky.addClass('fixed');
        } 
        else {
            sticky.removeClass('fixed');
        }
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
      $('.ipm-step-header .component-content').each(function (i, e) {
		var numParts = 4;
		for (var i = 1; i <= numParts; i++) {
			$(e).append($('<div/>').addClass('component-' + i));
			$(e).addClass('components-added');
		}
	});
    $('.ipm-button-video').each(function (i, e) {
		var numParts = 1;
		for (var i = 1; i <= numParts; i++) {
			$(e).append($('<span/>').addClass('component-' + i));
			$(e).addClass('components-added');
		}
	});
     function textlenght(){
        $('.feature-content-tiles.variant-5 .field-subtext').each(function(){
            var count = $(this).text().length;
            if(count > 17 )
            $(this).addClass('highlight-product-small-text')
        })
     }
     textlenght()
  	initBorders('.productimage .image-placeholder', 4);
    initBorders('.stylebox-image > .component-content', 4);
	initBorders('.stylebox-labels', 3);
    initBorders('.home-banner .banner-content-wrapper', 4);
    $('.product-variant select').on('change', function (e) {
        initBorders('.productimage .image-placeholder', 4);       
    })
    pageactive(window.location.pathname);
    pageheight();
    $(window).scroll(backTotop);

    if($('.product-variant select:not(:disabled)').length){
        inpurwapper();
    }
    
    if($('.banner-adjustable-height').length){
        banneradjustable();
    }
    if($('.tabs-accordion .btn-link').length){
        tabaccordian();
      }
    if($('.heading-fixed').length){
        var stickyOffset = $('.heading-fixed').offset().top;
        $('.heading-fixed').each(function(){
            $(this).css('min-height', $(this).height())
        })
        $(window).scroll(updateTableHeaders);
        $(window).resize(updateTableHeaders);
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
   
    
  
    $(".component.hero-carousel:not(.mulitple-bottom-links) .slides").slick('slickSetOption', {
        adaptiveHeight: true
     }, true);

     $('.hero-banner-with-tab .tab').on('click', function(){
        var tabContent = $(this).data('toggle');
        $(this).addClass('active');
        $(this).siblings().removeClass('active');
        $('.hero-banner-with-tab .tab-content.'+ tabContent ).show();
        $('.hero-banner-with-tab .tab-content.'+ tabContent ).siblings().hide();
     })
     $('.ipm-header .component.image').prepend('<span class="hamburger-box"><span class="hamburger-inner"></span></span>')

     $('.hamburger-box').on('click',function(){
        $(this).toggleClass('open');
        $('.ipm-mobile-menu').slideToggle();
     })
     $('.ipm-header .component.link-list>.component-content ul li a').on('click',function(){  
        if (this.hash !== "") { 
            event.preventDefault();
            var hash = this.hash;
            $('html, body').animate({
                scrollTop: $(hash).offset().top
              }, 800);
          }
          if ($(window).width() < 992) {
            $('.ipm-mobile-menu').slideUp();
            $('.hamburger-box').toggleClass('open');
         }
          
     })
     $('.ipm-back-to-home').on('click', function(){
        $('html, body').animate({
            scrollTop: 0
          }, 800);
          if ($(window).width() < 992) {
            $('.ipm-mobile-menu').slideUp();
            $('.hamburger-box').toggleClass('open');
         }
     })
     
    
})(jQuery);