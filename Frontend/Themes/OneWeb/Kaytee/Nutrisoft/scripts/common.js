(function ($) {   
  var inlineLink = $('.link-list.pdp-navigation');
      if(inlineLink.length > 0){
        inlineLink.parents('.container-fluid').eq(0).append('<div class="button-item"></div>')
      }

      $('.link-list.pdp-navigation li').each(function (){
        var url = window.location.pathname,
            inlineLinkHref = $(this).find('.field-link a').attr('href');
        if(url == inlineLinkHref){
          $(this).addClass('active');
          
        }       
        if($(this).hasClass('active')) {
          var cloneItem = $(this).clone();
          $('.button-item').append(cloneItem);
        }
      });




    // var globalMenuItem = $('.header-global-menu ul.items li.item .has-sublist');    
    // globalMenuItem.on('click', function(){
    //     $(this).parents('.header-shell').toggleClass('active-items');
        
    // })
    // $(window).resize(function () {
    //     if ($(window).width() > 991) {
    //       globalMenuItem.parents('li.item').removeClass('open');
    //     }
    //   });

  })(jQuery);