$('.tabb').ready(function(){
   
    /*gr();
    cm();*/
    $('.delcours').on('click',function(){
        del($(this).val());})
});
function gr(){ 
    /*
    let request=1
    $.ajax({
        type:'post',
        url:'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"groups"}}',
        data: request,
        cach: false,
        error:(function(){alert('ошибка')}),
        success:function(data_g){        
            console.log ('zz')          
            console.log (data_g)           
}});
*/
}
function cm(){
    /*
    let request=1
    $.ajax({
        type:'post',
        url:'http://195.46.123.219:22555/?request={"method":"select","params":{"table_name":"competence"}}',
        data: request,
        cach: false,
        error:(function(){alert('ошибка')}),
        success:function(data_g){        
            console.log ('zz')          
            console.log (data_g) 
               for(i=0;i<data_g.result.length;i++){
                $( "#s_c" ).append( ' <option>'+data_g.result[i].name+'</option>' ) 
               }

            }});*/
}


function del (zn){ 
       
       alert(zn)
    let request=1
    let request_z='http://195.46.123.219:22555/?request={"method":"delete","params":{"table_name":"course","rows":'+zn+'}} '
    console.log('_____')
    console.log(request_z)
    $.ajax({
        type:'post',
        url:request_z,
        data: request,
        cach: false,
        error:(function(data){alert('ошибка')
        console.log(data);}),
        success:function(data){


          alert("zzzzz");   
   console.log(data);
   
            
         }});
}