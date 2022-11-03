(function($) {   

  if (!($(".on-page-editor").length)) {
      $('.product-documents-list .mobile-carousel').each(function(i, e) {
          var settings = {
              infinite: true,
              speed: 300,
              dots: false,              
              slidesToShow: 3,
              adaptiveHeight: false,
              responsive: [{
                      breakpoint: 9999,
                      settings: 'unslick'
                  },
                  {
                      breakpoint: 768,
                      settings: {
                          slidesToShow: 1,
                          slidesToScroll: 1
                      }
                  },
                  {
                    breakpoint: 991,
                    settings: {
                        slidesToShow: 3,
                        slidesToScroll: 1
                    }
                }
              ]
          };
          $(e).slick(settings);

          //Resize function...
          var w = 0;
          $(window).resize(_.debounce(function() {
              if (!($(".on-page-editor").length) && w != $(window).width()) {
                  if ($(window).width() <= 1023 && !$(e).hasClass('slick-initialized')) {
                      $(e).slick(settings);
                  }
                  w = $(window).width();
              }
          }, 400));
      });
  } 

})(jQuery);