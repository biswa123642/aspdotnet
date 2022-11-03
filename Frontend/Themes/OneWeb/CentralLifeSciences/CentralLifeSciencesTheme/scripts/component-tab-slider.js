(function ($) {
  //Sliders//
  $(".component.tabs .tab").each(function () {
    $(this)
      .find(".score-slick-slider")
      .slick({
        centerMode: true,
        infinite: true,
        slidesToShow: 2,
        slidesToScroll: 1,
        dots: true,
        autoplay: true,
        autoplaySpeed: 6000,
        responsive: [
          {
            breakpoint: 768,
            settings: {
              slidesToShow: 2,
              centerMode: false /* set centerMode to false to show complete slide instead of 3 */,
              slidesToScroll: 1,
            },
          },
          {
            breakpoint: 991,
            settings: {
              slidesToShow: 3,
              slidesToScroll: 1,
            },
          },
        ],
      });
  });
  $(".component.tabs .tab").on("click", function () {
    $(this).find(".score-slick-slider").slick("refresh");
  });

})(jQuery);
