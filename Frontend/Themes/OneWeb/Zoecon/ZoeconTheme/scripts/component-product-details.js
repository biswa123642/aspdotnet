(function ($) {
    // data group attribute assigning in body for different styling
    var url = window.location.pathname.split('/');
    if (url[1] === "all-products" && url[3]) {
        $("body").attr('data-group', url[2]);
    }

    // accordion can open multiple disable
    var accordionHeader = $('.product-tab-accordion .card-header button.btn-link ');

    function closesiblingitem($this) {
        var $sibling = $this.parents('.card').siblings().find('.collapse');
        var $sibling2 = $this.parents('.card').siblings().find(`button[aria-expanded="true"]`);
        if ($sibling.hasClass('show')) {
            $sibling.removeClass('show');
            $sibling2.attr("aria-expanded", "false");
        }
    }

    accordionHeader.on('click', function () {
        closesiblingitem($(this));
    })
})(jQuery);