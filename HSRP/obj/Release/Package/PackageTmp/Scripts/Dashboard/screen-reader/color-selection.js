$(function () {
	
	$("#color-theme-dark").on("click", function () {
		$("body").colorChanger(customlist, $(this).attr("data-bg"), $(this).attr("data-fg"), false, "#" + $(this).attr("id"));
	});

	$("#color-theme-light").on("click", function () {
		$("body").colorChanger(customlist, $(this).attr("data-bg"), $(this).attr("data-fg"), false, "#" + $(this).attr("id"));

	});

	$("#color-theme-default").on("click", function () {
		$("body").colorChanger(customlist, $(this).attr("data-bg"), $(this).attr("data-fg"), true, "#" + $(this).attr("id"));
	});

	if (window.localStorage !== null) {
		var themeclass = window.localStorage.getItem("rajColorTheme");
		$("body").colorChanger(customlist, $(themeclass).attr("data-bg"), $(themeclass).attr("data-fg"));
	} else {
		console.log("Localstorage not available");
	}
    $('body').setFont(fontSelectors);
});


var customlist = {

	"main-column": {
		"fg": [
        ".navbar-default",
            ".utilityContainer",
            ".header-bluebg",
        "#footer"


         ],
		"bg": [
        ".navbar-default",
            ".utilityContainer",
            ".header-bluebg",
        "#footer"


        ]
	}
}

var fontSelectors = {
    "minFontSize" : 12,
    "maxFontSize" :28,
    "p": 16,
    ".copyright" : 13,
    ".slider-content h2" : 40,
    ".contentDiv h3": 19,
    ".listing-wrap .listing-content ul li h4 a": 15,
    ".listing-wrap h3": 24,
    "#footer ul.linklist li a":14,
    "#footer ul#extlink li a":14,
    ".project_label":20

  };