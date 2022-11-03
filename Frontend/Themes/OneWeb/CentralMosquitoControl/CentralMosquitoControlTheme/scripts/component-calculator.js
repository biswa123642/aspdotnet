(function ($) {
    $('.calculator-selector .dropdown-menu a').click(function (e) {
        e.preventDefault();
        var $target = $(e.target);
        var url = $target.attr('href');
        $(this).parents(".calculator-selector").find("button").trigger("click");
        var $parent = $target.closest('.calculator-selector');
        var $selectorLabel = $parent.find('[data-toggle="dropdown"]');
        var labelSuffix = $('<span></span>', {
            'class': 'caret'
        }).get(0).outerHTML + $('<span></span>', {
            'class': 'sr-only'
        }).get(0).outerHTML;
        var selectionLabel = $target.text();
        $selectorLabel.html(selectionLabel + " " + labelSuffix);
        loadCalculator(url).done(function (data) {
            // Get calculator from ajaxed DOM
            var $dom = getDomObject(data);
            var $calc = $dom.find('main .plain-html .component-content');
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
                if (type == 'input') {
                    rendered = `<div class="form-group">
                        <label for="${fields[i].id}">${fields[i].label}</label>
                        <input type="text" class="input form-control" id="${fields[i].id}">
                    </div>`
                } else if (type == 'output') {
                    rendered = `<div class="form-group">
                        <label for="${fields[i].id}">${fields[i].label}</label>
                        <span class="output" data-formula="${fields[i].formula}" data-format="${fields[i].format}" id="${fields[i].id}"></span>
                    </div>`
                } else if (type == 'input-select') {
                    var options = "";
                    for (var j in fields[i].options) {
                        options = options + `<option value="${fields[j].value}">${fields[j].label}</option>`;
                    }
                    rendered = `<div class="form-group">
                        <label for="${fields[i].id}">${fields[i].label}</label>
                        <select class="form-control input" id="${fields[i].id}">
                        ${options}
                        </select>
                    </div>`
                }
                renderedFields.push(rendered);
            }
        }
        $calculator.replaceWith('<div class="calculator">' + renderedFields.join("") + '</div>');
        let calcName = $(".btn").text().trim();
        // Capture selected calculator with Google analytics
        //g("Calculators", "Select Calculator", calcName);
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
        } else {
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
    $(".calculator-selector button").each(function () {
        $(this).on("click", function (e) {
            e.stopPropagation();
            $(this).attr("aria-expanded") == 'false' ? ($(this).attr("aria-expanded", 'true'), $(this).parent().addClass("open")) : ($(this).attr("aria-expanded", 'false'), $(this).parent().removeClass("open"));
        })
    })

    $('html,body').on('click', function(){
        var $this = $(".score-button-menu.calculator-selector");
        if($this.hasClass('open')) {
            $this.removeClass('open');
            $this.find('.btn').attr('aria-expanded','false');
        }
    })

})(jQuery)