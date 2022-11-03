(function ($) {
if ($(".multi-column-content-tiles .is-mobile-carousel").length > 0) {
  $(".multi-column-content-tiles .is-mobile-carousel ").each(function (i, e) {
    var settings = {
      infinite: true,
      autoplay: true,
      autoplaySpeed: 10000,
      speed: 300,
      dots: true,
      slidesToShow: 3,
      adaptiveHeight: false,
      responsive: [
        {
          breakpoint: 9999,
          settings: "unslick",
        },
        {
          breakpoint: 960,
          settings: {
            slidesToShow: 1,
            slidesToScroll: 1,
          },
        },
      ],
    };
    $(e).slick(settings);

    //Resize function...
    var w = 0;
    $(window).resize(
      _.debounce(function () {
        if (!$(".on-page-editor").length && w != $(window).width()) {
          if (
            $(window).width() <= 1023 &&
            !$(e).hasClass("slick-initialized")
          ) {
            $(e).slick(settings);
          }
          w = $(window).width();
        }
      }, 400)
    );
  });
}
})(jQuery);