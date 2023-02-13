$(function() {

    // $('body').setFont(fontSelectors);

    $('#font-in').on('click', function(e) {
        $('body').increaseFont(fontSelectors);
        e.preventDefault();
    });

    $('#font-dec').on('click', function(e) {
        console.log("Nothing is added in selectors list");
        $('body').decreaseFont(fontSelectors);
        e.preventDefault();

    });

    $('#font-df').on('click', function(e) {
        $('body').setFont(fontSelectors);
        e.preventDefault();
    });

    $('#bootstrap-font-space').change(function() {
        $('html').setFontSpacing($(this).val());h
    });

var min_f;
    var max_f;
    
    $.fn.increaseFont = function( fontSections ) {
        if(fontSections != null){
            for ( var element in fontSections ){
                if( $(element) != null){
                    var font = $(element).css('font-size');
                    if( font != null || font != undefined){
                        font = parseInt(font.replace('px',''));

                        if(  font < max_f ){
                            font = font + 1;
                            $(element).css('font-size', font);
                        }
                    }
                }
            }
        }else{
            console.log("Nothing is added in selectors list");
        }
    };
    
    
    $.fn.setFont = function( fontSections ){
        if(fontSections != null){
            for ( var element in fontSections ){
                if( $(element) != null){
                    $(element).css('font-size', fontSections[ element ]);
                }
                if( element == "minFontSize"){
                    min_f = fontSections[ element ];
                }
                if( element == "maxFontSize"){
                    max_f = fontSections[ element ];
                }
            }
        }else{
            console.log("Nothing is added in selectors list");
        }
    };
    
    $.fn.decreaseFont = function( fontSections ) {
        if(fontSections != null){
            for ( var element in fontSections ){
                if( $(element) != null){
                    var font = $(element).css('font-size');
                    if( font != null || font != undefined){
                        font = parseInt(font.replace('px',''));
                        if( font > min_f ){
                            font = font - 1;
                            $(element).css('font-size', font);    
                        }
                    }
                }
            }
        }else{
            console.log("2 Nothing is added in selectors list");
        }
    };
});
