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
        urlRegExp = url[1], // create regexp to match current url pathname and remove trailing slash if present as it could collide with the link in navigation in case trailing slash wasn't present there
        urlRegExpEs = url[1] + "/" + url[2];
        
        // now grab every link from the navigation
        
        $('.header-global-menu ul li .list-wrap a').each(function () {
          var refLink = $(this).attr('href').replace(/^\/|\/$/g, '');
          
        // and test its normalized href against the url pathname regexp\
        if (urlRegExp== refLink || urlRegExpEs == refLink) {
          $(this).parents('.item').addClass('active-item');
        }
      });
        

  })(jQuery);
  