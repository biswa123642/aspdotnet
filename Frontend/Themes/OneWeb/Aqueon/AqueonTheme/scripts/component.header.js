(function ($) {
  // to close toggle-popup
  $(".close-btn").on("click", function (e) {
    e.preventDefault();
    $(".toggle-content").removeAttr("open");
  });
})(jQuery);
