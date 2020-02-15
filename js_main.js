
$(document).ready(function(){
    $('.singin').on('click',function(){
        $('#back1').remove();
        $('#z').remove();
        $('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylet.css" >')
    })


    $('.add_kurs').on('click',function(){
        $('#menu').fadeOut();
        $('.page-wrap').fadeOut();
        $('#z').remove();
        $('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylef.css" >')
    })


    $('.buttonr').on('click',function(){
        $('#z').remove();
        $('head').append('<link id="z" rel="stylesheet" type="text/css" href="stylet.css" >')
        $('#menu').fadeIn()
        $('.page-wrap').fadeIn();
    })

})