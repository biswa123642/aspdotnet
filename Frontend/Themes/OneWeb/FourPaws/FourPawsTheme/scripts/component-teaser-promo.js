(function ($) {
  var imageUrl = "";
  var teaserMobImageUrl = "";
if (window.innerWidth > 767) {
  imageUrl = $(".teaser-promo.variant-5 figure").attr("data-desktop-image");
  teaserMobImageUrl = $(".teaser-promo.variant-5 .teaser-mob-image").attr(
    "data-desktop-image"
  );
} else {
  imageUrl = $(".teaser-promo.variant-5 figure").attr("data-mobile-image");
  teaserMobImageUrl = $(".teaser-promo.variant-5 .teaser-mob-image").attr(
    "data-mobile-image"
  );
}

if ($(".teaser-promo.variant-5 figure").length > 0) {
  $(".teaser-promo.variant-5 figure").css({ "background-image": imageUrl });
  $(".teaser-promo.variant-5 .teaser-mob-image").css({
    "background-image": teaserMobImageUrl,
  });
}
})(jQuery);
