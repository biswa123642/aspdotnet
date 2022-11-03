(function ($) {
    function ZenivexRebateCalculatorViewModel(context) {
        var self = this;
        // Observables
        self.quantity = ko.observable();
        self.product = ko.observable();
        self.minimumMessage = ko.observable();

        // Computed values
        self.isNumeric = function (n) {
            return !isNaN(parseFloat(n)) && isFinite(n);
        }

        self.minimumMessage = ko.computed(function () {
            var product = self.product();
            var quantity = self.quantity();
            if (product == 'zenivex_e20' || product == 'aqua_zenivex_e20') {
                if (quantity < 90) {
                    return "Minimum required amount purchased: single order of 90 gallons";
                }
            }
            else if (product == 'zenivex_e4_rtu') {
                if (quantity < 120) {
                    return "Minimum required amount purchased: single order of 120 gallons";
                }
            }
        });

        self.rebatePercent = ko.computed(function () {
            var product = self.product();
            var quantity = self.quantity();
            if (quantity && !self.isNumeric(quantity)) {
                $('.zenivex-rebate-calculator').addClass('error');
                return '0%';
            }
            else {
                $('.zenivex-rebate-calculator').removeClass('error');
            }
            if (product == 'zenivex_e20' || product == 'aqua_zenivex_e20') {
                if (quantity >= 90) {
                    return '10%';
                }
                else {
                    return '0%';
                }
            }
            else if (product == 'zenivex_e4_rtu') {
                if (quantity >= 120) {
                    return '7.5%';
                }
                else {
                    return '0%';
                }
            }
            else {
                return '0%';
            }
        });
    }

    $(document).ready(function () {
        if ($("div.zenivex-rebate-calculator").length) {
            $("div.zenivex-rebate-calculator").each(function (i, context) {
                ko.applyBindings(new ZenivexRebateCalculatorViewModel(context));
            });
        }
    });

})(jQuery)