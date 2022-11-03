(function ($) {
  if ($(".solution-finder-rte".length > 0)) {
    $(".breadcrumb-bg").insertBefore($(".solution-finder-rte"));
    $(".breadcrumb-bg").attr("display", "block");
  }
})(jQuery);
