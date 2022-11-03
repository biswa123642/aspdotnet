(function ($) {
  if(!($(".on-page-editor").length)){
    $(".header-shell").each(function(){
      var utilitynav = $(this).find(".utility-navigation").clone();
      utilitynav.addClass("d-block d-lg-none");
      $(this).find(".header-global-menu").append(utilitynav);
    })
  }
})(jQuery);