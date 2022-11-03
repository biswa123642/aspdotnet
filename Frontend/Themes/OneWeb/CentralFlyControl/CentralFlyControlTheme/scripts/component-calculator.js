function CalculatorViewModel(id, config, context) {
	var self = this;

	// Observables
	self.id = id;
	self.productOptions = ko.observable(getProductOptions());
	self.selectedProduct = ko.observable(); // nothing set by default
	self.animalWeightLabel = ko.observable(config.animalName + ' (lbs)');
	self.feedIntake = ko.observable(config.feedIntake);
	self.feedIntakeLabel = ko.observable("Feed intake (lbs)");

	self.swineOptions = ko.observable(getSwineOptions());
	self.selectedSwine = ko.observable();

	if(self.id == 'swine' && self.selectedSwine() && self.selectedSwine().name == 'Swine < 90') {
		self.animalWeight = ko.observable(85);
	}
	else {
		self.animalWeight = ko.observable(config.animalWeight);
	}



	function getSwineOptions() {
		if(self.id == 'swine') {
			return [
				{'name': 'Swine > 90'},
				{'name': 'Swine < 90'},
				{'name': 'Limited Fed Swine'}
			]
		}
		else {
			return [];
		}
	}

	function getProductOptions() {
		if(self.id == 'swine') {
			return [{'name': '0.67%', 'value': 0.0067}, {'name': '2.67%', 'value': .0267}]
		}
		else {
			return [{'name': '0.67%', 'value': 0.0067}, {'name': '8.00%', 'value': .08}]
		}
	}

	self.output = ko.computed(function() {
		if(self.selectedProduct()) {
			var A5 = parseFloat(self.animalWeight()); // input
			var B5 = A5 / 2.2;
			var C5 = getDfbDosage(); // DFB Dosage
			console.log(C5);
			var D5 = B5 * C5;
			var E5 = parseFloat(self.selectedProduct().value); // input
			console.log(E5)
			var F5 = D5 / E5;
			var G5 = F5 / 1000;
			var H5 = parseFloat(self.feedIntake()); // input
			console.log(H5);
			var output = 2000 * (G5 / (H5 * 454))
			return output.toFixed(2);
		}
		else {
			return "nothing";
		}
	});

	function getDfbDosage() {
		var id = self.id;
		if(id == 'swine') {
			console.log(self.selectedSwine());
			if(self.selectedSwine() && self.selectedSwine().name == 'Limited Fed Swine') {
				return 0.1;
			}
			else if(self.selectedSwine() && self.selectedSwine().name == 'Swine > 90') {
				return 0.2;
			}
			else if(self.selectedSwine() && self.selectedSwine().name == 'Swine < 90') {
				return 0.3;
			}
		}
		else if(id == 'cattle') {
			return 0.1;
		}
		else if(id == 'equine') {
			return 0.15;
		}
		else if(id == 'sheep_goat') {
			return 0.4;
		}
		return 1; // we shouldn't hit this line, but just in case
	}

}

String.prototype.replaceAllOccurence = function(str1, str2, ignore)
{
	return this.replace(new RegExp(str1.replace(/([\/\,\!\\\^\$\{\}\[\]\(\)\.\*\+\?\|\<\>\-\&])/g,"\\$&"),(ignore?"gi":"g")),(typeof(str2)=="string")?str2.replace(/\$/g,"$$$$"):str2);
};


(function ($) {
    $('.calculator-selector .dropdown-menu a').click(function (e) {
        e.preventDefault();
        var $target = $(e.target);
        var url = $target.attr('href');
        var $parent = $target.closest('.calculator-selector');
        var $selectorLabel = $parent.find('[data-toggle="dropdown"]');
        var labelSuffix = $('<span></span>', { 'class': 'caret' }).get(0).outerHTML + $('<span></span>', { 'class': 'sr-only' }).get(0).outerHTML;
        var selectionLabel = $target.text();
        $selectorLabel.html(selectionLabel + " " + labelSuffix);
        loadCalculator(url).done(function (data) {
            // Get calculator from ajaxed DOM
            var $dom = getDomObject(data);
            var $calc = $dom.find('.template-calculator');
            // Load calculator into container
            $('.calculator-container').empty();
            $('.calculator-container').append($calc);
            initCalculator($calc);
        });
    });

    function loadCalculator(url) {
        var def = $.Deferred();
        loadItem(url).done(function (data) {
            def.resolve(data);
        });
        return def;
    }

    function loadItem(url) {
        return $.get(url);
    }

    function getDomObject(data) {
        // If data is undefined, just return current dom object
        if (typeof data === 'undefined')
            return $(document);
        // Otherwise, return new dom object with current data
        else
            return $('<div>' + data + '</div>');
    }

    function initCalculator($calculator) {
        var definition = JSON.parse($calculator.html());
        var fields = definition.fields;
        var renderedFields = [];
        for (var i in fields) {
            var type = fields[i].type;
            var rendered = "";
            if (type.indexOf('input') != -1 || type.indexOf('output') != -1) {
                type = type.replace('.', '-');
                rendered = Handlebars.render('calculator-field-' + type, fields[i]);
                renderedFields.push(rendered);
            }
        }
        $calculator.replaceWith(Handlebars.render('calculator', { 'fields': renderedFields }));
        let calcName = $(".btn").text().trim();
        // Capture selected calculator with Google analytics
        g("Calculators", "Select Calculator", calcName);
    };

    $(document).on('change', '.calculator select.input', function (e) {
        updateCalculatorOutputs($(e.target).closest('.calculator'));
    });

    $(document).on('keyup', '.calculator .input', function (e) {
        updateCalculatorOutputs($(e.target).closest('.calculator'));
    });

    function updateCalculatorOutputs($parent) {
        $parent.find('.output').each(function (i, e) {
            updateOutput($(e), $parent);
        });
    }

    function updateOutput($output, $parent) {
        var formula = $output.data('formula');
        var parsed = parseFormula(formula, $parent);
        if (parsed !== false) {
            var formatted = formatOutput($output, parsed);
            $output.text(formatted);
        }
    }

    function formatOutput($el, value) {
        if ($el.data('format') == 'currency') {
            return "$" + value.formatMoney(2);
        }
        else {
            return value;
        }
    }

    function parseFormula(formula, $parent) {
        var keys = getKeys(formula);
        for (var i in keys) {
            var value = $parent.find('#' + keys[i]).val();
            // If on of values is NaN
            // return false to abort parsing
            if (!value)
                return false;
            formula = formula.replace("{" + keys[i] + "}", value);
        }
        return eval(formula);
    }

    function getKeys(formula) {
        return formula.match(/[^{}]+(?=\})/g);
    }
    
    if ($("[data-calculator]").length) {
        $("[data-calculator]").each(function(i,context) {
            var config = JSON.parse($(context).data('calculator').replaceAllOccurence("'", '"'));
            var id = config.id;
            $(context).addClass('init');
            ko.applyBindings(new CalculatorViewModel(id, config, context), context);
        });
    }
})(jQuery)