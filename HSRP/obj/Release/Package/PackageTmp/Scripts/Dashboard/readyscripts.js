/*
 *
 * readyScripts.js is the place to put all calls that require the DOM to be ready first.
 * When a script begins with $(function) it is the same as $(document).ready(function).
 *
 * Because this self-invoking function begins with $(function), all scripts inside it
 * will automatically inherit that feature. Most site scripts will live in here.
 *
 * readyScripts.js should be placed just before the closing body tag in the document.
 *
 */

$(function() {
    var doc = document,
        body = $(doc.body),
        win = window,
        ww = $(window).width(),
        readyScripts = {
            utils: {
                openSelectedSection: function() {
                    $(".subMenuContainer").on("click", "#ctablog", function() {
                        $('.highlightsContainer').find('#blog').trigger('click');
                        body.find('.closeSideNav').trigger('click');
                    });
                    $(".subMenuContainer").on("click", "#ctaGroups", function() {
                        $('.highlightsContainer').find('#blog').trigger('click');
                        body.find('.closeSideNav').trigger('click');
                    });
                    $(".subMenuContainer").on("click", "#ctaTasks", function() {
                        $('.highlightsContainer').find('#tasks').trigger('click');
                        body.find('.closeSideNav').trigger('click');
                    });
                    $(".subMenuContainer").on("click", "#ctaDiscussions", function() {
                        $('.highlightsContainer').find('#dicussions').trigger('click');
                        body.find('.closeSideNav').trigger('click');
                    });
                },
                toggleUtilityContainer: function() {
                        $('.utilityHandle').on('click', function() {
                            if (!$(this).parent().hasClass('active')) {
                                $(this).parent().addClass('active closeUtility');
                                body.css({ marginTop: 32 });
                                body.find('.utilityContainer').css({ top: 0, position: 'fixed' });
                            } else if ($(this).parent().hasClass('active')) {
                                $(this).parent().removeClass('active closeUtility');
                                body.css({ marginTop: 0 });
                                body.find('.utilityContainer').css({ top: -64 });
                            }
                        });
                    } //toggleUtilityContainer

            }, //utils
            ui: {
                initFullpageSlide: function() {
                    var slideTimeout;
                    var totalItems = $('#fullpage .section').length;
                    console.log(totalItems);
                    if ($('#fullpage').length > 0) {
                        $('#fullpage').fullpage({
                            anchors: ['firstPage', 'secondPage', 'thirdPage'],
                            verticalCentered: false,
                            scrollingSpeed: 1100,
                            easing: 'easeInQuart',
                            css3: true,
                            navigation: false,
                            continuousVertical: false,
                            loopBottom: false,
                            loopTop: false,
                            responsiveWidth: 700,
                            animateAnchor: true,
                            afterLoad: function(anchorLink, index) {
                                var loadedSection = $(this);
                                if (index == totalItems) {
                                    $('#preloader').fadeIn('slow');
                                    $("#footer").fadeIn();
                                } else if (index != totalItems) {
                                    $('#preloader').fadeOut('slow');
                                    $("#footer").fadeOut();
                                }

                            }
                        });
                    }
                    $("#footerClose").click(function() {
                        $("#footer").fadeOut();
                    });
                    $('[data-toggle="tooltip"]').tooltip();
                    $('.state-counter').counterUp({
                        delay: 10,
                        time: 1000
                    });
                    $('#digital_font').counterUp({
                        delay: 10,
                        time: 1000
                    });

                },
                togglePanelModal: function() {
                    //open modal
                    body.find('.highlightsContainer .highlight').on('click', function() {
                        var highlightContent = $(this).find('.highlightContent').html();
                        $('.highlightModalContainer').html('<span class="lnr lnr-cross"></span>' + highlightContent);
                        $('#preloader, #status').fadeOut();
                        $('.highlightModal').addClass('active');
                        $.fn.fullpage.setAllowScrolling(false);
                    });
                    //close modal
                    body.find('.highlightModal').on('click', '.lnr-cross', function() {
                        $('.highlightModal').removeClass('active');
                        $('#preloader, #status').fadeIn();
                        $.fn.fullpage.setAllowScrolling(true);
                    });
                }, // togglePanelModal
                highlightContainerScroll: function() {
                        body.find(".highlightModalContainer").mCustomScrollbar();
                    } //highlightContainerScroll
            },
            navigation: {
                mainNavCustomScroll: function() {
                    body.find(".main-nav-container .menu-container").mCustomScrollbar();
                }, //mainNavCustomScroll
                toggleMainNav: function() {
                    body.find('#nav-toggle').on('click', function() {
                        if (!$(this).hasClass('active')) {
                            $(this).addClass('active');
                            readyScripts.navigation.openMainNav();
                            $.fn.fullpage.setMouseWheelScrolling(false);
                        } else if ($(this).hasClass('active')) {
                            //alert("yes");
                            $(this).removeClass('active');
                            readyScripts.navigation.closeMainNav();
                            $.fn.fullpage.setMouseWheelScrolling(true);
                        }
                    });
                }, //
                openSideNav: function() {
                    body.find('.sideNav ul > li').on('click', function() {
                        $(this).parent().find('.activeItem').removeClass('activeItem');
                        $(this).addClass('activeItem');
                        body.find('.container-menu-left').css({ left: 360 });
                        readyScripts.navigation.populateSideNavContent();
                    });
                }, //openSideNav
                closeSideNav: function() {

                    body.find('.closeSideNav').on('click', function() {
                        body.find('.container-menu-left').css({ left: 0 });
                        body.find('.sideNav .activeItem').removeClass('activeItem');
                    });
                }, //closeSideNav
                populateSideNavContent: function() {
                    var content = body.find('.sideNav .activeItem .subMenu').html();
                    var $contentContainer = body.find('.subMenuContainer').find('.content');
                    $contentContainer.css({ opacity: 0 });
                    $contentContainer.html(content).animate({
                        opacity: 1
                    }, 1000);
                    var delay = 100;
                    body.find('.subMenuContainer .stayOnLeft').each(function() {
                        var _this = this;
                        window.setTimeout(function() {
                            $(_this).removeClass('stayOnLeft');
                        }, 100);
                    });
                    // side nav links section scroll
                    body.find(".subMenuContainer .links").mCustomScrollbar({
                        theme: "dark"
                    });
                }, //populateSideNavContent

                openMainNav: function() {
                    $('.main-nav-container').finish().animate({ height: '100%', opacity: 0.95 }, 200, function() {
                        var delay = 0;
                        $('.menu-item').each(function() {
                            $(this).delay(delay).animate({
                                opacity: 1
                            }, 500);
                            delay += 100;
                        });
                    });
                },
                closeMainNav: function() {
                    var delay = 0;
                    $($('.menu-item').get().reverse()).each(function() {
                        $(this).delay(delay).finish().animate({
                            opacity: 0
                        }, 500, function() {
                            $('.main-nav-container').css({ height: 0, opacity: 0 });
                        });
                        delay += 100;
                    });
                },
                addPlusButton: function() {
                    $('.main-nav-container').find('li').each(function() {
                        if ($(this).find('.subMenu').size() != 0) {
                            $(this).find('> a').append("<span class='lnr lnr-chevron-down-circle expandMenu'></span>");
                        }
                    });
                }, //addPlusButton
                toggleSubMenu: function() {
                    $('.main-nav-container .expandMenu').on('click', function() {
                        $('.main-nav-container').find('.activeSubMenu').removeClass('activeSubMenu');
                        $(this).parents('a').siblings('.subMenu').slideToggle(function() {
                            if ($(this).is(':visible')) {
                                $(this).siblings('a').addClass('activeSubMenu');
                            }
                        });
                        return false;

                    });
                }
            } //navigation
        }

    //calls to the functions
   // readyScripts.ui.initFullpageSlide();
    readyScripts.navigation.mainNavCustomScroll();
    readyScripts.navigation.toggleMainNav();
    readyScripts.navigation.openSideNav();
    readyScripts.navigation.closeSideNav();
    readyScripts.navigation.addPlusButton();
    readyScripts.navigation.toggleSubMenu();
    //readyScripts.utils.toggleUtilityContainer();
    readyScripts.ui.togglePanelModal();
    readyScripts.ui.highlightContainerScroll();
    readyScripts.utils.openSelectedSection();

});