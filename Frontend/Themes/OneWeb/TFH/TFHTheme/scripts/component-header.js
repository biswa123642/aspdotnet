(function ($) {
    function showDropdownImage(){
        var subMenuList= $(".page-list.variant-1 .submenu-utility-linklist")
        subMenuList ? subMenuList.parents(".item").addClass("caret-dropdown"):'';
    }
    $('.header-shell .menu-wrapper').on('click', function(e){
        e.stopPropagation();
      })
    //initialize function
    showDropdownImage();
})(jQuery);