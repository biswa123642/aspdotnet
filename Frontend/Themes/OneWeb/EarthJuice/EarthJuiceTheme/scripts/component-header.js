(function ($) {
  var $searchBox = $(".header-shell.variant-3.single-row-shell .search-box"),    
      $searhinput = $searchBox.find(".search-input-wrapper input");

  // serch box
  var clonesearch = $searchBox.clone();
  if ($(".header-global-menu.variant-3 .search-box").length <= 0) {
    $(".header-global-menu.variant-3 ul.items").append(clonesearch);
  }

  // header input hide show
  $searhinput.length ? $searhinput.val("") : "";

  //active for desktop menu
  var url = window.location.pathname.split('/'),
    urlRegExp = new RegExp(url[1].replace(/\/$/, '') + "$"); // create regexp to match current url pathname and remove trailing slash if present as it could collide with the link in navigation in case trailing slash wasn't present there
  // now grab every link from the navigation
  $('.header-global-menu.variant-3 ul li a').each(function () {
    // and test its normalized href against the url pathname regexp
    if (urlRegExp.test(this.href.replace(/\/$/, '')) && url[1] != "") {
      $(this).parents('.item').addClass('active');
    }
  });



})(jQuery);