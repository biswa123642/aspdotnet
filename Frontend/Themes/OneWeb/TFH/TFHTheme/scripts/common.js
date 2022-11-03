(function ($) {
   var cloneMedia = $('.article-detail .file-type-icon-media-link').clone().addClass('d-lg-none'),
        cloneAdsense = $('.article-detail .plain-html').clone().addClass('d-lg-none'),
        adsenseStatus = $('.article-detail .plain-html .adsbygoogle'),
        articleHeading = $('.article-detail .component.content .field-content h1:first');
    
    if(articleHeading && cloneMedia && adsenseStatus){
        articleHeading.append(cloneMedia).append(cloneAdsense);
    }else if(articleHeading && cloneMedia && !adsenseStatus){
        articleHeading.append(cloneMedia);
    }else if(articleHeading && !cloneMedia && adsenseStatus){
        articleHeading.append(cloneAdsense);
    }
    
 })(jQuery);