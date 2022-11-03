(function ($) {
  // Function Defination...
   

  function clicablebrandicon() {
    var fieldImage = $(".submenu-utility-linklist .linked-thumbnail ul li .field-image");
    fieldImage.each(function () {
      var andchorTag = $(this).find("a").clone();
      var descriptionText = $(this).parent().find(".field-description").text();
      $(this).parent().find(".field-description").text("");
      $(this).parent().find(".field-description").append(andchorTag);
      $(this).parent().find(".field-description a").text(descriptionText);
    });
  }  

  function expandFooterMenu(){
     //footer submenu click event
  $(".footer-container-width .link-list h3").on("click", function () {
    if (window.innerWidth < 992) {
      $(this).toggleClass("expanded");
      $(this).parent().find("ul").toggle();
      //e.preventDefault();
    }
  });
  }

  $('.header-shell.variant-1 .menu-wrapper').on('click', function(e){
    e.stopPropagation();
  })

  //Initiate Function  
  clicablebrandicon(); 
  expandFooterMenu();

})(jQuery);
