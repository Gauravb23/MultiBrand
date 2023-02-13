$(function(){

	var doc = document, body = $(doc.body), win = window,
		customScript = {
			ui: {
				initParallaxSlider: function(){
					if($('#parallaxContainer').size() != 0){
						$('#parallaxContainer').multiscroll({
							sectionsColor: ['#0086AC', '#FCE7BC', '#ee465e', '#989AA3'],
							anchors: ['first', 'second', 'third', 'forth'],
							menu: '#menu',
							navigation: true,
							navigationTooltips: ['One', 'Two', 'Three', 'Four'],
							loopBottom: true,
							loopTop: true,
							sectionSelector: '.section',
							leftSelector: '.left',
							rightSelector: '.right',
							css3: true,
							afterLoad: function(anchorLink, index){
								if(index == '1' && anchorLink == 'first'){
									$('#preloader .fa').css("color", "#373434");
									$('#next').css("background", "#0A3150");
								}
								else if(index == '2' && anchorLink == 'second'){
									$('#preloader .fa').css("color", "#ee465e");
									$('#next').css("background", "#373434");
								}
								else if(index == '3' && anchorLink == 'third'){
									$('#preloader .fa').css("color", "#292929");
									$('#next').css("background", "#53111B");
								}
								else if(index == '4' && anchorLink == 'forth'){
									$('#preloader .fa').css("color", "#ee465e");
									$('#next').css("background", "#292929");
								}
							}
						});
					}
                    if ($('#heroSlider').length > 0) {
        $('#heroSlider').owlCarousel({
            loop: true,
            margin: 0,
            nav: true,
            autoplay: true,
            responsive: {
                0: {
                    items: 1
                },
                600: {
                    items: 1
                },
                1000: {
                    items: 1
                }
            }
        });
    }

	heroBanner();
    function heroBanner() {
        var wh = $(window).height();
        //var headerH = $('.utilityContainer').outerHeight();
        //var bannerHT = wh - headerH;
        var ww = $(window).width();
         if (ww <= 1024) {
             $('#heroSlider .slide-img img').css({
                 "height": "auto"
             });
         } else {
             $('#heroSlider .slide-img img').css({
                 "height": 450 + 'px'
            });
         }
    }


     $(".scroll-arrow a").click(function(e) {
        var current = $(this).attr("href");
        $('html, body').stop().animate({ scrollTop: $(current).offset().top -= 51 }, 1000);
        return false;
    });
				},//initParallaxSlider
				initParallaxScrolButton: function(){
					if ($('#parallaxContainer').length > 0) {
						$('#next').click(function () {
							$.fn.multiscroll.moveSectionDown();
						});
					}
					if ($('#fullpage').length > 0) {
						$('#next').click(function () {
							$.fn.fullpage.moveSectionDown();
						});
					}
					$('#move-up').click(function () {
						$.fn.fullpage.moveSectionUp();
					});
					$('#move-down').click(function () {
						$.fn.fullpage.moveSectionDown();
					});
					console.time("DOM update");
				},//initParallaxScrolButton
				initSectionDataBg: function(){
					$(".section").css('background', function () {
						return $(this).data('color')
					});
				},//initSectionDataBg
				initMapClickEvent:function(){
					$(".mapEvent").click(function(){
						$(".map-overlay").css('right','0').addClass("ani-l-r");
						$("#nav-toggle").css('z-index','0')
					});
					$(".mapEventCta").click(function(){
						$(".satellite-map-overlay").css('right','0').addClass("ani-l-r");
						$("#nav-toggle").css('z-index','0');
						$(".map-overlay").css('right','-100%').removeClass("ani-l-r");
					});

					$(".mapEventGraph").click(function(){
						$(".graph-overlay").css('right','0').addClass("ani-l-r");
						$("#nav-toggle").css('z-index','0')
					});
					$(".mapEventNews").click(function(){
						$(".news-overlay").css('right','0').addClass("ani-l-r");
						$("#nav-toggle").css('z-index','0')
					});
					$(".mapClose").click(function(){
						$(".map-overlay,.graph-overlay,.news-overlay,.satellite-map-overlay").css('right','-100%').removeClass("ani-l-r");
						$("#nav-toggle").css('z-index','9999999')
					});
					//$(".map-data").fadeOut();
					$(".cta").click(function(){
						$(".map-data").slideDown();
					});
				},
				initPreloaderScroll:function(){
					preloaderScroll();
				},//initPreloaderScroll
				initNewsSlider:function(){
					if ($('.news-slider').length > 0) {
						$('.news-slider').each(function () {
							if ($(this).find(".item").length > 1) $(this).owlCarousel({
								loop: true,
								margin: 0,
								autoplay: true,
								nav: false,
								responsive: {
									0: {
										items: 1
									},
									600: {
										items: 1
									},
									1000: {
										items: 1
									}
								}

							});
						});
					}
				},//initNewsSlider
				// initNewsGridMasonry:function() {
				// 	$('.grid').isotope({
				// 		itemSelector: '.grid-item',
				// 		masonry: {
				// 			columnWidth: 100
				// 		}
				// 	});
				// },
				initNumbersAnimation:function () {
					$('.count').each(function () {
						$(this).prop('Counter',0).animate({
							Counter: $(this).text()
						}, {
							duration: 3000,
							easing: 'swing',
							step: function (now) {
								$(this).text(Math.ceil(now));
							}
						});
					});
				}
			}

		};
	customScript.ui.initParallaxSlider();
	customScript.ui.initParallaxScrolButton();
	customScript.ui.initSectionDataBg();
	customScript.ui.initNewsSlider();
	customScript.ui.initMapClickEvent();
	//customScript.ui.initNewsGridMasonry();
	customScript.ui.initNumbersAnimation();
	//customScript.ui.initPreloaderScroll();

});
$(window).load(function() {
    $('#digital_font, .state-counter').counterUp({
                delay: 10,
                time: 3000
            });
	loadCustomScript={
		preLoader: function(){
			setTimeout(function(){
				$("#preloader .icon-emblem").removeClass('element');
				//$("#status").addClass('active');
				$("#status").addClass('moved');
					$("#preloader").addClass('moved');
			},0);

		}, //preloader
		toggleUtilityOnLoad: function(){
			$('.utilityHandle').trigger('click');
			window.setTimeout(function(){
				if($('.utilityContainer').hasClass('active')){
					$('.utilityHandle').trigger('click');
				}
			},4000);
		}
	};
	loadCustomScript.preLoader();
	loadCustomScript.toggleUtilityOnLoad();


    /* JS for Current Date and Time*/
    var monthNames = [ "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" ]; 
    var dayNames= ["Sun","Mon","Tue","Wed","Thurs","Fri","Sat"]
    var newDate = new Date();
    newDate.setDate(newDate.getDate());
    $('#Date').html(dayNames[newDate.getDay()] + " " + monthNames[newDate.getMonth()] + ' ' + newDate.getDate() + ',' +  newDate.getFullYear());
    setInterval( function() {
        var seconds = new Date().getSeconds(); 
        $("#sec").html(( seconds < 10 ? "0" : "" ) + seconds);
    },1000);

    setInterval( function() { 
        var minutes = new Date().getMinutes(); 
        $("#min").html(( minutes < 10 ? "0" : "" ) + minutes);
    },1000);

    setInterval( function() { 
        var hours = new Date().getHours(); 
        $("#hours").html(( hours < 10 ? "0" : "" ) + hours);
    }, 1000);

    /* JS ends here for Date and Time*/

});
/* 30 Aug Add by Ashish*/	
	$('#navigation ul li').each(function() {
        if ($(this).children('ul').length > 0) {
			$(this).addClass('parent');
        }
    });
    $('#navigation ul li.parent > a').click(function(e) {
        $(this).parent().toggleClass('active');
        $(this).parent().children('ul').slideToggle('fast');
        $(this).parent().siblings().children().next().slideUp();
		$(this).parent().siblings().removeClass('active');
	});

	var bannerwrap = $(".bannerWrap");
	if(bannerwrap.length == 0){
		$(".contentWrap").css('margin-top','50px');
	}

	/* 30 Aug Add by Ashish*/

    $('.stellarnav').stellarNav({
				theme: 'light'
			
		});

