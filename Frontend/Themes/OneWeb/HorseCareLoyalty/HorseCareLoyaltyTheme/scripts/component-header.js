(function ($) {
  if (!$(".on-page-editor").length) {
    $(window).scroll(function () {
      if ($(document).scrollTop() > 50) {
        $(".component.header-shell.variant-1 ").addClass("sticky");
      } else {
        $(".component.header-shell.variant-1").removeClass("sticky");
      }
    });
  }
})(jQuery);
