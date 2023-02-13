$(document).ready(function () {

    // var secH = $(window).height();

    // $(".section").height(secH);


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

   if ($('.listing-content li').length > 3) {
       $('.listing-content ul').marquee({
			delay: 0,
            timing: 50
       });
    }

});