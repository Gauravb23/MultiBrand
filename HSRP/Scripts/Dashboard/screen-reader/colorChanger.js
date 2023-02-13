/**
 * --------------
 * Color Changer
 * --------------
 * Change background color and front text color from a given
 * object with different sections configurations
 *
 * @author Prince.Arora,
 * NextEon IT Solutions
 */
jQuery(function( $ ) {

    $.fn.colorChanger = function( colorSections , bgcolor, fgcolor, setDefault, themeClass) {
        try{
            for(var conatainer in colorSections){
                var current = colorSections[conatainer];
                if(themeClass != null){chgr.loadColorTheme(themeClass);}

                if(setDefault){
                    chgr.processDefault(current);
                }else{
                    chgr.processFront(current[common.FRONT], fgcolor);
                    chgr.processBackground(current[common.BACKGROUND], bgcolor);
                }
            }
        }catch (e){
            console.log("Color Selection Error: "+ e.toString());
        }
    };

}( jQuery ));


var chgr = {
    /**
     * Process front styling for given theme
     * @param $fg
     * @param fgcolor
     */
    processFront : function($fg, fgcolor){
        if($fg != null){
            for(var element in $fg){
                try{
                    $($fg[element]).css("color", fgcolor);
                }catch(e){
                    console.log("Cannot find: "+ element);
                }
            }
        }else{console.log("Configurations not found for front processing.")}

    },
    /**
     * Process background for given theme
     * @param $bg
     * @param bgcolor
     */
    processBackground : function($bg, bgcolor){
        if($bg != null){
            for(var element in $bg){
                try{
                    $($bg[element]).css("background", bgcolor);
                }catch(e){
                    console.log("Cannot find: "+ element);
                }
            }
        }else{console.log("Configurations not found for background processing.")}

    },
    /**
     * Revert webpage to default theme and color
     * @param $element
     */
    processDefault : function($element){
        if($element != null){
            var $fg = $element[common.FRONT];
            var $bg = $element[common.BACKGROUND];
            for(var item in $fg){
                common.removeStyle($fg[item]);
            }
            for(var item in $bg){
                common.removeStyle($bg[item]);
            }
        }
        window.localStorage.setItem("rajColorTheme", "");
    },

    loadColorTheme : function (themeClass) {
        if(window.localStorage !== null){
            window.localStorage.setItem("rajColorTheme", themeClass);
        }
    }

}

/**
 * Commons
 * @type {{FRONT: string, BACKGROUND: string, removeStyle: Function}}
 */
var common = {
    "FRONT":"fg",
    "BACKGROUND":"bg",
    removeStyle : function($element){
        if($element != null){
            $($element).attr("style", "");
        }
    }

}