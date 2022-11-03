(function ($) {
  if ($(window).width() < 821) {
    if ($(".video video-small.initialized".length > 0)) {
      $(".text-right").insertBefore($(".video-left"));
    }
  }
})(jQuery);
