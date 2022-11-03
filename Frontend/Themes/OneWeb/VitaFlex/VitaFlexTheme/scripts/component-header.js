(function ($) {
  // var newsletterSignupHtml = $(".top-nav").html();
  // $(".search-wrapper .search-input-wrapper input").before(newsletterSignupHtml);

  $(".header-shell .search-box").each(function () {
    $(this).parent().addClass("search-box-wrapper");
    $(this)
      .clone()
      .addClass("d-lg-none")
      .appendTo(".header-shell .header-global-menu > .component-content");
  });

  $(".header-global-menu > .component-content").append("<div class='newsletter-txt-mobile'>Newsletter Sign-up</div>");

  var url = window.location.pathname.split("/"),
    urlRegExp = new RegExp(url[1].replace(/\/$/, "") + "$");
  $(".header-global-menu ul li a").each(function () {
    if (urlRegExp.test(this.href.replace(/\/$/, "")) && url[1] != "") {
      $(this).parents(".item").addClass("active");
    }
  });
  $(".header-global-menu ul li, .header-global-menu ul li a").click(
    function () {
      $(".header-global-menu ul li").removeClass("active");
    }
  );

})(jQuery);
