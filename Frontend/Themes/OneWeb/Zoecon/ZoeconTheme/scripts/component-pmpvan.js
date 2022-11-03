(function ($) {
    if ($('body.template--zoecon2-na-interactive-van').length && !($(".on-page-editor").length)) {
        window.preloadedContent = $.Deferred();
        window.preloadedImages = $.Deferred();

        console.log('spinning up');
        window.preloadedImagesArr = [];

        function initHubspotForm($hubspotForm) {
            $('.hs-form-booleancheckbox').each(function(i,e) {
                $(e).find('input').after($('<span class="checkmark"></span>'));
            });
        }
        window.initHubspotForm = initHubspotForm;
        
        $('link[rel=preload]').each(function (i, e) {
            var src = $(e).attr('href');
            var $img = $('<img />').attr('src', src);
            var $def = $.Deferred();
            window.preloadedImagesArr.push($def);
            $img.on('load', function () {
                $def.resolve();
            })
            $img.appendTo('body').css('display', 'none');
        });


        $.when.apply($, window.preloadedImagesArr).done(function () {
            console.log('done loading images');
            window.preloadedImages.resolve();
        });



        $(document).on('load', function () {
            console.log('preloadImages');
            window.preloadedImages.resolve();
        });


        $.when(window.preloadedContent, window.preloadedImages).done(function () {
            console.log('hide preloader');
            $(".preloader").fadeOut(1000, function () { });
        });


        window.initPmpTruck = function () {

            $.getScript('https://js.hsforms.net/forms/v2.js').done(function () {
                if (window.initHubspotForms) {
                    window.initHubspotForms();
                    setTimeout(function () {
                        $("div[id^='hubspot-form']").each(function () {
                            window.initHubspotForm($(this));
                        });
                    }, 1500);
                }
            });

            console.log('preloadedContent');
            window.preloadedContent.resolve();

            // Clickable Highlights
            $('.score-highlight').each(function (i, e) {
                var $cta = $(e).find('.score-call-to-action a.score-button');
                var href = $cta.attr('href');
                var target = $cta.attr('target') || "_self";
                if ($cta.length) {
                    $cta.remove();
                    $(e).find('.caption').wrap($('<a/>').attr({ 'class': 'caption', 'href': href, 'target': target }))
                    $(e).find('img').wrap($('<a/>').attr({ 'class': 'img', 'href': href, 'target': target }));
                    $(e).attr('data-url', href);
                }
            });

            function initTruckAddToListButtons() {
                $('.pmp-truck__product:not(.exclude)').each(function (i, e) {
                    var id = $(e).find('.score-anchorpoint').attr('name');
                    var $addToMyListButton = $('<a/>').addClass('add-to-list-button').text('Add to my list').attr('data-id', id);
                    $(e).append($addToMyListButton);
                });
            }

            function initTruckCloseButtons() {
                $('.pmp-truck__page').each(function (i, e) {
                    var $closeButton = $('<a/>').addClass('close-button').text('Return to Van');
                    $closeButton.attr('href', '#open-van');
                    $(e).append($closeButton);
                });
            }

            $('body').on('click', '.pmp-truck__product .add-to-list-button', function (e) {
                if ($(e.target).hasClass('added')) {
                    removeFromList($(e.target).data('id'));
                } else {
                    addToList($(e.target).data('id'));
                }
                ;
            });

            function addMuteButton() {
                var $target = $();
                var $wrapper = $('<div/>').addClass('mute-button-wrapper');
                var $muteButton = $('<a/>').addClass('mute-button active');
                var audio = $('audio.truck-sound').get(0);
                var promise = audio.play();

                if (promise !== undefined) {
                    promise.then(function () {
                        $muteButton.removeClass('active');
                    }).catch(error => {
                        // Autoplay not allowed!
                    });
                }

                $wrapper.append($muteButton);
                $muteButton.click(function () {
                    $(this).toggleClass('active');
                    if ($(this).hasClass('active')) {
                        window.muteTruck();
                    }
                    else {
                        window.unmuteTruck();
                    }
                });
                
                $("header .rich-text .score-right").prepend($wrapper);
            }

            window.audioVolumeIn = function (q) {
                var InT = 0;
                var setVolume = 0.5; // Target volume
                var speed = 0.01; // Rate of increase
                q.volume = InT;
                window.eAudio = setInterval(function () {
                    InT += speed;
                    q.volume = InT.toFixed(1);
                    if (InT.toFixed(1) >= setVolume) {
                        clearInterval(window.eAudio);
                    };
                }, 50);

            };

            window.audioVolumeOut = function (q) {
                var InT = 0;
                var setVolume = 0.0; // Target volume
                var speed = 0.01; // Rate of decrease
                q.volume = InT;
                window.eAudio = setInterval(function () {
                    InT += speed;
                    q.volume = InT.toFixed(1);
                    if (InT.toFixed(1) >= setVolume) {
                        clearInterval(window.eAudio);
                    };
                }, 50);

            };

            window.muteTruck = function (fade = false) {
                var audio = $('.truck-sound').get(0);
                if (fade) {
                    window.audioVolumeOut(audio);
                }
                else {
                    clearInterval(window.eAudio);
                    audio.volume = 0;
                }
            };

            window.unmuteTruck = function () {
                var audio = $('.truck-sound').get(0);

                if (audio.paused) {
                    audio.play();
                }
                window.audioVolumeIn(audio);
            };

            function addTruckSound() {
                var sources = window.pmpAudioSources ? window.pmpAudioSources : [];
                var $audio = $('<audio/>').attr({ 'controls': '', 'autoplay': '', 'loop': '', 'class': 'truck-sound' });
                // $audio.get(0).volume = 0;
                for (var s in sources) {
                    var source = sources[s];
                    var $source = $('<source/>').attr({ 'src': source.url, 'type': source.type });
                    $audio.append($source);
                }
                $('body').append($audio);
                $('.truck-sound').hide();
                $('.truck-sound').on('timeupdate', function (e) { if (e.currentTarget.currentTime >= 35) { e.currentTarget.currentTime = 8; } });
                unmuteTruck();
            }

            function addTruckShake() {
                $('.pmp-truck__closed-truck').addClass('shaking');
            }

            function removeTruckShake() {
                $('.pmp-truck__closed-truck').removeClass('shaking');
            }

            function addToList(id) {
                var $productPage = $('#' + id).closest('.pmp-truck__product');
                var productData = {};
                productData.imageUrl = $productPage.find('.wide-right > .score-left > img').attr('src').trim();
                productData.name = $productPage.find('.score-document-header > h1').html().trim();
                productData.id = $productPage.find('.score-anchorpoint').attr('id');
                var myList = getMyList();
                var isNew = true;
                for (var p in myList.products) {
                    var product = myList.products[p];
                    if (product.id == productData.id) {
                        isNew = false;
                        myList.products[p] = productData;
                        break;
                    }
                }
                if (isNew) {
                    myList.products.push(productData);
                }
                setMyList(myList);
                updateMyList();
            }

            function getMyList() {
                var myList = localStorage.getItem('myList') ? localStorage.getItem('myList') : JSON.stringify({ products: [] });
                myList = JSON.parse(myList);
                return myList;
            }

            function setMyList(myList) {
                localStorage.setItem('myList', JSON.stringify(myList));
            }

            function removeFromList(id) {
                var myList = getMyList();
                var newList = [];
                for (var p in myList.products) {
                    var product = myList.products[p];
                    if (product.id != id) {
                        newList.push(product);
                    }
                }
                myList.products = newList;
                setMyList(myList);
                updateMyList();
            }

            $('body').on('click', 'a[href="#print"]', function () {
                window.print();
            });

            if ($('.pmp-truck__layout').length) {
                updateMyList();

                var $closeButton = $('<a/>').attr('href', '#closed-van').addClass('pmp-truck__truck-close-button').text('Close the van');
                $closeButton.click(function () {

                });
                $('.pmp-truck__open-truck').append($closeButton);


            }

            $('body').on('click', '.pmp-truck__list-item', function (e) {
                if ($(e.target).hasClass('pmp-truck__list-item-star')) {
                    var id = $(e.target).closest('.pmp-truck__list-item').data('id');
                    removeFromList(id);
                    return;
                }
                var id = $(this).data('id');
                var $originatingPage = $('.pmp-truck__page.active');
                goToPage(id, $originatingPage);
            });

            $('.pmp-truck__closed-truck').each(function (i, e) {
                $(e).addClass('active');
                $(window).trigger('resize');
            });

            function resizeActivePage($page) {
                if ($page.length) {
                    $('.pmp-truck__layout').each(function (i, e) {
                        $('> .score-left', e).height($page.get(0).scrollHeight);
                    })
                }
            }

            setInterval(function () {
                resizeActivePage($('.pmp-truck__page.active'));
            }, 1000);

            function goToPage(id, $originatingPage) {

                var $page = getPageById(id);

                $("img", $page).each(function () {
                    if ($(this).data('src') !== '') {
                        $(this).attr("src", $(this).data('src'));
                    }
                });

                if ($page.is($originatingPage)) {
                    return;
                }

                if ($originatingPage.hasClass('pmp-truck__closed-truck')) {
                    $originatingPage.find('.pmp-truck__copy').slideUp(300, function () {
                        $page.addClass('active');
                        $originatingPage.removeClass('active');
                        setTimeout(_goToPage, 350);
                        resizeActivePage($page);
                    });
                } else if ($page.hasClass('pmp-truck__closed-truck')) {
                    $page.find('.pmp-truck__copy').show();
                    $page.addClass('active');
                    $originatingPage.removeClass('active');
                    setTimeout(_goToPage, 350);
                    resizeActivePage($page);

                } else {
                    $page.addClass('active');
                    $originatingPage.removeClass('active');
                    setTimeout(_goToPage, 350);
                    resizeActivePage($page);
                }


                function _goToPage() {
                    if (id == 'open-van') {
                        removeTruckShake();
                        muteTruck(true);
                        window.animation1 = setTimeout(function () {
                            $('.pmp-truck__open-truck > .score-center > .score-column1:nth-child(2) > .score-center').addClass('play');
                            window.animation2 = setTimeout(function () {
                                $('.pmp-truck__open-truck > .score-center > .score-column1:nth-child(2) > .score-center').addClass('transition');
                            }, 4250);
                        }, 500);
                    } else if (id == 'closed-van') {
                        addTruckShake();
                        if (!$('.mute-button').hasClass('active')) {
                            unmuteTruck();
                        }
                        $('.pmp-truck__open-truck > .score-center > .score-column1:nth-child(2) > .score-center').removeClass('transition').removeClass('play');
                        clearTimeout(window.animation1);
                        clearTimeout(window.animation2);
                    } else if (id == 'open-truck' && !$('.pinch-zoom-parent').length) {
                        $('.pmp-truck__open-truck > .score-center > .score-column1:nth-child(2) > .score-center').addClass('play');
                        $('.pmp-truck__open-truck').find('> .score-center > div:last-child').addClass('pinch-zoom-parent');
                        $('.pmp-truck__open-truck').find('> .score-center > div:last-child > div').addClass('pinch-zoom');
                        $.getScript('https://manuelstofer.github.io/pinchzoom/dist/pinch-zoom.umd.js', function () {
                            var el = document.querySelector('div.pinch-zoom');
                            new PinchZoom.default(el, { zoomOutFactor: 1, draggableUnzoomed: false });
                        });
                    } else if (id == 'close-truck') {

                    }
                    $("html, body").animate({
                        scrollTop: 0
                    }, 1);
                }
            }

            function getPageById(id) {
                var $page = $('#' + id.trim()).closest('.pmp-truck__page');
                return $page.length ? $page : false;
            }

            function initTruckVideoModals() {
                var tag = document.createElement('script');
                tag.src = "https://www.youtube.com/iframe_api";
                var firstScriptTag = document.getElementsByTagName('script')[0];
                firstScriptTag.parentNode.insertBefore(tag, firstScriptTag);


                $('.pmp-truck__product .video').click(function (e) {
                    e.preventDefault();
                    var uidValue = Math.floor(Math.random() * 10000 + 1000);
                    var href = $(e.target).attr('href');
                    var regExp = /^.*((youtu.be\/)|(v\/)|(\/u\/\w\/)|(embed\/)|(watch\?))\??v?=?([^#&?]*).*/;
                    var match = href.match(regExp);
                    var videoId = (match && match[7].length == 11) ? match[7] : false;
                    if (!videoId) {
                        return;
                    }
                    var uid = 'modal-' + uidValue;
                    var template = '<div class="modal video-modal fade" id="' + uid + '" tabindex="-1" role="dialog" aria-hidden="true"><div class="modal-dialog modal-lg" role="document"><div class="modal-content"><button type="button" class="close" data-dismiss="modal" aria-label="Close"></button><div class="modal-body"></div></div></div></div>'
                    var $modal = $(template);
                    $modal.on('hidden.bs.modal', function () {
                        $(this).remove();
                    });
                    $modal.find('.modal-body').append($('<div/>').attr('id', 'video-' + uidValue));
                    $('body').append($modal);
                    var player = new YT.Player('video-' + uidValue, {
                        height: '390',
                        width: '640',
                        videoId: videoId,
                        events: {
                            'onReady': function (event) {
                                event.target.playVideo()
                            }
                        }
                    });
                    $modal.modal('show');
                });
            }

            function updateMyList() {
                $('.pmp-truck__list > .score-center > .page').remove();
                $('.add-to-list-button').text('Add to my list').removeClass('added');
                var myList = getMyList();
                for (var p in myList.products) {
                    var product = myList.products[p];
                    var $template = $('.pmp-truck__list .score-content-spot .pmp-truck__list-item');
                    var $instance = $template.clone();
                    $instance.find('.pmp-truck__list-item-label').html(product.name);
                    $instance.find('.pmp-truck__list-item-image img').attr('src', product.imageUrl);
                    $instance.attr('data-id', product.id);
                    $('.pmp-truck__list > .score-center').append($instance);
                    updateProductPageStatus(product.id, true);
                }
                var $items = $('.pmp-truck__list > .score-center > .pmp-truck__list-item');
                $.fn.chunk = function (size) {
                    var arr = [];
                    for (var i = 0; i < this.length; i += size) {
                        arr.push(this.slice(i, i + size));
                    }
                    return this.pushStack(arr, "chunk", size);
                }
                //
                $items.chunk(3).wrap($("<div class='page'></div>"));
            }

            function updateProductPageStatus(id, added) {
                if (added) {
                    $('#' + id).closest('.pmp-truck__product').find('.add-to-list-button').text('Remove from my list').addClass('added');
                } else {
                    $('#' + id).closest('.pmp-truck__product').find('.add-to-list-button').text('Add to my list').removeClass('added');
                }
            }

            setInterval(function () {
                $('.pmp-truck__layout .score-image-button').each(function (i, e) {
                    var $refImage = $(e).parent().find('> .score-image').first();
                    var refImageWidth = $refImage.get(0).naturalWidth;
                    var refImageHeight = $refImage.get(0).naturalHeight;

                    var $item = $(e);
                    var naturalWidth = $item.find('img').get(0).naturalWidth;
                    var naturalHeight = $item.find('img').get(0).naturalHeight;
                    var widthPercent = naturalWidth / refImageWidth * 100;
                    $item.css({ 'width': widthPercent + '%' });
                    $item.css({ 'position': 'absolute' });

                    var theClass = $item.attr('class');
                    if (theClass.match(/x(\d+\-*\d*)/) && theClass.match(/y(\d+\-*\d*)/)) {
                        var x = theClass.match(/x(\d+\-*\d*)/) ? theClass.match(/x(\d+\-*\d*)/)[1] : false;
                        var y = theClass.match(/y(\d+\-*\d*)/) ? theClass.match(/y(\d+\-*\d*)/)[1] : false;
                        var w = theClass.match(/w(\d+\-*\d*)/) ? theClass.match(/w(\d+\-*\d*)/)[1] : false;
                        x ? $item.css({ 'left': x.replace('-', '.') + '%' }) : false;
                        y ? $item.css({ 'top': y.replace('-', '.') + '%' }) : false;
                        w ? $item.css({ 'width': w.replace('-', '.') + '%' }) : false;
                    }

                });
            }, 1500);

            $('body').on('click', 'a[href^="#"]', function (e) {
                e.preventDefault();
                var id = $(this).attr('href').substring(1);
                var $originatingPage = $('.pmp-truck__page.active');
                goToPage(id, $originatingPage);
            });

            var $salesForm = $('#hsForm_cad2ced7-5371-4bbf-b26d-760cf552c3ab');
            $('body').on('change', '#hsForm_cad2ced7-5371-4bbf-b26d-760cf552c3ab [name=pet_cls_zoecon_pmp_van_include_product_list]', function (e) {
                var $hiddenField = $salesForm.find('[type=hidden]');

                if ($(e.currentTarget).prop("checked")) {
                    $hiddenField.val(generateCustomerProductList());
                } else {
                    $hiddenField.val('');
                }
            });

            function generateCustomerProductList() {
                var myList = getMyList();
                var items = [];
                for (var p in myList.products) {
                    var product = myList.products[p];
                    items.push("<li>" + product.name + "</li>");
                }
                var message = "<ol>" + items.join('') + "</ol>";
                return message;
            }

            function initRelatedProducts() {
                $('.pmp-truck__product-related-products').each(function (i, e) {
                    var $scrollbar = $('<div/>').addClass('scrollbar').append($('<div/>').addClass('bar'));
                    var numSlides = $('> .score-center > div', e).length;
                    $(e).children('.score-center').slick({
                        slidesToShow: 1,
                        mobileFirst: true,
                        responsive: [
                            {
                                breakpoint: 1600,
                                settings: {
                                    slidesToShow: Math.min(4, numSlides)
                                }
                            },
                            {
                                breakpoint: 1200,
                                settings: {
                                    slidesToShow: Math.min(3, numSlides)
                                }
                            },

                            {
                                breakpoint: 1024,
                                settings: {
                                    slidesToShow: Math.min(2, numSlides)
                                }
                            },
                            {
                                breakpoint: 768,
                                settings: {
                                    slidesToShow: Math.min(4, numSlides)
                                }
                            },
                            {
                                breakpoint: 480,
                                settings: {
                                    slidesToShow: Math.min(2, numSlides)
                                }
                            },

                        ]
                    }).on('beforeChange', function (event, slick, currentSlide, nextSlide) {
                        var numSlides = slick.slideCount;
                        var percentage = nextSlide / (numSlides - 1);
                        var left = ($scrollbar.width() - $scrollbar.find('.bar').width()) * percentage;
                        $(event.target).parent().find('.scrollbar .bar').css({ left: left + "px" });
                    });

                    $(e).append($scrollbar);
                });
            }
            $('.pmp-truck__products').each(function (i, e) {
                $('> .score-center > div', e).each(function (i, f) {
                    $(f).addClass('pmp-truck__product pmp-truck__page');
                    $('> .score-center > .score-column2 + .score-section-header + .score-column1', f).addClass('pmp-truck__product-related-products');
                    $(f).find('> .score-center > .score-column2 >.score-right > .score-content-spot').eq(1).addClass('pmp-truck__links');
                    $(f).find('> .score-center > .score-column2 >.score-right > .score-content-spot').eq(1).find('a').each(function (i, g) {
                        if ($(g).attr('href').indexOf('watch') !== -1) {
                            $(g).addClass('video');
                        }
                    });
                });
                $(e).show();
                if (window.pmpAudioSources) {
                    addTruckSound();
                    addMuteButton();
                }
                addTruckShake();
                initRelatedProducts();
                initTruckCloseButtons();
                initTruckAddToListButtons();
                initTruckVideoModals();
            });
            setInterval(function () {
                $('.pmp-truck__product-related-products:visible').each(function (i, e) {
                    if ($(e).find('.slick-cloned').length) {
                        $('.scrollbar', e).show();
                    } else {
                        $('.scrollbar', e).hide();
                    }
                })
            }, 1000);
        }
        $.get(window.pmpvan_url ? window.pmpvan_url : '/pmpvancontent', function (data) {
            //data = data.split('<img').join('<img loading="lazy"');
            var content = /(<div class="page-wrapper".*?>[\s\S]*)<footer/.exec(data)[1];
            // var header = /(<header.*?>[\s\S]*<\/header>)/.exec(data)[1];
            // var footer = /(<footer.*?>[\s\S]*<\/footer>)/.exec(data)[1];
            // $(header).insertBefore('.page-wrapper');
            // $(footer).insertAfter('.page-wrapper');
            $('.page-wrapper').load('...', window.initPmpTruck);
            $('.page-wrapper').replaceWith(content);
            // setTimeout(initPmpTruck, 100);
        });
    }
})(jQuery);