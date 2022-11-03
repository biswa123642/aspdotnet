(function ($) {
  $(".product-variant select").on("change", function () {
    if ($(".nav-tabs li").length > 0) {
      $(".nav-tabs li").each(function () {
        if ($(this).text().trim().length == 0) {
          $(this).hide();
        }
      });
      $(".nav-tabs > li:first-child").find("a").addClass("active");
      $(".tab-content > .tab-pane:first-child").show().addClass("active");
    }
  });

  $(".product-variant").on(
    "click",
    ".variant-description .nav-tabs li a",
    function () {
      $(this).closest(".container").find(" li a").removeClass("active");
      $(this).addClass("active");
      var currentTab = $(this).attr("href");
      $(this)
        .closest(".container")
        .find(".tab-pane")
        .hide()
        .removeClass("active");
      $(currentTab).show().addClass("active");
      return false;
    }
  );
})(jQuery);
