(function ($) {
  var url = window.location.pathname.split("/"),
    urlRegExp = new RegExp(url[1].replace(/\/$/, "") + "$");
  $(".page-list ul li a").each(function () {
    if (urlRegExp.test(this.href.replace(/\/$/, "")) && url[1] != "") {
      $(this).parents(".item").addClass("active");
    }
  });
})(jQuery);
