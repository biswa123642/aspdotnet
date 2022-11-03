(function ($) {
    $(window).scroll(function() {
        if ($(document).scrollTop() > 50) {
          $(".component.header-shell.variant-3 ").addClass("sticky");
        } else {
          $(".component.header-shell.variant-3").removeClass("sticky");
        }  
      });
      $(".us-icon").parent().addClass("us-icon-wrap");
  })(jQuery);