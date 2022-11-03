(function ($) {
  //for hiding remaining accordion

  $(".accordion .items li").on("click", function () {
    var accordion = $(".accordion .items li");

    if (accordion) {
      accordion.not($(this)).removeClass("active");
      accordion.not($(this)).find(".toggle-content").hide();
    }
  });
})(jQuery);
