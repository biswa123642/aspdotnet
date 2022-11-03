(function ($) {
    //clone searchbox
  
      $(".header-shell.variant-2 .search-box").each(function () {
      $(this)
        .clone()
        .addClass("d-lg-none")
        .appendTo(".header-shell.variant-2 .header-global-menu");
    });

    //active for desktop menu

  var url = window.location.pathname.split('/'),
  urlRegExp = new RegExp(url[1].replace(/\/$/, '') + "$"); // create regexp to match current url pathname and remove trailing slash if present as it could collide with the link in navigation in case trailing slash wasn't present there

// now grab every link from the navigation

$('.header-global-menu ul li a').each(function () {

  // and test its normalized href against the url pathname regexp

  if (urlRegExp.test(this.href.replace(/\/$/, '')) && url[1] != "") {
    $(this).parents('.item').addClass('active-item');
  }

});

function switch_component(component) {
  if (component.next().hasClass('component-solution-finder')) {
      component.slideUp().next().slideToggle();
  }
}
 
/*===============================================
            Solution finder Teaser promo
    =================================================*/
    $('.teaser-promo').each(function () {
      var componenturl = $(this).find('.field-firstcta a').attr('href');
      $(this).find('.field-teaserimage').wrapInner('<a href="' + componenturl + '"></a>');
  })

  $('.teaser-promo .field-firstcta a, .teaser-promo .field-teaserimage a').on('click', function () {
      var component = $(this).parents('.teaser-promo');
      switch_component(component)
  })

  $('#primaryvariationvalue').wrap('<div class="dropdown-wrapper"></div>');
  
  })(jQuery);
  