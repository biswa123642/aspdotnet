(function ($) {
  //Join our club popup
  $(".popup-close, .club-button").on("click", function (e) {
    e.preventDefault();
    $(".modal-container").toggle();
  });

  //Feature Content Tiles
  $.fn.matchDimensions = function (dimension) {
    var itemsToMatch = $(this);
    var maxHeight = 0;
    var maxWidth = 0;

    if (itemsToMatch.length > 0) {
      switch (dimension) {
        case "height":
          itemsToMatch
            .css("min-height", "initial")
            .each(function () {
              maxHeight = Math.max(maxHeight, $(this).height());
            })
            .css("min-height", maxHeight);

          break;

        case "width":
          itemsToMatch
            .css("width", "auto")
            .each(function () {
              maxWidth = Math.max(maxWidth, $(this).width());
            })
            .width(maxWidth);

          break;

        default:
          itemsToMatch.each(function () {
            var thisItem = $(this);
            maxHeight = Math.max(maxHeight, thisItem.height());
            maxWidth = Math.max(maxWidth, thisItem.width());
          });

          itemsToMatch
            .css({
              width: "auto",
              "min-height": "initial",
            })
            .css("min-height", maxHeight)
            .width(maxWidth);

          break;
      }
    }
    return itemsToMatch;
  };

  $(".feature-content-tiles.variant-2 .field-data")
    .matchDimensions()
    .addClass("align-content");

  //Article Detail Page FAQ
  $(".article-detail .panel-collapse").each(function () {
    $(this).css("height", "auto");
  });
  $(".article-detail .panel-collapse:eq(0)").css("display", "block");
  $(".article-detail .panel-heading a[data-toggle]").on("click", function (e) {
    e.preventDefault();
    var tabId = $(this).attr("href");
    $(this).parents(".panel").siblings().find(".panel-collapse").slideUp();
    $("div" + tabId).slideToggle();
  });

  //Sales support page on page load
  if ($(".tabs .tab").length > 0 && window.innerWidth <= 768) {
    var $firstTab = $(".tab:eq(0)").find(".caption").clone();
    $(".tab:eq(0)").find(".caption").hide();
    $(".tab-content").html($firstTab);
  }

  $(window).resize(function () {
    if ($(".tabs .tab").length > 0 && window.innerWidth <= 768) {
      $(".tab-content").empty();
      var captionText = $(".tab.active .caption");
      captionText.hide();
      var $tab = $(".tab.active .caption").clone();
      $(".tab.active .caption").hide();
      $(".tab-content").html($tab);
      $(".tab-content .caption").show();
    }
  });

  $(window).resize(function () {
    if ($(".tabs .tab").length > 0 && window.innerWidth >= 768) {
      $(".tab-content").empty();
      var captionText = $(".tab .caption");
      captionText.show();
    }
  });

  //Sales Support Page

  $(".tab-heading-mob").on("click", function () {
    if ($(window).width() <= 768) {
      $(".tab").removeClass("active");
      $(this).parent().addClass("active");
      $(".tab-content").empty();
      var captionText = $(".tab.active .caption");
      captionText.hide();
      var $tab = $(".tab.active .caption").clone();
      $(".tab.active .caption").hide();
      $(".tab-content").html($tab);
      $(".tab-content .caption").show();
    }
  });
})(jQuery);
