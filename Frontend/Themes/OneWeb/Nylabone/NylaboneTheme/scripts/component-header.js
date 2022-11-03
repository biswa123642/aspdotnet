//Nylabone Header
(function ($) {
  
// Global Variable declaration
var topNavMainMenu = $('.component.page-list ul.items li.has-submenu-list'),
    offSideCanvas = $('html,body'),    
    secondLevelNav = topNavMainMenu.find('.submenu-utility-linklist .sub-menu-wrapper .has-sublist .component.link'),
    secondLevelNavListItem = $('.component.page-list');



// function declaration
function toplevelnavigation($this) { 
  var $item = $this.parents('.items').eq(0);    
  if ($item.hasClass("active")) {    
    $item.removeClass('active');
  } else {    
    $item.addClass('active');
  }
}

function secondlevelnavigation($this) {   
  var $parentItem = $this.parents('.item'),
      $parentWrapper = $this.parents('.sub-menu-wrapper'),
      $mainWrapper =  $this.parents('.submenu-utility-linklist');        
  if ($parentItem.hasClass("enable-second-level") && $parentWrapper.hasClass('active') && $mainWrapper.hasClass('active')) {    
    $parentItem.removeClass('enable-second-level');
    $parentWrapper.removeClass('active');
    $mainWrapper.removeClass('active');    
  } else {
    $parentItem.addClass('enable-second-level');
    $parentWrapper.addClass('active');
    $mainWrapper.addClass('active');    
    $parentItem.siblings().removeClass('enable-second-level');    
  }
}

function offcanvas() {      
  topNavMainMenu.parents('.items').removeClass('active');
}

// trigger event 

topNavMainMenu.on('click', function(e){
  e.stopPropagation();
  toplevelnavigation($(this));
});

secondLevelNav.on('click',function(e){
  e.stopPropagation();
  secondlevelnavigation($(this));
})

// event for off canvas click 
offSideCanvas.on('click', function () {
  offcanvas();
})

//prevent redirect
secondLevelNavListItem.on('click', function(e){
  e.stopPropagation();
})

  $(".header-shell .search-box").each(function () {
    $(this).parent().addClass("search-box-wrapper");
    $(this).clone().addClass("d-lg-none").appendTo(".header-shell .page-list > .component-content");
  });
 

  $(window).scroll(function () {
    if ($(window).width() > 969) {
      if ($(window).scrollTop() > 69) {
        $(".header-shell.single-row-shell").addClass("fixed-top");
      } else {
        $(".header-shell.single-row-shell").removeClass("fixed-top");
      }
    }
  }); 

  $(document).on("click", ".global-search-wrapper .search-input", function (e) {
    e.stopPropagation();
  });

})(jQuery);