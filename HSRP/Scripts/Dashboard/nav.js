$(document).ready(function(){
    $('.hamberg').click(function() {
        $('.overlay').addClass("overlay-show");
        $('.menu-wrap').addClass("menu-wrap-show");
        return false;
    });
    $("#nav-close").click(function() {
        $('.overlay').removeClass("overlay-show");
        $('.menu-wrap').removeClass("menu-wrap-show");
    });
    $('.overlay').click(function(){
        $('.overlay').removeClass("overlay-show");
        $('.menu-wrap').removeClass("menu-wrap-show");
    });
    $('#nav ul li').each(function() {
        if ($(this).children('ul').length > 0) {
            $(this).addClass('parent');
        }
    });
    $('#nav ul li.parent > a').click(function() {
        $(this).parent().toggleClass('active');
        $(this).parent().children('ul').slideToggle('fast');
        $(this).parent().siblings().children().next().slideUp();
        $(this).parent().siblings().removeClass('active');
    });
	getnav();
     navControll();
    
});
$(window).load(function() {
    if ($('#nav').length > 0) {
        $('#nav').mCustomScrollbar({
            scrollbarPosition: "outside",
            mouseWheel: { scrollAmount: 188 },
            snapAmount: 188,
            snapOffset: 65
        });
    }
});
function navControll(){
    var wh = $(window).height();
    var headerH = $('#header-wrap').outerHeight();
    var navHeight = wh-headerH;
    $('#nav').css({ "height": navHeight + 'px' });
}
function getnav(){
        $('.navigation ul.nav.navbar-nav li.dropdown .dropdown-menu').click(function(e) {
            e.stopPropagation();
        });

        function customScrollBar() {
            var Cscroll = $(".megaSubMenu-cont");
            if (Cscroll.length) {
                Cscroll.mCustomScrollbar();
            };
        }

        (function($) {
            customScrollBar();
        })(jQuery);
}