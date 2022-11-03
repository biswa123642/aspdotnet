(function ($) {
  //clone searchbox

  $(".header-shell.variant-2 .search-box").each(function () {
    $(this)
      .clone()
      .addClass("d-lg-none")
      .appendTo(".header-shell.variant-2 .header-global-menu");
  });
})(jQuery);
