function RebateCalculatorViewModel(config, context) {
    var self = this;

    self.config = config;
    // Observables
    self.amountSpent = ko.observable();

    // Computed values

    self.cashbackPercent = ko.computed(function () {
        var ret = getPercentDiscount(self.amountSpent());
        return ret + "%";
    });

    self.amountSaved = ko.computed(function () {
        var ret = getDiscount(self.amountSpent()) || 0;
        ret = parseFloat(ret.toFixed(2)) + 0;
        return "$" + ret;
    });

    function getPercentDiscount(amount) {
        var brackets = self.config;
        // console.log(brackets);
        var currentBracket = 0;
        for(var i=0; i < brackets.length; i++) {
            if(amount >= brackets[i].bracket) {
                currentBracket = brackets[i].percent;
            }
        }
        return currentBracket;
    }

    function getDiscount(amount) {
        var percentDiscount = getPercentDiscount(amount);
        var discount = (amount * percentDiscount / 100);
        return discount;
    }
}

(function ($){
    if ($("div.rebate-calculator").length) {
        $("div.rebate-calculator").each(function(i,context) {
            var config = $.trim($('.config textarea', context).val());
            // console.log(config);
            config = JSON.parse(config);

            ko.applyBindings(new RebateCalculatorViewModel(config, context));
        });
    }
})(jQuery)