(function ($) {
  $(".multi-column-content-tiles .score-anchorpoint").each(function () {
    $(this).insertBefore($(this).parents(".copy-wrap"));
  });
})(jQuery);
