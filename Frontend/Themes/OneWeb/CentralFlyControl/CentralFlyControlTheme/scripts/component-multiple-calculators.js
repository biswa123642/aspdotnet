function format(str) {
    str = str.toString().replace(/\$|\,/g, '');
    if (isNaN(str))
        str = "0";
    var sign = (str == (str = Math.abs(str)));
    str = Math.floor(str * 100 + 0.50000000001);
    var cents = str % 100;
    str = Math.floor(str / 100).toString();
    if (cents < 10)
        cents = "0" + cents;
    for (var i = 0; i < Math.floor((str.length - (1 + i)) / 3); i++)
        str = str.substring(0, str.length - (4 * i + 3)) + ',' +
            str.substring(str.length - (4 * i + 3));
    return (((sign) ? '' : '-') + str + '.' + cents);
}

function removeFormat(str) {
    var objRegExp = /\(/;
    var strMinus = '';

    //check if negative
    if (objRegExp.test(str)) {
        strMinus = '-';
    }

    objRegExp = /\)|\(|[,]/g;
    str = str.replace(objRegExp, '');
    if (str.indexOf('$') >= 0) {
        str = str.substring(1, str.length);
    }
    return strMinus + str;
}

///////////////////////////////////////////////////

/*
 * Calculator - "Feed Formulator" (Cattle)
 */
var feedFormulator = {
    init: function () {
        //initialize by looping through inputs and adding onchange event since text editor strips it out
        //this also initializes the cost form since all inputs are selected on the page
        this.inputs = document.querySelectorAll('input[type="text"]');
        for (var i = 0; i < this.inputs.length; i++) {
            if (!this.inputs[i].readOnly) {
                this.inputs[i].setAttribute('onchange', 'submit()');
            }
        }
        //bind click events for reset button
        this.resetButton = document.getElementById('feed-reset');
        if (this.resetButton != null) {
            this.resetButton.setAttribute('onclick', 'javascript:void(feedFormulator.reset())');
        }
    },
    calculate: function () {
        feedForm.weightHidden.value = removeFormat(feedForm.weight.value);
        if (feedForm.weightHidden.value > 200) {
            if (feedForm.intake.readOnly) {
                feedForm.intake.readOnly = false;
            }
            feedForm.dfb.value = format((feedForm.weightHidden.value / 2.2 * 0.1).toString());
        }
        else {
            //set intake rate to 1.0
            feedForm.intake.value = format(1);
            feedForm.intake.readOnly = true;
            feedForm.dfb.value = "N/A";
        }
        feedForm.intakeHidden.value = removeFormat(feedForm.intake.value);
        feedForm.feedConsumption.value = format((feedForm.weightHidden.value * (feedForm.intakeHidden.value / 100)).toString());
        if (feedForm.weightHidden.value > 200) {
            feedForm.feedConcentration.value = format(((feedForm.dfb.value / feedForm.feedConsumption.value * 2000) / 1000).toString());
        }
        else if (feedForm.weightHidden.value > 170 && feedForm.weightHidden.value <= 200) {
            feedForm.feedConcentration.value = 9.1;
        }
        else if (feedForm.weightHidden.value > 150 && feedForm.weightHidden.value <= 170) {
            feedForm.feedConcentration.value = 10.9;
        }
        //less than 150
        else {
            feedForm.feedConcentration.value = 13.6;
        }
        switch (feedForm.feedConcentration.value) {
            case "13.6":
                feedForm.inclusion.value = format(4.5);
                break;
            case "10.9":
                feedForm.inclusion.value = format(3.6);
                break;
            case "9.1":
                feedForm.inclusion.value = format(3.0);
                break;
            default:
                feedForm.inclusion.value = format((feedForm.feedConcentration.value / 3.04).toString());
        }
        feedForm.output.value = format((50 / feedForm.inclusion.value).toString());
    },
    reset: function () {
        feedForm.reset();
    }
}

/*
 * Calculator - "Cost Calculator" (Cattle)
 */
var costCalculator = {
    init: function () {
        this.resetButton = document.getElementById('cost-reset');
        if (this.resetButton != null) {
            this.resetButton.setAttribute('onclick', 'javascript:void(costCalculator.reset())');
        }
    },
    calculate: function () {
        costForm.weightHidden.value = removeFormat(costForm.weight.value);
        costForm.dfb.value = format((costForm.weightHidden.value / 2.2 * 0.1).toString());
        costForm.dfbHidden.value = (costForm.weightHidden.value / 2.2 * 0.1);
        costForm.supplementHidden.value = removeFormat(costForm.supplement.value);
        costForm.concentration.value = format((Math.round(costForm.dfbHidden.value / costForm.supplementHidden.value * 2000) / 1000).toString());
        costForm.E.value = format(((costForm.concentration.value * 0.674) / 0.75).toString());
        costForm.F.value = format(((costForm.concentration.value * 0.674) / 0.6).toString());
        costForm.G.value = format((costForm.E.value / 2000 * costForm.supplementHidden.value).toString());
        costForm.H.value = format((costForm.F.value / 2000 * costForm.supplementHidden.value).toString());
    },
    reset: function () {
        costForm.reset();
    }
}

///////////////////////////////////////////////////

/*
 * Calculator - "Dosage Calculator" (Finishing Pigs)
 */
var dosageCalculator = {
    init: function () {
        //cheating and using init function from feed calc to set onchange for text inputs
        feedFormulator.init();
        //also need to update radio buttons
        this.inputs = document.querySelectorAll('input[type="radio"]');
        for (var i = 0; i < this.inputs.length; i++) {
            if (!this.inputs[i].readOnly) {
                this.inputs[i].setAttribute('onchange', 'submit()');
            }
        }
        //bind click events for reset button
        this.resetButton = document.getElementById('dosage-reset');
        if (this.resetButton != null) {
            this.resetButton.setAttribute('onclick', 'javascript:void(dosageCalculator.reset())');
        }
    },
    calculate: function () {
        //jquery selector bc IE doesn't like mulptiple inputs with the samae name
        dosageForm.weightHidden.value = removeFormat(dosageForm.weight.value);
        dosageForm.kg.value = parseFloat((dosageForm.weightHidden.value / 2.2).toFixed(2));
        var dfb = dosageForm.weightHidden.value <= 90 ? 0.3 : 0.2;
        dosageForm.dfb.value = dfb;
        dosageForm.mgDFB.value = dosageForm.kg.value * dfb;
        dosageForm.additive.value = parseFloat((dosageForm.mgDFB.value / jQuery('input[name="concentration"]:checked').val() * 100).toFixed(2));
        dosageForm.gadditive.value = parseFloat((dosageForm.additive.value / 1000).toFixed(4));
        dosageForm.additiveFeed.value = parseFloat((dosageForm.gadditive.value / (dosageForm.intake.value * 454) * 100).toFixed(3));
        dosageForm.tons.value = parseFloat((dosageForm.additiveFeed.value * 2000 / 100).toFixed(2));
    },
    reset: function () {
        dosageForm.reset();
    }
}

/*
 * Calculator - "Cost per Pig" (Finishing Pigs)
 */
var swineCostCalculator = {
    init: function () {
        //bind click events for reset button
        this.resetButton = document.getElementById('sCost-reset');
        if (this.resetButton != null) {
            this.resetButton.setAttribute('onclick', 'javascript:void(swineCostCalculator.reset())');
        }
    },
    calculate: function () {
        swineCostForm.clariflyPercent.value = jQuery('input[name="clarifly"]:checked').val();
        swineCostForm.gain.value = swineCostForm.endWeight.value - swineCostForm.startWeight.value;
        swineCostForm.days.value = parseFloat((swineCostForm.gain.value / swineCostForm.dailyGain.value).toFixed(2));
        swineCostForm.pounds.value = parseFloat((swineCostForm.gain.value * swineCostForm.conversion.value).toFixed(2));
        swineCostForm.pigsFed.value = parseFloat((2000 / swineCostForm.pounds.value).toFixed(2));

        if (swineCostForm.clariflyPercent.value == 0.0067) {
            swineCostForm.costD.value = parseFloat(((swineCostForm.costC.value * 2.5) / swineCostForm.pigsFed.value / swineCostForm.days.value).toFixed(4));
        }
        else {
            swineCostForm.costD.value = parseFloat(((swineCostForm.costC.value * 0.6) / swineCostForm.pigsFed.value / swineCostForm.days.value).toFixed(4));
        }
        swineCostForm.costP.value = parseFloat((swineCostForm.costD.value * swineCostForm.daysAdded.value).toFixed(2));
    },
    reset: function () {
        swineCostForm.reset();
    }
}

///////////////////////////////////////////////////

/*
 * Calculator - "Dosage Calculator" (Sows)
 */
var sowDosageCalculator = {
    init: function () {
        //cheating and using init function from feed calc to set onchange for text inputs
        feedFormulator.init();
        //also need to update radio buttons
        this.inputs = document.querySelectorAll('input[type="radio"]');
        for (var i = 0; i < this.inputs.length; i++) {
            if (!this.inputs[i].readOnly) {
                this.inputs[i].setAttribute('onchange', 'submit()');
            }
        }
        //bind click events for reset button
        this.resetButton = document.getElementById('dosage-reset');
        if (this.resetButton != null) {
            this.resetButton.setAttribute('onclick', 'javascript:void(sowDosageCalculator.reset())');
        }
    },
    calculate: function () {
        //jquery selector bc IE doesn't like mulptiple inputs with the samae name
        sowDosageForm.weightHidden.value = removeFormat(sowDosageForm.weight.value);
        sowDosageForm.kg.value = parseFloat((sowDosageForm.weightHidden.value / 2.2).toFixed(2));
        var dfb = sowDosageForm.weightHidden.value <= 90 ? 0.3 : 0.2;
        sowDosageForm.dfb.value = dfb;
        sowDosageForm.mgDFB.value = sowDosageForm.kg.value * dfb;
        sowDosageForm.additive.value = parseFloat((sowDosageForm.mgDFB.value / jQuery('input[name="concentration"]:checked').val() * 100).toFixed(2));
        sowDosageForm.gadditive.value = parseFloat((sowDosageForm.additive.value / 1000).toFixed(4));
        sowDosageForm.additiveFeed.value = parseFloat((sowDosageForm.gadditive.value / (sowDosageForm.intake.value * 454) * 100).toFixed(4));
        sowDosageForm.tons.value = parseFloat((sowDosageForm.additiveFeed.value * 2000 / 100).toFixed(2));

        //update values on the cost-per-sow calculator as well
        sowSwineCostForm.intake2.value = sowDosageForm.intake.value;
        sowSwineCostForm.tonsOutput.value = sowDosageForm.tons.value;
        sowSwineCostForm.sowWeight2.value = sowDosageForm.weight.value;
    },
    reset: function () {
        sowDosageForm.reset();
    }
}

/*
 * Calculator - "Cost per Pig" (Sows)
 */
var sowSwineCostCalculator = {
    init: function () {
        //bind click events for reset button
        this.resetButton = document.getElementById('sCost-reset');
        if (this.resetButton != null) {
            this.resetButton.setAttribute('onclick', 'javascript:void(sowSwineCostCalculator.reset())');
        }
    },
    calculate: function () {
        // sowSwineCostForm.clariflyPercent.value = jQuery('input[name="clarifly"]:checked').val();
        sowSwineCostForm.dfbTon.value = parseFloat((sowSwineCostForm.costC.value * sowSwineCostForm.tonsOutput.value).toFixed(2));
        sowSwineCostForm.dfbLb.value = parseFloat((sowSwineCostForm.dfbTon.value / 2000).toFixed(4));

        sowSwineCostForm.costSow.value = parseFloat((sowSwineCostForm.dfbLb.value * sowSwineCostForm.intake2.value * sowSwineCostForm.daysAdded.value).toFixed(2));
        sowSwineCostForm.costDay.value = parseFloat((sowSwineCostForm.costSow.value / sowSwineCostForm.daysAdded.value).toFixed(4));

    },
    reset: function () {
        sowSwineCostForm.reset();
    }
}

///////////////////////////////////////////////////



/*
 * Action bindings
 */
var feedForm = document.getElementById('feed-calc');
if (feedForm != null) {
    feedForm.action = 'javascript:void(feedFormulator.calculate())';
    feedFormulator.init();
}

var costForm = document.getElementById('cost-calc');
if (costForm != null) {
    costForm.action = 'javascript:void(costCalculator.calculate())';
    costCalculator.init();
}

var dosageForm = document.getElementById('swine-dosage');
if (dosageForm != null) {
    dosageForm.action = 'javascript:void(dosageCalculator.calculate())';
    dosageCalculator.init();
}

var swineCostForm = document.getElementById('swine-calc');
if (swineCostForm != null) {
    swineCostForm.action = 'javascript:void(swineCostCalculator.calculate())';
    swineCostCalculator.init();
}

var sowDosageForm = document.getElementById('sow-dosage');
if (sowDosageForm != null) {
    sowDosageForm.action = 'javascript:void(sowDosageCalculator.calculate())';
    sowDosageCalculator.init();
}

var sowSwineCostForm = document.getElementById('sow-calc');
if (sowSwineCostForm != null) {
    sowSwineCostForm.action = 'javascript:void(sowSwineCostCalculator.calculate())';
    sowSwineCostCalculator.init();
}



(function ($) {

    /*
     * Calculator - "Micro Machine"
     *
    */
    
    var microMachineCalculator = {
        init: function (e) {
            console.log("Initializing Micro Machine");
            this.resetButton = $(e).find('.reset');
            $(e).find('.user-input').on('change', this.calculate);
            this.form = $(e).get(0);
            if (this.resetButton) {
                this.resetButton.click(this.reset);
            }
        },
        calculate: function () {
            var costForm = this.form;

            var weight = costForm.weight.value;

            // mg of DFB required

            // var DFBConcentrationOfAdditive = costForm.dfb.value
            // var DFBConcentrationOfAdditive = costForm.dfb.value;

            // mg of additive needed/d

            // costForm.intake.value = format((costForm.weightHidden.value / 2.2 * 0.1).toString());
            // var calc1mgOfDFBRequired = (costForm.weight.value / 2.2 * 0.1 / .08 / 1000 ((costForm.intake.value * 454) * 2000)).toString();
            // var mgOfAdditiveNeeded = (mgOfDFBRequired / .08) / 1000;
            // var lbsOfAdditivePerTon = mgOfAdditiveNeeded / (costForm.intake.value * 454) * 2000;
            var calc1 = ((costForm.weight.value / 2.2 * 0.1 / .08 / 1000) / (costForm.intake.value * 454) * 2000 * costForm.pricePerPound.value).toString();
            var calc2 = ((costForm.weight.value / 2.2 * 0.1 / .03 / 1000) / (costForm.intake.value * 454) * 2000 * costForm.pricePerPound2.value).toString();

            var additivePerTon = (costForm.weight.value / 2.2 * 0.1 / .08 / 1000) / (costForm.intake.value * 454) * 2000;
            costForm.additivePerTon.value = parseFloat(additivePerTon).toFixed(2);
            var additivePerTon2 = (costForm.weight.value / 2.2 * 0.1 / .03 / 1000) / (costForm.intake.value * 454) * 2000;
            costForm.additivePerTon2.value = parseFloat(additivePerTon2).toFixed(2);

            costForm.costPerTon.value = parseFloat(calc1).toFixed(2); //costForm.pricePerPound.value * costForm.additivePerTon.value;
            costForm.costPerTon2.value = parseFloat(calc2).toFixed(2); //costForm.pricePerPound.value * costForm.additivePerTon.value;

            // console.log(costForm.costPerTon.value);
            // costForm.intakeHidden.value = (costForm.weightHidden.value / 2.2 * 0.1);
            // costForm.E.value = format(((costForm.concentration.value * 0.674) / 0.75).toString());
            // costForm.F.value = format(((costForm.concentration.value * 0.674) / 0.6).toString());
            // costForm.G.value = format((costForm.E.value / 2000 * costForm.supplementHidden.value).toString());
            // costForm.H.value = format((costForm.F.value / 2000 * costForm.supplementHidden.value).toString());
        },
        reset: function () {
            costForm.reset();
        }
    }


    var $microMachineForm = $('#micro-machine-calc');
    if ($microMachineForm.length) {
        $microMachineForm.on('submit', function () {
            microMachineCalculator.calculate($microMachineForm);
            return false;
        });
        microMachineCalculator.init($microMachineForm);
        microMachineCalculator.calculate($microMachineForm);
    }

})(jQuery)