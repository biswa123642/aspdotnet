(function ($) {
    
    function getidfrompage(){
        var url = window.location.hash;
        return url.substring(url.lastIndexOf('#') + 1);
    }
    
    if (!($(".on-page-editor").length)) {
        $(".home-page-listing").each(function () {
            $(this).find(".slider .slides").slick({
                arrows: true,
            });
        })
    }

    function activenav(href){
        var hashpath = href;
        $('.section-nav li').attr('class', '');
        if(hashpath != ""){
            if($("#" + hashpath + "").length){
                $('.section-nav li a[data-id="'+ hashpath +'"]').parent().addClass("active");
                $('html,body').animate({ scrollTop: $('#'+ hashpath +'').offset().top }, 'slow');
            }
        }else{
            $('.section-nav li').first().addClass("active");
        }
    }

    // function checkscroll(event){
    //     var scrollPosition = $(document).scrollTop();
    //     $('.home-page-stripe').each(function () {
    //         var currentLink = $(this),
    //         refElement = currentLink;
    //         if (refElement.position().top < scrollPosition && refElement.position().top + refElement.height() > scrollPosition) {
    //             $('.section-nav li').removeClass('active');
    //             $(this).parent().addClass('active');
    //         }
    //     });
    // }
    $(document).on("scroll", onScroll);

    function onScroll(event){
        var scrollPos = $(document).scrollTop();
        $('.section-nav li').each(function () {
            var currLink = $(this),
            currLinkanchor = currLink.find("a"),
            refElement = $(currLinkanchor.attr("href"));

            if (refElement.offset().top - ($(window).height() / 2)  < scrollPos && (refElement.offset().top + refElement.height()) > scrollPos) {
                $('.section-nav li').removeClass("active");
                currLink.addClass("active");
                var refcontrast = refElement.is(".dark");
                refcontrast ? $('.section-nav').attr('data-style','dark') : $('.section-nav').attr('data-style','light');
            }
        });
    }

    setTimeout(function () {
        if($(".page-home").length){
            $('.section-nav').addClass('init');
            // activenav(getidfrompage());
        }
    }, 500);

    $('.section-nav li a').on('click', function (e) {
        e.preventDefault();
        $(document).off("scroll");
        
        $('.section-nav li').each(function () {
            $(this).removeClass('active');
        })
        $(this).parent().addClass('active');
      
        var target = $(this).attr("href"),
        $target = $(target);
        $('html, body').stop().animate({
            'scrollTop': $target.offset().top + 2
        }, 500, 'swing', function () {
            window.location.hash = target;
            $(document).on("scroll", onScroll);
        });
    });

    $('.section-nav li a').on("click",function(){
        activenav($(this).attr("data-id"));
    });

    // $(window).on("scroll", checkscroll);

})(jQuery)