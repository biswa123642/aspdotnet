(function ($) {  
  //clone searchbox
  
  $(".header-shell .search-box").each(function () {    
    $(this).clone().addClass("d-lg-none").appendTo(".header-shell .page-list > .component-content");
  });  
 
})(jQuery);
