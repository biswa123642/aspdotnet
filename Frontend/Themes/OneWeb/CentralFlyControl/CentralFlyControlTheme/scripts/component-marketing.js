(function ($){
    //Functions definations
    //takes in array of classes, finds a class matching regex passed to it and return the value after the dash
    function returnClassValue(regex, classes) {
        var returnVal = null;
        var re = new RegExp(regex);
        for (var i = 0; i < classes.length; i++) {
            //test with regualr expression above
            if (re.test(classes[i])) {
                returnVal = classes[i].split('-')[1]; //grab the value after the dash
            }
        }
        return returnVal;
    }
    
    /* Calling menthods... */
    $('.market .row-first .file-type-icon-media-link').on('click', function () {
        // Scroll to market bottom section
        $('html,body').animate({ scrollTop: $('.market .row-last').offset().top });
    });


    $('.graphic-wrapper').on({
        mouseenter: function () {
            var i = returnClassValue('fact-', $(this).attr("class").split(' '));
            var fact;
            if (!isNaN(i)) {
                fact = $('.graphic-content .fact-wrapper#fact-' + i);
            }
            else {
                i = 0;
            }
            fact.addClass('visible');
        },
        mouseleave: function () {
            //remove active from all facts
            $('.graphic-content .fact-wrapper').removeClass('visible');
        }
    }, '.svg.hot-spot');
})(jQuery)