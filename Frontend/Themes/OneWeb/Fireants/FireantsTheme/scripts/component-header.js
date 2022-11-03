(function ($) {
    //clone searchbox
    $(".header-shell.variant-2 .search-box").each(function () {
        $(this)
            .clone()
            .addClass("d-lg-none")
            .appendTo(".header-shell.variant-2 .header-global-menu");
    });
    $(window).scroll(function() {
        if ($(document).scrollTop() > 50) {
          $(".component.header-shell.variant-2 ").addClass("sticky");
        } else {
          $(".component.header-shell.variant-2").removeClass("sticky");
        }  
      });
      // code for underline active
      var url = window.location.pathname.split('/'),
      urlRegExp = new RegExp(url[1].replace(/\/$/, '') + "$"); // create regexp to match current url pathname and remove trailing slash if present as it could collide with the link in navigation in case trailing slash wasn't present there
    // now grab every link from the navigation
    $('.header-global-menu ul li a').each(function () {
      // and test its normalized href against the url pathname regexp
      if (urlRegExp.test(this.href.replace(/\/$/, '')) && url[1] != "") {
        $(this).parents('.item').addClass('active');
      }
    });
  
})(jQuery)