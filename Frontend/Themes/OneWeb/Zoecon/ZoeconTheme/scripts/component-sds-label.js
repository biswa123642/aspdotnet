(function ($) {
    $(window).scroll(function () {
        var headerHeight = $('.header-shell').height(),
            breacrumbHeight = $('.component.breadcrumb').height(),
            theadHeight = $(".component.sds-labels .heading-fixed").height(),
            sdsContentHeight = $(".component.sds-labels .inner-wrap .body-copy").height(),
            tableWidth = $(".component.sds-labels table").width(),
            theadTopOffset = (headerHeight + breacrumbHeight + sdsContentHeight) - theadHeight ;
        if ($(document).scrollTop() > theadTopOffset) {
            $(".component.sds-labels .heading-fixed").addClass("active").width(tableWidth);

        } else {
            $(".component.sds-labels .heading-fixed").removeClass("active").width("");
        }   
        
        
    });
})(jQuery);