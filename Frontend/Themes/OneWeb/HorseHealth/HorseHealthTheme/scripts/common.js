(function ($) {
  // to implement isotop in article-listing

  $(".article-listing .product-list").each(function () {
    var count = $(this).children("li").length;
    if (count > 3) {
      $(this).addClass("isotop");
    }
  });
})(jQuery);
