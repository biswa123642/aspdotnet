(function ($) {
    $(".sales-rep").each(function(){
        var $this = $(this),
        datarep = JSON.parse($this.attr("data-sales-rep")),
        dalaplaceholder = $this.find(".data-placeholder"),
        dalaplaceholderhtml = $this.find(".data-placeholder").clone();
        $this.find(".sales-representatives select").on("change",function(){
            var selectvalue = $(this).val();
            dalaplaceholder.empty();
            
            //change data on select box chnage
            datarep != "" ? datarep.filter(function (val) {
                var cloneplaceholder = dalaplaceholderhtml;
                dalaplaceholderhtml.removeClass("data-placeholder");
                if (val.StateCode == selectvalue) {
                    cloneplaceholder.find(".name-info").text(val.Name);
                    cloneplaceholder.find(".mo-info").text(val.Phone);
                    cloneplaceholder.find(".email-info").text(val.Email);
                    cloneplaceholder.find(".email-info").attr('href','mailto:'+val.Email+'');
                    cloneplaceholder.find(".image-info").html("<img src='"+val.ImageURL+"' />");
                    dalaplaceholder.append(cloneplaceholder);
                }
            }) : "";
        }) 
    })
})(jQuery);