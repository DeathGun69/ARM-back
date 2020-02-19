
$(document).ready(function(){
    $('.singin').on('click',function(){
        $('#back1').remove();
        $('#z').remove();
        $('.cours_page').css({"opacity":'1','pointer-events':'auto'});
        $('.competenc_page').css({"opacity":'0','pointer-events':'none'});
        $('.group_page').css({"opacity":'0','pointer-events':'none'});
    
        cours_table()
        
    })
  


    $('.add_kurs').on('click',function(){
        form_vis()
        $('#f1').css({"opacity":'1','pointer-events':'auto'});  
        $('#f2').css({"opacity":'0','pointer-events':'none'});  
        $('#f3').css({"opacity":'0','pointer-events':'none'});  
    })


    $('.buttonr').on('click',function(){
        $('#z').remove();
        $('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylet.css" >')
        $('#menu').fadeIn()
        $('.page-wrap').fadeIn();
        $('#f1').css({"opacity":'0','pointer-events':'none'});  
        $('#f2').css({"opacity":'0','pointer-events':'none'});  
        $('#f3').css({"opacity":'0','pointer-events':'none'});  
    })

    $('.cours_menu').on('click',function(){
        $('.cours_page').css({"opacity":'1','pointer-events':'auto'});
        $('.competenc_page').css({"opacity":'0','pointer-events':'none'});
        $('.group_page').css({"opacity":'0','pointer-events':'none'});
    })
    $('.competenc_menu').on('click',function(){
        
        $('.cours_page').css({"opacity":'0','pointer-events':'none'});
        $('.competenc_page').css({"opacity":'1','pointer-events':'auto'});
        $('.group_page').css({"opacity":'0','pointer-events':'none'});
    })
    $('.group_menu').on('click',function(){
        $('.cours_page').css({"opacity":'0','pointer-events':'none'});
        $('.competenc_page').css({"opacity":'0','pointer-events':'none'});
        $('.group_page').css({"opacity":'1','pointer-events':'auto'});
    })

    $('.add_competenc').on('click',function(){
        form_vis()
        $('#f1').css({"opacity":'0','pointer-events':'none'});  
        $('#f2').css({"opacity":'1','pointer-events':'auto'});  
        $('#f3').css({"opacity":'0','pointer-events':'none'});  
    })
    $('.add_group').on('click',function(){
        form_vis()
        $('#f1').css({"opacity":'0','pointer-events':'none'});  
        $('#f2').css({"opacity":'0','pointer-events':'none'});  
        $('#f3').css({"opacity":'3','pointer-events':'auto'});  
    })
})

function form_vis(){ 
    $('#menu').fadeOut();
    $('.page-wrap').fadeOut();
    $('#z').remove();
    $('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylef.css" >')
}
function cours_add_vis(){}


function cours_table(){$('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylet.css" >')
}