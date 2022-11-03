(function ($) {
  $(window).scroll(function () {
    if ($(window).width() > 991 && $(document).scrollTop() > 100) {
      $(".wrapper-nav ").slideUp();
      $(".logo-container .image ").css('top', '.7rem');
    } else if($(window).width() > 991){
      $(".wrapper-nav ").slideDown();
      $(".logo-container .image ").css('top', '6rem');
    }
  });

  $(window).on('resize',function(){
    if($(window).width() < 991){
      $(".wrapper-nav ").removeAttr('style');
      $(".logo-container .image ").removeAttr('style');
    }
  })

})(jQuery);