$(function(){setInterval(500);$('.container-gallery').gallery({
        height: '45vw',
        items: 6,
        thumbHeight: '10vw',
        showThumbnails: true,
        singleLine: true,
        0: {
            height: 200,
            items: 2,
            thumbHeight: 50
        },
        320: {
            height: 300,
            items: 3,
            thumbHeight: 70
        },
        480: {
            height: 300,
            thumbHeight: 100,
            items: 3
            
        },
        600: {
            height: 300,
            items: 4
        },
        768: {
            items: 4
        }
    });
                });


/* section height adjustment */
var homeSecHeight=$(window).height();
var contheight = $(".energy-eff-section").height();
var contheight2 = $(".listing-wrap").height()+20;


$(".listing-wrap").css({ "margin-top": (homeSecHeight-contheight2)/2});

$('#section4').css({'height':homeSecHeight})


/*    */
$(function(){
	$('#tender').vTicker({ 
		speed: 500,
		pause: 3000,
		animation: 'fade',
		
		showItems: 3
	});
    $('#news').vTicker({ 
		speed: 500,
		pause: 3000,
		animation: 'fade',
		
		showItems: 3
	});
     $('#infocus').vTicker({ 
		speed: 500,
		pause: 3000,
		animation: 'fade',
		
		showItems: 3
	});
    $('#upevents').vTicker({ 
		speed: 500,
		pause: 3000,
		animation: 'fade',
		
		showItems: 3
	});

});

$(document).ready(function($) {
 
  $('#Years').change(function() {
    $('#mytable-filter').show();
    var selection = $(this).val();
    var dataset = $('#mytable-filter tbody').find('tr');
    // show all rows first
    dataset.show();
    // filter the rows that should be hidden
    dataset.filter(function(index, item) {
      return $(item).find('td:nth-child(2)').text().split('-').indexOf(selection) === -1;
    }).hide();
    console.log(selection);  
      if(selection=='all')
          {
            dataset.show();
          }

  });
});

