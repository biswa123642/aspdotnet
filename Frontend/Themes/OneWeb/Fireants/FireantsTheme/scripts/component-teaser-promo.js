	(function ($) {
		$('.teaser-promo.promo-floting-box').eq(0).each(function(i,e){
			var $header = $(e).find('h2:eq(1)').clone();
			var $image = $(e).find('.content-left img').first();
			$image = $image.clone();
			var $ribbon = $(e).find('h2:eq(0)').parent().clone();
			var $buttons = $(e).find('.content-right a').clone();
			var $btnwrapper = $('<div/>').addClass('content-right');
			$btnwrapper.append($buttons);
			var $wrapper = $('<div/>').addClass('floating-highlight');
			$wrapper.append($header, $image, $ribbon, $btnwrapper);
			$wrapper.appendTo('body');
		});
		
		// Sticky Header
		$(window).scroll(function () {
			$('.floating-highlight').each(function(i,e) {

				var $topComponent = $('.teaser-promo.promo-floting-box').eq(0);
				var $bottomComponent = $('.related-articles-horizontal-view');

				var docViewTop = $(window).scrollTop();

				var elemTop = $topComponent.offset().top;
				var elemBottom = elemTop + $topComponent.height();
				var belowTop = elemBottom <= docViewTop;

				elemTop = $bottomComponent.offset().top;
				var aboveBottom = elemTop >= docViewTop + $(window).innerHeight() - 200;

				var $highlight = $(e);

				if(belowTop && aboveBottom ) {
					$highlight.addClass('active');
				}
				else {
					$highlight.removeClass('active');
				}
			});

		});

		//Function for smooth scroll ...
		var pauseHashOnChange = false;

		// Initialises the Smooth Scroll Functionality
		function initSmoothScroll( e, ele )
		{
			var url = $( ele ).prop('href');
			var hash = url.substring( url.indexOf('#') + 1 );
			hashChange( hash );
		}

		// Hash Change
		function hashChange( hash )
		{
			// Clean hash before use
			hash = hash.replace( /[^0-9a-z-]/gi , "");
			var $ele = $('#' + hash + '');

			if ( hash && $ele.length )
			{
				// $ele.trigger('click');
				smoothScroll( $ele );
			}
		}

		// Smooth Scroll
			function smoothScroll( $ele )
			{
				debugger;
				console.log($ele);
				var extraOffset = 0;
				var headerOffset = 120;
				var offset = headerOffset - extraOffset;

				$('html, body').stop().animate({
					'scrollTop' : $ele.offset().top - offset
				}, 900, 'swing', function() {

				});
			}

			var hash = window.location.hash;
			if(hash) {
				hash = hash.slice(1);
				hashChange(hash);
			}

			// Listen for Hash Change events.
			$(window).on('hashchange', function(e) {
				e.preventDefault();
				if ( !pauseHashOnChange ){
					var hash = window.location.hash;
					if(hash) {
						hash = hash.slice(1);
						hashChange(hash);
					}
				}
			});
		//Function for smooth scroll ...

		// Attach the Smooth Scroll event to a class.
		$(document).on('click', 'a[href^="#"]', function( e ) {
			initSmoothScroll( e, e.currentTarget);
		});

	})(jQuery);