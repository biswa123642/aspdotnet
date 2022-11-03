(function ($) {
    if($(".productimage").length && $(".image-cta").length){
        var imgcloned = $(".image-cta").clone(),
        productImage = $(".productimage").parent();
        imgcloned.addClass("d-none d-md-block");
        imgcloned.css('margin-top', '20px');
        $(".image-cta").addClass("d-md-none");
        imgcloned.appendTo(productImage);
    }
})(jQuery);