(function ($) {
    function setslick($this) {
    var $imgplaceholder = $this.parents(".productimage").find(".image-placeholder");  
    $imgplaceholder.slick({
        arrows: false,
        fade: true,
        mobileFirst: true,
        slidesToScroll: 1,
        slidesToShow: 1,
        responsive: [
          {
            breakpoint: 1199,
            settings: {
              // vertical: true,
            },
          },
        ],
      });
  
      $(".slick-initialized video").length ? $(".slick-initialized video").length.removeAttr("autoplay") : "";
  
      $('.swipebox').swipebox({
        removeBarsOnMobile: false,
        loopAtEnd: true,
      });

      setTimeout(function () {
        $(".image-placeholder").trigger("click");
      }, 1500);
    }

    function bindvariantdata($this) {

      var productimage = $this,
        gallarydata = JSON.parse(productimage.length ? productimage.attr("data-gallery") : "");
  
      // gallery data binding...
      gallarydata != "" ? gallarydata.ProductVariantList.filter(function (val) {
        if (val.ProductMediaList.length) {
          productimage.find(".image-container.slick-initialized").length ? productimage.find(".image-container.slick-initialized").slick('unslick') : "";
          productimage.find(".image-placeholder.slick-initialized").length ? productimage.find(".image-placeholder.slick-initialized").slick('unslick') : "";
          productimage.find(".image-container,.image-placeholder").empty();
          val.ProductMediaList.filter(function (val) {
            if (val.MediaType == "image") {
              productimage.find(".image-container").append(`<div class="product-tile field-image" tabindex="0"><img src="${val.MediaURL}" /></div>`);
              productimage.find(".image-placeholder").append(`<a href="${val.MediaURL}" class="swipebox"><img src="${val.MediaURL}" /></a>`);
            } else if (val.MediaType == "youtube") {
              productimage.find(".image-container").append(`<div class="product-tile video-checkbox" tabindex="0"><img src="https://i.ytimg.com/vi/${val.YoutubeId}/default.jpg" /></div>`);
              productimage.find(".image-placeholder").append(`<a href="https://www.youtube.com/embed/${val.YoutubeId}" class="swipebox"><iframe src="https://www.youtube.com/embed/${val.YoutubeId}" ></iframe></a>`);
            } else if (val.MediaType == "video") {
              productimage.find(".image-container").append(`<div class="product-tile internal" tabindex="0"><video><source src="${val.MediaURL}" /></video></div>`);
              productimage.find(".image-placeholder").append(`<a href="${val.MediaURL}" class="swipebox"><video><source src="${val.MediaURL}" /></video></a`);
            }
          });
          //productimage.find(".image-placeholder").html(productimage.find(".image-container > div").eq(0).html());
          setslick(productimage.find(".inner-wrap .image-container"));
        }
      }) : "";
    }

    if(!$(".product-variant").length){
        $(".product-image-gallery").each(function(){
            bindvariantdata($(this));
        })
    }

  })(jQuery);
  