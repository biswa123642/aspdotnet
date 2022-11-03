(function ($) {

    function RebateCalculatorViewModel(numberOfCans, context) {
        var self = this;

        // Observables
        var defaultValue = $('.rebate-calculator__input', context).attr('value');
        if(defaultValue) {
            numberOfCans = defaultValue;
        }
        self.numberOfCans = ko.observable(numberOfCans);

        // Computed values
        self.potentialReturn = ko.computed(function () {
            // Return a simple calculation of the potential return ( numberofCans * $4.00 )
			var multiplier = $('.rebate-calculator__multiplier', context).text().replace('$', '');
			if(multiplier) {
				multiplier = parseFloat(multiplier);
			}
			else {
				multiplier = 4;
			}
			var value = (parseFloat(self.numberOfCans()) * multiplier).toFixed(2);
			value = value.toString().replace(/\B(?=(\d{3})+(?!\d))/g, ",");
            return "$" + value;
        });

    }
    
    if ($("div.rebate-calculator").length) {
        $("div.rebate-calculator").each(function(i,context) {
            ko.applyBindings(new RebateCalculatorViewModel(10, context), context);
        });
    }
})(jQuery);
